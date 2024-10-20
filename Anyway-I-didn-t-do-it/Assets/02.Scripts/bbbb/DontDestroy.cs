using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace SourGrape.bbbb{
    public class DontDestroy : MonoBehaviourPunCallbacks
    {
        private static DontDestroy instance;

        void Awake()
        {
            // �̹� �����ϴ� �ν��Ͻ��� �ִ��� Ȯ��
            if (instance == null)
            {
                // ù ������ �ν��Ͻ��̹Ƿ�, �ν��Ͻ��� �Ҵ��ϰ� �ı����� �ʵ��� ����
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // �̹� �ν��Ͻ��� �ִٸ�, �ߺ��� ��ü�� �ı�
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            StartCoroutine(LogoutRequest());
        }

        private IEnumerator LogoutRequest()
        {
            // �α׾ƿ� ��û ����
            UnityWebRequest request = new UnityWebRequest(Parameters.URL + "api/user/logout", "POST");
            request.SetRequestHeader("Authorization", $"Bearer {PlayerPrefs.GetString("JWT")}");
            request.downloadHandler = new DownloadHandlerBuffer();

            // ��û ������
            yield return request.SendWebRequest();

            // ���� Ȯ��
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Logout successful.");
            }
            else
            {
                Debug.LogError($"Logout failed: {request.error}");
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            GameObject _obj = Instantiate(Resources.Load<GameObject>(Parameters.ErrorPanel));
            _obj.GetComponent<bbbb.ErrorPanel>().message = Parameters.MsgDisconnect;
            StartCoroutine(BackToMain());
        }

        private IEnumerator BackToMain()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("MainScene_bbbb");
        }
    }
}
