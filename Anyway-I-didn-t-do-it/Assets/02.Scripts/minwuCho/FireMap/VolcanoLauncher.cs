using SourGrape.hongyeop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SourGrape.kiyoung;

namespace SourGrape.minwu
{
    public class VolcanoLauncher : MonoBehaviourPun
    {
        [SerializeField]
        private GameObject _lavaPrefab; // ��� ������
        private float _spawnInterval = 2f; // ���� ����
        private float _minLaunchForce = 5f; // �ּ� �߻� ��
        private float _maxLaunchForce = 15f; // �ִ� �߻� ��
        private float _maxDistance = 32f;
        [SerializeField]
        private AudioClip _lavaSound; // ��� �Ҹ� ����� Ŭ��
        private AudioSource _audioSource; // ����� �ҽ� ������Ʈ
        // TODO : ȭ�� ���� ���� �� �ش� �κ��� �ݿ��ؼ� RPC�� ȭ�꿡�� �߻��ϵ��� / ������Ʈ Ǯ���� �����ؼ� �ڵ� ����
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating(nameof(SpawnLava), 0f, _spawnInterval);
            }
            _audioSource = GetComponent<AudioSource>();
        }

        private void SpawnLava()
        {
            GameObject lava = PhotonNetwork.Instantiate("Projectiles/Lava", transform.position, Quaternion.identity);

            float randomDirection = Random.Range(0f, 360f);
            float randomAngle = Random.Range(0f, 45f);

            Vector3 direction = Quaternion.Euler(randomAngle, randomDirection, 0) * Vector3.forward;

            float launchForce = Random.Range(_minLaunchForce, _maxLaunchForce);

            Rigidbody rb = lava.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction * launchForce, ForceMode.Impulse);
            }

            LavaDistanceChecker distanceChecker = lava.AddComponent<LavaDistanceChecker>();
            distanceChecker.Initialize(transform.position, _maxDistance);
            if (_audioSource != null && _lavaSound != null)
            {
                _audioSource.PlayOneShot(_lavaSound);
            }
        }
    }

    public class LavaDistanceChecker : MonoBehaviour
    {
        private Vector3 _centerPosition;
        private float _maxDistance;
        private GameManager _gameManager;  

        public void Initialize(Vector3 centerPosition, float maxDistance)
        {
            _centerPosition = centerPosition;
            _maxDistance = maxDistance;
            GameObject gameManagerObject = GameObject.Find("GameManager");

            if (gameManagerObject != null)
            {
                _gameManager = gameManagerObject.GetComponent<GameManager>();
            }
        }

        private void Update()
        {
            Vector3 currentPosition = transform.position;

            float distanceXZ = Vector3.Distance(new Vector3(currentPosition.x, 0, currentPosition.z),
                                                new Vector3(_centerPosition.x, 0, _centerPosition.z));

            if (PhotonNetwork.IsMasterClient && _gameManager._isRoundOver)
            {
                PhotonNetwork.Destroy(gameObject);
            }


            if (distanceXZ > _maxDistance)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}