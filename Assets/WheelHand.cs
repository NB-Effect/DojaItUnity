using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelHand : MonoBehaviour
{
    [SerializeField] private float hitDamage; // ���ڱⰡ �پ��� ������
    [SerializeField] private float improvement; // ���ڱⰡ �þ�� ��
    [SerializeField] private Wood wood; // ���ڱ⸦ ������ Wood ��ũ��Ʈ ����
    [SerializeField] private InputActionReference secondaryButtonAction; // Secondary Button Input Action ����

    private bool isSecondaryButtonPressed = false; // Secondary Button ����
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
        Debug.Log("�������� ��ư ����");
    }

    private void OnSecondaryButtonReleased(InputAction.CallbackContext context)
    {
        isSecondaryButtonPressed = false;
        Debug.Log("�������� ��ư ��");
    }

    private void OnTriggerStay(Collider other)
    {
        // ���ڱ�� �浹�� ��쿡�� ó��
        Coll coll = other.GetComponent<Coll>();
        if (coll != null)
        {
            if (isSecondaryButtonPressed)
            {
                // Secondary Button�� ���� �ִ� ���� ���ڱ� �þ
                coll.ImprovementCollider(improvement);
                if (wood != null)
                {
                    wood.Improve(coll.index, improvement);
                    Debug.Log("���ڱ� �þ");
                }
            }
            else
            {
                // Secondary Button�� ���� ���� ������ ���ڱ� �پ��
                coll.HitCollider(hitDamage);
                if (wood != null)
                {
                    wood.Hit(coll.index, hitDamage);
                    Debug.Log("���ڱ� �پ��");
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
