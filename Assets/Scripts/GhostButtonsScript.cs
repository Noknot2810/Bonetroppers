using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// ������, ���������� �� ������ ���������� ������, ���������������
// �� ������� ������� (��� ���������� Button)
public class GhostButtonsScript : ButtonsScript
{
    // �������� ���������� ������
    protected Animator anim;

    // �������, ����������� ��� ������� �� ������
    public override void OnMouseDown()
    {
        // ������ �������� �������
        if (anim != null)
        {
            anim.SetBool("onClick", true);
        }
    }

    // �������, ����������� ��� ������� �������
    public override void OnMouseUp()
    {
        // ������ �������� �������
        if (anim != null)
        {
            anim.SetBool("onClick", false);
        }
    }

    // �������, ����������� ��� ������� � ������� �������
    public override void OnMouseUpAsButton()
    {
        // ��������� ������ ������������ � ���������
        switch (gameObject.name)
        {
            // ������ "������� �����"
            case "Hardmode":
                // ������������� �������, ��� ���� ���� ��������
                // �� ������� ������
                if (PlayerPrefs.GetInt("regularCompleted", 0) != 0)
                {
                    // ���������� �������� ����
                    foreach (GameObject obj in 
                        GameObject.FindGameObjectsWithTag("MainMenuElem"))
                    {
                        obj.SetActive(false);
                    }
                    // ����� �������� ������� � ������ ����
                    interactiveObj[0].GetComponent<CloudNoteScript>().
                        TurnCloudNote("��������� ����� ��� �������", 
                                      firstDelay: true);
                    // ������ �������� �� ������� ������
                    FindObjectOfType<ExaminationScript>().StartHardGame();
                }
                else
                {
                    // ����� �������� ������� � ������������� ������ ����
                    interactiveObj[0].GetComponent<CloudNoteScript>().
                        TurnCloudNote("���� �� ����� ��������� ����� " +
                                      "������� ����������� �����", 
                                      isProblem: true);
                }
                break;
            // ������ "������" ��� �������� ������
            case "Begin":
                // ���������� �������� ����
                foreach (GameObject obj in 
                    GameObject.FindGameObjectsWithTag("MainMenuElem"))
                {
                    obj.SetActive(false);
                }
                // ����� �������� ������� � ������ ����
                interactiveObj[0].GetComponent<CloudNoteScript>().
                    TurnCloudNote("��������� ����� ��� �������",
                                  firstDelay: true);
                // ������ �������� c ������ ������
                FindObjectOfType<ExaminationScript>().StartGameFromBeg();
                break;
            // ������ ������ ����������
            case "Checkpoint":
                // ��������� ������ ����������
                var arr = interactiveObj[0].
                    GetComponentsInChildren<GreenButtonsScript>().
                    OrderBy(scr => scr.transform.name).ToArray();
                // ��������� ���������� ������������ ���������
                int lastAllowed = PlayerPrefs.GetInt("bestSection", 0);
                // ������������� ���� ��������� ����������
                for (int i = 0; i <= lastAllowed; i++)
                {
                    arr[i].UnlockButton();
                }

                // ����� ���� ����������
                foreach (Animator anim in interactiveObj[0].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", true);
                }

                // �������� �������� ����
                foreach (Animator anim in interactiveObj[1].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", false);
                }

                // �������� ���������
                foreach (Text tfield in interactiveObj[2].GetComponentsInChildren<Text>())
                {
                    tfield.enabled = false;
                }
                break;
            // ������ ���������� ������� ����
            case "CloudB":
                // ���������� ���� ������ �����
                foreach (SpriteRenderer rend in interactiveObj[0].
                    GetComponentsInChildren<SpriteRenderer>())
                {
                    rend.enabled = false;
                }
                // ���������� ���������������� ���������
                PlayerPrefs.SetString("userBack", "Cloud");
                break;
            // ������ ��������� ������� ����
            case "SunsetB":
                // ������������� �������, ��� ���� ���� ��������
                // �� ������� ������
                if (PlayerPrefs.GetInt("regularCompleted", 0) != 0)
                {
                    // ���������� ���� ������ �����
                    foreach (SpriteRenderer rend in interactiveObj[0].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    // ��������� ������� ���� ������
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                    // ���������� ���������������� ���������
                    PlayerPrefs.SetString("userBack", "Sunset");
                }
                else
                {
                    // ����� �������� �������
                    interactiveObj[2].GetComponent<CloudNoteScript>().
                        TurnCloudNote("������ ��� ��������� ����� " +
                                      "������� ����������� �����", 
                                      isProblem: true);
                }
                break;
            // ������ ������� ������� ����
            case "NightB":
                // ������������� �������, ��� ���� ���� ��������
                // �� ������� ������
                if (PlayerPrefs.GetInt("hardmodeCompleted", 0) != 0)
                {
                    // ���������� ���� ������ �����
                    foreach (SpriteRenderer rend in interactiveObj[0].GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    // ��������� ������� ���� ����
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                    // ���������� ���������������� ���������
                    PlayerPrefs.SetString("userBack", "Night");
                }
                else
                {
                    // ����� �������� �������
                    interactiveObj[2].GetComponent<CloudNoteScript>().
                        TurnCloudNote("������ ��� ��������� ����� ������� " +
                                      "����������� ����� �� ������� ������", 
                                      isProblem: true);
                }
                break;
        }
    }

    // �������, ����������� ����� ������ ������
    protected override void Start()
    {
        // ��������� ��������� ���������� ������
        anim = GetComponent<Animator>();
        // ��������� ������ ������������ � ���������
        switch (gameObject.name)
        {
            // ������ "������� �����"
            case "Checkpoint":
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // ������ ���������� ������� ����
            case "CloudB":
                interactiveObj.Add(GameObject.Find("Backgrounds"));
                break;
            // ������ ��������� ������� ����
            case "SunsetB":
                interactiveObj.Add(GameObject.Find("Backgrounds"));
                interactiveObj.Add(GameObject.Find("SunsetBackground"));
                interactiveObj.Add(GameObject.Find("CloudNote"));
                if (PlayerPrefs.GetString("userBack", "") == "Sunset")
                {
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                }
                break;
            // ������ ������� ������� ����
            case "NightB":
                interactiveObj.Add(GameObject.Find("Backgrounds"));
                interactiveObj.Add(GameObject.Find("NightBackground"));
                interactiveObj.Add(GameObject.Find("CloudNote"));
                if (PlayerPrefs.GetString("userBack", "") == "Night")
                {
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                }
                break;
            default:
                interactiveObj.Add(GameObject.Find("CloudNote"));
                break;
        }

    }
}
