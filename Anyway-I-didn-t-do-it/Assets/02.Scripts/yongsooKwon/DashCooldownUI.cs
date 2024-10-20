using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using SourGrape.hongyeop;


namespace SourGrape.Yongsoo
{
    public class DashCooldownUI : MonoBehaviour
    {
        private PlayerController _playerMoveController;  // ���� �÷��̾��� PlayerController
        public TextMeshProUGUI _cooldownText;  // ���� ��Ÿ���� ǥ���� TextMeshPro
        private float cooldown = 3f;
        public Image _hideImage;
        private float _ratioTime;


        // Start is called before the first frame update
        void Start()
        {
            // ��Ʈ��ũ ����� ��� ���� �÷��̾ ã�Ƽ� UI�� ����
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("��Ʈ��ũ ����. ���");
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                foreach (PlayerController player in players)
                {
                    Debug.Log("ã�Ҵ�. ���");
                    if (player.photonView.IsMine)  // �ڽ��� ���� �÷��̾����� Ȯ��
                    {
                        _playerMoveController = player;
                        Debug.Log("ã�Ҵ�. �� ���");
                        //cooldown = player.GetDashCooldown();
                        break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_playerMoveController != null)
            {
                //���� ��� ��Ÿ�� ǥ��
                float _currentCooldown = _playerMoveController.GetCurrentDashCooldown();
                _ratioTime = _currentCooldown / cooldown;
                _hideImage.fillAmount = _ratioTime;
                //Debug.Log(_ratioTime);
                //_cooldownText.text = $"Cooldown: {_currentCooldown:F1}s";
            }
            else
            {
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                foreach (PlayerController player in players)
                {
                    //Debug.Log("ã�Ҵ�. ���");
                    if (player.photonView.IsMine)  // �ڽ��� ���� �÷��̾����� Ȯ��
                    {
                        _playerMoveController = player;
                        //Debug.Log("ã�Ҵ�. �� ���");
                        //cooldown = player.GetDashCooldown();
                        break;
                    }
                }
            }
        }
    }
}
