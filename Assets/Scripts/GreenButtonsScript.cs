using UnityEngine;

// Скрипт, отвечающий за работу скрываемых кнопок чекпоинтов,
// функционирующих по старому образцу (без компонента Button)
public class GreenButtonsScript : GhostButtonsScript
{
    private int personalNum = 0; // Номер кнопки

    // Функция, запускаемая при нажатии на объект
    public override void OnMouseDown()
    {
        if (anim != null && is_active == true)
        {
            // Запуск анимации нажатия
            anim.SetBool("onClick", true);
        }
    }

    // Функция, запускаемая при отжатии объекта
    public override void OnMouseUp()
    {
        if (anim != null && is_active == true)
        {
            // Запуск анимации отжатия
            anim.SetBool("onClick", false);
        }
    }

    // Функция, запускаемая при нажатии и отжатии объекта
    public override void OnMouseUpAsButton()
    {
        if (is_active == true)
        {
            // Отключение главного меню
            foreach (GameObject obj in 
                GameObject.FindGameObjectsWithTag("MainMenuElem"))
            {
                obj.SetActive(false);
            }
            // Запуск экзамена с указанного чекпоинта
            FindObjectOfType<ExaminationScript>().
                StartGameFromCheck(personalNum);
        }
    }

    // Функция разблокировки кнопки
    public void UnlockButton()
    {
        is_active = true;
        anim.SetBool("alternative", true);
        // Получение номера кнопки
        personalNum = int.Parse(gameObject.name.
            Substring(gameObject.name.IndexOf("#") + 1));
    }
}
