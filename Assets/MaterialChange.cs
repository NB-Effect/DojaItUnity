using UnityEngine;
using UnityEngine.UI;

public class MaterialChange : MonoBehaviour
{
    public Button uiButton; // UI 버튼
    public GameObject targetObject; // 머터리얼을 변경할 대상 오브젝트
    public Material newMaterial; // 변경할 새로운 머터리얼

    private void Start()
    {
        if (uiButton != null)
        {
            uiButton.onClick.AddListener(ChangeMaterial); // 버튼 클릭 시 ChangeMaterial 메서드 호출
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
                renderer.material = newMaterial; // 머터리얼 변경
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
