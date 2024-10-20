using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SourGrape.kiyoung;

namespace SourGrape.hongyeop
{
    public class GameEndCameraManager : MonoBehaviour
    {
        private List<GameObject> _winners; // ���ڵ�
        private Camera _camera;

        void Start()
        {
            GameManager gameManager = FindObjectOfType<GameManager>(); // ��ü ������Ʈ���� GameEndManager ã��
            if (gameManager != null)
            {
                _winners = gameManager.Winners; // game end manager���� ���ڵ� ��������
            }
            else
            {
                Debug.LogError("gameManager ã�� �� �����ϴ�.");
                return;
            }
            _camera = Camera.main; // ���� ī�޶� ��������
            FocusWinners(); // ���ڵ鿡�� ���� ���߱�
        }

        private void FocusWinners()
        {
            StartCoroutine(FocusOnWinnersCoroutine());
        }

        private IEnumerator FocusOnWinnersCoroutine()
        {
            float totalDuration = 5f; // ��ü ī�޶� �Կ� �ð�
            float durationPerWinner = totalDuration / _winners.Count; // �� ���ڴ� �Կ� �ð�

            foreach (GameObject winner in _winners)
            {
                float elapsedTime = 0f;
                Vector3 initialPosition = _camera.transform.position;
                Quaternion initialRotation = _camera.transform.rotation;

                while (elapsedTime < durationPerWinner)
                {
                    float t = elapsedTime / durationPerWinner;
                    float angle = Mathf.Lerp(-30f, 10, t); // -60������ 0������ ȸ��

                    // ī�޶� ��ġ�� ȸ�� ����
                    Vector3 offset = winner.transform.forward * 5 + Vector3.up * 3; // �÷��̾� �������� �̵�
                    _camera.transform.position = winner.transform.position + offset;
                    _camera.transform.LookAt(winner.transform.position);
                    _camera.transform.RotateAround(winner.transform.position, Vector3.up, angle);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // ���� ���ڷ� �Ѿ�� ���� ��� ���
                yield return new WaitForSeconds(0.5f);
            }

            // ��Ÿ�� ���� ��: GameManager IsModalUp = true; �� ����
            FindObjectOfType<GameManager>().IsModalUp = true;
            Debug.Log("GameManager.IsModalUp = true;");
        }
    }
}