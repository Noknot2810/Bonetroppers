using UnityEngine;

// ������ ��� �������� ���������� �������
[CreateAssetMenu(fileName = "Dialogue", menuName = "Gameplay/New Dialogue")]
public class Dialogue : ScriptableObject
{
    // ����� �������
    [SerializeField] [TextArea(3, 10)] private string _text;
    // ���������, �������� �� ������ ��������
    [SerializeField] private bool _isTask;
    // �������� ������ �������
    [SerializeField] private string _level;
    // ������ �������� � ������ �������
    [SerializeField] private Question[] _questions;

    // ��������� ���������� ��� ������� � ����� �������
    #pragma warning disable IDE1006
    public string text => this._text;
    public bool isTask => this._isTask;
    public string level => this._level;
    public Question[] questions => this._questions;
    #pragma warning restore IDE1006
}
