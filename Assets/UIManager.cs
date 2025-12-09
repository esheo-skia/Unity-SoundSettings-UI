using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // ✅ 메인 메뉴(Graphic / Sound / Info 버튼 3개 들어있는 패널)
    [Header("Main Menu")]
    public GameObject mainMenuPanel;     // Panel_MenuButtons

    // ✅ 각 페이지 패널
    [Header("Pages")]
    public GameObject pageGraphic;       // Page_Graphic
    public GameObject pageSound;         // Page_Sound
    public GameObject pageInfo;          // Page_Info

    // ✅ 각 페이지 CanvasGroup (Fade-in 용)
    public CanvasGroup cgGraphic;
    public CanvasGroup cgSound;
    public CanvasGroup cgInfo;

    // ✅ Graphic Page UI 요소
    [Header("Graphic Page UI")]
    public TMP_Dropdown dropdownResolution;
    public Toggle toggleFullscreen;
    public Slider sliderBrightness;
    public Image brightnessOverlay;      // 화면 어두워지는 오버레이 이미지

    void Start()
    {
        // 시작하면 메인 메뉴만 보이게
        ShowMainMenu();

        // === UI 이벤트 등록 === //
        if (dropdownResolution != null)
            dropdownResolution.onValueChanged.AddListener(OnResolutionChanged);

        if (toggleFullscreen != null)
            toggleFullscreen.onValueChanged.AddListener(OnFullscreenChanged);

        if (sliderBrightness != null)
            sliderBrightness.onValueChanged.AddListener(OnBrightnessChanged);

        // 현재 화면 상태를 UI와 동기화
        if (toggleFullscreen != null)
            toggleFullscreen.isOn = Screen.fullScreen;

        if (sliderBrightness != null)
        {
            sliderBrightness.value = 0.8f;
            UpdateBrightness(sliderBrightness.value);
        }
    }

    // ---------- 공통 제어 ---------- //

    /// <summary>
    /// 모든 페이지를 한 번에 끄기 (알파 0, 비활성)
    /// </summary>
    void CloseAllPages()
    {
        if (cgGraphic != null)
        {
            cgGraphic.alpha = 0f;
            cgGraphic.gameObject.SetActive(false);
        }

        if (cgSound != null)
        {
            cgSound.alpha = 0f;
            cgSound.gameObject.SetActive(false);
        }

        if (cgInfo != null)
        {
            cgInfo.alpha = 0f;
            cgInfo.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// CanvasGroup을 0 → 1로 서서히 켜주는 코루틴
    /// </summary>
    IEnumerator FadeIn(CanvasGroup cg, float duration = 0.2f)
    {
        if (cg == null)
            yield break;

        cg.alpha = 0f;
        cg.gameObject.SetActive(true);

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;   // 게임 일시정지와 상관없이 동작
            cg.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        cg.alpha = 1f;
    }

    /// <summary>
    /// 메인 메뉴(Graphic / Sound / Info 버튼만 있는 화면)로 돌아가기
    /// </summary>
    public void ShowMainMenu()
    {
        // 혹시 남아 있는 Fade 코루틴 정리
        StopAllCoroutines();

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        CloseAllPages();
    }

    // ---------- 페이지 열기 ---------- //

    public void OpenGraphicPage()
    {
        StopAllCoroutines();
        CloseAllPages();

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);   // 메인 메뉴 숨기기

        StartCoroutine(FadeIn(cgGraphic));
    }

    public void OpenSoundPage()
    {
        StopAllCoroutines();
        CloseAllPages();

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        StartCoroutine(FadeIn(cgSound));
    }

    public void OpenInfoPage()
    {
        StopAllCoroutines();
        CloseAllPages();

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        StartCoroutine(FadeIn(cgInfo));
    }

    /// <summary>
    /// X 버튼에서 호출하는 함수
    /// 현재 열린 페이지를 닫고 메인 메뉴로 복귀
    /// </summary>
    public void CloseCurrentPage()
    {
        ShowMainMenu();
    }

    // ----------- Graphic Page UI 이벤트 ------------ //

    // 해상도 변경
    void OnResolutionChanged(int index)
    {
        // Dropdown 순서:
        // 0 : 1920 x 1080
        // 1 : 1600 x 900
        // 2 : 1280 x 720

        int width = 1920;
        int height = 1080;

        switch (index)
        {
            case 0:
                width = 1920;
                height = 1080;
                break;
            case 1:
                width = 1600;
                height = 900;
                break;
            case 2:
                width = 1280;
                height = 720;
                break;
        }

        bool isFullscreen = Screen.fullScreen;
        Screen.SetResolution(width, height, isFullscreen);

        Debug.Log($"Resolution changed to: {width} x {height}, fullscreen={isFullscreen}");
    }

    // 전체 화면 모드 변경
    void OnFullscreenChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
        Debug.Log("Fullscreen: " + isOn);
    }

    // 밝기 변경
    void OnBrightnessChanged(float value)
    {
        UpdateBrightness(value);
        Debug.Log("Brightness: " + value);
    }

    // 오버레이 이미지를 이용해 화면 밝기 조절
    void UpdateBrightness(float value)
    {
        if (brightnessOverlay == null)
            return;

        // 슬라이더 value(0~1) → 알파값(0.7~0.0) 로 변환
        float alpha = Mathf.Lerp(0.7f, 0.0f, value);

        Color c = brightnessOverlay.color;
        c.a = alpha;
        brightnessOverlay.color = c;
    }
}
