using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Скрипт, отвечающий за генерацию случайных объектов по пути движения игрока
// Производится генерация 3 разных типов объектов в 3 разных точках
// На месте игрового объекта, к которому привязан данный скрипт, генерируются
// объекты из случайно выбираемых указанных спрайтов
// На второй позиции - ObjSpawnPos - генерируется копия случайного игрового
// объекта из числа указанных
// На третьей позиции TombstoneSpawnPos - генерируюся на заднем фоне игровые
// объекты из случайных спрайтов могилок
public class SpawnRandomEnvironment : MonoBehaviour
{
    public float MinTime = 0.5f;        // Минимальная задержка генерации
    public float MaxTime = 5f;          // Максимальная задержка генерации

    public GameObject ObjSpawnPos;      // Позиция для генерации игровых
                                        // объектов
    public GameObject TombstoneSpawnPos;// Позиция для генерации могилок
                                        // на заднем фоне

    // Список спрайтов могилок для заднего фона
    private List<Sprite> tombstones;
    // Список спрайтов для случайной генерации
    public List<Sprite> SpritesOnWay;
    // Список игровых объектов для случайной генерации
    public List<GameObject> ObjectsOnWay;

    // Объект-декорация для привзяки случайных объектов
    private GameObject ground;
    // Объект-папка объекта-декорации для хранения случайных объектов
    private Transform groundFolder;

    private float tombstoneTimer = 0f;  // Таймер генерации могилок
    private float spriteTimer = 0f;     // Таймер генерации объектов
                                        // из спрайтов
    private float objectsTimer = 0f;    // Таймер генерации копий
                                        // игровых объектов

    // Текущее значение генерируемого объекта из спрайта по оси Z
    private float zValue = 0;
    // Переменная-флаг прекращения генерации объектов
    private bool stopGen = false;
    // Массив случайных текстов фраз для генерации облаков разговора
    private string[] randomPhrases;

    // Функция, запускаемая перед первым кадром
    void Start()
    {
        // Загрузка спрайтов могилок в список
        tombstones = Resources.LoadAll<Sprite>("").ToList();
        // Загрузка текстов фраз для облаков разговора
        randomPhrases = (Resources.Load("SoldierPhrases") as Dialogue).
            text.Split('\n').ToArray();
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    private void FixedUpdate()
    {
        // Поставлено условие, что флаг прекращения генерации не выставлен
        if (stopGen == false)
        {
            // Отсчёт таймера генерации могилок и генерация по истечении
            // таймера с последующим случайным перезапуском таймера
            tombstoneTimer -= Time.deltaTime;
            if (tombstoneTimer <= 0)
            {
                TombstoneSpawn();
                tombstoneTimer = Random.Range(MinTime, MaxTime);
            }

            // Отсчёт таймера генерации объектов из спрайтов и генерация по 
            // истечении таймера с последующим случайным перезапуском таймера
            spriteTimer -= Time.deltaTime;
            if (spriteTimer <= 0)
            {
                Spawn();
                spriteTimer = Random.Range(MinTime, MaxTime);
            }

            // Отсчёт таймера генерации копий игровых объектов и генерация по
            // истечении таймера с последующим случайным перезапуском таймера
            objectsTimer -= Time.deltaTime;
            if (objectsTimer <= 0)
            {
                ObjSpawn();
                objectsTimer = Random.Range(MinTime + 1f, MaxTime + 2f);
            }
        }
    }

    // Переопределение объекта-декорации для привязки
    public void NewGround(GameObject newGround)
    {
        ground = newGround;
        // Поставлено условие, что новый объект-декорация обнулён
        if (ground == null)
            // Прекращение генерации случайных объектов
            stopGen = true;
        else
            // Получение объекта-папки для сохранения в него случайных объектов
            groundFolder = newGround.GetComponent<SpawnGround>().
                EnvFolder.transform;
    }

    // Функция генерации случайной могилки на заднем фоне
    private void TombstoneSpawn()
    {
        // Создание нового игрового объекта
        GameObject newObj = new GameObject("RamdomTombstone");
        // Добавление к нему компоненты отрисовки спрайта
        SpriteRenderer rend = newObj.AddComponent<SpriteRenderer>();
        // Выбор и установка случайного спрайта могилки из списка
        rend.sprite = tombstones[Random.Range(0, tombstones.Count)];
        // Задание уровня отрисовки и порядка отрисовки спрайта
        rend.sortingLayerName = "Back";
        rend.sortingOrder = 5;
        // Задание начальной позиции нового объекта
        newObj.transform.position = TombstoneSpawnPos.transform.position;
        // Задание объекта-папки в качестве родительского
        newObj.transform.parent = groundFolder;
        // Добавление физического компонента
        Rigidbody2D rigit = newObj.AddComponent<Rigidbody2D>();
        // Запрет свободного вращения объекта
        rigit.freezeRotation = true;
        // Добавление компонента коллайдера
        newObj.AddComponent<BoxCollider2D>();
        // Добавление скрипта для привязки объекта к движущейся декорации
        StaffFreezer freezer = newObj.AddComponent<StaffFreezer>();
        // Указание цели для привязки
        freezer.AttachedRigidbody = ground.GetComponent<Rigidbody2D>();
    }

    // Функция генерации игрового объекта из случайного спрайта
    private void Spawn()
    {
        // Создание нового игрового объекта
        GameObject newObj = new GameObject("RandomObj");
        // Добавление к нему компоненты отрисовки спрайта
        SpriteRenderer rend = newObj.AddComponent<SpriteRenderer>();
        // Выбор и установка случайного спрайта из списка
        rend.sprite = SpritesOnWay[Random.Range(0, SpritesOnWay.Count)];

        // Случайное задание уровня отрисовки спрайта
        if (Random.Range(0, 2) == 0)
        {
            rend.sortingLayerName = "Ground -1";
        }
        else
        {
            newObj.layer = 6;
            rend.sortingLayerName = "Ground +1";
        }
        // Задание порядка отрисовки спрайта
        rend.sortingOrder = 5;
        // Задание начальной позиции нового объекта с текущим
        // уникальным значением по оси Z
        newObj.transform.position = transform.position + 
            new Vector3(0f, 0f, zValue);

        // Обновление текущего значения по оси Z
        zValue += 0.00001f;
        if (zValue == 1000)
            zValue = 0;

        // Задание объекта-папки в качестве родительского
        newObj.transform.parent = groundFolder;
        // Добавление физического компонента
        Rigidbody2D rigit = newObj.AddComponent<Rigidbody2D>();
        // Запрет свободного вращения объекта
        rigit.freezeRotation = true;
        // Установка значения силы гравитации для объекта
        rigit.gravityScale = 100f;
        // Добавление компонента коллайдера
        newObj.AddComponent<BoxCollider2D>();
        // Добавление скрипта для привязки объекта к движущейся декорации
        StaffFreezer freezer = newObj.AddComponent<StaffFreezer>();
        // Указание цели для привязки
        freezer.AttachedRigidbody = ground.GetComponent<Rigidbody2D>();
    }

    // Функция генерации копии случайного игрового объекта
    // У объекта уже должны быть заданы физическая компонента (Rigidbody2D)
    // и коллайдер
    private void ObjSpawn()
    {
        // Выбирается случайно игрокой объект
        GameObject choice = ObjectsOnWay[Random.Range(0, ObjectsOnWay.Count)];
        // Создаётся копия выбранного объекта
        GameObject newItem = Instantiate(choice, ObjSpawnPos.transform.position, 
                                         Quaternion.identity, groundFolder);

        // Случайным образом копия или остаётся неизменной или
        // поворачивается на 180 градусов по оси У
        if (Random.Range(0, 2) == 0)
        {
            newItem.transform.Rotate(0, 180, 0, Space.World);
        }

        // Добавление скрипта для привязки объекта к движущейся декорации
        StaffFreezer freezer = newItem.AddComponent<StaffFreezer>();
        // Указание цели для привязки
        freezer.AttachedRigidbody = ground.GetComponent<Rigidbody2D>();

        // Получение у объекта компоненты отрисовки спрайта
        SpriteRenderer rend = newItem.GetComponent<SpriteRenderer>();

        // Случайное задание уровня отрисовки спрайта
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
        // Задание порядка отрисовки спрайта
        rend.sortingOrder = 4;

        // Дополнительная инструкция, применяемая в зависимости
        // от имени выбранного игрового объекта
        // Для объекта скелета-игрока
        if (choice.name == "Player")
        {
            // Отключение анимации ходьбы
            newItem.GetComponent<Animator>().SetBool("IsWalking", false);
            // Случайное создание для каждого второго скелета облака разговора
            // со случайным наполнением из загруженного списка
            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(newItem.GetComponent<PlayerMovement>().
                    SoldierTalk(randomPhrases[Random.Range(0, randomPhrases.Length)]));
            }
        }
        // Для объекта баннера
        else if (choice.name == "Banner")
        {
            // Задание иного порядка отрисовки спрайта
            rend.sortingOrder = 7;
            // Получение объекта флага внутри баннера и
            // задание ему уровня и порядка отрисовки
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
