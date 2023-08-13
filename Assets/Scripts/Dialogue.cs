using UnityEngine;

// Объект для хранения текстового диалога
[CreateAssetMenu(fileName = "Dialogue", menuName = "Gameplay/New Dialogue")]
public class Dialogue : ScriptableObject
{
    // Текст диалога
    [SerializeField] [TextArea(3, 10)] private string _text;
    // Указатель, является ли диалог заданием
    [SerializeField] private bool _isTask;
    // Название уровня задания
    [SerializeField] private string _level;
    // Массив вопросов в рамках задания
    [SerializeField] private Question[] _questions;

    // Публичные переменные для доступа к полям объекта
    #pragma warning disable IDE1006
    public string text => this._text;
    public bool isTask => this._isTask;
    public string level => this._level;
    public Question[] questions => this._questions;
    #pragma warning restore IDE1006
}
