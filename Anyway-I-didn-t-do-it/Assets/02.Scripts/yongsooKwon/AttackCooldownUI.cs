using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using SourGrape.hongyeop;

namespace SourGrape.yongsoo
{

    public class AttackCooldownUI : MonoBehaviour
    {
        private PlayerAttackController _playerAttackController;  // ���� �÷��̾��� PlayerController
        public TextMeshProUGUI _cooldownText;  // �ν����Ϳ��� �Ҵ��� TextMeshProUGUI �ʵ�
        private float cooldown = 1f;
        public Image _hideImage;

        // Start is called before the first frame update
        void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("��Ʈ��ũ ����. ����");

                PlayerAttackController[] players = FindObjectsOfType<PlayerAttackController>();
                foreach (PlayerAttackController player in players)
                {
                    Debug.Log("ã�Ҵ�.");
                    if (player.photonView.IsMine)  // �ڽ��� ���� �÷��̾����� Ȯ��
                    {
                        _playerAttackController = player;
                        Debug.Log("ã�Ҵ�. �� ����");
                        //cooldown = _playerAttackController.EquippedWeapon.AttackRate;   
                        break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_playerAttackController != null)
            {
                if (cooldown > _playerAttackController.FireDelay)
                {
                    float remainingCooldown = cooldown - _playerAttackController.FireDelay;
                    float _ratioTime = remainingCooldown / cooldown;
                    _hideImage.fillAmount = _ratioTime;
                    //_cooldownText.text = remainingCooldown.ToString("F1");  // �Ҽ��� 1�ڸ����� ǥ��
                }
                else
                {
                    _hideImage.fillAmount = 0;
                    if (_cooldownText != null)  // null üũ �߰�
                    {
                        //_cooldownText.text = "0";  // ��ٿ��� ���� ��� 0�� ǥ��
                    }
                }
            }
            else
            {
                PlayerAttackController[] players = FindObjectsOfType<PlayerAttackController>();
                foreach (PlayerAttackController player in players)
                {
                    if (player.photonView.IsMine)  // �ڽ��� ���� �÷��̾����� Ȯ��
                    {
                        _playerAttackController = player;
                        cooldown = _playerAttackController.EquippedWeapon.AttackRate;
                        break;
                    }
                }
            }
        }
    }
}
