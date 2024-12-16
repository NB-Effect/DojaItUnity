using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public InputAction rightTriggerAction;
    public bool isTriggerButton = false;
    public string sceneToLoad;
    public LayerMask buttonLayer;

    private void Awake()
    {
        rightTriggerAction.performed += OnRightTriggerPressed;
    }

    private void OnEnable()
    {
        rightTriggerAction.Enable();
    }

    private void OnDisable()
    {
        rightTriggerAction.Disable();
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        if (isTriggerButton)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        Debug.Log("¾À ÀüÈ¯: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
