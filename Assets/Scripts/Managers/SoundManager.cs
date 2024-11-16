using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // 오디오 클립 저장소
    public AudioClip[] bgmClips; // 배경음악 클립 배열
    public AudioClip[] sfxClips; // 효과음 클립 배열
    public AudioSource bgmSource; // 배경음 재생 오디오 소스
    public AudioSource sfxSource; // 효과음 재생 오디오 소스

    // 현재 재생 중인 BGM 클립을 추적
    private AudioClip currentBGM;

    // 배경음악 재생
    public void PlayBGM(int bgmIndex, bool loop = true)
    {
        if (bgmIndex < 0 || bgmIndex >= bgmClips.Length) return;

        AudioClip bgmClip = bgmClips[bgmIndex];
        if (currentBGM == bgmClip) return; // 이미 같은 BGM이 재생 중이면 패스

        bgmSource.clip = bgmClip;
        bgmSource.loop = loop;
        bgmSource.Play();
        currentBGM = bgmClip;
    }

    // 효과음 재생 (단일 사운드)
    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex < 0 || sfxIndex >= sfxClips.Length) return;

        sfxSource.PlayOneShot(sfxClips[sfxIndex]);
    }
}
