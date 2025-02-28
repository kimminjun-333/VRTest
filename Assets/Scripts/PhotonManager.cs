using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 포톤 네트워크 연결을 시도하고, 연결 성공 시 콜백 함수 호출
    public void ConnectToPhotonNetwork()
    {
        PhotonNetwork.ConnectUsingSettings(); // 포톤 네트워크 접속 시작
    }

    // 포톤 네트워크 연결이 완료되면 호출됩니다.
    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 네트워크에 연결되었습니다.");
    }

    // 포톤 네트워크 연결이 끊어지면 호출됩니다.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("포톤 네트워크 연결 끊어짐: " + cause);
    }
}
