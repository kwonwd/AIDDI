using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SourGrape.hongyeop
{
    public class PlayerGhostStatusController : MonoBehaviour
    {
        #region public properties
        public bool IsDead = false; // ���� ����
        #endregion

        #region private properties
        private Animator _anim;
        #endregion

        void Start()
        {
            _anim = GetComponent<Animator>();
            _anim.applyRootMotion = false;  // Root Motion�� ��Ȱ��ȭ�Ͽ� �ִϸ��̼��� ȸ���� ������ ���� �ʵ��� ��
            GameObject summonParticle = PhotonNetwork.Instantiate("Effect/GrenadeExplosionBlue", transform.position, transform.rotation);
            StartCoroutine(DestroyAfterDelay(summonParticle, 2f));
        }

        void Update()
        {
            if (IsDead)
            {
                Death();
            }
        }

        private void Death()
        {
            IsDead = false;
            StartCoroutine(Dying());
        }

        private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            PhotonNetwork.Destroy(obj);
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
    }
}
