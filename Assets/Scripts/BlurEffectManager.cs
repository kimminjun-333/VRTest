using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 
using TMPro;
using System;

public class BlurEffectManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;  // Post-Processing Volume 참조
    public TextMeshPro titleText;  // 3D 타이틀 텍스트 컴포넌트 (TextMeshPro로 변경)
    public float titleFadeOutTime = 3f; // 타이틀 텍스트 서서히 사라지는 시간 (초)
    public float blurFadeOutTime = 3f;  // 블러 효과 서서히 사라지는 시간 (초)

    private DepthOfField depthOfField;  // Depth of Field 효과 참조
    private PostProcessProfile profile;  // PostProcessProfile 참조

    void Start()
    {
        // PostProcessVolume에서 Profile과 DepthOfField 효과를 가져옵니다.
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            postProcessVolume.profile.TryGetSettings(out depthOfField);
        }
        profile = postProcessVolume.profile;  // PostProcessProfile 참조

        // Focal Length를 최대값으로 설정 (한 번만 설정)
        if (depthOfField != null)
        {
            depthOfField.focalLength.value = Mathf.Max(depthOfField.focalLength.value, 200f);  // 원하는 최대값으로 설정
        }
    }

    // 타이틀 텍스트와 블러 효과를 동시에 실행하는 함수
    public void OnTitle()
    {
        ResetTitleText();  // 타이틀 텍스트 알파값 초기화
        titleText.gameObject.SetActive(true);  // 타이틀 텍스트를 화면에 표시

        OnBlurEffect(1f);  // Weight 1로 설정
    }

    // 타이틀 텍스트 알파값 초기화 (1로 되돌림)
    private void ResetTitleText()
    {
        Color color = titleText.color;
        color.a = 1f;  // 알파값을 1로 설정
        titleText.color = color;
    }

    // 블러 효과를 즉시 적용 (weight: PostProcess의 weight)
    private void OnBlurEffect(float weight)
    {
        postProcessVolume.weight = weight;  // PostProcessVolume의 weight 설정
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

    // 블러 효과와 Weight를 서서히 사라지게 하는 코루틴
    public IEnumerator FadeOutBlur()
    {
        float elapsedTime = 0f;
        float startWeight = postProcessVolume.weight;  // 초기 Weight 값

        // Weight 값을 0으로 서서히 감소
        while (elapsedTime < blurFadeOutTime)
        {
            elapsedTime += Time.deltaTime;  // 시간 경과
            float newWeight = Mathf.Lerp(startWeight, 0f, elapsedTime / blurFadeOutTime);  // Lerp로 Weight 변화
            OnBlurEffect(newWeight);  // Weight만 변경
            yield return null;  // 매 프레임마다 업데이트
        }

        // 최종적으로 0으로 설정
        OnBlurEffect(0f);
    }

    // 타이틀 텍스트와 블러 효과를 동시에 서서히 사라지게 하는 코루틴
    public void StartFadeOut(Action onComplete)
    {
        // 타이틀 텍스트와 블러 효과를 동시에 페이드 아웃
        StartCoroutine(StartFadeOutCoroutine(onComplete));
    }

    // 타이틀 텍스트와 블러 효과를 동시에 서서히 사라지게 하는 코루틴
    private IEnumerator StartFadeOutCoroutine(Action onComplete)
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
