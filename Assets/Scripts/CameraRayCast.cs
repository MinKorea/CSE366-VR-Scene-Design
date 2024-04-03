using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Drawing;
using Color = UnityEngine.Color;

public class CameraRayCast : MonoBehaviour
{
    public InputActionProperty interactionInput;

    public float range = 10.0f;

    private bool inZone = false;
    private bool inCart = false;
    public bool grabbedFruit = false;
    private bool isPressed = false;
    private bool showMap = false;

    public GameObject fruit = null;

    public float coolTime = 5.0f;
    public float getInCartTimeLeft = 5.0f;
    public float getOutCartTimeLeft = 5.0f;

    private int collectAppleCount = 0;
    private int collectOrangeCount = 0;
    private int collectPearCount = 0;

    public int feedAppleCount = 0;
    public int feedOrangeCount = 0;
    public int feedPearCount = 0;

    public Text checklist; 
    public Text interactionText;
    public Text outCartText;

    private void Start()
    {
        Input.gyro.enabled = true;
        GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward;

        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));

        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

        if (showMap == false)
        {
            InteractTeleportHub(ray);
        }
        else
        {
            Teleport(ray);
        }

        ChecklistText();

        if (interactionInput.action.ReadValue<float>() == 0.0f)
        {
            isPressed = false;
        }

        if (inZone == false)
        {
            GetFruit(ray);

            if(grabbedFruit == true && fruit != null)
            {
                // fruit.transform.position = GameObject.Find(fruit.name + "Box").transform.position;
                fruit = null;
                grabbedFruit = false;
            }
            else if (grabbedFruit == true)
            {
                grabbedFruit = false;
            }
        }
        else
        {
            if(grabbedFruit == true && fruit != null)
            {
                fruit.transform.position = GameObject.Find("Right Hand Model").transform.position;
                fruit.transform.rotation = GameObject.Find("Right Hand Model").transform.rotation;
            }
            else
            {
                GrabFruit(ray);
            }
        }

        

        if (inCart)
        {
            GameObject.Find("XR Origin").transform.position = GameObject.Find("LowpolySteamCar").GetNamedChild("TeleportPos").transform.position;
            GameObject.Find("XR Origin").transform.rotation = GameObject.Find("LowpolySteamCar").GetNamedChild("TeleportPos").transform.rotation;

            if(this.transform.rotation.eulerAngles.x > 50)
            {
                getOutCartTimeLeft -= Time.deltaTime;
                outCartText.text = (getOutCartTimeLeft).ToString("0");
                if (getOutCartTimeLeft < 0)
                {
                    GameObject.Find("XR Origin").transform.position = GameObject.Find("BasePos").transform.position;
                    GameObject.Find("XR Origin").transform.rotation = GameObject.Find("BasePos").transform.rotation;

                    GameObject.Find("LowpolySteamCar").GetComponent<SplineAnimate>().Pause();
                    GameObject.Find("LowpolySteamCar").transform.position = GameObject.Find("CartOrigin").transform.position;
                    GameObject.Find("LowpolySteamCar").transform.rotation = GameObject.Find("CartOrigin").transform.rotation;

                    getOutCartTimeLeft = coolTime;
                    outCartText.text = "";
                    inCart = false;
                }
            }
            else
            {
                getOutCartTimeLeft = coolTime;
                outCartText.text = "";
            }

        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit2, range))
            {
                if (hit2.collider.tag == "Cart")
                {
                    print("Cart Ray Collision Detected");
                    getInCartTimeLeft -= Time.deltaTime;
                    interactionText.text = (getInCartTimeLeft).ToString("0");
                    if (getInCartTimeLeft < 0)
                    {

                        GameObject.Find("XR Origin").transform.position = hit2.collider.gameObject.GetNamedChild("TeleportPos").transform.position;
                        GameObject.Find("XR Origin").transform.rotation = hit2.collider.gameObject.GetNamedChild("TeleportPos").transform.rotation;

                        hit2.collider.gameObject.GetComponent<SplineAnimate>().Play();

                        getInCartTimeLeft = coolTime;
                        interactionText.text = "";
                        inCart = true;
                    }
                }
                else
                {
                    getInCartTimeLeft = coolTime;
                    if(interactionText.text.Length < 2) interactionText.text = "";

                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AppleZone") || other.CompareTag("OrangeZone") || other.CompareTag("PearZone"))
        {
            inZone = true;
            // Debug.Log("In Zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AppleZone") || other.CompareTag("OrangeZone") || other.gameObject.tag == "PearZone")
        {
            // Debug.Log("Out Zone");
            inZone = false;
        }
    }

    void GetFruit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit2, range))
        {
            if(hit2.collider.tag == "Apple")
            {
                interactionText.text = "Get Apple - A";
                // Debug.Log("Input value: " + interactionInput.action.ReadValue<float>().ToString());
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    isPressed = true;
                    // Debug.Log("Let's get an apple!");
                    Transform boxTransform = GameObject.Find("AppleBox").transform;
                    hit2.collider.gameObject.transform.SetParent(null);
                    hit2.collider.gameObject.transform.position = boxTransform.position + new Vector3(0, 5, 0);
                    hit2.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    collectAppleCount += 1;
                }
            }
            else if(hit2.collider.tag == "Orange")
            {
                interactionText.text = "Get Orange - A";

                // Debug.Log("Input value: " + interactionInput.action.ReadValue<float>().ToString());
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    isPressed = true;
                    // Debug.Log("Let's get an orange!");
                    Transform boxTransform = GameObject.Find("OrangeBox").transform;
                    hit2.collider.gameObject.transform.SetParent(null);
                    hit2.collider.gameObject.transform.position = boxTransform.position + new Vector3(0, 5, 0);
                    hit2.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    collectOrangeCount += 1;
                }
            }
            else if( hit2.collider.tag == "Pear")
            {
                interactionText.text = "Get Pear - A";

                // Debug.Log("Input value: " + interactionInput.action.ReadValue<float>().ToString());
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    isPressed = true;
                    // Debug.Log("Let's get a pear!");
                    Transform boxTransform = GameObject.Find("PearBox").transform;
                    hit2.collider.gameObject.transform.SetParent(null);
                    hit2.collider.gameObject.transform.position = boxTransform.position + new Vector3(0, 5, 0);
                    hit2.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    collectPearCount += 1;
                }
            }
            /*else
            {
                interactionText.text = "";
            }*/
        }
        else
        {
            interactionText.text = "";
        }
    }

    void GrabFruit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit2, range))
        {
            if (hit2.collider.tag == "Apple")
            {
                interactionText.text = "Grab Apple - A";
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    grabbedFruit = true;
                    hit2.collider.gameObject.transform.position = GameObject.Find("Right Hand Model").transform.position;
                    fruit = hit2.collider.gameObject;
                    isPressed = true;
                }

            }
            else if (hit2.collider.tag == "Orange")
            {
                interactionText.text = "Grab Orange - A";
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    grabbedFruit = true;
                    hit2.collider.gameObject.transform.position = GameObject.Find("Right Hand Model").transform.position;
                    fruit = hit2.collider.gameObject;
                    isPressed = true;
                }


            }
            else if (hit2.collider.tag == "Pear")
            {
                interactionText.text = "Grab Pear - A";
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    grabbedFruit = true;
                    hit2.collider.gameObject.transform.position = GameObject.Find("Right Hand Model").transform.position;
                    fruit = hit2.collider.gameObject;
                    isPressed = true;
                }

            }
            /*else
            {
                interactionText.text = "";
            }*/
        }
        else
        {
            interactionText.text = "";
        }
    }

    void ChecklistText()
    {
        checklist.text = 
            "Collect Fruits\r\n" +
            "    Apple (" + collectAppleCount + "/3)   \r\n" +
            "    Orange (" + collectOrangeCount + "/3)\r\n" +
            "    Pear (" + collectPearCount + "/3)\r\n" +
            "Feed fruits to birds\r\n" +
            "    Apple (" + feedAppleCount + "/3)\r\n" +
            "    Orange (" + feedOrangeCount + "/3)\r\n" +
            "    Pear (" + feedPearCount + "/3)";
    }

    void InteractTeleportHub(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit2, range))
        {
            if(hit2.collider.tag == "TeleportHub" && isPressed == false)
            {
                interactionText.text = "Open the Map - A";

                if (interactionInput.action.ReadValue<float>() == 1.0f)
                {
                    GameObject.Find("FixedCanvas").gameObject.GetComponent<RectTransform>().position = this.gameObject.transform.GetChild(1).position - new Vector3(0, -0.5f, -0.2f);
                    GameObject.Find("FixedCanvas").gameObject.GetComponent<RectTransform>().rotation = this.gameObject.transform.GetChild(1).rotation;
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    showMap = true;
                    isPressed = true;
                }

            }
            /*else
            {
                interactionText.text = "";
            }*/
        }
        else
        {
            interactionText.text = "";
        }
    }

    void Teleport(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit2, range))
        {
            if(hit2.collider.name == "Teleport1")
            {
                interactionText.text = "Teleport - A";

                if (interactionInput.action.ReadValue<float>() == 1.0f)
                {
                    GameObject.Find("XR Origin").transform.position = GameObject.Find("TeleportHub1").transform.position + new Vector3(0, 2, 0);
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                    showMap = false;
                    isPressed = true;
                }
            }
            else if (hit2.collider.name == "Teleport2")
            {
                interactionText.text = "Teleport - A";

                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    GameObject.Find("XR Origin").transform.position = GameObject.Find("TeleportHub2").transform.position + new Vector3(0, 2, 0);
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                    showMap = false;
                    isPressed = true;
                }
            }
            else if (hit2.collider.name == "Teleport3")
            {

                interactionText.text = "Teleport - A";

                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    GameObject.Find("XR Origin").transform.position = GameObject.Find("TeleportHub3").transform.position + new Vector3(0, 2, 0);
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                    showMap = false;
                    isPressed = true;
                }
            }
            else if (hit2.collider.name == "Teleport4")
            {

                interactionText.text = "Teleport - A";

                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    GameObject.Find("XR Origin").transform.position = GameObject.Find("TeleportHub4").transform.position + new Vector3(0, 2, 0);
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                    showMap = false;
                    isPressed = true;
                }
            }
            else if (hit2.collider.name == "Teleport5")
            {

                interactionText.text = "Teleport - A";

                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    GameObject.Find("XR Origin").transform.position = GameObject.Find("TeleportHub5").transform.position + new Vector3(0, 2, 0);
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                    showMap = false;
                    isPressed = true;
                }
            }
            else
            {
                if (interactionInput.action.ReadValue<float>() == 1.0f && isPressed == false)
                {
                    GameObject.Find("Worldmap").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                    showMap = false;
                    isPressed = true;
                }
            }
        }
        else
        {
            interactionText.text = "";
        }
    }
}   
