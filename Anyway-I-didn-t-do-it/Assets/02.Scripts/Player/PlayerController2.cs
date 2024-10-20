using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Content;
using UnityEngine;

namespace SourGrape.hongyeop
{
    public class PlayerController2 : MonoBehaviour
    {
        #region exposed private properties
        [SerializeField] // private property�� Inspector â���� ������ �� �ְ� ����
        private Rigidbody _myRigid;

        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private float _mouseSensitivity;

        [SerializeField]
        private float _aniSmoothSpeed; // �ִϸ��̼� �ε巯�� ��ȯ�� ���� ����

        [SerializeField]
        private float _dashCooldown; // ��� ��ٿ�

        [SerializeField]
        private float _moveAcc;

        [SerializeField]
        private float _maxSpeed;

        #endregion

        #region private properties
        private Animator _anim;
        // ��ư ���� ����
        private bool _walkDown; // Walk button
        private bool _rotateCameraViewDown; // RotateCameraView button
        private bool _dashDown;

        private float _aniMoveDirX; // �ִϸ��̼� ���� ����
        private float _aniMoveDirZ; // �ִϸ��̼� ���� ����

        private float _dashCurrentCooldown = 0f; // ��� ��ٿ� Ÿ�̸�
        private float _dashSpeed = 50.0f; // ��� �ӵ�
        private float _dashMaxTime = 0.17f; // �뽬 ���� �ð�
        private float _dashCurrentTime;
        private bool _dashCan = true; // ��� ���� ����
        private bool _dashInvisible = false; // ��� ���� ����
        private bool _dashCheck = false; // ���� ��� ��Ȳ ����
        private Vector3 _dashDirection;

        #endregion

        void Start() // �ش� ������Ʈ�� ó�� ������ �� �� ���� ȣ��Ǵ� �ڵ�
        {
            _anim = GetComponentInChildren<Animator>();
            _anim.applyRootMotion = false;  // Root Motion�� ��Ȱ��ȭ�Ͽ� �ִϸ��̼��� ȸ���� ������ ���� �ʵ��� ��
            _myRigid = GetComponent<Rigidbody>();
            _moveSpeed = 5f;
            _mouseSensitivity = 4f;
            _aniSmoothSpeed = 6f;
            _dashCooldown = 3.0f;
            _moveAcc = 10f;
            _maxSpeed = 10f;
        }
        void Update()
        {
            GetInput(); // ��ư �Է� ����
            DashInput();
        }

        void FixedUpdate() // ���� ������ ó���ϴ� ������ �ð� ���ݸ��� ȣ�� (update�� �����Ӹ��� ȣ��)
        {
            if (_dashCheck)
            {
                Dash();
            }
            else
            {
                Move(); // WASD �̵�
            }
            Rotate(); // ���콺 X�� ȸ��
            AnimatorUpdate(); // Animator ���� ������Ʈ
        }
        private void GetInput()
        {
            _walkDown = Input.GetButton("Walk");
            _rotateCameraViewDown = Input.GetButton("RotateCameraView");
            _dashDown = Input.GetKeyDown(KeyCode.Space);
            if (_dashDown)
            {
                Debug.Log("����");
                Debug.Log(_dashCurrentCooldown);
            }
        }

        private void DashInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _dashCan && !_dashCheck && _dashCurrentCooldown <= 0)
            {
                DashStart();
                Debug.Log("���");
            }
            if (_dashCurrentCooldown > 0)
            {
                _dashCurrentCooldown -= Time.deltaTime;
            }
        }


        private void Dash()
        {
            if (_dashCurrentTime > 0)
            {
                _myRigid.MovePosition(transform.position + transform.forward * _dashSpeed * Time.deltaTime);
                _dashCurrentTime -= Time.deltaTime;
            }
            else
            {
                _dashCheck = false;
                _dashInvisible = false;
            }
        }


        private void DashStart()
        {
            _dashCheck = true;
            _dashCurrentTime = _dashMaxTime;
            _dashInvisible = true;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _dashDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

            if (_dashDirection == Vector3.zero)
            {
                // �ƹ� �Է��� ���� ��쿡�� ������ �뽬
                _dashDirection = transform.forward;
            }
            _anim.SetTrigger("isDash"); // �뽬 �ִϸ��̼� Ʈ����
            _dashCurrentCooldown = _dashCooldown; // ��ٿ� ����
        }

        private void Move()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

            if (moveDir.magnitude < _maxSpeed)
            {
                _myRigid.AddForce(moveDir * _moveAcc * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                _myRigid.velocity = _myRigid.velocity.normalized * _maxSpeed;
            }
        }

        private void Rotate()
        {
            if (_rotateCameraViewDown) // "RotateCameraView" ��ư�� ���� ��� Rotate�� ���õǾ� ȭ�� ȸ�� X (ī�޶� ȸ���ϰ� ��)
            {
                return;
            }
            float yRoation = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0f, yRoation, 0f) * _mouseSensitivity;
            _myRigid.MoveRotation(_myRigid.rotation * Quaternion.Euler(rotation));
        }



        private void AnimatorUpdate()
        {
            // �̵� �ӵ��� ��� (Walk ��ư�� ������ 0.3��� ����)
            float targetMoveDirX = Input.GetAxisRaw("Horizontal") * (_walkDown ? 0.3f : 1f);
            float targetMoveDirZ = Input.GetAxisRaw("Vertical") * (_walkDown ? 0.3f : 1f);

            // Target ���� ������ �� Ÿ�ٰ��� ���� (threshold)
            if (Mathf.Abs(_aniMoveDirX - targetMoveDirX) < 0.1f)
            {
                _aniMoveDirX = targetMoveDirX;
            }
            if (Mathf.Abs(_aniMoveDirZ - targetMoveDirZ) < 0.1f)
            {
                _aniMoveDirZ = targetMoveDirZ;
            }

            // Lerp�� �̿��Ͽ� �ε巯�� �̵� ó��
            if (_aniMoveDirX < targetMoveDirX)
            {
                _aniMoveDirX += Time.deltaTime * _aniSmoothSpeed;
            }
            else if (_aniMoveDirX > targetMoveDirX)
            {
                _aniMoveDirX -= Time.deltaTime * _aniSmoothSpeed;
            }

            if (_aniMoveDirZ < targetMoveDirZ)
            {
                _aniMoveDirZ += Time.deltaTime * _aniSmoothSpeed;
            }
            else if (_aniMoveDirZ > targetMoveDirZ)
            {
                _aniMoveDirZ -= Time.deltaTime * _aniSmoothSpeed;
            }

            // �ִϸ����� ���� ������Ʈ
            _anim.SetFloat("DirX", _aniMoveDirX); // �¿� �̵��� ���� �ִϸ��̼� ����
            _anim.SetFloat("DirZ", _aniMoveDirZ); // �յ� �̵��� ���� �ִϸ��̼� ����
        }
    }
}