using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
