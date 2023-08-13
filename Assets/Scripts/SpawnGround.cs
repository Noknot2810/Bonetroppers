using UnityEngine;

// ������, ���������� �� �������� ��������� ������ ������
public class SpawnGround : MonoBehaviour
{
    public float SpawnOffset = 0f;      // ������� � ������������� ���������
    public float Speed = 5f;            // �������� �������� ���������
    public bool UseComposite = true;    // ���� ������ ������������ ����������
    public Transform TrackingObject;    // ������������� ������
    public GameObject ObjectToSpawn;    // ������ ��� �������������
    public Transform ParentObject;      // ������ ��� �������� ���������������� �������

    // ������ ��� ��������� ��������� �������� �� ���� ��������
    public SpawnRandomEnvironment RandomSpawner;
    // ������-����� ��� �������� �������� ��������������� ��������
    // �� ���� ��������
    public GameObject EnvFolder;

    private Rigidbody2D rigit;          // ���������� ��������� ���������
    private Collider2D objCollider;     // ��������� ���������
    
    private float width;                // ������ ��������� �� ��� �
    private float middle;               // ������� ����� ��������� �� ��� �

    protected bool generated = false;   // ����, ��� ����� ��������� ����
                                        // �������������
    private GameObject newGround;       // ������ ����� ���������������
                                        // ���������
    private SpawnGround newScript;      // ������ ����� ���������������
                                        // ���������
    
    private bool stopGen = false;       // ���� ��������� ������������� �����
                                        // ���������

    // �������, ����������� ����� ������ ������
    void Start()
    {
        // ��������� ����������� ���������� ���������
        rigit = GetComponent<Rigidbody2D>();

        // ��������� ���������� ���������
        if (UseComposite == true)
        {
            objCollider = GetComponent<CompositeCollider2D>();
        }
        else
        {
            objCollider = GetComponent<BoxCollider2D>();
        }

        // ���������� ������� ����� ��������� �� ��� �
        middle = objCollider.bounds.center.x + SpawnOffset;
        // ���������� ������ ��������� �� ��� �
        width = objCollider.bounds.size.x;

        // ��� ������� ���������� ��������� ��������
        if (RandomSpawner != null)
        {
            // �����, ������������ � ��������������� �������-�����
            // ��� �������� ��������������� �������� �� ���� ��������
            EnvFolder = transform.Find("EnvFolder").gameObject;
            Destroy(EnvFolder);
            EnvFolder = new GameObject("EnvFolder");
            EnvFolder.transform.parent = transform;

            // ������� ��������� ��� �������� ��� ���������� ���������
            // ��������
            RandomSpawner.NewGround(gameObject);
        }
    }

    // �������, ����������� � ������ �����
    void Update()
    {
        // ����������� �������, ��� ����� ��������� �� ���� �������������,
        // � ������������� ������ ��������� ����� ������� ���������
        if (generated == false && TrackingObject.position.x >= middle)
        {
            // ���������� ����� ������� ��� ������������� / �������� ���������
            Vector3 new_pos = transform.position;
            new_pos.x += width;
            // ����������� ���������� ����� ��������� �������������
            // ����� ���������
            if (stopGen == false)
                // ����� ������������� ����� ��������� ���������
                Generate(new_pos);
            else
            {
                // ���������� ���������� ��������� ��������
                RandomSpawner.NewGround(null);
                // ����������� ������� ������� ������� ����� ��� ��������
                if (ObjectToSpawn != null)
                {
                    // ������� ������� ����� � ���� �� ������� ����������
                    ObjectToSpawn.transform.position = new_pos;
                    // ����� �������� ������� �����
                    ObjectToSpawn.GetComponent<MoveShrine>().Restart();
                }
                // �������� �������� ������� ����� ��������
                Destroy(gameObject, 20f);
            }
            generated = true;
        }
        // ����������� �������, ��� ����� ��������� ���� �������������
        else if (generated == true)
        {
            // ����������� �������, ��� � ����� ��������������� ���������
            // ���� ������, � ���� ����� ��������� ��� �������������
            // ����� ���������
            if (newScript != null && newScript.generated == true)
            {
                // �������� �������� ������� ���������
                Destroy(gameObject);
            }
        }
        else
        {
            // �������� ��������� ������ ������� ���������
            middle = objCollider.bounds.center.x + SpawnOffset;
        }
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    void FixedUpdate()
    {
        // ��������� ������ �������� ������� ���������
        rigit.velocity = new Vector2(-Speed, 0);
    }

    // ������� ������������� ����� ���������
    void Generate(Vector3 new_pos)
    {
        // �������� ��������� ������� ��� �������������
        newGround = Instantiate(ObjectToSpawn, new_pos, 
            Quaternion.identity, ParentObject);
        // ������� ����� �������������������� �������
        newGround.name = ObjectToSpawn.name;
        // ���������� ������� ������������������� ���������
        newScript = newGround.GetComponent<SpawnGround>();
    }

    // ������� ������ ��������� ����� ��������� ��
    // ��������� �������� ������� �����
    public void SpawnShrine()
    {
        // ��������� ������������� ����� ���������
        stopGen = true;
        // ������� ������� ��� ������������� �� ������ �����
        ObjectToSpawn = GameObject.Find("Shrine");
    }

    // ������� ������ ��������� ����� ���������
    public void TeleportShrine()
    {
        // ��������� ������������� ����� ���������
        stopGen = true;
        // ��������� ������� ��� �������������
        ObjectToSpawn = null;
    }
}
