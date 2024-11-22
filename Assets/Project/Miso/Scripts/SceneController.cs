using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveScene()
    {
        Debug.Log("MoveScene");
        if (SceneManager.GetActiveScene().name == "UI_Title")
        {
            Debug.Log("Move Scene To UI_Home");
            SceneManager.LoadScene("UI_Home");
        }
       
    }

    public void EnterButton()
    {
        Debug.Log("EnterButton");
    }

    public void ClickButton()
    {
        Debug.Log("ClickButton");
    }
}
