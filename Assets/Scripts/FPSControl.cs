using UnityEngine;

// ������, ��������������� � ���� ������������� ���������� ������ � �������
public class FPSControl : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
