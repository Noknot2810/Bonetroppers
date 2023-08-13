using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Скрипт, отвечающий за работу кнопки-иконки наличия у игрока права на ошибку
public class HealthButtonScript : MonoBehaviour
{
    public Text TipField;   // Текстовое поле с подсказкой к кнопке-иконке
    private Animator anim;  // Анимационный компонент
    private Collider2D col; // Коллайдер кнопки

    // Функция, запускаемая при нажатии и отжатии объекта
    public virtual void OnMouseUpAsButton()
    {
        // Активация посимвольного вывода предложения
        StopAllCoroutines();
        StartCoroutine(TypeSentence("У вас есть право на ошибку"));
    }

    // Функция посимвольного вывода текста предложения
    IEnumerator TypeSentence(string sentence)
    {
        // Очистка и включение текстового поля
        TipField.text = "";
        TipField.enabled = true;
        // Посимвольное наполнение текстового поля с задержкой
        foreach (char letter in sentence.ToCharArray())
        {
            TipField.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(4f);
        // Выключение текстового поля
        TipField.enabled = false;
    }

    // Функция включения кнопки-иконки наличия права на ошибку
    public void ButtonOn()
    {
        anim.SetBool("appeared", true);
        col.enabled = true;
    }

    // Функция выключения кнопки-иконки наличия права на ошибку
    public void ButtonOff()
    {
        anim.SetBool("appeared", false);
        col.enabled = false;
    }

    // Функция, запускаемая перед первым кадром
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<Collider2D>();
    }
}
