using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Скрипт, отвечающий за работу кнопок, функционирующих по старому образцу
// (без компонента Button)
public class ButtonsScript : MonoBehaviour
{
    // Статус активности кнопки
    protected bool is_active = false; 
    // Список объектов для взаимодействия у данной кнопки
    protected List<GameObject> interactiveObj = new List<GameObject>();
    // Вложенная в кнопку иконка
    private Transform icon; 

    // Функция, запускаемая при нажатии на объект
    public virtual void OnMouseDown()
    {
        gameObject.GetComponent<SpriteRenderer>().color = 
            new Color(1f, 1f, 1f, 1f);
        icon.GetComponent<SpriteRenderer>().color = 
            new Color(1f, 1f, 1f, 1f);
    }

    // Функция, запускаемая при отжатии объекта
    public virtual void OnMouseUp()
    {
        if (is_active == false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = 
                new Color(1f, 1f, 1f, 0.6f);
            icon.GetComponent<SpriteRenderer>().color = 
                new Color(1f, 1f, 1f, 0.6f);
        }
    }

    // Функция, запускаемая при нажатии и отжатии объекта
    public virtual void OnMouseUpAsButton()
    {
        // Поведение кнопки определяется её названием
        switch (gameObject.name)
        {
            // Кнопка "Играть"
            case "Play":

                // Скрываются подсказки
                foreach (Text tfield in interactiveObj[6].GetComponentsInChildren<Text>())
                {
                    tfield.enabled = false;
                }

                if (is_active == true)
                {
                    // Скрываются кнопки игрового меню
                    foreach (Animator anim in interactiveObj[4].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }

                    // Скрываются кнопки меню чекпоинтов
                    foreach (Animator anim in interactiveObj[5].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    is_active = false;
                }
                else
                {
                    // Отжимается кнопка "Настройки",
                    // свзяанные с ней кнопки закрываются
                    foreach (Animator anim in interactiveObj[0].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    interactiveObj[1].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[1].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // Отжимается кнопка "О нас",
                    // свзяанные с ней элементы скрываются и выключаются
                    foreach (SpriteRenderer rend in interactiveObj[2].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    interactiveObj[2].GetComponentInChildren<Text>().
                        enabled = false;
                    interactiveObj[2].GetComponentInChildren<CircleCollider2D>().
                        enabled = false;
                    interactiveObj[3].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[3].GetComponent<ButtonsScript>().
                        OnMouseUp();             

                    // Выводятся кнопки игрового меню
                    foreach (Animator anim in interactiveObj[4].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", true);
                    }
                    is_active = true;
                }
                break;
            // Кнопка "Настройки"
            case "Settings":
                if (is_active == true)
                {
                    // Скрываются кнопки меню настроек
                    foreach (Animator anim in interactiveObj[5].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    is_active = false;
                }
                else
                {
                    // Скрываются кнопки игрового меню
                    foreach (Animator anim in interactiveObj[0].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // Скрываются кнопки меню чекпоинтов
                    foreach (Animator anim in interactiveObj[1].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // Отжимается кнопка "Играть"
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // Отжимается кнопка "О нас",
                    // свзяанные с ней элементы скрываются и выключаются
                    foreach (SpriteRenderer rend in interactiveObj[3].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    interactiveObj[3].GetComponentInChildren<Text>().
                        enabled = false;
                    interactiveObj[3].GetComponentInChildren<CircleCollider2D>().
                        enabled = false;
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // Скрываются подсказки
                    foreach (Text tfield in interactiveObj[6].GetComponentsInChildren<Text>())
                    {
                        tfield.enabled = false;
                    }

                    // Выводятся кнопки меню настроек
                    foreach (Animator anim in interactiveObj[5].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", true);
                    }
                    is_active = true;
                }
                break;
            // Кнопка "Громкость"
            case "Volume":
                if (is_active == true)
                {
                    // Выключается слайдер громкости
                    interactiveObj[0].SetActive(false);
                    is_active = false;
                }
                else
                {
                    // Включается слайдер громкости
                    interactiveObj[0].SetActive(true);
                    is_active = true;
                }
                break;
            // Кнопка "О нас"
            case "AboutUs":
                if (is_active == true)
                {
                    // Скрываются и выключаются связанные с кнопкой элементы
                    foreach (SpriteRenderer rend in interactiveObj[5].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = false;
                    }
                    interactiveObj[5].GetComponentInChildren<Text>().
                        enabled = false;
                    interactiveObj[5].GetComponentInChildren<CircleCollider2D>().
                        enabled = false;
                    is_active = false;
                }
                else
                {
                    // Скрываются кнопки игрового меню
                    foreach (Animator anim in interactiveObj[0].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // Скрываются кнопки меню чекпоинтов
                    foreach (Animator anim in interactiveObj[1].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // Отжимается кнопка "Играть"
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[2].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // Скрываются кнопки меню настроек
                    foreach (Animator anim in interactiveObj[3].
                        GetComponentsInChildren<Animator>())
                    {
                        anim.SetBool("appeared", false);
                    }
                    // Отжимается кнопка настройки
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        is_active = false;
                    interactiveObj[4].GetComponent<ButtonsScript>().
                        OnMouseUp();

                    // Скрываются подсказки
                    foreach (Text tfield in interactiveObj[6].GetComponentsInChildren<Text>())
                    {
                        tfield.enabled = false;
                    }

                    // Выводятся и включаются связанные с кнопкой элементы
                    foreach (SpriteRenderer rend in interactiveObj[5].
                        GetComponentsInChildren<SpriteRenderer>())
                    {
                        rend.enabled = true;
                    }
                    interactiveObj[5].GetComponentInChildren<Text>().
                        enabled = true;
                    interactiveObj[5].GetComponentInChildren<CircleCollider2D>().
                        enabled = true;
                    interactiveObj[5].GetComponent<DialogueManager>().
                        StartDialogue((Resources.Load("Credits") as Dialogue).
                        text.Split('#').ToList<string>());
                    is_active = true;
                }
                break;
            // Кнопка "Получить подсказки"
            case "GetTips":
                // Скрываются кнопки меню чекпоинтов
                foreach (Animator anim in interactiveObj[0].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", false);
                }

                // Скрываются кнопки меню настроек
                foreach (Animator anim in interactiveObj[1].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", false);
                }
                // Отжимается кнопка настройки
                interactiveObj[2].GetComponent<ButtonsScript>().
                    is_active = false;
                interactiveObj[2].GetComponent<ButtonsScript>().
                    OnMouseUp();

                // Отжимается кнопка "О нас",
                // свзяанные с ней элементы скрываются и выключаются
                foreach (SpriteRenderer rend in interactiveObj[3].
                    GetComponentsInChildren<SpriteRenderer>())
                {
                    rend.enabled = false;
                }
                interactiveObj[3].GetComponentInChildren<Text>().
                    enabled = false;
                interactiveObj[3].GetComponentInChildren<CircleCollider2D>().
                    enabled = false;
                interactiveObj[4].GetComponent<ButtonsScript>().
                    is_active = false;
                interactiveObj[4].GetComponent<ButtonsScript>().
                    OnMouseUp();

                // Выводятся кнопки игрового меню
                foreach (Animator anim in interactiveObj[5].
                    GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("appeared", true);
                }
                // Нажимается кнопка "Играть"
                interactiveObj[6].GetComponent<ButtonsScript>().
                    is_active = true;
                interactiveObj[6].GetComponent<ButtonsScript>().
                    OnMouseDown();

                foreach (Text tfield in interactiveObj[7].GetComponentsInChildren<Text>())
                {
                    tfield.enabled = true;
                }
                break;
            // Кнопка "Следующий титр"
            case "NextCredit":
                interactiveObj[0].GetComponent<DialogueManager>().
                    DisplayNextSentence();
                break;
            // Кнопка "Перезапуск сцены"
            case "Restart":
                SceneManager.LoadScene(SceneManager.
                    GetActiveScene().buildIndex);
                break;
        }
    }

    // Функция, запускаемая перед первым кадром
    protected virtual void Start()
    {
        // Получение компонента Transform у иконки кнопки
        icon = gameObject.transform.Find("Icon");
        // Поведение кнопки определяется её названием
        switch (gameObject.name)
        {
            // Кнопка "Играть"
            case "Play":
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Settings"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("AboutUs"));
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // Кнопка "Настройки"
            case "Settings":
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("Play"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("AboutUs"));
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // Кнопка "Громкость"
            case "Volume":
                interactiveObj.Add(GameObject.Find("VolumeSetting"));
                interactiveObj[0].SetActive(false);
                break;
            // Кнопка "О нас"
            case "AboutUs":
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("Play"));
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Settings"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // Кнопка "Получить подсказки"
            case "GetTips":
                interactiveObj.Add(GameObject.Find("PointsMenu"));
                interactiveObj.Add(GameObject.Find("SettingsMenu"));
                interactiveObj.Add(GameObject.Find("Settings"));
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                interactiveObj.Add(GameObject.Find("AboutUs"));
                interactiveObj.Add(GameObject.Find("PlayMenu"));
                interactiveObj.Add(GameObject.Find("Play"));
                interactiveObj.Add(GameObject.Find("Tips"));
                break;
            // Кнопка "Следующий титр"
            case "NextCredit":
                interactiveObj.Add(GameObject.Find("MenuWindow"));
                break;
        }
        
    }
}
