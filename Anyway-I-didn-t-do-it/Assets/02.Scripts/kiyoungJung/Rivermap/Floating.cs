using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float floatStrength = 0.5f;  // Y������ ���ٴϴ� ����
    public float floatSpeed = 2f;       // ���ٴϴ� �ӵ�
    private float _startY;

    void Start()
    {
        _startY = transform.position.y;  // �ʱ� Y ��ġ ����
    }

    void Update()
    {
        // �����ĸ� ����� �ε巴�� ���Ϸ� ������
        transform.position = new Vector3(transform.position.x, _startY + Mathf.Sin(Time.time * floatSpeed) * floatStrength, transform.position.z);
    }
}