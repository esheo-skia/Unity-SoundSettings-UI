using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundPageController : MonoBehaviour
{
    [Header("Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Value Texts (0~100%)")]
    public TextMeshProUGUI masterValueText;
    public TextMeshProUGUI bgmValueText;
    public TextMeshProUGUI sfxValueText;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private void Start()
    {
        // 각 슬라이더가 연결된 경우에만 세팅 + 이벤트 등록
        if (masterSlider != null)
        {
            SetupSlider(masterSlider);
            masterSlider.onValueChanged.AddListener(_ => OnVolumeChanged());
        }

        if (bgmSlider != null)
        {
            SetupSlider(bgmSlider);
            bgmSlider.onValueChanged.AddListener(_ => OnVolumeChanged());
        }

        if (sfxSlider != null)
        {
            SetupSlider(sfxSlider);
            sfxSlider.onValueChanged.AddListener(_ => OnVolumeChanged());
        }

        OnVolumeChanged();  // 초기값 한 번 반영
    }

    private void SetupSlider(Slider slider)
    {
        if (slider == null) return;    // 안전장치

        slider.minValue = 0f;
        slider.maxValue = 1f;

        if (slider.value == 0f)
            slider.value = 1f;        // 기본 100%에서 시작
    }

    private void OnVolumeChanged()
    {
        float master = masterSlider != null ? masterSlider.value : 1f;
        float bgm    = bgmSlider    != null ? bgmSlider.value    : 1f;
        float sfx    = sfxSlider    != null ? sfxSlider.value    : 1f;

        // 1) 숫자 텍스트 업데이트
        if (masterValueText != null)
            masterValueText.text = Mathf.RoundToInt(master * 100f) + "%";
        if (bgmValueText != null)
            bgmValueText.text = Mathf.RoundToInt(bgm * 100f) + "%";
        if (sfxValueText != null)
            sfxValueText.text = Mathf.RoundToInt(sfx * 100f) + "%";

        // 2) 실제 볼륨 적용
        AudioListener.volume = master;

        if (bgmSource != null)
            bgmSource.volume = master * bgm;

        if (sfxSource != null)
            sfxSource.volume = master * sfx;

        // 3) (선택) 콘솔에서 값 확인용
        Debug.Log($"[Sound] master={master:F2}, bgm={bgm:F2}, sfx={sfx:F2}");
    }
}
