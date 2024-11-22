using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelHand : MonoBehaviour
{
    [SerializeField] private float hitDamage; // 도자기가 줄어드는 데미지
    [SerializeField] private float improvement; // 도자기가 늘어나는 값
    [SerializeField] private Wood wood; // 도자기를 조작할 Wood 스크립트 참조
    [SerializeField] private InputActionReference secondaryButtonAction; // Secondary Button Input Action 참조

    private bool isSecondaryButtonPressed = false; // Secondary Button 상태
    private void Start()
    {
        if (secondaryButtonAction != null)
        {
            secondaryButtonAction.action.performed += OnSecondaryButtonPressed;
            secondaryButtonAction.action.canceled += OnSecondaryButtonReleased;
        }
    }

    private void OnSecondaryButtonPressed(InputAction.CallbackContext context)
    {
        isSecondaryButtonPressed = true;
        Debug.Log("세컨더리 버튼 누름");
    }

    private void OnSecondaryButtonReleased(InputAction.CallbackContext context)
    {
        isSecondaryButtonPressed = false;
        Debug.Log("세컨더리 버튼 뗌");
    }

    private void OnTriggerStay(Collider other)
    {
        // 도자기와 충돌한 경우에만 처리
        Coll coll = other.GetComponent<Coll>();
        if (coll != null)
        {
            if (isSecondaryButtonPressed)
            {
                // Secondary Button이 눌려 있는 동안 도자기 늘어남
                coll.ImprovementCollider(improvement);
                if (wood != null)
                {
                    wood.Improve(coll.index, improvement);
                    Debug.Log("도자기 늘어남");
                }
            }
            else
            {
                // Secondary Button이 눌려 있지 않으면 도자기 줄어듦
                coll.HitCollider(hitDamage);
                if (wood != null)
                {
                    wood.Hit(coll.index, hitDamage);
                    Debug.Log("도자기 줄어듦");
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (secondaryButtonAction != null)
        {
            secondaryButtonAction.action.performed -= OnSecondaryButtonPressed;
            secondaryButtonAction.action.canceled -= OnSecondaryButtonReleased;
        }
    }
}
