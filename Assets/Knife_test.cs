using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Knife_test : MonoBehaviour
{
    public MeshFilter potteryMeshFilter;
    public XRGrabInteractable knifeInteractable;
    public float deformationStrength = 0.1f;
    public float interactionDistance = 0.1f;

    private Mesh originalMesh;
    private Mesh deformedMesh;
    private Vector3[] originalVertices;
    private Vector3[] deformedVertices;

    void Start()
    {
        originalMesh = potteryMeshFilter.sharedMesh;
        deformedMesh = Instantiate(originalMesh);
        potteryMeshFilter.mesh = deformedMesh;

        originalVertices = originalMesh.vertices;
        deformedVertices = deformedMesh.vertices;

        knifeInteractable.selectEntered.AddListener(OnKnifeGrabbed);
    }

    void OnKnifeGrabbed(SelectEnterEventArgs args)
    {
        // 칼을 잡았을 때 실행될 로직
        Debug.Log("Knife grabbed!");
    }

    void Update()
    {
        if (knifeInteractable.isSelected)
        {
            DeformPottery();
        }
    }

    void DeformPottery()
    {
        Vector3 knifePosition = transform.InverseTransformPoint(knifeInteractable.transform.position);
        bool meshChanged = false;

        for (int i = 0; i < deformedVertices.Length; i++)
        {
            float distance = Vector3.Distance(deformedVertices[i], knifePosition);

            if (distance < interactionDistance)
            {
                Vector3 deformationDirection = (deformedVertices[i] - knifePosition).normalized;
                Vector3 newPosition = Vector3.Lerp(originalVertices[i], deformedVertices[i] + deformationDirection * deformationStrength, Time.deltaTime);

                if (Vector3.Distance(newPosition, originalVertices[i]) < deformationStrength * 0.8f)
                {
                    deformedVertices[i] = newPosition;
                    meshChanged = true;
                }
            }
        }

        if (meshChanged)
        {
            deformedMesh.vertices = deformedVertices;
            deformedMesh.RecalculateNormals();
            deformedMesh.RecalculateBounds();
        }
    }
}