using UnityEngine;
using DG.Tweening;

public class LControllerModelAndRotationSwitcher : MonoBehaviour
{
    [Header("Controller Models")]
    [SerializeField] private GameObject defaultControllerModel; // 기본 컨트롤러 모델
    [SerializeField] private GameObject alternateControllerModel; // 대체 컨트롤러 모델

    [Header("Hand Rotation")]
    [SerializeField] private Transform handModel; // 손 모델 Transform
    [SerializeField] private Vector3 targetRotation = new Vector3(30f, 0f, 45f); // 도자기를 감쌀 목표 회전 값 (X, Y, Z)
    [SerializeField] private float rotationDuration = 0.5f; // 회전 애니메이션 지속 시간

    private Quaternion originalRotation; // 손 모델의 원래 회전 값

    private void Start()
    {
        // 컨트롤러 모델 초기 설정
        defaultControllerModel.SetActive(true);
        alternateControllerModel.SetActive(false);

        // 손 모델 초기 회전 값 저장
        if (handModel != null)
        {
            originalRotation = handModel.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController")) // 컨트롤러 태그 확인
        {
            Debug.Log("컨트롤러가 반경 안으로 들어왔습니다.");

            // 컨트롤러 모델 전환
            defaultControllerModel.SetActive(false);
            alternateControllerModel.SetActive(true);

            // 손 모델 회전 (목표 회전 값으로)
            if (handModel != null)
            {
                handModel.DORotate(targetRotation, rotationDuration)
                    .SetEase(Ease.OutCubic);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController")) // 컨트롤러 태그 확인
        {
            Debug.Log("컨트롤러가 반경 밖으로 나갔습니다.");

            // 컨트롤러 모델 원상 복구
            defaultControllerModel.SetActive(true);
            alternateControllerModel.SetActive(false);

            // 손 모델 회전 복구 (원래 회전 값으로)
            if (handModel != null)
            {
                handModel.DORotateQuaternion(originalRotation, rotationDuration)
                    .SetEase(Ease.OutCubic);
            }
        }
    }
}
