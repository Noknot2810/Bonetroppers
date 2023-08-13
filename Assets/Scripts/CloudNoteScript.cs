using UnityEngine;
using UnityEngine.UI;

// Скрипт, отвечающий за работу облачных заметок (за игровой объект CloudNote)
public class CloudNoteScript : MonoBehaviour
{
    // Задержка перед первым появлением заметки в игровом процессе
    public float FirstDelay = 6f;
    // Стандартная задержка появления заметки
    public float RegularDelay = 3f;
    // Задержка появления заметки для проблемной ситуации
    public float ProblemDelay = 1.5f;
    // Текстовый компонент заметки
    public Text NoteField;
    // Игровой аудио-менеджер
    public AudioManager MusicBox;

    // Аниматор заметки
    private Animator anim;

    // Функция, запускаемая перед первым кадром
    private void Start()
    {
        // Получение текстового компонента заметки
        NoteField = gameObject.transform.Find("Text").GetComponent<Text>();
        // Получение аниматора заметки
        anim = gameObject.GetComponent<Animator>();
        // Получение игрового аудио-менеджера
    }

    // Функция активации заметки с заданными параметрами
    public void TurnCloudNote(string text, 
                              bool firstDelay = false, 
                              bool isProblem = false, 
                              bool withoutSound = false)
    {
        // Присваивание текста заметке
        NoteField.text = text;
        // Вызов анимации появления заметки
        anim.SetBool("appeared", true);

        // В зависимости от входных параметров включение
        // звукового эффекта и вызов скрытия заметки после задержки
        if (withoutSound == true)
        {
            Invoke(nameof(OffCloudNote), RegularDelay);
        }
        else if (isProblem == true)
        {
            MusicBox.PlayTransit("Problem sound");
            Invoke(nameof(OffCloudNote), ProblemDelay);
        }
        else if (firstDelay == true)
        {
            MusicBox.PlayTransit("Appearance sound");
            Invoke(nameof(OffCloudNote), FirstDelay);
        }
        else
        {
            MusicBox.PlayTransit("Appearance sound");
            Invoke(nameof(OffCloudNote), RegularDelay);
        }
    }

    // Функция скрытия заметки
    public void OffCloudNote()
    {
        // Вызов анимации скрытия заметки
        anim.SetBool("appeared", false);
    }
}
