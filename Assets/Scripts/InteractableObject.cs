using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // ��ȣ�ۿ� �� ó���� ���
        Debug.Log("��ȣ�ۿ��� ������Ʈ: " + gameObject.name);
        // ��: ���� ���� UI�� ����ǥ UI�� ǥ���ϴ� �ڵ�
    }
}
