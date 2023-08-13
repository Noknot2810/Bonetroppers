using UnityEngine;

// ������, ���������� �� ��������� ����� ���������
public class BlueShiningScript : MonoBehaviour
{
    public Material mat;                                    // ��������
                                                            // ��������
    public float Duration = 2f;                             // ������ ��������
    private float timer = 0f;                               // ������
    private bool switched = false;                          // ����������-����
                                                            // ���������
                                                            // ��������
    private Color col1 = new Color(64f, 336f, 2048f, 0f);   // ������ ����
    private Color col2 = new Color(4f, 21f, 128f, 0f);      // ������ ����
    private Color colorDiff;                                // �������� �������


    // �������, ����������� ����� ������ ������
    void Start()
    {
        colorDiff = col1 - col2;
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    private void FixedUpdate()
    {
        // ���������� �������
        timer -= Time.deltaTime;

        // ��������� ����������� ���������, ����� ������� �� ��������� �������
        if (timer <= 0)
        {
            switched = !switched;
            timer = Duration;
        }

        // ��������� ����� ��������
        float diffTime = timer / Duration;
        if (switched == true)
        {
            mat.SetColor("_Color", col2 + (colorDiff * diffTime));
        }
        else
        {
            mat.SetColor("_Color", col1 - (colorDiff * diffTime));
        }
    }
}
