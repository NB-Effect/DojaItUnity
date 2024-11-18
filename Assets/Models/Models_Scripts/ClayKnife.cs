using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRKnife : XRGrabInteractable
{
    public float cutStrength = 0.01f;
    public float cutRadius = 0.05f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // 필요한 경우 여기에 그랩 시 로직 추가
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // 필요한 경우 여기에 릴리스 시 로직 추가
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ClaySimulator clay))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                clay.DeformMesh(contact.point, cutStrength, cutRadius);
            }
        }
    }
}