using UnityEngine;
using Photon.Pun;
using System.Collections;
using Photon.Realtime;

public class TitleManager : MonoBehaviour
{
    public BlurEffectManager blurEffectManager;  // BlurEffectManager 참조
    public PhotonManager photonManager;  // PhotonManager 참조

    public bool isComplete = false;  // 페이드 아웃 완료 여부

    private IEnumerator Start()
    {
        blurEffectManager.OnTitle();
        photonManager.ConnectToPhotonNetwork();  // 포톤 네트워크 접속 시도
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);  // 접속 완료까지 대기

        blurEffectManager.OnTitle();  // 타이틀 텍스트와 블러 효과 적용
        blurEffectManager.StartFadeOut(OnFadeOutComplete);  // 페이드 아웃 시작 후 콜백 함수 호출
    }

    // 페이드 아웃 완료 후 호출될 콜백 함수
    private void OnFadeOutComplete()
    {
        isComplete = true;  // 페이드 아웃 완료되면 isComplete를 true로 설정
        Debug.Log("페이드 아웃 완료!");
    }
}
