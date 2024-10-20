using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SourGrape.kiyoung
{
    public class Screenshot : MonoBehaviour
    {
        private int screenshotCount = 0;

        void Update()
        {
            // "P" Ű�� ������ �� ��ũ������ ��� ����
            if (Input.GetKeyDown(KeyCode.P))
            {
                // ��ũ������ ������Ʈ ������ `Screenshots` ������ ����
                string filename = $"Screenshots/screenshot_{screenshotCount}.png";
                ScreenCapture.CaptureScreenshot(filename);
                screenshotCount++;
                Debug.Log($"Screenshot saved at: {filename}");
            }
        }
    }
}
