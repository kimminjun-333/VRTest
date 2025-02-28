using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 
using TMPro;
using System;

public class BlurEffectManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;  // Post-Processing Volume ����
    public TextMeshPro titleText;  // 3D Ÿ��Ʋ �ؽ�Ʈ ������Ʈ (TextMeshPro�� ����)
    public float blurIntensity = 1f;  // �� ����
    public float titleFadeOutTime = 1f; // Ÿ��Ʋ �ؽ�Ʈ ������ ������� �ð� (��)
    public float blurFadeOutTime = 3f; // �� ȿ�� ������ ������� �ð� (��)

    private DepthOfField depthOfField;  // Depth of Field ȿ�� ����

    void Start()
    {
        // Post-Process Volume���� DepthOfField ȿ���� �����ɴϴ�.
        postProcessVolume.profile.TryGetSettings(out depthOfField);

        // �ʱ� �� ȿ���� �ٷ� ���� (���� �� �ٷ� �� ȿ�� Ȱ��ȭ)
        ApplyBlurEffect();
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� ���İ��� 1�� �����ϰ� Ÿ��Ʋ �ؽ�Ʈ�� Ȱ��ȭ, �� ȿ���� �ٷ� ����
    public void ShowTitleAndApplyBlur()
    {
        ResetTitleTextAlpha();  // Ÿ��Ʋ �ؽ�Ʈ ���İ� �ʱ�ȭ
        titleText.gameObject.SetActive(true);  // Ÿ��Ʋ �ؽ�Ʈ�� ȭ�鿡 ǥ��
    }

    // Ÿ��Ʋ �ؽ�Ʈ ���İ� �ʱ�ȭ (1�� �ǵ���)
    private void ResetTitleTextAlpha()
    {
        Color color = titleText.color;
        color.a = 1f;  // ���İ��� 1�� ����
        titleText.color = color;
    }

    // �� ȿ���� �ٷ� ����
    private void ApplyBlurEffect()
    {
        if (depthOfField != null)
        {
            depthOfField.focusDistance.value = blurIntensity;  // �� ���� �ٷ� ���� (1�� ����)
        }
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� ������ ������� �ϴ� �ڷ�ƾ
    public IEnumerator FadeOutTitleText()
    {
        float elapsedTime = 0f;
        Color startColor = titleText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // Ÿ��Ʋ �ؽ�Ʈ�� ������ �����ϰ� ����
        while (elapsedTime < titleFadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            titleText.color = Color.Lerp(startColor, endColor, elapsedTime / titleFadeOutTime);
            yield return null;  // �� �����Ӹ��� ������Ʈ
        }

        titleText.color = endColor; // ���� ���� ����
        titleText.gameObject.SetActive(false); // Ÿ��Ʋ �ؽ�Ʈ�� ��Ȱ��ȭ�Ͽ� ȭ�鿡�� ����
    }

    // �� ȿ�� ������ ������� �ϴ� �ڷ�ƾ
    public IEnumerator FadeOutBlur()
    {
        if (depthOfField != null)
        {
            float currentBlur = depthOfField.focusDistance.value;
            float elapsedTime = 0f;

            // �� ������ ������ 1���� 0���� �ٿ������ϴ�.
            while (elapsedTime < blurFadeOutTime)
            {
                elapsedTime += Time.deltaTime; // �ð��� ����
                currentBlur = Mathf.Lerp(blurIntensity, 0f, elapsedTime / blurFadeOutTime); // 1���� 0���� �پ��� ȿ��
                depthOfField.focusDistance.value = Mathf.Max(currentBlur, 0f); // ������ 0 ���Ϸ� �������� �ʵ��� ����
                yield return null;  // �� �����Ӹ��� ������Ʈ
            }
        }
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� ������ ������� �ϴ� �ڷ�ƾ
    public void StartTitleAndBlurFadeOut(Action onComplete)
    {
        // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� ���̵� �ƿ�
        StartCoroutine(TitleAndBlurFadeOutCoroutine(onComplete));
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� ������ ������� �ϴ� �ڷ�ƾ
    private IEnumerator TitleAndBlurFadeOutCoroutine(Action onComplete)
    {
        Coroutine fadeTitleCoroutine = StartCoroutine(FadeOutTitleText());  // Ÿ��Ʋ ���̵� �ƿ�
        Coroutine fadeBlurCoroutine = StartCoroutine(FadeOutBlur());  // �� ���̵� �ƿ�

        // �� �ڷ�ƾ�� ��� �Ϸ�� ������ ��ٸ�
        yield return fadeTitleCoroutine;
        yield return fadeBlurCoroutine;

        onComplete?.Invoke();  // ���̵� �ƿ� �Ϸ� �� �ݹ� ȣ��
    }

    // Update���� Ÿ��Ʋ �ؽ�Ʈ�� ���� ������Ʈ�� ȭ�� ���鿡 ������Ű��
    void Update()
    {
        // ī�޶��� ���� (ȭ�� ����)�� �������� ��ġ�� ������Ű�� ����
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 1f);  // ȭ���� �߾ӿ� ����
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(screenCenter);  // ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ

        // Ÿ��Ʋ �ؽ�Ʈ�� ��ġ�� ȭ�� �������� ����
        titleText.transform.position = worldPosition;
        titleText.transform.rotation = Camera.main.transform.rotation;  // ī�޶��� ȸ���� ���󰡵��� ����
    }
}
