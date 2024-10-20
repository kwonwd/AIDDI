using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SourGrape.kiyoung
{
    using UnityEngine;
    using UnityEngine.UI;

    public class GuideModal : MonoBehaviour
    {
        public GameObject guideModal;  // ��� ��ü �г�
        public Button helpButton;      // ? ��ư
        public Button closeButton;     // ��� �ݱ� ��ư

        void Start()
        {
            // �ʱ� ���¿��� ��� ��Ȱ��ȭ
            guideModal.SetActive(false);

            // ��ư Ŭ�� �̺�Ʈ ���
            helpButton.onClick.AddListener(OpenGuideModal);
            closeButton.onClick.AddListener(CloseGuideModal);
        }

        // ��� â ����
        void OpenGuideModal()
        {
            guideModal.SetActive(true);
        }

        // ��� â �ݱ�
        void CloseGuideModal()
        {
            guideModal.SetActive(false);
        }
    }
}
