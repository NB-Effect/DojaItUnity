using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Coll : MonoBehaviour
{
    public int index;
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        index = transform.GetSiblingIndex();
    }
    private void Start()
    {
        Debug.Log("박스 사이즈 " + boxCollider.size.x);
    }

    public void HitCollider(float damage)
    {
        // Resize the collider's width(X) depends on "damage"
        if (boxCollider.size.x - damage > 0.0f)
        {
            boxCollider.size = new Vector3(boxCollider.size.x - damage, boxCollider.size.y, boxCollider.size.z);

            // 콜라이더 중심점을 조정하여 한쪽에서만 깎이도록
            boxCollider.center = new Vector3(boxCollider.center.x - damage / 2, boxCollider.center.y, boxCollider.center.z);
        }
    }

    public void ImprovementCollider(float improvement)
    {
        // Resize the collider's width(X) depends on "damage"
        if (boxCollider.size.x + improvement > 0.0f)
        {
            boxCollider.size = new Vector3(boxCollider.size.x + improvement, boxCollider.size.y, boxCollider.size.z);

            // 콜라이더 중심점을 조정하여 한쪽에서만 깎이도록
            boxCollider.center = new Vector3(boxCollider.center.x + improvement / 2, boxCollider.center.y, boxCollider.center.z);
        }
    }
}