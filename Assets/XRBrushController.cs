using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XDPaint.Controllers;

public class XRBrushController : MonoBehaviour
{
    [SerializeField] private InputController inputController; // XDPaint Input Controller
    [SerializeField] private Transform controllerTip; // ��Ʈ�ѷ� �� (Brush ��ġ)
    [SerializeField] private InputActionReference grabAction; // Grab �׼�

    private void OnEnable()
    {
        // Grab Action �̺�Ʈ ����
        grabAction.action.started += OnGrabStarted;
        grabAction.action.canceled += OnGrabCanceled;
    }

    private void OnDisable()
    {
        // Grab Action �̺�Ʈ ����
        grabAction.action.started -= OnGrabStarted;
        grabAction.action.canceled -= OnGrabCanceled;
    }

    private void OnGrabStarted(InputAction.CallbackContext context)
    {
        // �׸� �׸��� ����
        inputController.PenTransform = controllerTip;
    }

    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        // �׸� �׸��� ����
        inputController.PenTransform = null;
    }
}