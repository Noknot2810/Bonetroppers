using UnityEngine;

// ������, ���������� �� �������� �� ����� ����� ��������, �������
// ����������� � ����������� �������, � �������� ������ ������ ��������.
public class BarrierScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
