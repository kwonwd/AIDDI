using Photon.Realtime;
using UnityEngine;

namespace SourGrape.kiyoung
{
    public class MapBorderMaterialSwitcher : MonoBehaviour
    {
        public Material DefaultMaterial;
        public Material CloseMaterial;
        public Transform Player;
        public float DistanceThreshold = 25f;

        private Renderer _pipeRenderer;
        private Material _currentMaterial;
        private float _lerpValue = 0;
        private float _adjustedThreshold;

        private void Start()
        {
            _pipeRenderer = GetComponent<Renderer>();
            _currentMaterial = new Material(DefaultMaterial);
            _pipeRenderer.material = _currentMaterial;

            _adjustedThreshold = DistanceThreshold * 0.95f; // 5% ������ �Ÿ�
        }

        private void Update()
        {
            if (!Player) { return; }
            float distance = Vector3.Distance(Player.position, transform.position);

            float closeThreshold = _adjustedThreshold * 0.95f;
            float farThreshold = _adjustedThreshold;

            // �Ÿ� ��� ��Ƽ���� ��ȯ �� ���
            float targetLerpValue = (distance <= closeThreshold) ? 1 : (distance > farThreshold ? 0 : _lerpValue);
            _lerpValue = Mathf.Lerp(_lerpValue, targetLerpValue, 3 * Time.deltaTime);

            // ������ ���� �����Ͽ� ���� �� ����
            Color lerpedColor = Color.Lerp(CloseMaterial.color, DefaultMaterial.color, _lerpValue);
            _currentMaterial.color = lerpedColor;
        }

        public void SetPlayer(Transform playerTransform)
        {
            Player = playerTransform;  // �÷��̾��� Transform�� ����
        }
    }
}