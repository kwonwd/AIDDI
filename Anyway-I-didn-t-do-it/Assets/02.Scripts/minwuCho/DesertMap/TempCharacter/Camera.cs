using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ���� ���, ���� ĳ������ Transform
    public Vector3 offset = new Vector3(0, 5, -10); // ī�޶�� ĳ���� ���� �⺻ �Ÿ�
    public float smoothSpeed = 0.125f; // ī�޶� �̵��� �� �ε巯�� ����

    void LateUpdate()
    {
        // ��ǥ ��ġ ���: ��� ��ġ + ������
        Vector3 desiredPosition = target.position + offset;

        // �ε巴�� ī�޶� ��ġ ����
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ī�޶� ��ġ�� ������Ʈ
        transform.position = smoothedPosition;
    }
}