using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XDPaint.Controllers;

public class VRInput : MonoBehaviour
{
    [SerializeField] private InputController inputController; // XDPaint Input Controller
    [SerializeField] private Transform controllerTip; // 컨트롤러 끝 (Brush 위치)
    [SerializeField] private InputActionReference grabAction; // Grab 액션


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
        // Grab Action 이벤트 연결
        grabAction.action.started += OnGrabStarted;
        grabAction.action.canceled += OnGrabCanceled;
    }

    private void OnDisable()
    {
        // Grab Action 이벤트 해제
        grabAction.action.started -= OnGrabStarted;
        grabAction.action.canceled -= OnGrabCanceled;
    }

    private void OnGrabStarted(InputAction.CallbackContext context)
    {
        // 그림 그리기 시작
        inputController.PenTransform = controllerTip;
    }

    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        // 그림 그리기 중지
        inputController.PenTransform = null;
    }
}
