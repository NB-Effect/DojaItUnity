using UnityEngine;
using UnityEngine.InputSystem;

namespace MDPackage.ExampleContent
{
    /// <summary>
    /// VR Input Wrapper for MD_MeshEditorRuntimeVR component.
    /// </summary>
    public class InputWrapper_MeshEditorRuntimeVR : MonoBehaviour
    {
        public MD_MeshEditorRuntimeVR targetMeshEditorRuntimeVR;

        [Header("VR Input Actions")]
        public InputActionProperty triggerInput; // Trigger button
        public InputActionProperty gripInput;    // Grip button for adding points
        public InputActionProperty primaryInput; // Primary button for removing points

        [Header("Controller Tracking")]
        public Transform controllerTransform;   // VR Controller Transform (used for ray or position)

        [Header("Raycast Settings")]
        public LayerMask interactionLayer;       // LayerMask for interactable objects
        public float rayLength = 10f;           // Maximum raycast length
        public LineRenderer rayLineRenderer;    // LineRenderer for visualizing the ray

        private void Update()
        {
            if (targetMeshEditorRuntimeVR == null)
            {
                Debug.LogWarning("[InputWrapper] Target Mesh Editor Runtime VR is not assigned.");
                return;
            }

            if (controllerTransform == null)
            {
                Debug.LogWarning("[InputWrapper] Controller Transform is not assigned.");
                return;
            }

            // Set up Ray
            Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
            RaycastHit hit;

            // Perform Raycast
            if (Physics.Raycast(ray, out hit, rayLength, interactionLayer))
            {
                Debug.Log($"[InputWrapper] Raycast hit detected at {hit.point} on object: {hit.collider.name}");

                // Handle interaction with the hit point
                HandleRaycastInteraction(hit);

                // Visualize ray (hit point)
                UpdateRayLineRenderer(ray.origin, hit.point);
            }
            else
            {
                Debug.Log("[InputWrapper] Raycast did not hit any object.");
                // No hit: visualize ray reaching max length
                UpdateRayLineRenderer(ray.origin, ray.origin + ray.direction * rayLength);
            }
        }

        private void HandleRaycastInteraction(RaycastHit hit)
        {
            if (targetMeshEditorRuntimeVR == null)
                return;

            // Update runtime VR editor cursor position
            targetMeshEditorRuntimeVR.InputHook_GenericButtonDown = triggerInput.action.IsPressed();

            // Control modes based on input actions
            if (gripInput.action.IsPressed())
            {
                targetMeshEditorRuntimeVR.VREditor_SwitchControlMode(1); // Switch to Pull mode
                Debug.Log("[InputWrapper] Switched to Pull mode.");
            }
            else if (primaryInput.action.IsPressed())
            {
                targetMeshEditorRuntimeVR.VREditor_SwitchControlMode(2); // Switch to Push mode
                Debug.Log("[InputWrapper] Switched to Push mode.");
            }
            else
            {
                targetMeshEditorRuntimeVR.VREditor_SwitchControlMode(0); // Default Grab/Drop mode
                Debug.Log("[InputWrapper] Switched to Grab/Drop mode.");
            }

            // Debug the current interaction
            Debug.Log($"[InputWrapper] Interaction handled at {hit.point}");
        }

        private void UpdateRayLineRenderer(Vector3 start, Vector3 end)
        {
            if (rayLineRenderer != null)
            {
                // Ensure LineRenderer has the correct number of positions
                if (rayLineRenderer.positionCount != 2)
                {
                    rayLineRenderer.positionCount = 2;
                    Debug.Log("[InputWrapper] LineRenderer position count set to 2.");
                }

                // Set start and end positions
                rayLineRenderer.SetPosition(0, start);
                rayLineRenderer.SetPosition(1, end);
                Debug.Log($"[InputWrapper] RayLineRenderer updated from {start} to {end}");
            }
            else
            {
                Debug.LogWarning("[InputWrapper] RayLineRenderer is not assigned.");
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            targetMeshEditorRuntimeVR = GetComponent<MD_MeshEditorRuntimeVR>();

            if (targetMeshEditorRuntimeVR != null)
            {
                Debug.Log("[InputWrapper] Target Mesh Editor Runtime VR successfully assigned in Reset.");
            }
            else
            {
                Debug.LogWarning("[InputWrapper] Failed to assign Target Mesh Editor Runtime VR in Reset.");
            }

            // Automatically add LineRenderer if not assigned
            if (rayLineRenderer == null)
            {
                rayLineRenderer = gameObject.AddComponent<LineRenderer>();
                rayLineRenderer.startWidth = 0.01f;
                rayLineRenderer.endWidth = 0.01f;
                Material lineMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
                lineMaterial.color = Color.green;
                rayLineRenderer.material = lineMaterial;
                Debug.Log("[InputWrapper] Added default LineRenderer with Alpha Blended Shader.");
            }

        }
#endif
    }
}
