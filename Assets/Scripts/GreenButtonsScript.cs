using UnityEngine;

// ������, ���������� �� ������ ���������� ������ ����������,
// ��������������� �� ������� ������� (��� ���������� Button)
public class GreenButtonsScript : GhostButtonsScript
{
    private int personalNum = 0; // ����� ������

    // �������, ����������� ��� ������� �� ������
    public override void OnMouseDown()
    {
        if (anim != null && is_active == true)
        {
            // ������ �������� �������
            anim.SetBool("onClick", true);
        }
    }

    // �������, ����������� ��� ������� �������
    public override void OnMouseUp()
    {
        if (anim != null && is_active == true)
        {
            // ������ �������� �������
            anim.SetBool("onClick", false);
        }
    }

    // �������, ����������� ��� ������� � ������� �������
    public override void OnMouseUpAsButton()
    {
        if (is_active == true)
        {
            // ���������� �������� ����
            foreach (GameObject obj in 
                GameObject.FindGameObjectsWithTag("MainMenuElem"))
            {
                obj.SetActive(false);
            }
            // ������ �������� � ���������� ���������
            FindObjectOfType<ExaminationScript>().
                StartGameFromCheck(personalNum);
        }
    }

    // ������� ������������� ������
    public void UnlockButton()
    {
        is_active = true;
        anim.SetBool("alternative", true);
        // ��������� ������ ������
        personalNum = int.Parse(gameObject.name.
            Substring(gameObject.name.IndexOf("#") + 1));
    }
}
