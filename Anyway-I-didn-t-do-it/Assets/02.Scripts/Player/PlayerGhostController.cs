using Photon.Pun;
using SourGrape.hongyeop;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Content;
using UnityEngine;

namespace SourGrape.hongyeop
{
    public class PlayerGhostController : MonoBehaviour
    {
        #region public properties
        #endregion

        #region exposed private properties
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
            _anim = GetComponent<Animator>();
            _anim.applyRootMotion = false;  // Root Motion�� ��Ȱ��ȭ�Ͽ� �ִϸ��̼��� ȸ���� ������ ���� �ʵ��� ��
            _moveSpeed = 3f;
            _mouseSensitivity = 4f;
        }
        void Update()
        {
            GetInput(); // ��ư �Է� ����
        }

        void FixedUpdate()
        {
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

        private IEnumerator Dying()
        {
            _anim.SetTrigger("Die");
            // ���� �ð� ���� ���
            yield return new WaitForSeconds(2);

            // ������Ʈ ��ü�� ��Ȱ��ȭ (�������� ��ȣ�ۿ��� ��Ȱ��ȭ)
            GameObject summonParticle = PhotonNetwork.Instantiate("Effect/GrenadeExplosionBlue", transform.position, transform.rotation);
            StartCoroutine(DestroyAfterDelay(summonParticle, 2f));
            PhotonNetwork.Destroy(gameObject);
            // gameObject.SetActive(false);
        }

        private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            PhotonNetwork.Destroy(obj);
        }
    }
}