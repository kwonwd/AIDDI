using SourGrape.hongyeop;
using System.Collections;
using UnityEngine;

namespace SourGrape.kiyoung
{
    public class Arrow : MonoBehaviour
    {

        [SerializeField]
        private float Speed;
        [SerializeField]
        private float _lifetime = 20f;
        private Rigidbody _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // �ڽ� ������Ʈ�� ArrowEffects ������Ʈ�� Boom �޼��� ȣ��
                ArrowEffects arrowEffects = GetComponentInChildren<ArrowEffects>();
                if (arrowEffects != null)
                {
                    arrowEffects.Boom(); // ���� ����Ʈ ����
                }
                gameObject.SetActive(false); // ȭ�� ����
            }
        }

        public void Shoot(Vector3 spawnPosition, Vector3 direction, float speed)
        {
            transform.position = spawnPosition;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.AddForce(direction.normalized * speed, ForceMode.Impulse);
            StartCoroutine(DisableArrowAfterTime());
        }

        private IEnumerator DisableArrowAfterTime()
        {
            yield return new WaitForSeconds(_lifetime);
            gameObject.SetActive(false);
        }
    }
}