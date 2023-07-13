using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{
    public GameObject panel;
    private bool panelActive = false;

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
                panelActive = true;
            }
        }
    }
}
