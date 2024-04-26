using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioMixerGroup sfxMixer;
    [SerializeField] List<SFXAudioClip> clips = new List<SFXAudioClip>();

    private Dictionary<SFXType, AudioSource> createdSFXAudio = new Dictionary<SFXType, AudioSource>();

    const string MIXER_BGM = "BGMVolume";
    const string MIXER_SFX = "SFXVolume";

    [SerializeField] float jumpSFXCooldown = 0.5f;

    private float jumpSFXTimer = 0f;

    private void Awake()
    {
        this.RegisterListener(EventID.SetMusicSetting, (param) =>
        {
            if ((bool)param)
            {
                UnmuteMusicVolume();
            }
            else
            {
                MuteMusicVolume();
            }
        });
        this.RegisterListener(EventID.SetSoundSetting, (param) =>
        {
            if ((bool)param)
            {
                UnmuteSFXVolume();
            }
            else
            {
                MuteSFXVolume();
            }
        });
        this.RegisterListener(EventID.PlaySFX, (param) => PlaySFXAudio((SFXType)param));
        this.RegisterListener(EventID.StopSFX, (param) => StopSFXAudio((SFXType)param));
    }

    private void Update()
    {
        jumpSFXTimer += Time.deltaTime;     
    }

    private void PlaySFXAudio(SFXType type)
    {
        if (!UIManager.Ins.GetUI<UISettings>().InitSetting)
        {
            return;
        }
        if (!DataManager.Ins.GetSoundSetting())
        {
            return;
        }

        if (type == SFXType.Jump || type == SFXType.GravityCoilJump)
        {
            if (jumpSFXTimer < jumpSFXCooldown)
            {
                return;
            }
            jumpSFXTimer = 0f;
        }

        if (createdSFXAudio.ContainsKey(type))
        {
            createdSFXAudio[type].Play();
        }
        else
        {
            AudioSource sfxAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource.clip = GetClip(type);
            sfxAudioSource.loop = false;
            sfxAudioSource.outputAudioMixerGroup = sfxMixer;
            sfxAudioSource.Play();

            createdSFXAudio.Add(type, sfxAudioSource);
        }
    }

    private void StopSFXAudio(SFXType type)
    {
        if (createdSFXAudio.ContainsKey(type))
        {
            createdSFXAudio[type].Stop();
        }
    }

    private AudioClip GetClip(SFXType type)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i].type == type)
            {
                return clips[i].clip;
            }
        }

        return null;
    }

    #region Better way to control volume
    private void SetMusicVolume(float value)
    {
        //value range from 0.0001 to 1
        audioMixer.SetFloat(MIXER_BGM, Mathf.Log10(value) * 20f);
    }

    private void SetSFXVolume(float value)
    {
        //value range from 0.0001 to 1
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20f);
    }

    private void MuteMusicVolume()
    {
        audioMixer.SetFloat(MIXER_BGM, -80f);
    }

    private void MuteSFXVolume()
    {
        audioMixer.SetFloat(MIXER_SFX, -80f);
    }

    private void UnmuteMusicVolume()
    {
        audioMixer.SetFloat(MIXER_BGM, 0f);
    }

    private void UnmuteSFXVolume()
    {
        audioMixer.SetFloat(MIXER_SFX, 0f);
    }
    #endregion
}

[System.Serializable]
public class SFXAudioClip
{
    public SFXType type;
    public AudioClip clip;
}
