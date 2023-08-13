using UnityEngine;
using UnityEngine.UI;

// ������, ���������� �� ������ �������� ������� (�� ������� ������ CloudNote)
public class CloudNoteScript : MonoBehaviour
{
    // �������� ����� ������ ���������� ������� � ������� ��������
    public float FirstDelay = 6f;
    // ����������� �������� ��������� �������
    public float RegularDelay = 3f;
    // �������� ��������� ������� ��� ���������� ��������
    public float ProblemDelay = 1.5f;
    // ��������� ��������� �������
    public Text NoteField;
    // ������� �����-��������
    public AudioManager MusicBox;

    // �������� �������
    private Animator anim;

    // �������, ����������� ����� ������ ������
    private void Start()
    {
        // ��������� ���������� ���������� �������
        NoteField = gameObject.transform.Find("Text").GetComponent<Text>();
        // ��������� ��������� �������
        anim = gameObject.GetComponent<Animator>();
        // ��������� �������� �����-���������
    }

    // ������� ��������� ������� � ��������� �����������
    public void TurnCloudNote(string text, 
                              bool firstDelay = false, 
                              bool isProblem = false, 
                              bool withoutSound = false)
    {
        // ������������ ������ �������
        NoteField.text = text;
        // ����� �������� ��������� �������
        anim.SetBool("appeared", true);

        // � ����������� �� ������� ���������� ���������
        // ��������� ������� � ����� ������� ������� ����� ��������
        if (withoutSound == true)
        {
            Invoke(nameof(OffCloudNote), RegularDelay);
        }
        else if (isProblem == true)
        {
            MusicBox.PlayTransit("Problem sound");
            Invoke(nameof(OffCloudNote), ProblemDelay);
        }
        else if (firstDelay == true)
        {
            MusicBox.PlayTransit("Appearance sound");
            Invoke(nameof(OffCloudNote), FirstDelay);
        }
        else
        {
            MusicBox.PlayTransit("Appearance sound");
            Invoke(nameof(OffCloudNote), RegularDelay);
        }
    }

    // ������� ������� �������
    public void OffCloudNote()
    {
        // ����� �������� ������� �������
        anim.SetBool("appeared", false);
    }
}
