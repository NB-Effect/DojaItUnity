using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh; // 변형할 메쉬를 저장할 변수
    Vector3[] originalVertices, displacedVertices; // 메쉬의 원래 상태를 나타내는 vertex 배열, 메쉬가 변형된 후의 vertex 배열
    Vector3[] vertexVelocities; // vertex의 현재 속도를 저장하는 배열

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh; // meshfilter에서 메쉬 가져와서 초기화하기
        originalVertices = deformingMesh.vertices; // 원래의 vertex 배열 가져오기
        displacedVertices = new Vector3[originalVertices.Length]; // 같은 크기의 vertex 배열을 생성

        for (int i = 0; i < originalVertices.Length; i++) // 각 vertex 초기화
        {
            displacedVertices[i] = originalVertices[i]; // 
        }
        vertexVelocities = new Vector3[originalVertices.Length]; // 각 vertex에 대한 속도를 저장하는 배열 초기화
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        // 매번 변형을 시작하기 전에 displacedVertices를 originalVertices로 초기화
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }

        // 변형 적용
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            ApplyForceToVertex(i, point, force);
        }

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();
    }

    void ApplyForceToVertex(int i, Vector3 point, float force)  // 메쉬의 각 vertex에 외부 힘을 적용하는 함수
    {
        Debug.Log("ApplyForceToVertex");

        // 메쉬의 중심 계산 (로컬 좌표 기준)
        Vector3 meshCenter = Vector3.zero;

        // 중심점으로부터 각 버텍스까지의 벡터 계산
        Vector3 vertexToCenter = meshCenter - displacedVertices[i];

        // 거리에 따라 힘 감소
        float attenuatedForce = force / (1f + vertexToCenter.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;  // 힘을 시간에 따라 계산한 속도

        // 중심을 향한 방향으로 속도 적용
        vertexVelocities[i] += vertexToCenter.normalized * velocity;

        // 변형 적용: 무조건 중심을 향해 이동하도록 설정
        displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;

        // 원래 위치에서 중심을 향한 이동만 가능하도록, 멀어지지 않도록 제한
        if ((originalVertices[i] - meshCenter).magnitude < (displacedVertices[i] - meshCenter).magnitude)
        {
            displacedVertices[i] = originalVertices[i];  // 원래 위치로 되돌림
            vertexVelocities[i] = Vector3.zero;          // 속도 초기화
        }
    }
}
