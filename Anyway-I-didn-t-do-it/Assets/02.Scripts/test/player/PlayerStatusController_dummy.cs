using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SourGrape.yewon;

namespace SourGrape.hongyeop
{
    public class PlayerStatusController_dummy : MonoBehaviourPun
    {
        #region public properties
        public GameObject NearObject;
        public int PlayerHealth = 1;
        public bool IsDead = false; // ���� ����
        public bool IsDance = false; // �� ����
        public int SlowCount = 0;
        #endregion

        #region exposed private properties
        #endregion

        #region private properties
        private float _deathSpinSpeed; // �ʴ� ȸ�� ����
        private float _deathLaunchForce; // ���ư� ��
        private float _deathLaunchForceY; // Y�� ���ư��� ����
        private Vector3 arrowImpactDirection;
        private Rigidbody _myRigid; // Rigidbody ������Ʈ�� ������ ����
        private Animator _anim; // Animator ������Ʈ�� ������ ����
        private PlayerController_dummy _playerMoveController;
        private PlayerAttackController_dummy _playerAttackController;
        private GameObject _playerList;
        #endregion

        void Start()
        {
            _deathSpinSpeed = 1500f; // �ʴ� ȸ�� ����
            _deathLaunchForce = 15f; // ���ư� ��
            _deathLaunchForceY = 1.2f; // Y�� ���ư��� ����                               
            // Rigidbody ������Ʈ ��������
            _myRigid = GetComponent<Rigidbody>();
            // Animator ������Ʈ ��������
            _anim = GetComponent<Animator>();
            _playerMoveController = GetComponent<PlayerController_dummy>();
            _playerAttackController = GetComponent<PlayerAttackController_dummy>();
            _playerList = GameObject.Find("PlayerList");
        }

        void FixedUpdate()
        {
            // �÷��̾��� ü���� 0�� �� Death �Լ� ����
            if (PlayerHealth <= 0 && !IsDead)
            {
                Death();
            }
            if (IsDance)
            {
                Dance();
            }
        }

        private void Dance()
        {
            // �� �ִϸ��̼� ����
            StartCoroutine(Dancing(5f)); // 5�ʰ� ��
            IsDance = false;
        }

        private IEnumerator Dancing(float duration)
        {
            _anim.SetBool("isDance", true); // �� �ִϸ��̼�
            _playerMoveController.enabled = false; // �÷��̾� �̵� ��Ȱ��ȭ
            _playerAttackController.enabled = false; // �÷��̾� ���� ��Ȱ��ȭ
            yield return new WaitForSeconds(duration);
            _playerMoveController.enabled = true; // �÷��̾� �̵� Ȱ��ȭ
            _playerAttackController.enabled = true; // �÷��̾� ���� Ȱ��ȭ
            _anim.SetBool("isDance", false); // �� �ִϸ��̼�
        }

        private void Death()
        {
            IsDead = true;

            // PlayerController ��ũ��Ʈ�� ��Ȱ��ȭ
            if (_playerMoveController != null)
            {
                Destroy(_playerMoveController);
            }

            // PlayerAttackController ��ũ��Ʈ�� ��Ȱ��ȭ
            if (_playerAttackController != null)
            {
                Destroy(_playerAttackController);
            }

            // ���ۺ��� ���� �ڷ�ƾ ȣ��
            StartCoroutine(SpinRapidly(5f));

            // �ݴ� �������� ���� ���� ȭ�� ������ ���ư��� ��
            Vector3 launchDirection = arrowImpactDirection + Vector3.up * _deathLaunchForceY;
            _myRigid.AddForce(launchDirection.normalized * _deathLaunchForce, ForceMode.Impulse);

            // �ڷ�ƾ�� ȣ���Ͽ� 3�� �Ŀ� ������Ʈ�� ��Ȱ��ȭ
            StartCoroutine(DisableAfterDelay(5f));
        }

        private IEnumerator SpinRapidly(float duration)
        {
            float elapsedTime = 0f;
            try
            {
                while (elapsedTime < duration)
                {
                    if (elapsedTime > 0.3 && _myRigid.transform.position.y < 0.3f) // ���� ���� ��� ����
                    {
                        yield break;
                    }
                    // x�� �������� ȸ�� �ӵ��� ������Ŵ
                    transform.Rotate(Vector3.up * _deathSpinSpeed * Time.deltaTime);
                    elapsedTime += Time.deltaTime;
                    yield return null; // ���� �����ӱ��� ���
                }
            }
            finally // �ڷ�ƾ ���� �� �׻� �״� �ִϸ��̼� ����
            {
                // �÷��̾� ��� �ִϸ��̼�
                _anim.SetTrigger("isDead");
                Debug.Log("died");
            }
        }

        private IEnumerator DisableAfterDelay(float delay)
        {
            // ���� �ð� ���� ���
            yield return new WaitForSeconds(delay - 2);

            // ī�޶� ����
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                CameraController cameraController = mainCamera.GetComponent<CameraController>();
                cameraController.enabled = false;
            }

            // Collider ��Ȱ��ȭ
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // 2�� ���� ������ ����ɰ� ��
            float sinkDuration = 2f;
            float elapsedTime = 0f;
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = initialPosition;
            targetPosition.y -= 1f; // ����ɴ� ��ǥ����

            while (elapsedTime < sinkDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / sinkDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // ���� �����ӱ��� ���
            }

            // ������Ʈ ��ü�� ��Ȱ��ȭ (�������� ��ȣ�ۿ��� ��Ȱ��ȭ)
            gameObject.SetActive(false);
        }

        [PunRPC]
        public void ImDie(int idx)
        {
            // ���߿� ���� �׾����� �Ǵ��ؼ� 1�� ������ ���� �ϴ� üũ�� ����(���� �ʿ�x)
            //_playerList.GetComponent<PlayerList>().DeathCount--;  // �׾����� ��ΰ� �˰� ��
            GameObject panel = _playerList.transform.GetChild(0).gameObject;
            GameObject player = panel.transform.GetChild(idx).gameObject;
            GameObject dieSig = player.transform.GetChild(1).gameObject;
            dieSig.SetActive(true);
        }

        // �ٸ� �ݶ��̴��� Ʈ���ſ� ������ �� ȣ��Ǵ� �޼���
        private void OnTriggerEnter(Collider other)
        {
            // AreaSlow Ʈ���ſ� ������ ���
            if (other.CompareTag("AreaSlow"))
            {
                SlowCount++;
            }

            // �뽬�� ��� ���� ó�� (�Ʒ��� �ִ� ��� ���� ����)
            if (_playerMoveController.IsDashing) 
            {
                return;
            }
            // ȭ�� ���� ���
            if (other.CompareTag("Arrows"))
            {
                // NearObject ������ Ʈ���ſ� ���� "Arrows" ������Ʈ ����
                NearObject = other.gameObject;

                // ȭ�� ���� ��� �÷��̾� ü�� ����
                PlayerHealth -= 1;
                Debug.Log("Player HP: " + PlayerHealth);

                //if (photonView.IsMine && PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerIdx", out object PlayerIdx))
                //{
                //    int idx = (int)PlayerIdx;
                //    photonView.RPC("ImDie", RpcTarget.All, idx);
                //}
                //else
                //{
                //    Debug.Log("���׾��µ�? �ƴϸ� ���� �Ƴ�");
                //}

                // ȭ���� ������ arrowImpactDirection�� ����
                arrowImpactDirection = (transform.position - other.transform.position).normalized;
            }
            // �ٸ� �÷��̾����� ���� ���
            Animator otherAnim = other.transform.root.GetComponent<Animator>();
            if (otherAnim != null && other != transform.root)
            {
                bool isMeleeEnabled = otherAnim.GetBool("isMeleeEnabled");
                if (other.CompareTag("Weapon") && isMeleeEnabled)
                {
                    if (!IsDead)
                    {
                        StartCoroutine(HitIn());
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // AreaSlow Ʈ���ſ��� ���� ���
            if (other.CompareTag("AreaSlow"))
            {
                SlowCount--;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // AreaSlow ���ǵ� ������Ʈ
            if (other.CompareTag("AreaSlow"))
            {
                UpdateSpeed();
            }
        }

        private void UpdateSpeed()
        {
            if (SlowCount > 0)
            {
                float newSpeed = _playerMoveController.PlayerDefaultSpeed - 2 * SlowCount > 0 ? _playerMoveController.PlayerDefaultSpeed - 2 * SlowCount : 0.3f;
                _playerMoveController.ChangeSpeedForDuration(newSpeed, 0.5f);
            }
        }

        private IEnumerator HitIn()
        {
            _anim.SetTrigger("isHit"); // �ǰ� �ִϸ��̼�
            _playerMoveController.enabled = false; // �÷��̾� �̵� ��Ȱ��ȭ
            _playerAttackController.enabled = false; // �÷��̾� ���� ��Ȱ��ȭ
            yield return new WaitForSeconds(1f);
            _playerMoveController.enabled = true; // �÷��̾� �̵� Ȱ��ȭ
            _playerAttackController.enabled = true; // �÷��̾� ���� Ȱ��ȭ
        }
    }
}
