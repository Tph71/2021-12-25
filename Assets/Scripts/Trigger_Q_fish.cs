using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Q_fish : MonoBehaviour
{
    //音效-----------

    public GameObject soundObject;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "Asia(Clone)")
        {
            Instantiate(soundObject, transform.position, Quaternion.identity);
            Debug.Log(gameObject.name);
            Destroy(gameObject, 0.1f);
        }
    }

}
