using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    public InputActionProperty buttonInput;
    public bool activeText = false;

    private bool isPressed = false;

    private void Start()
    {
        this.gameObject.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Checklist Input value: " + buttonInput.action.ReadValue<float>().ToString());
        if(buttonInput.action.ReadValue<float>() == 0.0f)
        {
            isPressed = false;
        }

        if (buttonInput.action.ReadValue<float>() == 1.0f && activeText == true && isPressed == false)
        {
            activeText = false;
            this.gameObject.GetComponent<Text>().enabled = activeText;
            isPressed = true;
        }
        else if(buttonInput.action.ReadValue<float>() == 1.0f && activeText == false && isPressed == false)
        {
            activeText = true;
            this.gameObject.GetComponent<Text>().enabled = activeText;
            isPressed = true;
        }

    }
}
