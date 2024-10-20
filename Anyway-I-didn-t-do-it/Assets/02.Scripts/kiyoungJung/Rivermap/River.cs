using SourGrape.hongyeop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SourGrape.kiyoung
{
    public class River : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // �浹�� ������Ʈ�� Player�� ���
            if (other.CompareTag("Player"))
            {
                PlayerStatusController playerStatus = other.GetComponent<PlayerStatusController>();

                if (playerStatus != null && !playerStatus.IsDead)
                {
                    // ü�� ����
                    playerStatus.PlayerHealth -= 1;
                }
            }
        }
    }
}
