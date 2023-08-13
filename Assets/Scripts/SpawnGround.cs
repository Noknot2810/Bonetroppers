using UnityEngine;

// Скрипт, отвечающий за движение декорации вокруг игрока
public class SpawnGround : MonoBehaviour
{
    public float SpawnOffset = 0f;      // Задежка в генерировании декорации
    public float Speed = 5f;            // Скорость движения декорации
    public bool UseComposite = true;    // Флаг выбора композитного коллайдера
    public Transform TrackingObject;    // Отслеживаемый объект
    public GameObject ObjectToSpawn;    // Объект для генерирования
    public Transform ParentObject;      // Объект для привязки сгенерированного объекта

    // Объект для генерации случайных объектов по пути движения
    public SpawnRandomEnvironment RandomSpawner;
    // Объект-папка для хранения случайно сгенерированных объектов
    // по пути движения
    public GameObject EnvFolder;

    private Rigidbody2D rigit;          // Физический компонент декорации
    private Collider2D objCollider;     // Коллайдер декорации
    
    private float width;                // Ширина декорации по оси Х
    private float middle;               // Средняя точка декорации по оси Х

    protected bool generated = false;   // Флаг, что новая декорация была
                                        // сгенерирована
    private GameObject newGround;       // Объект новой сгенерированной
                                        // декорации
    private SpawnGround newScript;      // Скрипт новой сгенерированной
                                        // декорации
    
    private bool stopGen = false;       // Флаг остановки генерирования новых
                                        // декораций

    // Функция, запускаемая перед первым кадром
    void Start()
    {
        // Получение физического компонента декорации
        rigit = GetComponent<Rigidbody2D>();

        // Получение коллайдера декорации
        if (UseComposite == true)
        {
            objCollider = GetComponent<CompositeCollider2D>();
        }
        else
        {
            objCollider = GetComponent<BoxCollider2D>();
        }

        // Вычисление средней точки декорации по оси Х
        middle = objCollider.bounds.center.x + SpawnOffset;
        // Вычисление ширины декорации по оси Х
        width = objCollider.bounds.size.x;

        // При наличии генератора случайных объектов
        if (RandomSpawner != null)
        {
            // Поиск, пересоздание и перезакрепление объекта-папки
            // для случайно сгенерированных объектов по пути движения
            EnvFolder = transform.Find("EnvFolder").gameObject;
            Destroy(EnvFolder);
            EnvFolder = new GameObject("EnvFolder");
            EnvFolder.transform.parent = transform;

            // Задание декорации для привязки для генератора случайных
            // объектов
            RandomSpawner.NewGround(gameObject);
        }
    }

    // Функция, запускаемая в каждом кадре
    void Update()
    {
        // Проверяется условие, что новая декорация не была сгенерирована,
        // а отслеживаемый объект преодолел центр текущей декорации
        if (generated == false && TrackingObject.position.x >= middle)
        {
            // Вычисление новой позиции для генерирования / переноса декорации
            Vector3 new_pos = transform.position;
            new_pos.x += width;
            // Проверяется отсутствие флага остановки генерирования
            // новых декораций
            if (stopGen == false)
                // Вызов генерирования новой указанной декорации
                Generate(new_pos);
            else
            {
                // Выключение генератора случайных объектов
                RandomSpawner.NewGround(null);
                // Проверяется условия наличия объекта храма для переноса
                if (ObjectToSpawn != null)
                {
                    // Перенос объекта храма в след за текущей декорацией
                    ObjectToSpawn.transform.position = new_pos;
                    // Вызов движения объекта храма
                    ObjectToSpawn.GetComponent<MoveShrine>().Restart();
                }
                // Удаление текущего объекта после задержки
                Destroy(gameObject, 20f);
            }
            generated = true;
        }
        // Проверяется условие, что новая декорация была сгенерирована
        else if (generated == true)
        {
            // Проверяется условие, что у новой сгенерированной декорации
            // есть скрипт, и сама новая декорация уже сгенерировала
            // новую декорацию
            if (newScript != null && newScript.generated == true)
            {
                // Удаление текущего объекта декорации
                Destroy(gameObject);
            }
        }
        else
        {
            // Пересчёт положения центра текущей декорации
            middle = objCollider.bounds.center.x + SpawnOffset;
        }
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    void FixedUpdate()
    {
        // Установка нового смещения объекта декорации
        rigit.velocity = new Vector2(-Speed, 0);
    }

    // Функция генерирования новой декорации
    void Generate(Vector3 new_pos)
    {
        // Создание экземляра объекта для генерирования
        newGround = Instantiate(ObjectToSpawn, new_pos, 
            Quaternion.identity, ParentObject);
        // Задание имени новосгенерированному объекту
        newGround.name = ObjectToSpawn.name;
        // Сохранение скрипта новосгенерированной декорации
        newScript = newGround.GetComponent<SpawnGround>();
    }

    // Функция замены генерации новой декорации на
    // активацию движения объекта храма
    public void SpawnShrine()
    {
        // Остановка генерирования новых декораций
        stopGen = true;
        // Подмена объекта для генерирования на объект храма
        ObjectToSpawn = GameObject.Find("Shrine");
    }

    // Функция отмены генерации новой декорации
    public void TeleportShrine()
    {
        // Остановка генерирования новых декораций
        stopGen = true;
        // Обнуление объекта для генерирования
        ObjectToSpawn = null;
    }
}
