using System.Collections.Generic;
using UnityEngine;

// ������, ���������� �� �������� ������� ����� (Shrine)
public class MoveShrine : MonoBehaviour
{
    // ����������� �������� �������� ������� �����
    public float SpeedTemp = 5f;
    // ������������� ������
    public Transform TrackingObject;
    // ������ ������������ ���������
    public List<Transform> StopTriggers;

    // ���������� ��������� ������� �����
    private Rigidbody2D rigit;
    // ������� �������� �������� ������� �����
    private float speed;
    // ������� ��������� �� ��� � �������������� �������
    private float trackingPos;

    // �������, ����������� ����� ������ ������
    void Start()
    {
        // ��������� ����������� ���������� ������� �����
        rigit = GetComponent<Rigidbody2D>();
        
        // ��������� ��������� ��������
        speed = 0;

        // �������� ����� �� ������ �� ��� �
        // ��� ��������� �������� � ������� ���������
        Vector3 cur = transform.position;
        cur.x += 100;
        transform.position = cur;

        // ��������� ��������� Idle ��� ��������-�������� � ����� ����
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Officer"))
        {
            obj.GetComponent<Animator>().SetBool("IsWalking", false);
        }
    }

    // �������, ����������� � ������ �����
    void Update()
    {
        // ���������� ��������� �� ��� � �������������� �������
        trackingPos = TrackingObject.position.x;

        // �������� ���������
        for (int i = 0; i < StopTriggers.Count; i++)
        {
            // ������������� �������, ����� ������������� ������ ��������
            // �� ��� � �� ������ ������������� ��������
            // ��� ������ ����� ������
            if (trackingPos >= StopTriggers[i].position.x)
            {
                // ��������� ������� �������� �������� ������� �����
                speed = 0;

                // ��������� �������� ������������� ��� ���������
                switch (StopTriggers[i].name)
                {
                    // ������� ��������� ������ �����
                    case "StopTriggerInside":
                        // ����� ������� ������ �������������������� �������
                        FindObjectOfType<ExaminationScript>().StartGame();
                        // �������� ���������� � ��������
                        // ����� ����� � ���� ��������
                        foreach (GameObject obj in GameObject.
                            FindGameObjectsWithTag("Finish"))
                        {
                            Destroy(obj);
                        }
                        break;
                    // ������� ��������� ������� �����
                    case "StopTriggerOutside":
                        // ����� ������� ��������� ������� ��������
                        FindObjectOfType<ExaminationScript>().HappyEnd();
                        // ������� ������ ������������ ���������
                        StopTriggers.Clear();

                        // ��������� ��������� LongFun ��� ��������-��������
                        // � ����� ����
                        foreach (GameObject obj in GameObject.
                            FindGameObjectsWithTag("Officer"))
                        {
                            obj.GetComponent<Animator>().
                                SetTrigger("LongFunTrigger");
                        }
                        return;
                }

                // �������� ������������ �������� �� ������
                StopTriggers.Remove(StopTriggers[i]);
                i--;
            }
        }
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    void FixedUpdate()
    {
        // ��������� ������ �������� ������� �����
        rigit.velocity = new Vector2(-speed, 0);
    }

    // ������� ������� �������� ������� �����
    public void Restart()
    {
        // ������������� ��������
        speed = SpeedTemp;
    }

    // ������� �������� ������� ������������� ��������
    public void RemoveFirstTrig()
    {
        // �������� ������� ��������
        StopTriggers.Remove(StopTriggers[0]);
        // �������� ���������� � �������� ����� ����� � ���� ��������
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Finish"))
        {
            Destroy(obj);
        }
    }
}
