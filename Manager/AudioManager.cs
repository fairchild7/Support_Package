using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    const string MIXER_BGM = "BGMVolume";
    const string MIXER_SFX = "SFXVolume";

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource bgmAudioSource;

    [SerializeField] AudioSource sfxAudioSourceShell;

    [SerializeField] SerializedDictionary<BGMType, AudioClip> bgmClipMap = new();
    [SerializeField] SerializedDictionary<SFXType, AudioClip> sfxClipMap = new();

    private List<AudioSource> sfxShellPool = new();
    private BGMType currentBGM;

    private Dictionary<SFXType, bool> playingSfx = new();
    private Dictionary<SFXType, bool> cooldownSfx = new();

    //Serialized for debugging
    [SerializeField] SerializedDictionary<AudioSource, Coroutine> waitingToClaimSFXSources = new();

    public void PlayBGM(BGMType type)
    {
        if (currentBGM == type)
            return;

        bgmAudioSource.clip = bgmClipMap[type];
        bgmAudioSource.Play();

        currentBGM = type;
    }

    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
            bgmAudioSource.Stop();
    }

    public AudioSource PlaySFX(SFXType type, bool isLoop = false, Transform target = null, bool setParent = false, bool canPlayWhenThereIsDuplicatedSFX = true,
        float delayBeforeCanPlayAgain = 0)
    {
        if (!canPlayWhenThereIsDuplicatedSFX)
        {
            if (!playingSfx.ContainsKey(type))
            {
                playingSfx.Add(type, true);
            }
            else
            {
                if (playingSfx[type])
                {
                    return null;
                }
                else
                {
                    playingSfx[type] = true;
                }
            }
        }

        if (delayBeforeCanPlayAgain > 0)
        {
            if (!cooldownSfx.ContainsKey(type))
            {
                cooldownSfx.Add(type, true);
                DOVirtual.DelayedCall(delayBeforeCanPlayAgain, () =>
                {
                    cooldownSfx[type] = false;
                });
            }
            else
            {
                if (cooldownSfx[type])
                {
                    return null;
                }
                else
                {
                    cooldownSfx[type] = true;
                    DOVirtual.DelayedCall(delayBeforeCanPlayAgain, () =>
                    {
                        cooldownSfx[type] = false;
                    });
                }
            }
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

        if (target != null)
        {
            sfxAudioSource.transform.position = target.position;
        }
        if (setParent)
        {
            sfxAudioSource.transform.parent = target;
        }

        sfxAudioSource.loop = isLoop;
        sfxAudioSource.clip = sfxClipMap[type];
        sfxAudioSource.spatialBlend = (target == null) ? 0 : 1;
        sfxAudioSource.gameObject.SetActive(true);
        sfxAudioSource.Play();

        if (!isLoop)
        {
            Coroutine cor = StartCoroutine(IECollectSFXShell(sfxAudioSource, sfxClipMap[type].length, canPlayWhenThereIsDuplicatedSFX, type));
            waitingToClaimSFXSources.Add(sfxAudioSource, cor);
        }

        return sfxAudioSource;
    }

    public void StopSFX(AudioSource audioSource)
    {
        if (audioSource == null)
            return;

        if (!waitingToClaimSFXSources.ContainsKey(audioSource))
        {
            return;
        }

        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
        audioSource.transform.parent = transform;
        sfxShellPool.Add(audioSource);

        waitingToClaimSFXSources.Remove(audioSource);
    }

    private IEnumerator IECollectSFXShell(AudioSource audioSource, float delay, bool canDuplicatedPlaying = true, SFXType nonDuplicatedSFXType = 0)
    {
        yield return new WaitForSeconds(delay);

        StopSFX(audioSource);

        if (!canDuplicatedPlaying)
        {
            if (playingSfx.ContainsKey(nonDuplicatedSFXType))
            {
                playingSfx[nonDuplicatedSFXType] = false;
            }
        }
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

    public void MuteMusicVolume()
    {
        audioMixer.SetFloat(MIXER_BGM, -80f);
    }

    public void MuteSFXVolume()
    {
        audioMixer.SetFloat(MIXER_SFX, -80f);
    }

    public void UnmuteMusicVolume()
    {
        audioMixer.SetFloat(MIXER_BGM, 0f);
    }

    public void UnmuteSFXVolume()
    {
        audioMixer.SetFloat(MIXER_SFX, 0f);
    }
    #endregion
}