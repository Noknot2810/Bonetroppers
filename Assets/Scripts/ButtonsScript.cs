using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ������, ���������� �� ������ ������, ��������������� �� ������� �������
// (��� ���������� Button)
public class ButtonsScript : MonoBehaviour
{
    // ������ ���������� ������
    protected bool is_active = false; 
    // ������ �������� ��� �������������� � ������ ������
    protected List<GameObject> interactiveObj = new List<GameObject>();
    // ��������� � ������ ������
    private Transform icon; 

    // �������, ����������� ��� ������� �� ������
    public virtual void OnMouseDown()
    {
        gameObject.GetComponent<SpriteRenderer>().color = 
            new Color(1f, 1f, 1f, 1f);
        icon.GetComponent<SpriteRenderer>().color = 
            new Color(1f, 1f, 1f, 1f);
    }

    // �������, ����������� ��� ������� �������
    public virtual void OnMouseUp()
    {
        if (is_active == false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = 
                new Color(1f, 1f, 1f, 0.6f);
            icon.GetComponent<SpriteRenderer>().color = 
                new Color(1f, 1f, 1f, 0.6f);
        }
    }

    // �������, ����������� ��� ������� � ������� �������
    public virtual void OnMouseUpAsButton()
    {
        // ��������� ������ ������������ � ���������
        switch (gameObject.name)
        {
            // ������ "������"
            case "Play":

                // ���������� ���������
                foreach (Text tfield in interactiveObj[6].GetComponentsInChildren<Text>())
                {
                    tfield.enabled = false;
                }

                if (is_active == true)
                {
                    // ���������� ������ �������� ����
                    foreach (Animator anim in interactiveObj[4].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }

                    // ���������� ������ ���� ����������
                    foreach (Animator anim in interactiveObj[5].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    is_active = false;
                }
                else
                {
                    // ���������� ������ "���������",
                    // ��������� � ��� ������ �����������
                    foreach (Animator anim in interactiveObj[0].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    interactiveObj[1].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[1].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // ���������� ������ "� ���",
                    // ��������� � ��� �������� ���������� � �����������
                    foreach (SpriteRenderer rend in interactiveObj[2].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    interactiveObj[2].GetComponentInChildren<Text>().
                        enabled = false;
                    interactiveObj[2].GetComponentInChildren<CircleCollider2D>().
                        enabled = false;
                    interactiveObj[3].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[3].GetComponent<ButtonsScript>().
                        OnMouseUp();             

                    // ��������� ������ �������� ����
                    foreach (Animator anim in interactiveObj[4].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", true);
                    }
                    is_active = true;
                }
                break;
            // ������ "���������"
            case "Settings":
                if (is_active == true)
                {
                    // ���������� ������ ���� ��������
                    foreach (Animator anim in interactiveObj[5].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    is_active = false;
                }
                else
                {
                    // ���������� ������ �������� ����
                    foreach (Animator anim in interactiveObj[0].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // ���������� ������ ���� ����������
                    foreach (Animator anim in interactiveObj[1].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // ���������� ������ "������"
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // ���������� ������ "� ���",
                    // ��������� � ��� �������� ���������� � �����������
                    foreach (SpriteRenderer rend in interactiveObj[3].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    interactiveObj[3].GetComponentInChildren<Text>().
                        enabled = false;
                    interactiveObj[3].GetComponentInChildren<CircleCollider2D>().
                        enabled = false;
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // ���������� ���������
                    foreach (Text tfield in interactiveObj[6].GetComponentsInChildren<Text>())
                    {
                        tfield.enabled = false;
                    }

                    // ��������� ������ ���� ��������
                    foreach (Animator anim in interactiveObj[5].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", true);
                    }
                    is_active = true;
                }
                break;
            // ������ "���������"
            case "Volume":
                if (is_active == true)
                {
                    // ����������� ������� ���������
                    interactiveObj[0].SetActive(false);
                    is_active = false;
                }
                else
                {
                    // ���������� ������� ���������
                    interactiveObj[0].SetActive(true);
                    is_active = true;
                }
                break;
            // ������ "� ���"
            case "AboutUs":
                if (is_active == true)
                {
                    // ���������� � ����������� ��������� � ������� ��������
                    foreach (SpriteRenderer rend in interactiveObj[5].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    interactiveObj[5].GetComponentInChildren<Text>().
                        enabled = false;
                    interactiveObj[5].GetComponentInChildren<CircleCollider2D>().
                        enabled = false;
                    is_active = false;
                }
                else
                {
                    // ���������� ������ �������� ����
                    foreach (Animator anim in interactiveObj[0].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // ���������� ������ ���� ����������
                    foreach (Animator anim in interactiveObj[1].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // ���������� ������ "������"
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // ���������� ������ ���� ��������
                    foreach (Animator anim in interactiveObj[3].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // ���������� ������ ���������
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // ���������� ���������
                    foreach (Text tfield in interactiveObj[6].GetComponentsInChildren<Text>())
                    {
                        tfield.enabled = false;
                    }

                    // ��������� � ���������� ��������� � ������� ��������
                    foreach (SpriteRenderer rend in interactiveObj[5].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = true;
                    }
                    interactiveObj[5].GetComponentInChildren<Text>().
                        enabled = true;
                    interactiveObj[5].GetComponentInChildren<CircleCollider2D>().
                        enabled = true;
                    interactiveObj[5].GetComponent<DialogueManager>().
                        StartDialogue((Resources.Load("Credits") as Dialogue).
                        text.Split('#').ToList<string>());
                    is_active = true;
                }
                break;
            // ������ "�������� ���������"
            case "GetTips":
                // ���������� ������ ���� ����������
                foreach (Animator anim in interactiveObj[0].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", false);
                }

                // ���������� ������ ���� ��������
                foreach (Animator anim in interactiveObj[1].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", false);
                }
                // ���������� ������ ���������
                interactiveObj[2].GetComponent<ButtonsScript>().
                    is_active = false;
                interactiveObj[2].GetComponent<ButtonsScript>().
                    OnMouseUp();

                // ���������� ������ "� ���",
                // ��������� � ��� �������� ���������� � �����������
                foreach (SpriteRenderer rend in interactiveObj[3].
                    GetComponentsInChildren<SpriteRenderer>())
                {
                    rend.enabled = false;
                }
                interactiveObj[3].GetComponentInChildren<Text>().
                    enabled = false;
                interactiveObj[3].GetComponentInChildren<CircleCollider2D>().
                    enabled = false;
                interactiveObj[4].GetComponent<ButtonsScript>().
                    is_active = false;
                interactiveObj[4].GetComponent<ButtonsScript>().
                    OnMouseUp();

                // ��������� ������ �������� ����
                foreach (Animator anim in interactiveObj[5].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", true);
                }
                // ���������� ������ "������"
                interactiveObj[6].GetComponent<ButtonsScript>().
                    is_active = true;
                interactiveObj[6].GetComponent<ButtonsScript>().
                    OnMouseDown();

                foreach (Text tfield in interactiveObj[7].GetComponentsInChildren<Text>())
                {
                    tfield.enabled = true;
                }
                break;
            // ������ "��������� ����"
            case "NextCredit":
                interactiveObj[0].GetComponent<DialogueManager>().
                    DisplayNextSentence();
                break;
            // ������ "���������� �����"
            case "Restart":
                SceneManager.LoadScene(SceneManager.
                    GetActiveScene().buildIndex);
                break;
        }
    }

    // �������, ����������� ����� ������ ������
    protected virtual void Start()
    {
        // ��������� ���������� Transform � ������ ������
        icon = gameObject.transform.Find("Icon");
        // ��������� ������ ������������ � ���������
        switch (gameObject.name)
        {
            // ������ "������"
            case "Play":
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Settings"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("AboutUs"));
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // ������ "���������"
            case "Settings":
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("Play"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("AboutUs"));
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // ������ "���������"
            case "Volume":
                interactiveObj.Add(GameObject.Find("VolumeSetting"));
                interactiveObj[0].SetActive(false);
                break;
            // ������ "� ���"
            case "AboutUs":
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("Play"));
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Settings"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // ������ "�������� ���������"
            case "GetTips":
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Settings"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("AboutUs"));
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("Play"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // ������ "��������� ����"
            case "NextCredit":
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                break;
        }
        
    }
}
