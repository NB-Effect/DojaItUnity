using UnityEngine;
using UnityEngine.UI;

public class MaterialChange : MonoBehaviour
{
    public Button uiButton; // UI ��ư
    public GameObject targetObject; // ���͸����� ������ ��� ������Ʈ
    public Material newMaterial; // ������ ���ο� ���͸���

    private void Start()
    {
        if (uiButton != null)
        {
            uiButton.onClick.AddListener(ChangeMaterial); // ��ư Ŭ�� �� ChangeMaterial �޼��� ȣ��
        }
        else
        {
            Debug.LogError("UI Button is not assigned!");
        }
    }

    private void ChangeMaterial()
    {
        if (targetObject != null && newMaterial != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial; // ���͸��� ����
                Debug.Log($"Changed material of {targetObject.name} to {newMaterial.name}");
            }
            else
            {
                Debug.LogError($"No Renderer component found on {targetObject.name}");
            }
        }
        else
        {
            Debug.LogError("Target object or new material is not assigned!");
        }
    }
}
