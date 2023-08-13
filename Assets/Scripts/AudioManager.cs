using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Скрипт отвечающий за воспроизведение музыки и аудиоэффектов
public class AudioManager : MonoBehaviour
{
    public AudioMixer mixerToEdit;  // Используемый миксер
    public Slider sliderToEdit;     // Используемый слайдер громкости
    public Sound[] sounds;          // Массив объектов-мелодий проекта

    private Sound lastSound;        // Последняя проигрываемая мелодия
    private float savedVolume = -1; // Сохраняемые настройки звука

    // Функция, запускаемая при загрузке сцены
    private void Awake()
    {
        // Создание их объектов-мелодий компонент AudioSource
        // с указанными настройками
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixer;
        }
    }

    // Функция, запускаемая перед первым кадром
    private void Start()
    {
        // Установка начального значения слайдера громкости
        sliderToEdit.value = PlayerPrefs.GetFloat("userVolume", 0.7f);
        // Запуск музыкальной темы главного меню.
        Play("MenuTheme");
    }

    // Функция вызова нового музыкального произведения
    public void Play(string name, bool offLast = true)
    {
        // Остановка последней музыкальной темы
        if (offLast == true && lastSound != null)
        {
            lastSound.source.Stop();
        }

        // Поиск новой музыкальной темы в массиве объектов-мелодий
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        // Запуск найденной музыкальной темы
        s.source.Play();
        // Переопределение последней проигрываемой мелодии
        lastSound = s;
    }

    // Функция прямой остановки текущего музыкального произведения
    public void Stop()
    {
        lastSound.source.Stop();
    }

    // Функия вызова музыкальной перебивки
    public void PlayTransit(string name)
    {
        // Сохранение настройки звука последней мелодии и её заглушение
        if (lastSound != null && savedVolume < 0)
        {
            savedVolume = lastSound.source.volume;
            lastSound.source.volume = 0f;
        }

        // Поиск новой музыкальной темы в массиве объектов-мелодий
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        // Запуск найденной музыкальной темы
        s.source.Play();

        // Перезапуск с задержкой последней сохранённой мелодии
        StopAllCoroutines();
        StartCoroutine(ReturnVolume());
    }

    // Функция возвращения исходной громкости последней мелодии
    IEnumerator ReturnVolume()
    {
        yield return new WaitForSeconds(4f);
        lastSound.source.volume = savedVolume;
        savedVolume = -1;
    }

    // Функция-геттер для получения статуса работа аудиоменеджера
    public bool IsPlaying()
    {
        return lastSound.source.isPlaying;
    }

    // Функция настройки громкости с помощью слайдера
    public void SetVolume(float sliderValue)
    {
        mixerToEdit.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        // Сохранение пользовательской настройки в памяти
        PlayerPrefs.SetFloat("userVolume", sliderValue);
    }
    
}
