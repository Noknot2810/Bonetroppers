using UnityEngine;
using UnityEngine.Audio;

// Объект для хранения мелодии или звукового эффекта
[System.Serializable]
public class Sound
{
    public string name;             // Название мелодиии
    public AudioClip clip;          // Аудиоклип данной мелодии
    public AudioMixerGroup mixer;   // Свзяанный с мелодией миксер

    [Range(0f, 1f)]
    public float volume;            // Громкость мелодии
    [Range(1f, 3f)]
    public float pitch;             // Скорость вопроизведения мелодии

    public bool loop;               // Флаг зацикленной мелодии

    [HideInInspector]
    public AudioSource source;      // Объект AudioSource,
                                    // привязываемый к мелодии
}
