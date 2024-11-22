using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;

public class Knife : MonoBehaviour
{
    [SerializeField] private float hitDamage; // 데미지
    [SerializeField] private float improvement; // 추가
    [SerializeField] private Wood wood; // Wood 참조
    [SerializeField] private InputActionReference secondaryButtonAction; // 

    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private bool isSecondaryButtonPressed = false;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }


        if (secondaryButtonAction != null)
        {
            secondaryButtonAction.action.performed += OnSecondaryButtonPressed;
            secondaryButtonAction.action.canceled += OnSecondaryButtonReleased;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        Debug.Log("잡음");
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        isSecondaryButtonPressed = false; // 놓을 때 버튼 상태도 리셋
        Debug.Log("놓음");
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
        if (!isGrabbed) return;

        Coll coll = other.GetComponent<Coll>();
        if (coll != null)
        {
            if (isSecondaryButtonPressed)
            {
                // 그랩 상태에서 A 버튼을 누르고 있을 때 늘어나는 효과
                coll.ImprovementCollider(improvement);
                if (wood != null)
                {
                    wood.Improve(coll.index, improvement);
                    Debug.Log("늘어남");
                }
            }
            else
            {
                // 그랩 상태에서 A 버튼을 누르지 않았을 때 줄어드는 효과
                coll.HitCollider(hitDamage);
                if (wood != null)
                {
                    wood.Hit(coll.index, hitDamage);
                    Debug.Log("줄어듬");
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