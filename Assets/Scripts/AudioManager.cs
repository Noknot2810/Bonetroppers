using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// ������ ���������� �� ��������������� ������ � �������������
public class AudioManager : MonoBehaviour
{
    public AudioMixer mixerToEdit;  // ������������ ������
    public Slider sliderToEdit;     // ������������ ������� ���������
    public Sound[] sounds;          // ������ ��������-������� �������

    private Sound lastSound;        // ��������� ������������� �������
    private float savedVolume = -1; // ����������� ��������� �����

    // �������, ����������� ��� �������� �����
    private void Awake()
    {
        // �������� �� ��������-������� ��������� AudioSource
        // � ���������� �����������
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

    // �������, ����������� ����� ������ ������
    private void Start()
    {
        // ��������� ���������� �������� �������� ���������
        sliderToEdit.value = PlayerPrefs.GetFloat("userVolume", 0.7f);
        // ������ ����������� ���� �������� ����.
        Play("MenuTheme");
    }

    // ������� ������ ������ ������������ ������������
    public void Play(string name, bool offLast = true)
    {
        // ��������� ��������� ����������� ����
        if (offLast == true && lastSound != null)
        {
            lastSound.source.Stop();
        }

        // ����� ����� ����������� ���� � ������� ��������-�������
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        // ������ ��������� ����������� ����
        s.source.Play();
        // ��������������� ��������� ������������� �������
        lastSound = s;
    }

    // ������� ������ ��������� �������� ������������ ������������
    public void Stop()
    {
        lastSound.source.Stop();
    }

    // ������ ������ ����������� ���������
    public void PlayTransit(string name)
    {
        // ���������� ��������� ����� ��������� ������� � � ����������
        if (lastSound != null && savedVolume < 0)
        {
            savedVolume = lastSound.source.volume;
            lastSound.source.volume = 0f;
        }

        // ����� ����� ����������� ���� � ������� ��������-�������
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        // ������ ��������� ����������� ����
        s.source.Play();

        // ���������� � ��������� ��������� ���������� �������
        StopAllCoroutines();
        StartCoroutine(ReturnVolume());
    }

    // ������� ����������� �������� ��������� ��������� �������
    IEnumerator ReturnVolume()
    {
        yield return new WaitForSeconds(4f);
        lastSound.source.volume = savedVolume;
        savedVolume = -1;
    }

    // �������-������ ��� ��������� ������� ������ ��������������
    public bool IsPlaying()
    {
        return lastSound.source.isPlaying;
    }

    // ������� ��������� ��������� � ������� ��������
    public void SetVolume(float sliderValue)
    {
        mixerToEdit.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        // ���������� ���������������� ��������� � ������
        PlayerPrefs.SetFloat("userVolume", sliderValue);
    }
    
}
