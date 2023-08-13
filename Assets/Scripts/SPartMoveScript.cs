using UnityEngine;

// Скрипт, отвечающий за движение части тела скелета
public class SPartMoveScript : MonoBehaviour
{
    public bool IsActive = false;   // Флаг активации полёта части тела
    private Rigidbody2D rgd;        // Физический компонент части тела
    private Vector2 direction;      // Направление движения части тела
    private Vector2 speed;          // Скорость движения части тела
    private float timer = 0.2f;     // Таймер движения части тела

    // Функция, запускаемая перед первым кадром
    void Start()
    {
        // Получение физического компонента части тела
        rgd = gameObject.GetComponent<Rigidbody2D>();
        // Случайный выбор направления движения части тела
        direction = new Vector2(Random.Range(-1, 1), 1);
        // Случайный выбор скорости движения части тела
        speed = new Vector2(Random.Range(5, 10), Random.Range(10, 40));
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    private void FixedUpdate()
    {
        // Условие активации полёта
        if (IsActive == true)
        {
            // Установка нового смещения объекта части тела
            rgd.velocity = new Vector2(direction.x * speed.x, direction.y * speed.y);
            // Вызов функции контроля времени полёта
            TimedFlying(Time.deltaTime);
        }
    }

    // Функция отсчёта времени полёта части тела и
    // инициализации завершения полёта
    void TimedFlying(float delta)
    {
        // Отсчёт таймера
        timer -= delta;
        // Выключение флага полёта по завершении таймера
        if (timer <= 0)
        {
            IsActive = false;
        }
    }
}
