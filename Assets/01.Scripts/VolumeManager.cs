using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Input Fields (1-100)")]
    [SerializeField] private TMP_InputField bgmInput;
    [SerializeField] private TMP_InputField ambientInput;
    [SerializeField] private TMP_InputField sfxInput;

    private void Start()
    {
        float savedBGM = PlayerPrefs.GetFloat("Volume_BGM", 1f);
        float savedAmbient = PlayerPrefs.GetFloat("Volume_Ambient", 1f);
        float savedSFX = PlayerPrefs.GetFloat("Volume_SFX", 1f);

        if (bgmSlider != null)
        {
            bgmSlider.value = savedBGM;
            UpdateInputField(bgmInput, savedBGM);
            SoundManager.Instance.SetBGMVolume(savedBGM);
            bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        }
        if (bgmInput != null) bgmInput.onEndEdit.AddListener(OnBGMInputEndEdit);

        if (ambientSlider != null)
        {
            ambientSlider.value = savedAmbient;
            UpdateInputField(ambientInput, savedAmbient);
            SoundManager.Instance.SetAmbientVolume(savedAmbient);
            ambientSlider.onValueChanged.AddListener(OnAmbientSliderChanged);
        }
        if (ambientInput != null) ambientInput.onEndEdit.AddListener(OnAmbientInputEndEdit);

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
            UpdateInputField(sfxInput, savedSFX);
            SoundManager.Instance.SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }
        if (sfxInput != null) sfxInput.onEndEdit.AddListener(OnSFXInputEndEdit);
    }

    private void OnBGMSliderChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("Volume_BGM", value);
        UpdateInputField(bgmInput, value);
    }

    private void OnBGMInputEndEdit(string text)
    {
        float value = ConvertToVolume(text, bgmSlider != null ? bgmSlider.value : 1f);
        if (bgmSlider != null) bgmSlider.value = value;
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("Volume_BGM", value);
        UpdateInputField(bgmInput, value);
    }

    private void OnAmbientSliderChanged(float value)
    {
        SoundManager.Instance.SetAmbientVolume(value);
        PlayerPrefs.SetFloat("Volume_Ambient", value);
        UpdateInputField(ambientInput, value);
    }

    private void OnAmbientInputEndEdit(string text)
    {
        float value = ConvertToVolume(text, ambientSlider != null ? ambientSlider.value : 1f);
        if (ambientSlider != null) ambientSlider.value = value;
        SoundManager.Instance.SetAmbientVolume(value);
        PlayerPrefs.SetFloat("Volume_Ambient", value);
        UpdateInputField(ambientInput, value);
    }

    private void OnSFXSliderChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("Volume_SFX", value);
        UpdateInputField(sfxInput, value);
    }

    private void OnSFXInputEndEdit(string text)
    {
        float value = ConvertToVolume(text, sfxSlider != null ? sfxSlider.value : 1f);
        if (sfxSlider != null) sfxSlider.value = value;
        SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("Volume_SFX", value);
        UpdateInputField(sfxInput, value);
    }

    private void UpdateInputField(TMP_InputField inputField, float volumeValue)
    {
        if (inputField != null)
        {
            int displayValue = Mathf.RoundToInt(volumeValue * 100f);
            inputField.text = displayValue.ToString();
        }
    }

    private float ConvertToVolume(string text, float defaultValue)
    {
        if (int.TryParse(text, out int intValue))
        {
            intValue = Mathf.Clamp(intValue, 0, 100);
            return intValue / 100f;
        }
        return defaultValue;
    }

    private void OnDestroy()
    {
        if (bgmSlider != null) bgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
        if (bgmInput != null) bgmInput.onEndEdit.RemoveListener(OnBGMInputEndEdit);

        if (ambientSlider != null) ambientSlider.onValueChanged.RemoveListener(OnAmbientSliderChanged);
        if (ambientInput != null) ambientInput.onEndEdit.RemoveListener(OnAmbientInputEndEdit);

        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
        if (sfxInput != null) sfxInput.onEndEdit.RemoveListener(OnSFXInputEndEdit);
    }
}