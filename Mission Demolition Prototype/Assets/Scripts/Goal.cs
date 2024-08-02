using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
