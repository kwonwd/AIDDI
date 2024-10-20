using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SourGrape.hongyeop
{
    public class CameraController_dummy : MonoBehaviour
    {
        #region private exposed properties
        [SerializeField]
        private float _verticalRotationLimit; // ���� ȸ�� ���� ����
        [SerializeField]
        private float _distance; // ī�޶�� �÷��̾� �� �Ÿ�
        [SerializeField]
        private float _mouseSensitivityX; // ī�޶� ȸ�� �ӵ� x
        [SerializeField]
        private float _mouseSensitivityY; // ī�޶� ȸ�� �ӵ� y


        [SerializeField]
        private float _cameraAdjustY; // ī�޶� Y�� ����
        #endregion

        #region private properties
        private Transform _target; // �÷��̾��� Transform
        private float _horizontalRotation; // ī�޶� ȸ�� ���� x
        private float _verticalRotation; // ���� ȸ�� ���� y
        private PlayerStatusController_dummy _playerStatusController; // �÷��̾� ���� ��Ʈ�ѷ�
        #endregion
        void Start()
        {
            _verticalRotationLimit = 80.0f;
            _distance = 5.0f;
            _mouseSensitivityX = 2.0f;
            _mouseSensitivityY = 2.0f;
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _cameraAdjustY = 1.0f;
            _playerStatusController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusController_dummy>();
        }

        void Update()
        {
            // UpdateCameraPosition();
        }

        //private void UpdateCameraPosition()
        //{
        //    bool qDown = Input.GetButton("RotateCameraView");
        //    // ���콺 Y �����ӿ� ���� ���� ȸ�� ���� ����
        //    _verticalRotation -= Input.GetAxisRaw("Mouse Y") * _mouseSensitivityY;
        //    _verticalRotation = Mathf.Clamp(_verticalRotation, 20, _verticalRotationLimit);

        //    if (qDown || _playerStatusController.IsDead)
        //    {
        //        // ���콺 X �����ӿ� ���� ���� ȸ�� ���� ����
        //        float horizontalInput = Input.GetAxisRaw("Mouse X");
        //        _horizontalRotation += horizontalInput * _mouseSensitivityX;
        //    }
        //    else
        //    {
        //        // ĳ���� ���⿡ ���� ���� ȸ�� ���� ����
        //        _horizontalRotation = _target.eulerAngles.y;
        //    }
        //    // ȸ�� ���
        //    Quaternion targetRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);

        //    // ī�޶� ��ġ ���
        //    Vector3 offset = new Vector3(0, 0, -_distance); // Z �������� �ڷ� �̵�
        //    Vector3 position = _target.position + targetRotation * offset;

        //    // ī�޶� ��ġ�� ȸ�� ����
        //    transform.position = position;
        //    transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
        //    transform.LookAt(_target); // ī�޶� �׻� �÷��̾ �ٶ󺸵��� ����
        //    transform.position += new Vector3(0, _cameraAdjustY, 0); // ī�޶� ��������
        //}
    }
}
