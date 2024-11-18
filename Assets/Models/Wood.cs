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

        // Skinned mesh renderer key's value is clamped between 0 & 100
        float currentWeight = skinnedMeshRenderer.GetBlendShapeWeight(keyIndex);
        float newWeight = Mathf.Clamp(currentWeight + damage * 100, 0, 60);
        skinnedMeshRenderer.SetBlendShapeWeight(keyIndex, newWeight);

        // 콜라이더 너비 업데이트 (선택적)
        colliderWidth = Mathf.Max(colliderWidth - damage, 0f);
    }

    public void Improve(int keyIndex, float improvement)
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not assigned in Wood script!");
            return;
        }

        // Skinned mesh renderer key's value is clamped between 0 & 100
        float currentWeight = skinnedMeshRenderer.GetBlendShapeWeight(keyIndex);
        float newWeight = Mathf.Clamp(currentWeight - improvement * 100, 0, 60);
        skinnedMeshRenderer.SetBlendShapeWeight(keyIndex, newWeight);

        // 콜라이더 너비 업데이트 (선택적)
        colliderWidth = Mathf.Min(colliderWidth + improvement, 2.000469f); // 초기 너비를 넘지 않도록 제한
    }
}