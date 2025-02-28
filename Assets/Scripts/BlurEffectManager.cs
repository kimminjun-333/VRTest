using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 
using TMPro;
using System;

public class BlurEffectManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;  // Post-Processing Volume ����
    public TextMeshPro titleText;  // 3D Ÿ��Ʋ �ؽ�Ʈ ������Ʈ (TextMeshPro�� ����)
    public float titleFadeOutTime = 3f; // Ÿ��Ʋ �ؽ�Ʈ ������ ������� �ð� (��)
    public float blurFadeOutTime = 3f;  // �� ȿ�� ������ ������� �ð� (��)

    private DepthOfField depthOfField;  // Depth of Field ȿ�� ����
    private PostProcessProfile profile;  // PostProcessProfile ����

    void Start()
    {
        // PostProcessVolume���� Profile�� DepthOfField ȿ���� �����ɴϴ�.
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            postProcessVolume.profile.TryGetSettings(out depthOfField);
        }
        profile = postProcessVolume.profile;  // PostProcessProfile ����

        // Focal Length�� �ִ밪���� ���� (�� ���� ����)
        if (depthOfField != null)
        {
            depthOfField.focalLength.value = Mathf.Max(depthOfField.focalLength.value, 200f);  // ���ϴ� �ִ밪���� ����
        }
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� �����ϴ� �Լ�
    public void OnTitle()
    {
        ResetTitleText();  // Ÿ��Ʋ �ؽ�Ʈ ���İ� �ʱ�ȭ
        titleText.gameObject.SetActive(true);  // Ÿ��Ʋ �ؽ�Ʈ�� ȭ�鿡 ǥ��

        OnBlurEffect(1f);  // Weight 1�� ����
    }

    // Ÿ��Ʋ �ؽ�Ʈ ���İ� �ʱ�ȭ (1�� �ǵ���)
    private void ResetTitleText()
    {
        Color color = titleText.color;
        color.a = 1f;  // ���İ��� 1�� ����
        titleText.color = color;
    }

    // �� ȿ���� ��� ���� (weight: PostProcess�� weight)
    private void OnBlurEffect(float weight)
    {
        postProcessVolume.weight = weight;  // PostProcessVolume�� weight ����
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

    // �� ȿ���� Weight�� ������ ������� �ϴ� �ڷ�ƾ
    public IEnumerator FadeOutBlur()
    {
        float elapsedTime = 0f;
        float startWeight = postProcessVolume.weight;  // �ʱ� Weight ��

        // Weight ���� 0���� ������ ����
        while (elapsedTime < blurFadeOutTime)
        {
            elapsedTime += Time.deltaTime;  // �ð� ���
            float newWeight = Mathf.Lerp(startWeight, 0f, elapsedTime / blurFadeOutTime);  // Lerp�� Weight ��ȭ
            OnBlurEffect(newWeight);  // Weight�� ����
            yield return null;  // �� �����Ӹ��� ������Ʈ
        }

        // ���������� 0���� ����
        OnBlurEffect(0f);
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� ������ ������� �ϴ� �ڷ�ƾ
    public void StartFadeOut(Action onComplete)
    {
        // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� ���̵� �ƿ�
        StartCoroutine(StartFadeOutCoroutine(onComplete));
    }

    // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ���� ���ÿ� ������ ������� �ϴ� �ڷ�ƾ
    private IEnumerator StartFadeOutCoroutine(Action onComplete)
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
