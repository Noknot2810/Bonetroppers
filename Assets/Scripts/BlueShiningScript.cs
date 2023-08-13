using UnityEngine;

// Скрипт, отвечающий за изменение синей подсветки
public class BlueShiningScript : MonoBehaviour
{
    public Material mat;                                    // Цветовой
                                                            // материал
    public float Duration = 2f;                             // Период свечения
    private float timer = 0f;                               // Таймер
    private bool switched = false;                          // Переменная-флаг
                                                            // затухания
                                                            // свечения
    private Color col1 = new Color(64f, 336f, 2048f, 0f);   // Первый цвет
    private Color col2 = new Color(4f, 21f, 128f, 0f);      // Второй цвет
    private Color colorDiff;                                // Цветовая разница


    // Функция, запускаемая перед первым кадром
    void Start()
    {
        colorDiff = col1 - col2;
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    private void FixedUpdate()
    {
        // Обновление таймера
        timer -= Time.deltaTime;

        // Изменение направления затухания, сброс таймера по истечении таймера
        if (timer <= 0)
        {
            switched = !switched;
            timer = Duration;
        }

        // Изменение цвета свечения
        float diffTime = timer / Duration;
        if (switched == true)
        {
            mat.SetColor("_Color", col2 + (colorDiff * diffTime));
        }
        else
        {
            mat.SetColor("_Color", col1 - (colorDiff * diffTime));
        }
    }
}
