using UnityEngine;

// ������, ���������� �� �������� ������� � ��������� ���������� ���������
public class StaffFreezer : MonoBehaviour
{
    // ���������� ��������� ��� ��������
    public Rigidbody2D AttachedRigidbody;
    // ���� ������������� ������������� ������������� ��������� �� ��� �
    public bool MakeCorrection = false;
    // �������� ������������� ��������� �� ��� �
    public float CorrectionValue = 0.04f;

    // �������, ���������� ��� ������������ ���������� ������� �������
    // � ����������� ������� �������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (AttachedRigidbody != null)
        {
            // ���������� ���������� ��� ������ �������� �������
            // � ��������� ����������
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = AttachedRigidbody;

            // ���������� ���� ����������� �������� �������
            foreach (Collider2D col in gameObject.GetComponents<Collider2D>())
            {
                col.enabled = false;
            }

            // ������������� ������������� �� ��� � ��� �������������
            if (MakeCorrection == true)
            {
                transform.position += new Vector3(0f, CorrectionValue, 0f);
            }
        }
    }
}
