using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;

public class XRMeshDeformer : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public float deformationForce = 0.5f;
    public InputActionReference triggerAction;

    private void OnEnable()
    {
        if (triggerAction != null)
            triggerAction.action.performed += OnTriggerPerformed;
    }

    private void OnDisable()
    {
        if (triggerAction != null)
            triggerAction.action.performed -= OnTriggerPerformed;
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("트리거 눌림");
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
        {
            MeshDeformer deformer = hitInfo.collider.GetComponent<MeshDeformer>();
            if (deformer != null)
            {
                // 충돌 지점을 메쉬의 로컬 좌표로 변환
                Vector3 localHitPoint = deformer.transform.InverseTransformPoint(hitInfo.point);
                deformer.AddDeformingForce(localHitPoint, deformationForce);
            }
        }
    }
}