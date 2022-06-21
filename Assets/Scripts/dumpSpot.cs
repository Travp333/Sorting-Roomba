using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumpSpot : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player"){
            //Debug.Log("{TEST");
            other.gameObject.GetComponent<Scan>().dumpInventory();
        }
    }
}
