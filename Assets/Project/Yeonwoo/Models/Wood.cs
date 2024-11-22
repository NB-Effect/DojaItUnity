using UnityEngine;
using DG.Tweening;

public class Wood : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Transform woodTransform;
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotationDuration;
    [SerializeField] private float colliderWidth = 2.000469f; // 콜라이더의 초기 너비

    private void Start()
    {
        // 회전 시작
        StartRotating();
    }

    private void StartRotating()
    {
        woodTransform
            .DOLocalRotate(rotationVector, rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public void Hit(int keyIndex, float damage)
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not assigned in Wood script!");
            return;
        }

        // 현재 key 값 조정
        AdjustBlendShape(keyIndex, damage);

        // 주변 key 값 조정
        float adjacentDamage = damage * 0.3f; // 근처 key는 변화량의 50%만 반영
        AdjustAdjacentBlendShapes(keyIndex, adjacentDamage);

        // 콜라이더 너비 업데이트
        colliderWidth = Mathf.Max(colliderWidth - damage, 0f);
    }

    public void Improve(int keyIndex, float improvement)
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not assigned in Wood script!");
            return;
        }

        // 현재 key 값 조정
        AdjustBlendShape(keyIndex, -improvement);

        // 주변 key 값 조정
        float adjacentImprovement = improvement * 0.3f; // 근처 key는 변화량의 50%만 반영
        AdjustAdjacentBlendShapes(keyIndex, -adjacentImprovement);

        // 콜라이더 너비 업데이트
        colliderWidth = Mathf.Min(colliderWidth + improvement, 2.000469f); // 초기 너비를 넘지 않도록 제한
    }

    // 현재 key 값 조정 메서드
    private void AdjustBlendShape(int keyIndex, float change)
    {
        float currentWeight = skinnedMeshRenderer.GetBlendShapeWeight(keyIndex);
        float newWeight = Mathf.Clamp(currentWeight + change * 100, 0, 60);
        skinnedMeshRenderer.SetBlendShapeWeight(keyIndex, newWeight);
    }

    // 주변 key 값 조정 메서드
    private void AdjustAdjacentBlendShapes(int keyIndex, float change)
    {
        // 이전 key 값 조정
        if (keyIndex > 0)
        {
            float prevWeight = skinnedMeshRenderer.GetBlendShapeWeight(keyIndex - 1);
            float newPrevWeight = Mathf.Clamp(prevWeight + change * 100, 0, 60);
            skinnedMeshRenderer.SetBlendShapeWeight(keyIndex - 1, newPrevWeight);
        }

        // 다음 key 값 조정
        if (keyIndex < skinnedMeshRenderer.sharedMesh.blendShapeCount - 1)
        {
            float nextWeight = skinnedMeshRenderer.GetBlendShapeWeight(keyIndex + 1);
            float newNextWeight = Mathf.Clamp(nextWeight + change * 100, 0, 60);
            skinnedMeshRenderer.SetBlendShapeWeight(keyIndex + 1, newNextWeight);
        }
    }
}
