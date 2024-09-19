using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] AudioMixer audioMixer;

    [SerializeField] AudioClip bgm1;
    [SerializeField] AudioClip bgm2;

    [SerializeField] AudioClip se1;
    [SerializeField] AudioClip se2;

    public void SetBGM()
    {
        audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM"));
    }

    public void SetSE()
    {
        audioMixer.SetFloat("SE", PlayerPrefs.GetFloat("SE"));
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void StopSE()
    {
        seAudioSource.Stop();
    }

    public void PlayBGM1()
    {
        //bgmAudioSource.PlayOneShot(bgm1);
    }

    public void PlayBGM2()
    {
        //bgmAudioSource.PlayOneShot(bgm2);
    }

    public void PlaySE1()
    {
        seAudioSource.PlayOneShot(se1);
    }

    public void PlaySE2()
    {
        seAudioSource.PlayOneShot(se2);
    }
}