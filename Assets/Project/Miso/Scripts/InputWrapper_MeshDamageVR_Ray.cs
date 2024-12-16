using UnityEngine;
using UnityEngine.InputSystem;
using MDPackage.Modifiers;

namespace MDPackage.ExampleContent
{
    /// <summary>
    /// VR Input Wrapper for MDM_MeshDamage component.
    /// Allows control for applying mesh damage using a Ray from the VR controller.
    /// </summary>
    public sealed class InputWrapper_MeshDamageVR_Ray : MonoBehaviour
    {
        public MDM_MeshDamage targetMeshDamage;

        [Header("VR Input Actions")]
        public InputActionProperty damageInput;  // Trigger button for applying damage
        public InputActionProperty restoreInput; // Grip button for restoring mesh

        [Header("Controller Tracking")]
        public Transform controllerTransform;   // Transform of the VR controller

        [Header("Raycast Settings")]
        public LayerMask interactionLayer;      // LayerMask for interactable objects
        public float rayLength = 10f;           // Maximum length of the ray
        public LineRenderer rayLineRenderer;    // LineRenderer for visualizing the ray

        [Header("Damage Parameters")]
        public float damageRadius = 0.5f;       // Radius for damage application
        public float damageForce = 0.15f;       // Force for damage application

#if UNITY_EDITOR
        private void Reset()
        {
            targetMeshDamage = GetComponent<MDM_MeshDamage>();
            Debug.Log("[Reset] Target Mesh Damage component assigned.");
        }
#endif

        private void Update()
        {
            if (targetMeshDamage == null)
            {
                Debug.LogWarning("[Update] Target Mesh Damage component is not assigned.");
                return;
            }

            if (controllerTransform == null)
            {
                Debug.LogWarning("[Update] Controller Transform is not assigned.");
                return;
            }

            // Set up the Ray
            Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
            RaycastHit hit;

            // Debug Trigger Input Value
            if (damageInput.action == null)
            {
                Debug.LogError("[Update] Damage Input Action is not assigned.");
            }
            else
            {
                float triggerValue = damageInput.action.ReadValue<float>();
                bool isTriggerPressed = triggerValue > 0.5f;
                Debug.Log($"[Update] Trigger Input Value: {triggerValue}, Trigger Pressed: {isTriggerPressed}");
            }

            // Perform the Raycast
            if (Physics.Raycast(ray, out hit, rayLength, interactionLayer))
            {
                Debug.Log($"[Update] Raycast hit detected at {hit.point} on object: {hit.collider.name}");

                // Visualize the Ray (to the hit point)
                UpdateRayLineRenderer(ray.origin, hit.point);

                // Apply Damage
                if (damageInput.action != null && damageInput.action.IsPressed())
                {
                    targetMeshDamage.MeshDamage_ModifyMesh(hit.point, damageRadius, damageForce, ray.direction);
                    Debug.Log($"[Update] Applied damage at {hit.point} with radius {damageRadius} and force {damageForce}");
                }
                else
                {
                    Debug.Log("[Update] Trigger is not pressed; skipping damage application.");
                }
            }
            else
            {
                Debug.Log("[Update] Raycast did not hit any object.");

                // Visualize the Ray (to the maximum length)
                UpdateRayLineRenderer(ray.origin, ray.origin + ray.direction * rayLength);
            }

            // Restore Mesh
            if (restoreInput.action != null && restoreInput.action.IsPressed())
            {
                Debug.Log("[Update] Restore input detected; restoring mesh.");
                targetMeshDamage.MeshDamage_RestoreMesh(0.1f); // Restore speed
                Debug.Log("[Update] Mesh restored to its original state.");
            }
        }

        /// <summary>
        /// Updates the LineRenderer to visualize the Ray.
        /// </summary>
        private void UpdateRayLineRenderer(Vector3 start, Vector3 end)
        {
            if (rayLineRenderer != null)
            {
                if (rayLineRenderer.positionCount != 2)
                {
                    rayLineRenderer.positionCount = 2;
                    Debug.Log("[UpdateRayLineRenderer] LineRenderer position count set to 2.");
                }

                rayLineRenderer.SetPosition(0, start);
                rayLineRenderer.SetPosition(1, end);
                Debug.Log($"[UpdateRayLineRenderer] RayLineRenderer updated: Start({start}), End({end})");
            }
            else
            {
                Debug.LogWarning("[UpdateRayLineRenderer] LineRenderer is not assigned.");
            }
        }
    }
}
