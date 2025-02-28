using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // 상호작용 시 처리할 기능
        Debug.Log("상호작용한 오브젝트: " + gameObject.name);
        // 예: 게임 설정 UI나 순위표 UI를 표시하는 코드
    }
}
