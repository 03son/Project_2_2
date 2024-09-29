using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // 배경음악 소스
    public AudioSource[] soundEffects; // 효과음 소스

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        // 배경음악 재생
        backgroundMusic.Play();
    }

    public void PlaySoundEffect(int index)
    {
        // 효과음 재생
        if (index >= 0 && index < soundEffects.Length)
        {
            soundEffects[index].Play();
        }
    }
}
