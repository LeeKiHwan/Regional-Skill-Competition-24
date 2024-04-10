using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource bgmSource;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }

        bgmSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip sfx, float volume = 1, bool isLoop = false, float pitch = 1)
    {
        AudioSource source = new GameObject(sfx.name).AddComponent<AudioSource>();
        source.clip = sfx;
        source.volume = volume;
        source.loop = isLoop;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, sfx.length);
    }

    public void PlayBGM(AudioClip bgm)
    {
        bgmSource.clip = bgm;
        bgmSource.Play();
    }
}
