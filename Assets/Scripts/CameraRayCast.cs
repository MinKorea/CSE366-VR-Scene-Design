using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class CameraRayCast : MonoBehaviour
{
    public float range = 100.0f;

    private bool inCart = false;

    public float coolTime = 5.0f;

    public float getInCartTimeLeft = 5.0f;
    public float getOutCartTimeLeft = 5.0f;

    public Text inCartText;
    public Text outCartText;

    private void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward;

        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));

        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

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

            /*if (Physics.Raycast(ray, out RaycastHit hit1, range))
            {
                print("Ray casting inside of cart");
                if (hit1.collider.tag == "Board")
                {
                    print("Board Ray Collision Detected");
                    getOutCartTimeLeft -= Time.deltaTime;
                    outCartText.text = (getOutCartTimeLeft).ToString("0");
                    if(getOutCartTimeLeft < 0)
                    {
                        GameObject.Find("XR Origin").transform.position = GameObject.Find("BasePos").transform.position;
                        GameObject.Find("XR Origin").transform.rotation = GameObject.Find("BasePos").transform.rotation;

                        GameObject.Find("LowpolySteamCar").GetComponent<SplineAnimate>().Pause();
                        GameObject.Find("LowpolySteamCar").transform.position = GameObject.Find("CartOrigin").transform.position;
                        GameObject.Find("LowpolySteamCar").transform.rotation = GameObject.Find("CartOrigin").transform.rotation;

                        getOutCartTimeLeft = coolTime;
                        inCart = false;
                    }
                }
                else
                {
                    getOutCartTimeLeft = coolTime;
                }
            }*/
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit2, range))
            {
                if (hit2.collider.tag == "Cart")
                {
                    print("Cart Ray Collision Detected");
                    getInCartTimeLeft -= Time.deltaTime;
                    inCartText.text = (getInCartTimeLeft).ToString("0");
                    if (getInCartTimeLeft < 0)
                    {

                        GameObject.Find("XR Origin").transform.position = hit2.collider.gameObject.GetNamedChild("TeleportPos").transform.position;
                        GameObject.Find("XR Origin").transform.rotation = hit2.collider.gameObject.GetNamedChild("TeleportPos").transform.rotation;
                        // this.transform.position = hit2.collider.gameObject.GetNamedChild("TeleportPos").transform.position;
                        // this.transform.rotation = hit2.collider.gameObject.GetNamedChild("TeleportPos").transform.rotation;

                        hit2.collider.gameObject.GetComponent<SplineAnimate>().Play();

                        getInCartTimeLeft = coolTime;
                        inCartText.text = "";
                        inCart = true;
                    }
                        
                }
                else
                {
                    getInCartTimeLeft = coolTime;
                    inCartText.text = "";
                }
            }
        }
    }
}
