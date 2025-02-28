using UnityEngine;
using UnityEngine.XR;

public class VRController : MonoBehaviour
{
    public XRNode inputSource; // ���� Ȥ�� ������ ��Ʈ�ѷ�
    private InputDevice device; // �Է� ��ġ

    private void Start()
    {
        // �Է� ��ġ �ʱ�ȭ
        device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    private void Update()
    {
        // ��Ʈ�ѷ� Ʈ���� ���� üũ
        if (device.isValid)
        {
            bool triggerValue;
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Debug.Log(inputSource.ToString() + " Ʈ���� ��ư�� ���Ƚ��ϴ�.");
            }
        }
    }
}
