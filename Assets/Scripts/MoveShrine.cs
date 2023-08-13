using System.Collections.Generic;
using UnityEngine;

// Скрипт, отвечающий за движение объекта храма (Shrine)
public class MoveShrine : MonoBehaviour
{
    // Необходимая скорость движения объекта храма
    public float SpeedTemp = 5f;
    // Отслеэиваемвй объект
    public Transform TrackingObject;
    // Список остановочных триггеров
    public List<Transform> StopTriggers;

    // Физический компонент объекта храма
    private Rigidbody2D rigit;
    // Текущая скорость движения объекта храма
    private float speed;
    // Текущее положение по оси Х отслеживаемого объекта
    private float trackingPos;

    // Функция, запускаемая перед первым кадром
    void Start()
    {
        // Получение физического компонента объекта храма
        rigit = GetComponent<Rigidbody2D>();
        
        // Установка начальной скорости
        speed = 0;

        // Смещение храма на старте по оси Х
        // для избежания коллизии с другими объектами
        Vector3 cur = transform.position;
        cur.x += 100;
        transform.position = cur;

        // Установка положения Idle для скелетов-офицеров в конце пути
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Officer"))
        {
            obj.GetComponent<Animator>().SetBool("IsWalking", false);
        }
    }

    // Функция, запускаемая в каждом кадре
    void Update()
    {
        // Обновление положения по оси Х отслеживаемого объекта
        trackingPos = TrackingObject.position.x;

        // Проверка триггеров
        for (int i = 0; i < StopTriggers.Count; i++)
        {
            // Запрашивается условие, чтобы отслеживаемый объект оказался
            // по оси Х на уровне остановочного триггера
            // или дальше этого уровня
            if (trackingPos >= StopTriggers[i].position.x)
            {
                // Установка нулевой скорости движения объекта храма
                speed = 0;

                // Поведение триггера опеределяется его названием
                switch (StopTriggers[i].name)
                {
                    // Триггер остановки внутри храма
                    case "StopTriggerInside":
                        // Вызов функции начала предэкзаменационного диалога
                        FindObjectOfType<ExaminationScript>().StartGame();
                        // Удаление оставшихся в движении
                        // после входа в храм объектов
                        foreach (GameObject obj in GameObject.
                            FindGameObjectsWithTag("Finish"))
                        {
                            Destroy(obj);
                        }
                        break;
                    // Триггер остановки снаружи храма
                    case "StopTriggerOutside":
                        // Вызов функции активации хорошей концовки
                        FindObjectOfType<ExaminationScript>().HappyEnd();
                        // Очистка списка остановочных триггеров
                        StopTriggers.Clear();

                        // Установка положения LongFun для скелетов-офицеров
                        // в конце пути
                        foreach (GameObject obj in GameObject.
                            FindGameObjectsWithTag("Officer"))
                        {
                            obj.GetComponent<Animator>().
                                SetTrigger("LongFunTrigger");
                        }
                        return;
                }

                // Удаление сработавшего триггера из списка
                StopTriggers.Remove(StopTriggers[i]);
                i--;
            }
        }
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    void FixedUpdate()
    {
        // Установка нового смещения объекта храма
        rigit.velocity = new Vector2(-speed, 0);
    }

    // Функция запуска движения объекта храма
    public void Restart()
    {
        // Переустановка скорости
        speed = SpeedTemp;
    }

    // Функция удаления первого остановочного триггера
    public void RemoveFirstTrig()
    {
        // Удаление первого триггера
        StopTriggers.Remove(StopTriggers[0]);
        // Удаление оставшихся в движении после входа в храм объектов
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Finish"))
        {
            Destroy(obj);
        }
    }
}
