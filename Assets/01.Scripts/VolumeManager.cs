using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Input Fields (1-100)")]
    [SerializeField] private TMP_InputField bgmInput;
    [SerializeField] private TMP_InputField sfxInput;

    private void Start()
    {
        // 1. 기존에 저장된 볼륨 값 로드 (기본값: 1f)
        float savedBGM = PlayerPrefs.GetFloat("Volume_BGM", 1f);
        float savedSFX = PlayerPrefs.GetFloat("Volume_SFX", 1f);

        // 2. BGM 컴포넌트 초기화 및 리스너 등록
        if (bgmSlider != null)
        {
            bgmSlider.value = savedBGM;
            UpdateInputField(bgmInput, savedBGM);
            SoundManager.instance.SetBGMVolume(savedBGM);
            bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        }
        if (bgmInput != null) bgmInput.onEndEdit.AddListener(OnBGMInputEndEdit);

        // 3. SFX 컴포넌트 초기화 및 리스너 등록
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
            UpdateInputField(sfxInput, savedSFX);
            SoundManager.instance.SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }
        if (sfxInput != null) sfxInput.onEndEdit.AddListener(OnSFXInputEndEdit);
    }

    #region BGM 조절 이벤트
    private void OnBGMSliderChanged(float value)
    {
        SoundManager.instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("Volume_BGM", value);
        UpdateInputField(bgmInput, value);
    }

    private void OnBGMInputEndEdit(string text)
    {
        float value = ConvertToVolume(text, bgmSlider != null ? bgmSlider.value : 1f);
        if (bgmSlider != null) bgmSlider.value = value;
        SoundManager.instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("Volume_BGM", value);
        UpdateInputField(bgmInput, value);
    }
    #endregion

    #region SFX 조절 이벤트
    private void OnSFXSliderChanged(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("Volume_SFX", value);
        UpdateInputField(sfxInput, value);
    }

    private void OnSFXInputEndEdit(string text)
    {
        float value = ConvertToVolume(text, sfxSlider != null ? sfxSlider.value : 1f);
        if (sfxSlider != null) sfxSlider.value = value;
        SoundManager.instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("Volume_SFX", value);
        UpdateInputField(sfxInput, value);
    }
    #endregion

    #region 유틸리티 함수 (텍스트 변환 및 업데이트)
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
    #endregion

    private void OnDestroy()
    {
        // 메모리 누수 방지를 위한 리스너 해제
        if (bgmSlider != null) bgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
        if (bgmInput != null) bgmInput.onEndEdit.RemoveListener(OnBGMInputEndEdit);

        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
        if (sfxInput != null) sfxInput.onEndEdit.RemoveListener(OnSFXInputEndEdit);
    }
}