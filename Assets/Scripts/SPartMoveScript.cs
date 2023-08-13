using UnityEngine;

// ������, ���������� �� �������� ����� ���� �������
public class SPartMoveScript : MonoBehaviour
{
    public bool IsActive = false;   // ���� ��������� ����� ����� ����
    private Rigidbody2D rgd;        // ���������� ��������� ����� ����
    private Vector2 direction;      // ����������� �������� ����� ����
    private Vector2 speed;          // �������� �������� ����� ����
    private float timer = 0.2f;     // ������ �������� ����� ����

    // �������, ����������� ����� ������ ������
    void Start()
    {
        // ��������� ����������� ���������� ����� ����
        rgd = gameObject.GetComponent<Rigidbody2D>();
        // ��������� ����� ����������� �������� ����� ����
        direction = new Vector2(Random.Range(-1, 1), 1);
        // ��������� ����� �������� �������� ����� ����
        speed = new Vector2(Random.Range(5, 10), Random.Range(10, 40));
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    private void FixedUpdate()
    {
        // ������� ��������� �����
        if (IsActive == true)
        {
            // ��������� ������ �������� ������� ����� ����
            rgd.velocity = new Vector2(direction.x * speed.x, direction.y * speed.y);
            // ����� ������� �������� ������� �����
            TimedFlying(Time.deltaTime);
        }
    }

    // ������� ������� ������� ����� ����� ���� �
    // ������������� ���������� �����
    void TimedFlying(float delta)
    {
        // ������ �������
        timer -= delta;
        // ���������� ����� ����� �� ���������� �������
        if (timer <= 0)
        {
            IsActive = false;
        }
    }
}
