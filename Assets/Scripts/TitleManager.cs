using UnityEngine;
using Photon.Pun;
using System.Collections;
using Photon.Realtime;

public class TitleManager : MonoBehaviour
{
    public BlurEffectManager blurEffectManager;  // BlurEffectManager ����
    public PhotonManager photonManager;  // PhotonManager ����

    public bool isComplete = false;  // ���̵� �ƿ� �Ϸ� ����

    private IEnumerator Start()
    {
        blurEffectManager.OnTitle();
        photonManager.ConnectToPhotonNetwork();  // ���� ��Ʈ��ũ ���� �õ�
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);  // ���� �Ϸ���� ���

        blurEffectManager.OnTitle();  // Ÿ��Ʋ �ؽ�Ʈ�� �� ȿ�� ����
        blurEffectManager.StartFadeOut(OnFadeOutComplete);  // ���̵� �ƿ� ���� �� �ݹ� �Լ� ȣ��
    }

    // ���̵� �ƿ� �Ϸ� �� ȣ��� �ݹ� �Լ�
    private void OnFadeOutComplete()
    {
        isComplete = true;  // ���̵� �ƿ� �Ϸ�Ǹ� isComplete�� true�� ����
        Debug.Log("���̵� �ƿ� �Ϸ�!");
    }
}
