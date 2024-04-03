using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveThirdPersonView : MonoBehaviour
{
    public InputActionProperty buttonInput;

    public GameObject firstPersonCam;
    public GameObject ThirdPersonCam;

    public bool activeThirdPersonView = false;
    private bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        firstPersonCam.SetActive(true);
        ThirdPersonCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonInput.action.ReadValue<float>() == 0.0f)
        {
            isPressed = false;
        }

        if (buttonInput.action.ReadValue<float>() == 1.0f && activeThirdPersonView == false && isPressed == false)
        {
            activeThirdPersonView = true;

            /*firstPersonCam.GetComponent<Camera>().enabled = false;
            ThirdPersonCam.GetComponent<Camera>().enabled = true;*/

            firstPersonCam.SetActive(false);
            ThirdPersonCam.SetActive(true);

            isPressed = true;
        }
        else if (buttonInput.action.ReadValue<float>() == 1.0f && activeThirdPersonView == true && isPressed == false)
        {
            activeThirdPersonView = false;

            /*firstPersonCam.GetComponent<Camera>().enabled = true;
            ThirdPersonCam.GetComponent<Camera>().enabled = false;*/

            firstPersonCam.SetActive(true);
            ThirdPersonCam.SetActive(false);

            isPressed = true;
        }

        if(activeThirdPersonView)
        {
            // ThirdPersonCam.transform.GetChild(0).rotation = ThirdPersonCam.transform.rotation;s
            ThirdPersonCam.transform.GetChild(0).position = GameObject.Find("XR Origin").transform.position;
        }
    }
}
