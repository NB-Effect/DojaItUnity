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
        // �ʿ��� ��� ���⿡ �׷� �� ���� �߰�
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // �ʿ��� ��� ���⿡ ������ �� ���� �߰�
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