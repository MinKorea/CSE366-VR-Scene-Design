using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBird : MonoBehaviour
{
    private GameObject player;
    private CameraRayCast playerRay;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera");
        playerRay = player.GetComponent<CameraRayCast>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "AppleZone" && playerRay.grabbedFruit == true && other.gameObject.name == "lb_cardinal" && playerRay.fruit != null)
        {
            if(playerRay.fruit.tag == "Apple")
            {
                GameObject.Destroy(playerRay.fruit);
                playerRay.feedAppleCount += 1;
            }
        }
        else if (this.gameObject.tag == "OrangeZone" && playerRay.grabbedFruit == true && other.gameObject.name == "lb_robin" && playerRay.fruit != null)
        {
            if(playerRay.fruit.tag == "Orange")
            {
                GameObject.Destroy(playerRay.fruit);
                playerRay.feedOrangeCount += 1;
            }
            
        }
        else if (this.gameObject.tag == "PearZone" && playerRay.grabbedFruit == true && other.gameObject.name == "lb_goldFinch" && playerRay.fruit != null)
        {
            if(playerRay.fruit.tag == "Pear")
            {
                GameObject.Destroy(playerRay.fruit);
                playerRay.feedPearCount += 1;
            }
            
        }
    }
}
