using UnityEngine;
using UnityEngine.UI;

public class VRControllerRaycast : MonoBehaviour
{
    public float maxDistance = 100f; // �ִ� ����ĳ��Ʈ �Ÿ�
    public LayerMask interactableLayer; // ��ȣ�ۿ��� ������Ʈ�鸸 ���̷� �ν�
    private GameObject lastTarget; // ���������� ���̸� �� ������Ʈ
    private LineRenderer lineRenderer; // ���̸� �ð������� ǥ���� ���� ������
    private bool isPointerActive = false; // ������ Ȱ��ȭ ����

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
    }

    private void Update()
    {
        RaycastHit hit;

        // ���� �߻�
        Ray ray = new Ray(transform.position, transform.forward);

        // ���̰� ��ȣ�ۿ� ������ ���̾��� ������Ʈ�� ������
        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            GameObject target = hit.collider.gameObject;

            // ������ Ÿ���� ����Ǿ��� �� �ƿ������� �Ѱ�, ���� Ÿ���� �ƿ������� ���� ó��
            if (lastTarget != target)
            {
                // ���� ������Ʈ�� Outline ��Ȱ��ȭ
                if (lastTarget != null)
                {
                    Outline outlineComponent = lastTarget.GetComponent<Outline>();
                    if (outlineComponent != null)
                    {
                        outlineComponent.enabled = false;
                    }
                }

                // ���ο� Ÿ���� Outline Ȱ��ȭ
                Outline newOutline = target.GetComponent<Outline>();
                if (newOutline != null)
                {
                    newOutline.enabled = true;
                }

                // ���� Ÿ���� lastTarget���� ����
                lastTarget = target;
            }

            // ���̸� �� ������ �ð������� ǥ��
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.enabled = true; // ���� ������ Ȱ��ȭ
            }

            isPointerActive = true;
        }
        else
        {
            // ���̰� ��ȣ�ۿ��� �� �ִ� ������Ʈ�� ����� ��
            if (lastTarget != null)
            {
                Outline outlineComponent = lastTarget.GetComponent<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.enabled = false; // ���� Ÿ���� �ƿ����� ��Ȱ��ȭ
                }
                lastTarget = null;
            }

            // ���̰� ��ȣ�ۿ��� ������Ʈ�� ã�� ������ �� ���� ������ ��Ȱ��ȭ
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }

            isPointerActive = false;
        }

        // ���̰� ��ȣ�ۿ��� �� �ִ� ������Ʈ�� Ŭ������ ��
        if (isPointerActive && Input.GetButtonDown("Fire1")) // ��: VR ��ư �Ǵ� Ʈ���� ��ư
        {
            if (lastTarget != null)
            {
                HandleObjectClick(lastTarget);
            }
        }
    }

    // ������Ʈ�� Ŭ������ �� ó���ϴ� �Լ�
    private void HandleObjectClick(GameObject target)
    {
        // ����: ������Ʈ�� ��ư ������ �� �� UI�� ǥ���ϴ� �ڵ� �߰�
        // ���� ���, ��ư�� Ŭ���ؼ� ����ǥ�� ǥ���Ѵٰų�, ���� ���� UI�� ǥ���Ѵٰų�
        if (target.CompareTag("Leaderboard"))
        {
            Debug.Log("����ǥ�� ǥ���մϴ�.");
            // ����ǥ UI�� ǥ���ϴ� �ڵ� �߰�
        }
        else if (target.CompareTag("Settings"))
        {
            Debug.Log("���� ������ ǥ���մϴ�.");
            // ���� ���� UI�� ǥ���ϴ� �ڵ� �߰�
        }
        else if (target.CompareTag("SongSelect"))
        {
            Debug.Log("�� ���� UI�� ǥ���մϴ�.");
            // �� ���� UI�� ǥ���ϴ� �ڵ� �߰�
        }
    }
}
