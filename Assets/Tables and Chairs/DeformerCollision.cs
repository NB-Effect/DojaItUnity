using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformerCollision : MonoBehaviour
{
    public float force = 100f; // �޽��� �������� ���� ũ��

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹");

        MeshDeformer deformer = collision.collider.GetComponent<MeshDeformer>(); // �浹�� ������Ʈ�� meshDeformer ������Ʈ�� ������
        if (deformer != null)
        {
            Debug.Log("����");
            Vector3 point = collision.contacts[0].point; // collision.contacts�� �浹 ������ �迭, [0]�� ù��° ���� ������ ����
            deformer.AddDeformingForce(point, force); // �浹 ������ force ũ���� ���� ���� �޽��� ������
        }
    }
}
