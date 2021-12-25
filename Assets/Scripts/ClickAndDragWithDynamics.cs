using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDragWithDynamics : MonoBehaviour
{
    private float ogDrag;
    public float forceMP = 20;
    public float rotationDrag = 10;
    //Debug
    public float offset_x = 0;
    public float offset_y = 0;
    public float Newoffset_x = 0;
    public float Newoffset_y = 0;
    public float offset_A = 0;
    public float Rad = 0;

    public Collider2D targetObject;
    private Rigidbody2D selectedObject;
    private LineRenderer lineRend;
    private HingeJoint2D _hingeJoint;
    private onBlockSpawn spawnScript;
    Vector3 ObjPos;
    Vector3 Newoffset;
    Vector3 mousePosition;

    Vector2 dragForce;
    Vector2 offset;

    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                if (targetObject.transform.gameObject.GetComponent<Rigidbody2D>() == null ||
                    targetObject.transform.gameObject.layer == LayerMask.NameToLayer("blocks")) return;
                selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
                spawnScript = targetObject.transform.gameObject.GetComponent<onBlockSpawn>();
                selectedObject.transform.gameObject.layer = LayerMask.NameToLayer("FreeBlocks");


                if (spawnScript.enabled)
                {
                    selectedObject.gravityScale = spawnScript.gravity;
                    spawnScript.enabled = false;
                }//turn off slow drop when block is grabbed

                ogDrag = selectedObject.angularDrag;
                selectedObject.angularDrag = rotationDrag;
                ObjPos = selectedObject.transform.position;

                offset = new Vector2(mousePosition.x - ObjPos.x,
                                        mousePosition.y - ObjPos.y);
                offset = Quaternion.Euler(0, 0, -selectedObject.rotation) * offset;
                //Debug
                offset_x = offset.x; offset_y = offset.y;
                offset_A = Vector2.SignedAngle(Vector2.right, offset);

                Rad = offset.magnitude;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.transform.gameObject.layer = LayerMask.NameToLayer("Blocks");
            selectedObject.angularDrag = ogDrag;
            selectedObject = null;
            lineRend.SetPosition(0, new Vector3(0f, 0f, 0f));
            lineRend.SetPosition(1, new Vector3(0f, 0f, 0f));
        }
    }

    private Vector3 anchorPoint;
    private Vector2 objV;
    private float offsetDeg;

    void FixedUpdate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selectedObject)
        {
            ObjPos = selectedObject.transform.position;

            offsetDeg = Mathf.Deg2Rad * (selectedObject.rotation + Vector2.SignedAngle(Vector2.right, offset));
            Newoffset.x = Mathf.Cos(offsetDeg) * Rad;
            Newoffset.y = Mathf.Sin(offsetDeg) * Rad;

            Newoffset_x = Newoffset.x; Newoffset_y = Newoffset.y;

            anchorPoint = new Vector3(ObjPos.x + Newoffset.x, ObjPos.y + Newoffset.y, 0f);
            lineRend.SetPosition(0, anchorPoint);
            lineRend.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0f));

            dragForce = (mousePosition - anchorPoint);
            selectedObject.AddForceAtPosition(dragForce * forceMP, anchorPoint, ForceMode2D.Force);
            //Capping object max speed for better control
            objV = new Vector2(selectedObject.velocity.x, selectedObject.velocity.y);
            if (objV.magnitude >= dragForce.magnitude + 1)
            {
                objV = Vector2.ClampMagnitude(objV, dragForce.magnitude + 1);
                selectedObject.velocity = new Vector3(objV.x, objV.y, 0f);
            }
        }
    }
}