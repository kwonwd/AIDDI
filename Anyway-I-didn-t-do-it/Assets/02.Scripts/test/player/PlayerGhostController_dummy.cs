using SourGrape.hongyeop;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Content;
using UnityEngine;

namespace SourGrape.hongyeop
{
    public class PlayerGhostController_dummy : MonoBehaviour
    {
        #region public properties
        #endregion

        #region exposed private properties
        [SerializeField]
        private GameObject _summonParticle; // Effect spawned when ghost is summoned
        [SerializeField]
        private float _moveSpeed;
        [SerializeField]
        private float _mouseSensitivity;
        #endregion

        #region private properties
        private Animator _anim;
        // ��ư ���� ����
        private bool _walkDown; // Walk button
        private bool _rotateCameraViewDown; // RotateCameraView button
        #endregion

        void Start() // �ش� ������Ʈ�� ó�� ������ �� �� ���� ȣ��Ǵ� �ڵ�
        {
            _anim = GetComponentInChildren<Animator>();
            _anim.applyRootMotion = false;  // Root Motion�� ��Ȱ��ȭ�Ͽ� �ִϸ��̼��� ȸ���� ������ ���� �ʵ��� ��
            _moveSpeed = 2f;
            _mouseSensitivity = 4f;
            _summonParticle = Instantiate(_summonParticle, transform.position, transform.rotation);
            Destroy(_summonParticle, 3.5f);
        }
        void Update()
        {
            GetInput(); // ��ư �Է� ����
            Move();
            Rotate(); // ���콺 X�� ȸ��
        }

        private void GetInput()
        {
            _walkDown = Input.GetButton("Walk");
            _rotateCameraViewDown = Input.GetButton("RotateCameraView");
        }

        private void Move()
        {
            float moveDirX = Input.GetAxisRaw("Horizontal");
            float moveDirZ = Input.GetAxisRaw("Vertical");

            Vector3 moveHorizontal = transform.right * moveDirX;
            Vector3 moveVertical = transform.forward * moveDirZ;

            Vector3 velocity = (moveHorizontal + moveVertical).normalized * _moveSpeed * (_walkDown ? 0.3f : 1f);

            transform.position += velocity * Time.deltaTime; // Transform�� ����Ͽ� ��ġ�� ���� ����
        }

        private void Rotate()
        {
            if (_rotateCameraViewDown) // "RotateCameraView" ��ư�� ���� ��� Rotate�� ���õǾ� ȭ�� ȸ�� X (ī�޶� ȸ���ϰ� ��)
            {
                return;
            }
            float yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0f, yRotation, 0f) * _mouseSensitivity;
            transform.Rotate(rotation); // Transform�� ����Ͽ� ȸ���� ���� ����
        }
    }
}