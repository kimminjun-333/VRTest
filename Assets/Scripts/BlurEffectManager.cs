using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 
using TMPro;
using System;

public class BlurEffectManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;  // Post-Processing Volume 참조
    public TextMeshPro titleText;  // 3D 타이틀 텍스트 컴포넌트 (TextMeshPro로 변경)
    public float blurIntensity = 1f;  // 블러 강도
    public float titleFadeOutTime = 1f; // 타이틀 텍스트 서서히 사라지는 시간 (초)
    public float blurFadeOutTime = 3f; // 블러 효과 서서히 사라지는 시간 (초)

    private DepthOfField depthOfField;  // Depth of Field 효과 참조

    void Start()
    {
        // Post-Process Volume에서 DepthOfField 효과를 가져옵니다.
        postProcessVolume.profile.TryGetSettings(out depthOfField);

        // 초기 블러 효과를 바로 적용 (시작 시 바로 블러 효과 활성화)
        ApplyBlurEffect();
    }

    // 타이틀 텍스트의 알파값을 1로 설정하고 타이틀 텍스트를 활성화, 블러 효과를 바로 적용
    public void ShowTitleAndApplyBlur()
    {
        ResetTitleTextAlpha();  // 타이틀 텍스트 알파값 초기화
        titleText.gameObject.SetActive(true);  // 타이틀 텍스트를 화면에 표시
    }

    // 타이틀 텍스트 알파값 초기화 (1로 되돌림)
    private void ResetTitleTextAlpha()
    {
        Color color = titleText.color;
        color.a = 1f;  // 알파값을 1로 설정
        titleText.color = color;
    }

    // 블러 효과를 바로 적용
    private void ApplyBlurEffect()
    {
        if (depthOfField != null)
        {
            depthOfField.focusDistance.value = blurIntensity;  // 블러 강도 바로 적용 (1로 설정)
        }
    }

    // 타이틀 텍스트가 서서히 사라지게 하는 코루틴
    public IEnumerator FadeOutTitleText()
    {
        float elapsedTime = 0f;
        Color startColor = titleText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // 타이틀 텍스트를 서서히 투명하게 만듬
        while (elapsedTime < titleFadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            titleText.color = Color.Lerp(startColor, endColor, elapsedTime / titleFadeOutTime);
            yield return null;  // 매 프레임마다 업데이트
        }

        titleText.color = endColor; // 최종 색상 설정
        titleText.gameObject.SetActive(false); // 타이틀 텍스트를 비활성화하여 화면에서 제거
    }

    // 블러 효과 서서히 사라지게 하는 코루틴
    public IEnumerator FadeOutBlur()
    {
        if (depthOfField != null)
        {
            float currentBlur = depthOfField.focusDistance.value;
            float elapsedTime = 0f;

            // 블러 강도를 서서히 1에서 0까지 줄여나갑니다.
            while (elapsedTime < blurFadeOutTime)
            {
                elapsedTime += Time.deltaTime; // 시간을 갱신
                currentBlur = Mathf.Lerp(blurIntensity, 0f, elapsedTime / blurFadeOutTime); // 1에서 0으로 줄어드는 효과
                depthOfField.focusDistance.value = Mathf.Max(currentBlur, 0f); // 강도가 0 이하로 내려가지 않도록 제한
                yield return null;  // 매 프레임마다 업데이트
            }
        }
    }

    // 타이틀 텍스트와 블러 효과를 동시에 서서히 사라지게 하는 코루틴
    public void StartTitleAndBlurFadeOut(Action onComplete)
    {
        // 타이틀 텍스트와 블러 효과를 동시에 페이드 아웃
        StartCoroutine(TitleAndBlurFadeOutCoroutine(onComplete));
    }

    // 타이틀 텍스트와 블러 효과를 동시에 서서히 사라지게 하는 코루틴
    private IEnumerator TitleAndBlurFadeOutCoroutine(Action onComplete)
    {
        Coroutine fadeTitleCoroutine = StartCoroutine(FadeOutTitleText());  // 타이틀 페이드 아웃
        Coroutine fadeBlurCoroutine = StartCoroutine(FadeOutBlur());  // 블러 페이드 아웃

        // 두 코루틴이 모두 완료될 때까지 기다림
        yield return fadeTitleCoroutine;
        yield return fadeBlurCoroutine;

        onComplete?.Invoke();  // 페이드 아웃 완료 시 콜백 호출
    }

    // Update에서 타이틀 텍스트의 게임 오브젝트를 화면 정면에 고정시키기
    void Update()
    {
        // 카메라의 정면 (화면 정면)을 기준으로 위치를 고정시키기 위해
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 1f);  // 화면의 중앙에 고정
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(screenCenter);  // 화면 좌표를 월드 좌표로 변환

        // 타이틀 텍스트의 위치를 화면 정면으로 고정
        titleText.transform.position = worldPosition;
        titleText.transform.rotation = Camera.main.transform.rotation;  // 카메라의 회전을 따라가도록 설정
    }
}
