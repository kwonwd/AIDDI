using SourGrape.hongyeop;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Content;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace SourGrape.hongyeop
{
    public class PlayerController_dummy : MonoBehaviourPun
    {
        #region public properties
        public GameObject ProjectileParticle; // Effect attached to the gameobject as child
        public bool IsDashing = false;
        public float PlayerDefaultSpeed = 5.0f; // deflaut �̵� �ӵ�
        #endregion

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
        #endregion

        #region private properties
        private Coroutine _changeSpeedCoroutine; // �ӵ� ���� �ڷ�ƾ
        private Animator _anim;
        // ��ư ���� ����
        private bool _walkDown; // Walk button
        private bool _rotateCameraViewDown; // RotateCameraView button
        private bool _dashDown; // �ϴ��� space
        private bool _teaBagging1Down; // TeaBagging1 button
        private bool _teaBagging2Down; // TeaBagging2 button
        private bool _teaBagging3Down; // TeaBagging3 button

        private float _aniMoveDirX; // �ִϸ��̼� ���� ����
        private float _aniMoveDirZ; // �ִϸ��̼� ���� ����

        private float _dashCurrentCooldown = 0f; // ��� ��ٿ� Ÿ�̸�
        private float _dashSpeed = 50.0f; // ��� �� �ӵ�
        private float _dashCurrentTime; // �뽬 Ÿ�̸�
        private float _dashMaxTime = 0.17f; // �뽬 ���� �ð�
        private Vector3 _dashDirection; // �뽬 ����
        #endregion

        public void ChangeSpeedForDuration(float newSpeed, float duration)
        {
            // ���� ���� ���� �ڷ�ƾ�� ������ ����
            if (_changeSpeedCoroutine != null)
            {
                StopCoroutine(_changeSpeedCoroutine);
            }
            // ���ο� �ڷ�ƾ ����
            _changeSpeedCoroutine = StartCoroutine(ChangeSpeedCoroutine(newSpeed, duration));
        }

        void Start() // �ش� ������Ʈ�� ó�� ������ �� �� ���� ȣ��Ǵ� �ڵ�
        {
            _anim = GetComponentInChildren<Animator>();
            _anim.applyRootMotion = false;  // Root Motion�� ��Ȱ��ȭ�Ͽ� �ִϸ��̼��� ȸ���� ������ ���� �ʵ��� ��
            _myRigid = GetComponent<Rigidbody>();
            _moveSpeed = PlayerDefaultSpeed;
            _mouseSensitivity = 4f;
            _aniSmoothSpeed = 6f;
            _dashCooldown = 3.0f;
        }
        void Update()
        {
            // if(photonView.IsMine)
            GetInput(); // ��ư �Է� ����
        }


        void FixedUpdate() // ���� ������ ó���ϴ� ������ �ð� ���ݸ��� ȣ�� (update�� �����Ӹ��� ȣ��)
        {
            // if (photonView.IsMine)
            {
                Dash();
                Move();
                Rotate(); // ���콺 X�� ȸ��
                AnimatorUpdate(); // Animator ���� ������Ʈ
                TeaBagging();
            }
        }

        private IEnumerator ChangeSpeedCoroutine(float newSpeed, float duration)
        {
            _moveSpeed = newSpeed;
            yield return new WaitForSeconds(duration);  // �־��� �ð� ���� ���
            _moveSpeed = PlayerDefaultSpeed;
            _changeSpeedCoroutine = null; // �ڷ�ƾ�� ����Ǿ����� ǥ��
        }

        private void GetInput()
        {
            _walkDown = Input.GetButton("Walk");
            _rotateCameraViewDown = Input.GetButton("RotateCameraView");
            _dashDown = Input.GetKeyDown(KeyCode.Space);
            _teaBagging1Down = Input.GetButton("TeaBagging1");
            _teaBagging2Down = Input.GetButton("TeaBagging2");
            _teaBagging3Down = Input.GetKeyDown(KeyCode.Alpha3);
        }

        private void Dash()
        {
            _dashCurrentCooldown -= Time.deltaTime;
            if (_dashCurrentCooldown > 0) // cooldown�� ��� �뽬 �Ұ�
            {
                Debug.Log("Dash on cooldown: " + _dashCurrentCooldown);
                return;
            }

            if (_dashDown) // �뽬 ��ư ���� ���
            {
                _dashCurrentCooldown = _dashCooldown; // ��ٿ� �ʱ�ȭ

                // �뽬 ���� ����
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                _dashDirection = (transform.right * horizontal + transform.forward * vertical).normalized;
                if (_dashDirection == Vector3.zero)// �ƹ� �Է��� ���� ��쿡�� ������ �뽬
                {
                    _dashDirection = transform.forward;
                }
                StartCoroutine(PerformDash()); // �뽬 �ڷ�ƾ
                _anim.SetTrigger("isDash"); // �뽬 �ִϸ��̼� Ʈ����
                Quaternion dashRotation = Quaternion.LookRotation(_dashDirection, Vector3.up);
                if (ProjectileParticle != null)
                {
                    GameObject projectileParticle = Instantiate(ProjectileParticle, transform.position, dashRotation);
                    projectileParticle.transform.rotation = Quaternion.LookRotation(_dashDirection, Vector3.up);
                    Destroy(projectileParticle, _dashMaxTime);
                }
            }
        }

        private IEnumerator PerformDash()
        {
            _dashCurrentTime = _dashMaxTime; // �뽬 ���� �ð� �ʱ�ȭ
            IsDashing = true; // �뽬 Ȯ�� ����

            // �뽬 ���� �ð� ����
            while (_dashCurrentTime > 0)
            {
                // �뽬 �� �ӵ� ����
                _myRigid.velocity = _dashDirection * _dashSpeed;

                // �뽬 ���� �ð� ����
                _dashCurrentTime -= Time.deltaTime;

                yield return null; // ���� �����ӱ��� ���
            }

            // �뽬�� ������ �ӵ��� 0���� ���� (�Ǵ� ���� �̵� �ӵ��� ����)
            _myRigid.velocity = Vector3.zero;
            IsDashing = false; // �뽬 ����
        }

        private void Move()
        {
            float moveDirX = Input.GetAxisRaw("Horizontal");
            float moveDirZ = Input.GetAxisRaw("Vertical");

            Vector3 moveHorizontal = transform.right * moveDirX;
            Vector3 moveVertical = transform.forward * moveDirZ;

            Vector3 velocity = (moveHorizontal + moveVertical).normalized * _moveSpeed * (_walkDown ? 0.3f : 1f);

            _myRigid.MovePosition(transform.position += velocity * Time.deltaTime); // rigidbody�� ó���� ��� ������ ���� ó��
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

        private void TeaBagging()
        {
            // teabagging1
            if (_teaBagging1Down)
            {
                _anim.SetBool("isTeaBagging1", true);
            }
            else
            {
                _anim.SetBool("isTeaBagging1", false);
            }

            // teabagging2
            if (_teaBagging2Down)
            {
                _anim.SetBool("isTeaBagging2", true);
            }
            else
            {
                _anim.SetBool("isTeaBagging2", false);
            }

            // teabagging3
            if (_teaBagging3Down)
            {
                Debug.Log("TeaBagging3 ��ư�� ���Ƚ��ϴ�!");
                _anim.SetTrigger("DoTeaBagging3");
                _anim.SetBool("isTeaBagging", true);

            }
            else
            {
                _anim.SetBool("isTeaBagging", false);
            }
        }
    }
}
