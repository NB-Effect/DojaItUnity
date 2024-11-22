using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformerCollision : MonoBehaviour
{
    public float force = 100f; // 메쉬에 가해지는 힘의 크기

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌");

        MeshDeformer deformer = collision.collider.GetComponent<MeshDeformer>(); // 충돌한 오브젝트의 meshDeformer 컴포넌트를 가져옴
        if (deformer != null)
        {
            Debug.Log("변형");
            Vector3 point = collision.contacts[0].point; // collision.contacts는 충돌 지점의 배열, [0]은 첫번째 접촉 지점을 말함
            deformer.AddDeformingForce(point, force); // 충돌 지점에 force 크기의 힘을 가해 메쉬를 변형함
        }
    }
}
