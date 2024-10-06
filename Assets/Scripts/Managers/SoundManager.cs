using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // ����� Ŭ�� �����
    public AudioClip[] bgmClips; // ������� Ŭ�� �迭
    public AudioClip[] sfxClips; // ȿ���� Ŭ�� �迭
    public AudioSource bgmSource; // ����� ��� ����� �ҽ�
    public AudioSource sfxSource; // ȿ���� ��� ����� �ҽ�

    // ���� ��� ���� BGM Ŭ���� ����
    private AudioClip currentBGM;

    // ������� ���
    public void PlayBGM(int bgmIndex, bool loop = true)
    {
        if (bgmIndex < 0 || bgmIndex >= bgmClips.Length) return;

        AudioClip bgmClip = bgmClips[bgmIndex];
        if (currentBGM == bgmClip) return; // �̹� ���� BGM�� ��� ���̸� �н�

        bgmSource.clip = bgmClip;
        bgmSource.loop = loop;
        bgmSource.Play();
        currentBGM = bgmClip;
    }

    // ȿ���� ��� (���� ����)
    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex < 0 || sfxIndex >= sfxClips.Length) return;

        sfxSource.PlayOneShot(sfxClips[sfxIndex]);
    }
}
