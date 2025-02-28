using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // ���� ��Ʈ��ũ ������ �õ��ϰ�, ���� ���� �� �ݹ� �Լ� ȣ��
    public void ConnectToPhotonNetwork()
    {
        PhotonNetwork.ConnectUsingSettings(); // ���� ��Ʈ��ũ ���� ����
    }

    // ���� ��Ʈ��ũ ������ �Ϸ�Ǹ� ȣ��˴ϴ�.
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ��Ʈ��ũ�� ����Ǿ����ϴ�.");
    }

    // ���� ��Ʈ��ũ ������ �������� ȣ��˴ϴ�.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("���� ��Ʈ��ũ ���� ������: " + cause);
    }
}
