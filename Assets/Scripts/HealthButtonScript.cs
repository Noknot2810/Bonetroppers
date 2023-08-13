using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ������, ���������� �� ������ ������-������ ������� � ������ ����� �� ������
public class HealthButtonScript : MonoBehaviour
{
    public Text TipField;   // ��������� ���� � ���������� � ������-������
    private Animator anim;  // ������������ ���������
    private Collider2D col; // ��������� ������

    // �������, ����������� ��� ������� � ������� �������
    public virtual void OnMouseUpAsButton()
    {
        // ��������� ������������� ������ �����������
        StopAllCoroutines();
        StartCoroutine(TypeSentence("� ��� ���� ����� �� ������"));
    }

    // ������� ������������� ������ ������ �����������
    IEnumerator TypeSentence(string sentence)
    {
        // ������� � ��������� ���������� ����
        TipField.text = "";
        TipField.enabled = true;
        // ������������ ���������� ���������� ���� � ���������
        foreach (char letter in sentence.ToCharArray())
        {
            TipField.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(4f);
        // ���������� ���������� ����
        TipField.enabled = false;
    }

    // ������� ��������� ������-������ ������� ����� �� ������
    public void ButtonOn()
    {
        anim.SetBool("appeared", true);
        col.enabled = true;
    }

    // ������� ���������� ������-������ ������� ����� �� ������
    public void ButtonOff()
    {
        anim.SetBool("appeared", false);
        col.enabled = false;
    }

    // �������, ����������� ����� ������ ������
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<Collider2D>();
    }
}
