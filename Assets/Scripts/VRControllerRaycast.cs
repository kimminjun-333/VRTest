using UnityEngine;
using UnityEngine.UI;

public class VRControllerRaycast : MonoBehaviour
{
    public float maxDistance = 100f; // 최대 레이캐스트 거리
    public LayerMask interactableLayer; // 상호작용할 오브젝트들만 레이로 인식
    private GameObject lastTarget; // 마지막으로 레이를 쏜 오브젝트
    private LineRenderer lineRenderer; // 레이를 시각적으로 표현할 라인 렌더러
    private bool isPointerActive = false; // 포인터 활성화 여부

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
    }

    private void Update()
    {
        RaycastHit hit;

        // 레이 발사
        Ray ray = new Ray(transform.position, transform.forward);

        // 레이가 상호작용 가능한 레이어의 오브젝트에 닿으면
        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            GameObject target = hit.collider.gameObject;

            // 레이의 타겟이 변경되었을 때 아웃라인을 켜고, 이전 타겟의 아웃라인을 끄는 처리
            if (lastTarget != target)
            {
                // 기존 오브젝트의 Outline 비활성화
                if (lastTarget != null)
                {
                    Outline outlineComponent = lastTarget.GetComponent<Outline>();
                    if (outlineComponent != null)
                    {
                        outlineComponent.enabled = false;
                    }
                }

                // 새로운 타겟의 Outline 활성화
                Outline newOutline = target.GetComponent<Outline>();
                if (newOutline != null)
                {
                    newOutline.enabled = true;
                }

                // 현재 타겟을 lastTarget으로 저장
                lastTarget = target;
            }

            // 레이를 쏜 방향을 시각적으로 표현
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.enabled = true; // 라인 렌더러 활성화
            }

            isPointerActive = true;
        }
        else
        {
            // 레이가 상호작용할 수 있는 오브젝트를 벗어났을 때
            if (lastTarget != null)
            {
                Outline outlineComponent = lastTarget.GetComponent<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.enabled = false; // 이전 타겟의 아웃라인 비활성화
                }
                lastTarget = null;
            }

            // 레이가 상호작용할 오브젝트를 찾지 못했을 때 라인 렌더러 비활성화
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }

            isPointerActive = false;
        }

        // 레이가 상호작용할 수 있는 오브젝트를 클릭했을 때
        if (isPointerActive && Input.GetButtonDown("Fire1")) // 예: VR 버튼 또는 트리거 버튼
        {
            if (lastTarget != null)
            {
                HandleObjectClick(lastTarget);
            }
        }
    }

    // 오브젝트를 클릭했을 때 처리하는 함수
    private void HandleObjectClick(GameObject target)
    {
        // 예시: 오브젝트가 버튼 역할을 할 때 UI를 표시하는 코드 추가
        // 예를 들어, 버튼을 클릭해서 순위표를 표시한다거나, 게임 설정 UI를 표시한다거나
        if (target.CompareTag("Leaderboard"))
        {
            Debug.Log("순위표를 표시합니다.");
            // 순위표 UI를 표시하는 코드 추가
        }
        else if (target.CompareTag("Settings"))
        {
            Debug.Log("게임 설정을 표시합니다.");
            // 게임 설정 UI를 표시하는 코드 추가
        }
        else if (target.CompareTag("SongSelect"))
        {
            Debug.Log("곡 선택 UI를 표시합니다.");
            // 곡 선택 UI를 표시하는 코드 추가
        }
    }
}
