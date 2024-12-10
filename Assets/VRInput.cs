using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XDPaint.Controllers;

public class VRInput : MonoBehaviour
{
    [SerializeField] private InputController inputController; // XDPaint Input Controller
    [SerializeField] private Transform controllerTip; // ��Ʈ�ѷ� �� (Brush ��ġ)
    [SerializeField] private InputActionReference grabAction; // Grab �׼�


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
