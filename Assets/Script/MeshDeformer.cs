using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh; // ������ �޽��� ������ ����
    Vector3[] originalVertices, displacedVertices; // �޽��� ���� ���¸� ��Ÿ���� vertex �迭, �޽��� ������ ���� vertex �迭
    Vector3[] vertexVelocities; // vertex�� ���� �ӵ��� �����ϴ� �迭

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh; // meshfilter���� �޽� �����ͼ� �ʱ�ȭ�ϱ�
        originalVertices = deformingMesh.vertices; // ������ vertex �迭 ��������
        displacedVertices = new Vector3[originalVertices.Length]; // ���� ũ���� vertex �迭�� ����

        for (int i = 0; i < originalVertices.Length; i++) // �� vertex �ʱ�ȭ
        {
            displacedVertices[i] = originalVertices[i]; // 
        }
        vertexVelocities = new Vector3[originalVertices.Length]; // �� vertex�� ���� �ӵ��� �����ϴ� �迭 �ʱ�ȭ
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        // �Ź� ������ �����ϱ� ���� displacedVertices�� originalVertices�� �ʱ�ȭ
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }

        // ���� ����
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            ApplyForceToVertex(i, point, force);
        }

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();
    }

    void ApplyForceToVertex(int i, Vector3 point, float force)  // �޽��� �� vertex�� �ܺ� ���� �����ϴ� �Լ�
    {
        Debug.Log("ApplyForceToVertex");

        // �޽��� �߽� ��� (���� ��ǥ ����)
        Vector3 meshCenter = Vector3.zero;

        // �߽������κ��� �� ���ؽ������� ���� ���
        Vector3 vertexToCenter = meshCenter - displacedVertices[i];

        // �Ÿ��� ���� �� ����
        float attenuatedForce = force / (1f + vertexToCenter.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;  // ���� �ð��� ���� ����� �ӵ�

        // �߽��� ���� �������� �ӵ� ����
        vertexVelocities[i] += vertexToCenter.normalized * velocity;

        // ���� ����: ������ �߽��� ���� �̵��ϵ��� ����
        displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;

        // ���� ��ġ���� �߽��� ���� �̵��� �����ϵ���, �־����� �ʵ��� ����
        if ((originalVertices[i] - meshCenter).magnitude < (displacedVertices[i] - meshCenter).magnitude)
        {
            displacedVertices[i] = originalVertices[i];  // ���� ��ġ�� �ǵ���
            vertexVelocities[i] = Vector3.zero;          // �ӵ� �ʱ�ȭ
        }
    }
}
