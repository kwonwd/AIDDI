using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SourGrape.bbbb
{
    public class FirstLoading : MonoBehaviour
    {
        public GameObject mainCanvas;
        public Image targetImage;
        
        float fadeDuration = 2.0f;
        

        // Start is called before the first frame update
        void Start()
        {
            mainCanvas.SetActive(false);
            StartCoroutine(FadeIn());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator FadeIn()
        {
            // ���� ���� 0���� 1���� ������ ����
            float elapsedTime = 0.0f;
            Color imageColor = targetImage.color;
            imageColor.a = 0;
            targetImage.color = imageColor;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                imageColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                targetImage.color = imageColor;
                yield return null;
            }
            StartCoroutine(FadeOut());
        }

        // ���̵� �ƿ� (������ ������� ��)
        IEnumerator FadeOut()
        {
            // ���� ���� 1���� 0���� ������ ����
            float elapsedTime = 0.0f;
            Color imageColor = targetImage.color;
            imageColor.a = 1;
            targetImage.color = imageColor;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                imageColor.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
                targetImage.color = imageColor;
                yield return null;
            }
            yield return new WaitForSeconds(0.4f);
            mainCanvas.SetActive(true);
            this.gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
