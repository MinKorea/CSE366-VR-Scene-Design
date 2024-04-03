using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MinimapButtonToggle : MonoBehaviour
{
    public InputActionProperty buttonInput;
    public bool activeMap = false;
    private bool isPressed = false;

    private void Start()
    {
        this.gameObject.GetComponent<Image>().enabled = false;
        this.gameObject.GetNamedChild("Minimap").GetComponent<RawImage>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Minimap Input value: " + buttonInput.action.ReadValue<float>().ToString());

        if (buttonInput.action.ReadValue<float>() == 0.0f)
        {
            isPressed = false;
        }

        if (buttonInput.action.ReadValue<float>() == 1.0f && activeMap == true && isPressed == false)
        {
            activeMap = false;
            this.gameObject.GetComponent<Image>().enabled = activeMap;
            this.gameObject.GetNamedChild("Minimap").GetComponent<RawImage>().enabled = activeMap;
            isPressed = true;
        }
        else if (buttonInput.action.ReadValue<float>() == 1.0f && activeMap == false && isPressed == false)
        {
            activeMap = true;
            this.gameObject.GetComponent<Image>().enabled = activeMap;
            this.gameObject.GetNamedChild("Minimap").GetComponent<RawImage>().enabled = activeMap;
            isPressed = true;
        }
    }
}
