using UnityEngine;

// Скрипт, устанавливающий в игре фиксированное количество кадров в секунду
public class FPSControl : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
