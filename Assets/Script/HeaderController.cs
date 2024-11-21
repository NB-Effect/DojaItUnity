using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderController : MonoBehaviour
{
    [SerializeField] private GameObject headerMenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHeaderMenu()
    {
        if (headerMenuPanel != null)
        {
            headerMenuPanel.SetActive(!headerMenuPanel.activeSelf);
        }
    }
}
