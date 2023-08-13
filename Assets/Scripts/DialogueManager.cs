using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������, �������������� ������ ���������� ����
public class DialogueManager : MonoBehaviour
{
    // ��������� ��������� ����������� ����
    public Text TextField;
    // ���������, ����� �� �������� �� ����� ����� ��� ������� �� �����
    public bool UseTyping = true;

    // ������� ����������� � ������ �������
    private Queue<string> sentences;

    // �������, ����������� ����� ������ ������
    void Start()
    {
        sentences = new Queue<string>();
    }

    // ������� ��������� �������
    public void StartDialogue(List<string> texts)
    {
        // ������� ������� ����������� �������
        sentences.Clear();
        // ���������� ������� ����������� �������
        foreach(string text in texts)
        {
            sentences.Enqueue(text);
        }
        // ����� ������� ����������� �� �������
        DisplayNextSentence();
    }

    // ������� ��������� ���������� ����������� �� ������� �������
    public void DisplayNextSentence()
    {
        // ���������� ������� � ������ ���������� ����������� � �������
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        // ��������� ������ ����������� �� �������
        string sentence = sentences.Dequeue();

        if (UseTyping == true)
        {
            // ��������� ������������� ������ �����������
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            // ��������� ������� ������ �����������
            TextField.text = sentence;
        }
    }

    // ������� ������������� ������ ����� �����������
    IEnumerator TypeSentence(string sentence)
    {
        // ������� ���������� ����
        TextField.text = "";
        // ������������ ���������� ���������� ���� � ���������
        foreach (char letter in sentence.ToCharArray())
        {
            TextField.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
    }

    // ������� ���������� �������
    public void EndDialogue()
    {
        // ��������� ����������� ���� ������������ ��� ���������
        switch (gameObject.name)
        {
            // ���������� ���� ��������
            case "ExaminationDialog":
                // ����� ������ ��������
                FindObjectOfType<ExaminationScript>().
                    StartExamination();
                break;
            // ���������� ���� �� �������� ����
            case "MenuWindow":
                // ������� ������ "� ���" � �������� ����
                ButtonsScript scr = GameObject.Find("AboutUs").
                    GetComponent<ButtonsScript>();
                scr.OnMouseUpAsButton();
                scr.OnMouseUp();
                break;
        }
    }
}
