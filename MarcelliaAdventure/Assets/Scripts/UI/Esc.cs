using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{
    public GameObject panel;
    public bool panelActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelActive)
            {
                panel.SetActive(false);
                panelActive = false;
                
            }
            else
            {
                panel.SetActive(true);
                Time.timeScale = 0f;
                panelActive = true;
                
            }
        }
    }
}
