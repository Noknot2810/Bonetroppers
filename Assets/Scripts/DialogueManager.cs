using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт, контролирующий работу диалоговых окон
public class DialogueManager : MonoBehaviour
{
    // Текстовый компонент диалогового окна
    public Text TextField;
    // Указатель, нужно ли печатать по одной букве или вывести всё разом
    public bool UseTyping = true;

    // Очередь предложений в рамках диалога
    private Queue<string> sentences;

    // Функция, запускаемая перед первым кадром
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Функция активации диалога
    public void StartDialogue(List<string> texts)
    {
        // Очистка очереди предложений диалога
        sentences.Clear();
        // Перезапись очереди предложений диалога
        foreach(string text in texts)
        {
            sentences.Enqueue(text);
        }
        // Вывод первого предложения из очереди
        DisplayNextSentence();
    }

    // Функция получения следующего предложения из очереди диалога
    public void DisplayNextSentence()
    {
        // Завершение диалога в случае отсутствия предложений в очереди
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        // Получение нового предложения из очереди
        string sentence = sentences.Dequeue();

        if (UseTyping == true)
        {
            // Активация посимвольного вывода предложения
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            // Активация полного вывода предложения
            TextField.text = sentence;
        }
    }

    // Функция посимвольного вывода теста предложения
    IEnumerator TypeSentence(string sentence)
    {
        // Очистка текстового поля
        TextField.text = "";
        // Посимвольное наполнение текстового поля с задержкой
        foreach (char letter in sentence.ToCharArray())
        {
            TextField.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
    }

    // Функция завершения диалога
    public void EndDialogue()
    {
        // Поведение диалогового окна определяется его названием
        switch (gameObject.name)
        {
            // Диалоговое окно экзамена
            case "ExaminationDialog":
                // Вызов начала экзамена
                FindObjectOfType<ExaminationScript>().
                    StartExamination();
                break;
            // Диалоговое окно из главного меню
            case "MenuWindow":
                // Отжатие кнопки "О нас" и сокрытие окна
                ButtonsScript scr = GameObject.Find("AboutUs").
                    GetComponent<ButtonsScript>();
                scr.OnMouseUpAsButton();
                scr.OnMouseUp();
                break;
        }
    }
}
