using UnityEngine;
using UnityEngine.XR;

public class VRController : MonoBehaviour
{
    public XRNode inputSource; // 왼쪽 혹은 오른쪽 컨트롤러
    private InputDevice device; // 입력 장치

    private void Start()
    {
        // 입력 장치 초기화
        device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    private void Update()
    {
        // 컨트롤러 트리거 상태 체크
        if (device.isValid)
        {
            bool triggerValue;
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Debug.Log(inputSource.ToString() + " 트리거 버튼이 눌렸습니다.");
            }
        }
    }
}
