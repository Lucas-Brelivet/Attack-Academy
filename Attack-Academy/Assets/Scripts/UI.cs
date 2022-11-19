using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    private Controls controls;


    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.UI.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was clicked
        if (controls.UI.Click.IsPressed())
        {
            // Check if the mouse was clicked over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
            }
        }
    }
}
