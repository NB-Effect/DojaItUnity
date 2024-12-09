using UnityEngine;
using UnityEngine.InputSystem; // Input System v2 사용
using UnityEngine.XR;

namespace MDPackage.ExampleContent
{
    public sealed class GenericInputWrapper_VR_Ray : MonoBehaviour
    {
        public MonoBehaviour targetGenericInputTemplate;
        [Space]
        public InputActionProperty triggerInput; // 트리거 버튼 입력 액션
        public InputActionProperty rayOriginInput; // 컨트롤러 위치 입력 액션 
        public InputActionProperty rayDirectionInput; // 컨트롤러 방향 입력 액션
        [Space]
        public LayerMask interactableLayerMask; // 상호작용 가능한 레이어

        private IDMGenericInputTemplate targetGeneric;
        private Camera mainCamera;

        private void Start()
        {
            Debug.Log("GenericInputWrapper_VR_Ray script has started.");

            if (targetGenericInputTemplate != null)
            {
                targetGeneric = targetGenericInputTemplate.GetComponent<MD_MeshEditorRuntimeVR>().GetComponent<IDMGenericInputTemplate>();
                if (targetGeneric != null)
                    Debug.Log("[Start] Target Generic Input Template successfully assigned.");
                else
                    Debug.LogError("[Start] Failed to assign Target Generic Input Template. It must implement IDMGenericInputTemplate.");
            }
            else
            {
                Debug.LogError("[Start] Target Generic Input Template is not assigned in the Inspector.");
            }

            mainCamera = Camera.main;
            if (mainCamera != null)
                Debug.Log("[Start] Main Camera found and assigned.");
            else
                Debug.LogError("[Start] Main Camera is not found in the scene.");
        }

#if UNITY_EDITOR
        private void Reset()
        {
            targetGenericInputTemplate = GetComponent<MonoBehaviour>();
            Debug.Log("[Reset] Target Generic Input Template automatically assigned if available.");
        }
#endif

        public void ChangeGenericTarget(IDMGenericInputTemplate target)
        {
            targetGeneric = target;
            Debug.Log($"[ChangeGenericTarget] Changed Target Generic Input Template to: {target}");
        }

        private void Update()
        {
            if (targetGeneric == null)
            {
                Debug.LogError("[Update] Target Generic Input Template is not assigned or does not implement IDMGenericInputTemplate.");
                return;
            }

            if (triggerInput.action == null)
            {
                Debug.LogError("[Update] Trigger Input Action is not assigned.");
                return;
            }

            if (rayOriginInput.action == null)
            {
                Debug.LogError("[Update] Ray Origin Input Action is not assigned.");
                return;
            }

            if (rayDirectionInput.action == null)
            {
                Debug.LogError("[Update] Ray Direction Input Action is not assigned.");
                return;
            }

            // XR 시뮬레이터의 입력 상태 확인
            Debug.Log($"[Update] Trigger Input: {triggerInput.action.name}, Ray Origin Input: {rayOriginInput.action.name}, Ray Direction Input: {rayDirectionInput.action.name}");

            // 레이 데이터 가져오기
            Vector3 rayOrigin = rayOriginInput.action.ReadValue<Vector3>();
            Vector3 rayDirection = rayDirectionInput.action.ReadValue<Vector3>();

            Debug.Log($"[Update] Ray Origin: {rayOrigin}, Ray Direction: {rayDirection}");

            // 레이캐스트를 통한 상호작용 지점 계산
            Ray ray = new Ray(rayOrigin, rayDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayerMask))
            {
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(hit.point);
                targetGeneric.InputHook_CursorScreenPosition = screenPosition;

                // 트리거 버튼 상태 전달
                bool isTriggerPressed = triggerInput.action.ReadValue<float>() > 0.5f;
                targetGeneric.InputHook_GenericButtonDown = isTriggerPressed;

                // 디버그용 레이 시각화
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);

                // 디버그 로그 추가: 레이캐스트 성공
                Debug.Log($"[Update] Ray Hit! Object: {hit.collider.name}, Hit Point: {hit.point}, Screen Position: {screenPosition}, Trigger Pressed: {isTriggerPressed}");
            }
            else
            {
                targetGeneric.InputHook_GenericButtonDown = false;

                // 디버그용 레이 시각화
                Debug.DrawRay(rayOrigin, rayDirection * 10, Color.red);

                // 디버그 로그 추가: 레이캐스트 실패
                Debug.LogWarning($"[Update] Raycast missed! Origin: {rayOrigin}, Direction: {rayDirection}, LayerMask: {interactableLayerMask.value}");
            }

            // 트리거 버튼 상태 확인
            float triggerValue = triggerInput.action.ReadValue<float>();
            Debug.Log($"[Update] Trigger Value: {triggerValue}, Pressed: {triggerValue > 0.5f}");

            // XR Input 시스템 디버그
            Debug.Log($"[Update] XR Simulated Controller detected: {triggerInput.action.activeControl?.name ?? "None"}");
        }
    }
}
