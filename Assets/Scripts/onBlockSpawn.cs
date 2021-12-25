using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onBlockSpawn : MonoBehaviour, IPooledObject
{
    private Collider2D thisCollider;
    private Rigidbody2D thisRigidbody;
    public float gravity;
    public void OnObjectSpawn()
    {
        //thisCollider = gameObject.GetComponent<Collider2D>();
        thisRigidbody = gameObject.GetComponent<Rigidbody2D>();
        gameObject.layer = LayerMask.NameToLayer("FreeBlocks");
        gravity = thisRigidbody.gravityScale;
        thisRigidbody.gravityScale = 0f;
        thisRigidbody.velocity = new Vector3(0f, -1f, 0f);
    }

    void OnCollisionEnter2D()
    {
        //OnCollisionEnter is still called when scipt is disabled
        //Debug.Log("OnCollisionEnter2D called.");
        if(this.enabled)
        {
            gameObject.layer = LayerMask.NameToLayer("Blocks");
            thisRigidbody.gravityScale = gravity;
            this.enabled = false;
        }
    }

    public void recycled()
    {
        if(this.enabled)
        {
            gameObject.layer = LayerMask.NameToLayer("Blocks");
            thisRigidbody.gravityScale = gravity;
            this.enabled = false;
        }
    }
}