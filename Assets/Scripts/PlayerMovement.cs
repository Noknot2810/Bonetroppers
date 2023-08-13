using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт, отвечающий за движения и действия объекта скелета-игрока (Player)
public class PlayerMovement : MonoBehaviour
{
    // Шаблон объекта облака разговора
    public GameObject TalkCloudTemplate;

    // Аниматор скелета-игрока
    private Animator anim;
    // Список скриптов для частей тела скелета
    private List<SPartMoveScript> s_parts;
    // Объект облака разговора скелета-игрока
    private GameObject talkCloud;

    // Функция, запускаемая перед первым кадром
    void Start()
    {
        // Наполнение списка скриптов для частей тела скелета
        s_parts = new List<SPartMoveScript>
        {
            gameObject.transform.Find("Player_skull").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_body").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_lhand").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_rhand").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_lleg").GetComponent<SPartMoveScript>(),
            gameObject.transform.Find("Player_rleg").GetComponent<SPartMoveScript>()
        };
        // Получение компонента аниматора скелета-игрока
        anim = gameObject.GetComponent<Animator>();
    }

    // Функция вызова подрыва скелета-игрока
    public void Explode()
    {
        // Вызов анимации взрыва
        anim.SetTrigger("ExplodeTrigger");
        // Вызов остановки работы аниматора
        anim.SetBool("StopMashine", true);
        // Выключение одного из коллайдеров скелета-игрока
        GetComponent<BoxCollider2D>().enabled = false;
        // Активация разлёта частей тела скелета-игрока
        for (int i = 0; i < s_parts.Count; i++)
        {
            s_parts[i].IsActive = true;
        }
    }

    // Функция телепортирования скелета-игрока по оси Х
    public void TeleportForward(float distance)
    {
        // Установка новой позиции скелета-игрока
        Vector3 tpos = transform.position;
        tpos.x += distance;
        transform.position = tpos;
        // Выключение анимации ходьбы
        anim.SetBool("IsWalking", false);
    }

    // Функция включения анимации проявления радости
    public void Celebrate()
    {
        anim.SetTrigger("FunTrigger");
    }

    // Функция включения анимации ходьбы
    public void StartWalk()
    {
        anim.SetBool("IsWalking", true);
    }

    // Функция изменения типа анимации Idle
    public void ChangeIdle()
    {
        anim.SetBool("IsRespect", !anim.GetBool("IsRespect"));
    }

    // Функция, вызываемая при столкновении объекта игрока-скелета с триггером
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Отключение анимации ходьбы
        anim.SetBool("IsWalking", false);
    }

    // Функция создания и привязки облака разговора
    public IEnumerator SoldierTalk(string text)
    {
        // Вызов задержки
        yield return new WaitForSeconds(1f);
        // Вычисление новой позиции облака разговора
        Vector3 newPos = transform.position;
        newPos.x -= 2f;
        newPos.y += 2.3f;
        // Создание экземпляра объекта облака разговора
        talkCloud = Instantiate(TalkCloudTemplate, newPos, 
            Quaternion.identity, GameObject.Find("MainCanvas").transform);

        // Задание указанного текста внутри облака разговора
        talkCloud.GetComponentInChildren<Text>().text = text;

        // Привзяка облака разговора к той же поверхности,
        // к которой привязан объект скелета-игрока
        FixedJoint2D joint = talkCloud.AddComponent<FixedJoint2D>();
        joint.connectedBody = gameObject.GetComponent<StaffFreezer>().
            AttachedRigidbody;
    }

    // Функция вызываемая при удалении объекта игрока-скелета со сцены
    private void OnDestroy()
    {
        // Удаление объекта облака разговора
        Destroy(talkCloud);
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    private void FixedUpdate()
    {
        // Установка нового смещения объекта скелета-игрока
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }
}
