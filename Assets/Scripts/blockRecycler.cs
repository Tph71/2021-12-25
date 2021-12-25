using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockRecycler : MonoBehaviour
{
    GameObject H;
    Health HP;
    void Start()
    {
        HP = GameObject.Find("PlayerHealth").GetComponent<Health>();
    }
    void OnTriggerEnter2D(Collider2D Block)
    {
        Block.gameObject.GetComponent<onBlockSpawn>().recycled();
        Block.gameObject.SetActive(false);
        HP.health -= (HP.health == 0 ? 0 : 1);
        //Debug.Log("Block " + Block.gameObject.name + " recycled.");
    }
}
