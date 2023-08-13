using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Скрипт, отвечающий за работу скрываемых кнопок, функционирующих
// по старому образцу (без компонента Button)
public class GhostButtonsScript : ButtonsScript
{
    // Аниматор скрываемой кнопки
    protected Animator anim;

    // Функция, запускаемая при нажатии на объект
    public override void OnMouseDown()
    {
        // Запуск анимации нажатия
        if (anim != null)
        {
            anim.SetBool("onClick", true);
        }
    }

    // Функция, запускаемая при отжатии объекта
    public override void OnMouseUp()
    {
        // Запуск анимации отжатия
        if (anim != null)
        {
            anim.SetBool("onClick", false);
        }
    }

    // Функция, запускаемая при нажатии и отжатии объекта
    public override void OnMouseUpAsButton()
    {
        // Поведение кнопки определяется её названием
        switch (gameObject.name)
        {
            // Кнопка "Сложный режим"
            case "Hardmode":
                // Запрашивается условие, что игра была пройдена
                // на обычном уровне
                if (PlayerPrefs.GetInt("regularCompleted", 0) != 0)
                {
                    // Выключение главного меню
                    foreach (GameObject obj in 
                        GameObject.FindGameObjectsWithTag("MainMenuElem"))
                    {
                        obj.SetActive(false);
                    }
                    // Вывод облачной заметки о начале игры
                    interactiveObj[0].GetComponent<CloudNoteScript>().
                        TurnCloudNote("Некромант готов вас принять", 
                                      firstDelay: true);
                    // Запуск экзамена на сложном режиме
                    FindObjectOfType<ExaminationScript>().StartHardGame();
                }
                else
                {
                    // Вывод облачной заметки о невозможности начала игры
                    interactiveObj[0].GetComponent<CloudNoteScript>().
                        TurnCloudNote("Игра на время откроется после " +
                                      "полного прохождения теста", 
                                      isProblem: true);
                }
                break;
            // Кнопка "Начать" для обычного режима
            case "Begin":
                // Выключение главного меню
                foreach (GameObject obj in 
                    GameObject.FindGameObjectsWithTag("MainMenuElem"))
                {
                    obj.SetActive(false);
                }
                // Вывод облачной заметки о начале игры
                interactiveObj[0].GetComponent<CloudNoteScript>().
                    TurnCloudNote("Некромант готов вас принять",
                                  firstDelay: true);
                // Запуск экзамена c самого начала
                FindObjectOfType<ExaminationScript>().StartGameFromBeg();
                break;
            // Кнопка вывода чекпоинтов
            case "Checkpoint":
                // Получение кнопок чекпоинтов
                var arr = interactiveObj[0].
                    GetComponentsInChildren<GreenButtonsScript>().
                    OrderBy(scr => scr.transform.name).ToArray();
                // Получение последнего достигнутого чекпоинта
                int lastAllowed = PlayerPrefs.GetInt("bestSection", 0);
                // Разблокировка всех доступных чекпоинтов
                for (int i = 0; i <= lastAllowed; i++)
                {
                    arr[i].UnlockButton();
                }

                // Вывод меню чекпоинтов
                foreach (Animator anim in interactiveObj[0].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", true);
                }

                // Сокрытие игрового меню
                foreach (Animator anim in interactiveObj[1].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", false);
                }

                // Сокрытие подсказок
                foreach (Text tfield in interactiveObj[2].GetComponentsInChildren<Text>())
                {
                    tfield.enabled = false;
                }
                break;
            // Кнопка пасмурного заднего фона
            case "CloudB":
                // Отключение всех задних фонов
                foreach (SpriteRenderer rend in interactiveObj[0].
                    GetComponentsInChildren<SpriteRenderer>())
                {
                    rend.enabled = false;
                }
                // Сохранение пользовательской настройки
                PlayerPrefs.SetString("userBack", "Cloud");
                break;
            // Кнопка закатного заднего фона
            case "SunsetB":
                // Запрашивается условие, что игра была пройдена
                // на обычном уровне
                if (PlayerPrefs.GetInt("regularCompleted", 0) != 0)
                {
                    // Отключение всех задних фонов
                    foreach (SpriteRenderer rend in interactiveObj[0].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    // Включение заднего фона заката
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                    // Сохранение пользовательской настройки
                    PlayerPrefs.SetString("userBack", "Sunset");
                }
                else
                {
                    // Вывод облачной заметки
                    interactiveObj[2].GetComponent<CloudNoteScript>().
                        TurnCloudNote("Данный фон откроется после " +
                                      "полного прохождения теста", 
                                      isProblem: true);
                }
                break;
            // Кнопка ночного заднего фона
            case "NightB":
                // Запрашивается условие, что игра была пройдена
                // на сложном уровне
                if (PlayerPrefs.GetInt("hardmodeCompleted", 0) != 0)
                {
                    // Отключение всех задних фонов
                    foreach (SpriteRenderer rend in interactiveObj[0].GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    // Включение заднего фона ночи
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                    // Сохранение пользовательской настройки
                    PlayerPrefs.SetString("userBack", "Night");
                }
                else
                {
                    // Вывод облачной заметки
                    interactiveObj[2].GetComponent<CloudNoteScript>().
                        TurnCloudNote("Данный фон откроется после полного " +
                                      "прохождения теста на сложном уровне", 
                                      isProblem: true);
                }
                break;
        }
    }

    // Функция, запускаемая перед первым кадром
    protected override void Start()
    {
        // Получение аниматора скрываемой кнопки
        anim = GetComponent<Animator>();
        // Поведение кнопки определяется её названием
        switch (gameObject.name)
        {
            // Кнопка "Сложный режим"
            case "Checkpoint":
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // Кнопка пасмурного заднего фона
            case "CloudB":
                interactiveObj.Add(GameObject.Find("Backgrounds"));
                break;
            // Кнопка закатного заднего фона
            case "SunsetB":
                interactiveObj.Add(GameObject.Find("Backgrounds"));
                interactiveObj.Add(GameObject.Find("SunsetBackground"));
                interactiveObj.Add(GameObject.Find("CloudNote"));
                if (PlayerPrefs.GetString("userBack", "") == "Sunset")
                {
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                }
                break;
            // Кнопка ночного заднего фона
            case "NightB":
                interactiveObj.Add(GameObject.Find("Backgrounds"));
                interactiveObj.Add(GameObject.Find("NightBackground"));
                interactiveObj.Add(GameObject.Find("CloudNote"));
                if (PlayerPrefs.GetString("userBack", "") == "Night")
                {
                    interactiveObj[1].GetComponent<SpriteRenderer>().
                        enabled = true;
                }
                break;
            default:
                interactiveObj.Add(GameObject.Find("CloudNote"));
                break;
        }

    }
}
