using UnityEngine;

public class ClaySimulator : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float deformStrength = 0.1f;
    public float deformRadius = 0.1f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
        System.Array.Copy(originalVertices, modifiedVertices, originalVertices.Length);
    }

    void Update()
    {
        // 물레 회전
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        UpdateMesh();
    }

    public void DeformMesh(Vector3 contactPoint, float strength, float radius)
    {
        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            Vector3 vertex = transform.TransformPoint(modifiedVertices[i]);
            float distance = Vector3.Distance(vertex, contactPoint);
            if (distance < radius)
            {
                float deformAmount = (1 - (distance / radius)) * strength;
                modifiedVertices[i] -= (contactPoint - vertex).normalized * deformAmount;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}