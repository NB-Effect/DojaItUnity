using UnityEngine;
using DG.Tweening;

public class LControllerModelAndRotationSwitcher : MonoBehaviour
{
    [Header("Controller Models")]
    [SerializeField] private GameObject defaultControllerModel; // �⺻ ��Ʈ�ѷ� ��
    [SerializeField] private GameObject alternateControllerModel; // ��ü ��Ʈ�ѷ� ��

    [Header("Hand Rotation")]
    [SerializeField] private Transform handModel; // �� �� Transform
    [SerializeField] private Vector3 targetRotation = new Vector3(30f, 0f, 45f); // ���ڱ⸦ ���� ��ǥ ȸ�� �� (X, Y, Z)
    [SerializeField] private float rotationDuration = 0.5f; // ȸ�� �ִϸ��̼� ���� �ð�

    private Quaternion originalRotation; // �� ���� ���� ȸ�� ��

    private void Start()
    {
        // ��Ʈ�ѷ� �� �ʱ� ����
        defaultControllerModel.SetActive(true);
        alternateControllerModel.SetActive(false);

        // �� �� �ʱ� ȸ�� �� ����
        if (handModel != null)
        {
            originalRotation = handModel.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController")) // ��Ʈ�ѷ� �±� Ȯ��
        {
            Debug.Log("��Ʈ�ѷ��� �ݰ� ������ ���Խ��ϴ�.");

            // ��Ʈ�ѷ� �� ��ȯ
            defaultControllerModel.SetActive(false);
            alternateControllerModel.SetActive(true);

            // �� �� ȸ�� (��ǥ ȸ�� ������)
            if (handModel != null)
            {
                handModel.DORotate(targetRotation, rotationDuration)
                    .SetEase(Ease.OutCubic);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController")) // ��Ʈ�ѷ� �±� Ȯ��
        {
            Debug.Log("��Ʈ�ѷ��� �ݰ� ������ �������ϴ�.");

            // ��Ʈ�ѷ� �� ���� ����
            defaultControllerModel.SetActive(true);
            alternateControllerModel.SetActive(false);

            // �� �� ȸ�� ���� (���� ȸ�� ������)
            if (handModel != null)
            {
                handModel.DORotateQuaternion(originalRotation, rotationDuration)
                    .SetEase(Ease.OutCubic);
            }
        }
    }
}
