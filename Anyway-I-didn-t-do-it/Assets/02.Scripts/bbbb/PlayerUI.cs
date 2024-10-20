using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using ExitGames.Client;


namespace SourGrape.bbbb
{
    public class PlayerUI : MonoBehaviourPun
    {
        public TMPro.TextMeshProUGUI pingText;
        public TMPro.TextMeshProUGUI timeText;
        private int leftTime = 120;
        private bool isUpdatingPing = false;  // �ڷ�ƾ�� ���� ������ Ȯ���ϴ� �÷���

        // Start is called before the first frame update
        void Start()
        {
            // ���� �� �ٷ� pingUpdate �ڷ�ƾ�� ����
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            StartCoroutine(pingUpdate());
        }

        // ping ������ 1�ʸ��� �����ϴ� �ڷ�ƾ
        private IEnumerator pingUpdate()
        {
            isUpdatingPing = true;
            while (PhotonNetwork.IsConnected) // ������ �����Ǵ� ���� �� ������ ����
            {
                pingText.text = $"Ping: {PhotonNetwork.GetPing()} ms";
                yield return new WaitForSeconds(1f);  // 1�ʸ��� ������Ʈ
            }
            isUpdatingPing = false; // ������ ����� �ڷ�ƾ ����
        }

        private IEnumerator TimeUpdater()
        {
            while (true)
            {
                timeText.text = $"{leftTime/60}:{leftTime%60}";
                yield return new WaitForSeconds(1f);
                leftTime--;
            }
        }

        [PunRPC]
        void TimerStart()
        {
            StartCoroutine(TimeUpdater());
        }

    }
}
