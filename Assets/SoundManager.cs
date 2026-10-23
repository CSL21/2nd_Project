using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    MainScene,
    FieldScene,
    GameScene,
}

public enum SFXType
{
    Arrow,
    Die,
    Attack,
    Button,
    Casting,
    Catch,
    EquipGear,
    ShipClick,
    FacilityClick,
    GearClick,
    ProductComplete,
    SellItem
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public static SoundManager Instance => instance;

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

            maxBGMVolume = bgmAudioSource.volume;
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

    #region BGM 제어 기능

    public void ChangeBGM(BGMType type, float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(BGMChangeRoutine(type, duration));
    }

    public void StopBGM() => bgmAudioSource?.Stop();

    public void SetBGMVolume(float volume)
    {
        maxBGMVolume = Mathf.Clamp01(volume);
        if (bgmAudioSource != null) bgmAudioSource.volume = maxBGMVolume;
    }

    public void SetAmbientVolume(float volume)
    {
        SetBGMVolume(volume);
    }

    private IEnumerator BGMChangeRoutine(BGMType type, float duration)
    {
        if ((int)type >= bgmClips.Length || bgmClips[(int)type] == null) yield break;

        if (duration <= 0f)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = bgmClips[(int)type];
            bgmAudioSource.volume = maxBGMVolume;
            bgmAudioSource.Play();
            yield break;
        }

        float startVolume = bgmAudioSource.volume;
        float halfDuration = duration / 2f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, timer / halfDuration);
            yield return null;
        }

        bgmAudioSource.volume = 0f;
        bgmAudioSource.Stop();

        bgmAudioSource.clip = bgmClips[(int)type];
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();

        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(0f, maxBGMVolume, timer / halfDuration);
            yield return null;
        }

        bgmAudioSource.volume = maxBGMVolume;
    }

    #endregion

    #region SFX 제어 기능

    public void PlaySFX(SFXType type)
    {
        if ((int)type >= soundClips.Length || soundClips[(int)type] == null)
            return;

        if (Time.time - lastPlayTimes[type] < SFX_COOLDOWN)
            return;

        lastPlayTimes[type] = Time.time;
        sfxAudioSource.PlayOneShot(soundClips[(int)type]);
    }

    public void SetSFXVolume(float volume) => sfxAudioSource.volume = Mathf.Clamp01(volume);

    #endregion
}