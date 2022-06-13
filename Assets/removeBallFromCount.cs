using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeBallFromCount : MonoBehaviour
{
    public GameObject player;
    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Ball"){
            player.GetComponent<Scan>().removeBall(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Ball"){
            player.GetComponent<Scan>().addBall(other.gameObject);
        }
    }
}
