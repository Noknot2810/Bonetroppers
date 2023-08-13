using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ������, ���������� �� ��������� ��������� �������� �� ���� �������� ������
// ������������ ��������� 3 ������ ����� �������� � 3 ������ ������
// �� ����� �������� �������, � �������� �������� ������ ������, ������������
// ������� �� �������� ���������� ��������� ��������
// �� ������ ������� - ObjSpawnPos - ������������ ����� ���������� ��������
// ������� �� ����� ���������
// �� ������� ������� TombstoneSpawnPos - ����������� �� ������ ���� �������
// ������� �� ��������� �������� �������
public class SpawnRandomEnvironment : MonoBehaviour
{
    public float MinTime = 0.5f;        // ����������� �������� ���������
    public float MaxTime = 5f;          // ������������ �������� ���������

    public GameObject ObjSpawnPos;      // ������� ��� ��������� �������
                                        // ��������
    public GameObject TombstoneSpawnPos;// ������� ��� ��������� �������
                                        // �� ������ ����

    // ������ �������� ������� ��� ������� ����
    private List<Sprite> tombstones;
    // ������ �������� ��� ��������� ���������
    public List<Sprite> SpritesOnWay;
    // ������ ������� �������� ��� ��������� ���������
    public List<GameObject> ObjectsOnWay;

    // ������-��������� ��� �������� ��������� ��������
    private GameObject ground;
    // ������-����� �������-��������� ��� �������� ��������� ��������
    private Transform groundFolder;

    private float tombstoneTimer = 0f;  // ������ ��������� �������
    private float spriteTimer = 0f;     // ������ ��������� ��������
                                        // �� ��������
    private float objectsTimer = 0f;    // ������ ��������� �����
                                        // ������� ��������

    // ������� �������� ������������� ������� �� ������� �� ��� Z
    private float zValue = 0;
    // ����������-���� ����������� ��������� ��������
    private bool stopGen = false;
    // ������ ��������� ������� ���� ��� ��������� ������� ���������
    private string[] randomPhrases;

    // �������, ����������� ����� ������ ������
    void Start()
    {
        // �������� �������� ������� � ������
        tombstones = Resources.LoadAll<Sprite>("").ToList();
        // �������� ������� ���� ��� ������� ���������
        randomPhrases = (Resources.Load("SoldierPhrases") as Dialogue).
            text.Split('\n').ToArray();
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    private void FixedUpdate()
    {
        // ���������� �������, ��� ���� ����������� ��������� �� ���������
        if (stopGen == false)
        {
            // ������ ������� ��������� ������� � ��������� �� ���������
            // ������� � ����������� ��������� ������������ �������
            tombstoneTimer -= Time.deltaTime;
            if (tombstoneTimer <= 0)
            {
                TombstoneSpawn();
                tombstoneTimer = Random.Range(MinTime, MaxTime);
            }

            // ������ ������� ��������� �������� �� �������� � ��������� �� 
            // ��������� ������� � ����������� ��������� ������������ �������
            spriteTimer -= Time.deltaTime;
            if (spriteTimer <= 0)
            {
                Spawn();
                spriteTimer = Random.Range(MinTime, MaxTime);
            }

            // ������ ������� ��������� ����� ������� �������� � ��������� ��
            // ��������� ������� � ����������� ��������� ������������ �������
            objectsTimer -= Time.deltaTime;
            if (objectsTimer <= 0)
            {
                ObjSpawn();
                objectsTimer = Random.Range(MinTime + 1f, MaxTime + 2f);
            }
        }
    }

    // ��������������� �������-��������� ��� ��������
    public void NewGround(GameObject newGround)
    {
        ground = newGround;
        // ���������� �������, ��� ����� ������-��������� ������
        if (ground == null)
            // ����������� ��������� ��������� ��������
            stopGen = true;
        else
            // ��������� �������-����� ��� ���������� � ���� ��������� ��������
            groundFolder = newGround.GetComponent<SpawnGround>().
                EnvFolder.transform;
    }

    // ������� ��������� ��������� ������� �� ������ ����
    private void TombstoneSpawn()
    {
        // �������� ������ �������� �������
        GameObject newObj = new GameObject("RamdomTombstone");
        // ���������� � ���� ���������� ��������� �������
        SpriteRenderer rend = newObj.AddComponent<SpriteRenderer>();
        // ����� � ��������� ���������� ������� ������� �� ������
        rend.sprite = tombstones[Random.Range(0, tombstones.Count)];
        // ������� ������ ��������� � ������� ��������� �������
        rend.sortingLayerName = "Back";
        rend.sortingOrder = 5;
        // ������� ��������� ������� ������ �������
        newObj.transform.position = TombstoneSpawnPos.transform.position;
        // ������� �������-����� � �������� �������������
        newObj.transform.parent = groundFolder;
        // ���������� ����������� ����������
        Rigidbody2D rigit = newObj.AddComponent<Rigidbody2D>();
        // ������ ���������� �������� �������
        rigit.freezeRotation = true;
        // ���������� ���������� ����������
        newObj.AddComponent<BoxCollider2D>();
        // ���������� ������� ��� �������� ������� � ���������� ���������
        StaffFreezer freezer = newObj.AddComponent<StaffFreezer>();
        // �������� ���� ��� ��������
        freezer.AttachedRigidbody = ground.GetComponent<Rigidbody2D>();
    }

    // ������� ��������� �������� ������� �� ���������� �������
    private void Spawn()
    {
        // �������� ������ �������� �������
        GameObject newObj = new GameObject("RandomObj");
        // ���������� � ���� ���������� ��������� �������
        SpriteRenderer rend = newObj.AddComponent<SpriteRenderer>();
        // ����� � ��������� ���������� ������� �� ������
        rend.sprite = SpritesOnWay[Random.Range(0, SpritesOnWay.Count)];

        // ��������� ������� ������ ��������� �������
        if (Random.Range(0, 2) == 0)
        {
            rend.sortingLayerName = "Ground -1";
        }
        else
        {
            newObj.layer = 6;
            rend.sortingLayerName = "Ground +1";
        }
        // ������� ������� ��������� �������
        rend.sortingOrder = 5;
        // ������� ��������� ������� ������ ������� � �������
        // ���������� ��������� �� ��� Z
        newObj.transform.position = transform.position + 
            new Vector3(0f, 0f, zValue);

        // ���������� �������� �������� �� ��� Z
        zValue += 0.00001f;
        if (zValue == 1000)
            zValue = 0;

        // ������� �������-����� � �������� �������������
        newObj.transform.parent = groundFolder;
        // ���������� ����������� ����������
        Rigidbody2D rigit = newObj.AddComponent<Rigidbody2D>();
        // ������ ���������� �������� �������
        rigit.freezeRotation = true;
        // ��������� �������� ���� ���������� ��� �������
        rigit.gravityScale = 100f;
        // ���������� ���������� ����������
        newObj.AddComponent<BoxCollider2D>();
        // ���������� ������� ��� �������� ������� � ���������� ���������
        StaffFreezer freezer = newObj.AddComponent<StaffFreezer>();
        // �������� ���� ��� ��������
        freezer.AttachedRigidbody = ground.GetComponent<Rigidbody2D>();
    }

    // ������� ��������� ����� ���������� �������� �������
    // � ������� ��� ������ ���� ������ ���������� ���������� (Rigidbody2D)
    // � ���������
    private void ObjSpawn()
    {
        // ���������� �������� ������� ������
        GameObject choice = ObjectsOnWay[Random.Range(0, ObjectsOnWay.Count)];
        // �������� ����� ���������� �������
        GameObject newItem = Instantiate(choice, ObjSpawnPos.transform.position, 
                                         Quaternion.identity, groundFolder);

        // ��������� ������� ����� ��� ������� ���������� ���
        // �������������� �� 180 �������� �� ��� �
        if (Random.Range(0, 2) == 0)
        {
            newItem.transform.Rotate(0, 180, 0, Space.World);
        }

        // ���������� ������� ��� �������� ������� � ���������� ���������
        StaffFreezer freezer = newItem.AddComponent<StaffFreezer>();
        // �������� ���� ��� ��������
        freezer.AttachedRigidbody = ground.GetComponent<Rigidbody2D>();

        // ��������� � ������� ���������� ��������� �������
        SpriteRenderer rend = newItem.GetComponent<SpriteRenderer>();

        // ��������� ������� ������ ��������� �������
        if (Random.Range(0, 2) == 0)
        {
            newItem.layer = 0;
            rend.sortingLayerName = "Ground -1";
        }
        else
        {
            newItem.layer = 6;
            rend.sortingLayerName = "Ground +1";
            freezer.MakeCorrection = true;
        }
        // ������� ������� ��������� �������
        rend.sortingOrder = 4;

        // �������������� ����������, ����������� � �����������
        // �� ����� ���������� �������� �������
        // ��� ������� �������-������
        if (choice.name == "Player")
        {
            // ���������� �������� ������
            newItem.GetComponent<Animator>().SetBool("IsWalking", false);
            // ��������� �������� ��� ������� ������� ������� ������ ���������
            // �� ��������� ����������� �� ������������ ������
            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(newItem.GetComponent<PlayerMovement>().
                    SoldierTalk(randomPhrases[Random.Range(0, randomPhrases.Length)]));
            }
        }
        // ��� ������� �������
        else if (choice.name == "Banner")
        {
            // ������� ����� ������� ��������� �������
            rend.sortingOrder = 7;
            // ��������� ������� ����� ������ ������� �
            // ������� ��� ������ � ������� ���������
            Transform flag = newItem.transform.Find("Flag #0");
            foreach (SpriteRenderer frend in flag.
                GetComponentsInChildren<SpriteRenderer>())
            {
                frend.sortingLayerName = rend.sortingLayerName;
                frend.sortingOrder = rend.sortingOrder - 1;
            }
        }
    }
}
