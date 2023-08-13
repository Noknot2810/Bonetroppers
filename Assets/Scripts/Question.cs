using UnityEngine;

// Объект для хранения вопроса экзамена
[System.Serializable]
[ExecuteInEditMode]
public class Question
{
    [TextArea(3, 10)]
    public string question;                 // Текст вопроса
    public string[] options = new string[4];// Тексты вариантов ответа
    public int correctIs;                   // Номер правильного ответа
}
