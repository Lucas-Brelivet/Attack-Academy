using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableUI : MonoBehaviour
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
        //Prevent player from moving when you click on UI
        // Check if the mouse is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if(!Player.Instance.controls.Player.Move.IsPressed())
            {
                Player.Instance.controls.Player.Move.Disable();
            }
        }
        else
        {
            Player.Instance.controls.Player.Move.Enable();
        }
    }
}
