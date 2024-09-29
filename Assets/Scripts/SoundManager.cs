using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // ������� �ҽ�
    public AudioSource[] soundEffects; // ȿ���� �ҽ�

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        // ������� ���
        backgroundMusic.Play();
    }

    public void PlaySoundEffect(int index)
    {
        // ȿ���� ���
        if (index >= 0 && index < soundEffects.Length)
        {
            soundEffects[index].Play();
        }
    }
}
