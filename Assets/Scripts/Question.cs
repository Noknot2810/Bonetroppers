using UnityEngine;

// ������ ��� �������� ������� ��������
[System.Serializable]
[ExecuteInEditMode]
public class Question
{
    [TextArea(3, 10)]
    public string question;                 // ����� �������
    public string[] options = new string[4];// ������ ��������� ������
    public int correctIs;                   // ����� ����������� ������
}
