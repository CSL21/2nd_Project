using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    MainScene,
    GameScene,
}

public enum SFXType
{
    Die,
    EnemyBreak,
    Attack,
    Button,
    GearSelect,
    FacilityClick,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] soundClips;

    [Header("SFX Cooldown Settings")]
    private Dictionary<SFXType, float> lastPlayTimes = new Dictionary<SFXType, float>();
    private const float SFX_COOLDOWN = 0.04f;

    private float maxBGMVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (bgmAudioSource != null) maxBGMVolume = bgmAudioSource.volume;
            InitializeCooldowns();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCooldowns()
    {
        foreach (SFXType type in System.Enum.GetValues(typeof(SFXType)))
        {
            lastPlayTimes[type] = 0f;
        }
    }

    #region BGM 제어 기능 (단순화 완료)

    public void ChangeBGM(BGMType type)
    {
        if (bgmAudioSource == null) return;

        int clipIndex = (int)type;
        if (clipIndex >= bgmClips.Length || bgmClips[clipIndex] == null) return;

        if (bgmAudioSource.clip == bgmClips[clipIndex] && bgmAudioSource.isPlaying) return;

        bgmAudioSource.Stop();
        bgmAudioSource.clip = bgmClips[clipIndex];
        bgmAudioSource.volume = maxBGMVolume;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    public void StopBGM() => bgmAudioSource?.Stop();

    public void SetBGMVolume(float volume)
    {
        maxBGMVolume = Mathf.Clamp01(volume);
        if (bgmAudioSource != null) bgmAudioSource.volume = maxBGMVolume;
    }

    #endregion

    #region SFX 제어 기능

    public void PlaySFX(SFXType type)
    {
        if (sfxAudioSource == null) return;
        if ((int)type >= soundClips.Length || soundClips[(int)type] == null) return;

        if (Time.time - lastPlayTimes[type] < SFX_COOLDOWN) return;

        lastPlayTimes[type] = Time.time;
        sfxAudioSource.PlayOneShot(soundClips[(int)type]);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxAudioSource != null) sfxAudioSource.volume = Mathf.Clamp01(volume);
    }

    #endregion
}