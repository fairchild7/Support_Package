using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] AudioMixerGroup sfxMixer;

    #region Pool Part
    [SerializeField] AudioSource sfxAudioSourceShell;

    private List<AudioSource> sfxShellPool = new List<AudioSource>();

    #endregion

    [SerializeField] List<BGMAudio> bgms = new List<BGMAudio>();
    [SerializeField] List<SFXAudioClip> clips = new List<SFXAudioClip>();

    const string MIXER_BGM = "BGMVolume";
    const string MIXER_SFX = "SFXVolume";

    AudioSource currentBGM;

   
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
        this.RegisterListener(EventID.PlaySFX, (param) => PlaySFX((SFXType)param));
        //this.RegisterListener(EventID.StopSFX, (param) => StopSFXAudio((SFXType)param));
    }

    public void PlayBGM(BGMType type)
    {
        for (int i = 0; i < bgms.Count; i++)
        {
            if (bgms[i].type == type)
            {
                if (currentBGM == bgms[i].audioSource)
                {
                    return;
                }
                else
                {
                    if (currentBGM != null)
                    {
                        if (currentBGM.isPlaying)
                        {
                            currentBGM.Stop();
                        }
                    }
                    currentBGM = bgms[i].audioSource;
                    currentBGM.Play();
                    return;
                }
            }
        }
    }

    public void StopBGM()
    {
        if (currentBGM != null)
        {
            if (currentBGM.isPlaying)
            {
                currentBGM.Stop();
                currentBGM = null;
            }
        }
    }

    public void PlaySFX(SFXType type)
    {
        if (!UIManager.Ins.GetUI<UISettings>().InitSetting)
        {
            return;
        }
        if (!DataManager.Ins.GetSoundSetting())
        {
            return;
        }

        AudioSource sfxAudioSource;
        if (sfxShellPool.Count > 0)
        {
            sfxAudioSource = sfxShellPool[0];

            sfxShellPool.Remove(sfxAudioSource);   
        }
        else
        {
            sfxAudioSource = Instantiate(sfxAudioSourceShell, transform);
        }

        InitSFXAudioSource(sfxAudioSource, type);
        sfxAudioSource.gameObject.SetActive(true);
        sfxAudioSource.Play();
        StartCoroutine(IEWaitToCollectSFXShell(sfxAudioSource));
    }

    [ContextMenu("Regen SFX List")]
    private void RegenSFX()
    {
        foreach (SFXType value in System.Enum.GetValues(typeof(SFXType)))
        {
            SFXAudioClip clip = new SFXAudioClip(value, null);
            clips.Add(clip);
        }
    }

    private void InitSFXAudioSource(AudioSource audioSource, SFXType type)
    {
        audioSource.clip = GetClip(type);
    }

    private IEnumerator IEWaitToCollectSFXShell(AudioSource audioSource)
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = null;
                audioSource.gameObject.SetActive(false);
                sfxShellPool.Add(audioSource);
                yield break;
            }

            yield return null;
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

    public SFXAudioClip(SFXType type, AudioClip clip)
    {
        this.type = type;
        this.clip = clip;
    }
}

[System.Serializable]
public class BGMAudio
{
    public BGMType type;
    public AudioSource audioSource;
}
