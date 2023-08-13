using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������, ���������� �� �������� � �������� ������� �������-������ (Player)
public class PlayerMovement : MonoBehaviour
{
    // ������ ������� ������ ���������
    public GameObject TalkCloudTemplate;

    // �������� �������-������
    private Animator anim;
    // ������ �������� ��� ������ ���� �������
    private List<SPartMoveScript> s_parts;
    // ������ ������ ��������� �������-������
    private GameObject talkCloud;

    // �������, ����������� ����� ������ ������
    void Start()
    {
        // ���������� ������ �������� ��� ������ ���� �������
        s_parts = new List<SPartMoveScript>
        {
            gameObject.transform.Find("Player_skull").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_body").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_lhand").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_rhand").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_lleg").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_rleg").GetComponent<SPartMoveScript>()
        };
        // ��������� ���������� ��������� �������-������
        anim = gameObject.GetComponent<Animator>();
    }

    // ������� ������ ������� �������-������
    public void Explode()
    {
        // ����� �������� ������
        anim.SetTrigger("ExplodeTrigger");
        // ����� ��������� ������ ���������
        anim.SetBool("StopMashine", true);
        // ���������� ������ �� ����������� �������-������
        GetComponent<BoxCollider2D>().enabled = false;
        // ��������� ������ ������ ���� �������-������
        for (int i = 0; i < s_parts.Count; i++)
        {
            s_parts[i].IsActive = true;
        }
    }

    // ������� ���������������� �������-������ �� ��� �
    public void TeleportForward(float distance)
    {
        // ��������� ����� ������� �������-������
        Vector3 tpos = transform.position;
        tpos.x += distance;
        transform.position = tpos;
        // ���������� �������� ������
        anim.SetBool("IsWalking", false);
    }

    // ������� ��������� �������� ���������� �������
    public void Celebrate()
    {
        anim.SetTrigger("FunTrigger");
    }

    // ������� ��������� �������� ������
    public void StartWalk()
    {
        anim.SetBool("IsWalking", true);
    }

    // ������� ��������� ���� �������� Idle
    public void ChangeIdle()
    {
        anim.SetBool("IsRespect", !anim.GetBool("IsRespect"));
    }

    // �������, ���������� ��� ������������ ������� ������-������� � ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������� �������� ������
        anim.SetBool("IsWalking", false);
    }

    // ������� �������� � �������� ������ ���������
    public IEnumerator SoldierTalk(string text)
    {
        // ����� ��������
        yield return new WaitForSeconds(1f);
        // ���������� ����� ������� ������ ���������
        Vector3 newPos = transform.position;
        newPos.x -= 2f;
        newPos.y += 2.3f;
        // �������� ���������� ������� ������ ���������
        talkCloud = Instantiate(TalkCloudTemplate, newPos, 
            Quaternion.identity, GameObject.Find("MainCanvas").transform);

        // ������� ���������� ������ ������ ������ ���������
        talkCloud.GetComponentInChildren<Text>().text = text;

        // �������� ������ ��������� � ��� �� �����������,
        // � ������� �������� ������ �������-������
        FixedJoint2D joint = talkCloud.AddComponent<FixedJoint2D>();
        joint.connectedBody = gameObject.GetComponent<StaffFreezer>().
            AttachedRigidbody;
    }

    // ������� ���������� ��� �������� ������� ������-������� �� �����
    private void OnDestroy()
    {
        // �������� ������� ������ ���������
        Destroy(talkCloud);
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    private void FixedUpdate()
    {
        // ��������� ������ �������� ������� �������-������
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }
}
