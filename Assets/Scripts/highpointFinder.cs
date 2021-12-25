using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highpointFinder : MonoBehaviour
{
    private GameObject Cam;
    private GameObject blockRecycler;
    private EdgeCollider2D BRcollider;
    private GameObject blockSpawner;
    private RaycastHit2D hit;
    private RaycastHit2D noHit;
    private RaycastHit2D Spawnpoint;
    public Vector3 rayPos;
    public float startHeight;
    //public float lastHeight;
    public float spawnerHeight = 15f;
    public string detectedBlock;

    public float Hight;                     //block 最高高度

    private float flag = 15.1f;             //設定 多高

    public float T_counter;                 //Time counter 過設定高度 _ 秒

    public bool toohigh;                    //是否過設定高度

    public GameObject player;
    public GameObject Fish;

    private bool Generate;                  //player 是否 生成


    void Start()
    {
        Cam = Camera.main.gameObject;
        blockRecycler = GameObject.Find("blockRecycler");
        BRcollider = blockRecycler.GetComponent<EdgeCollider2D>();
        blockSpawner = GameObject.Find("blockSpawner");
        startHeight = gameObject.transform.position.y;
        //lastHeight = startHeight;
        blockSpawner.transform.position = new Vector3(0f, startHeight + spawnerHeight, 0f);
        toohigh = false;
        Generate = false;

    }
    void Update()
    {
        rayPos = gameObject.transform.position;
        hit = Physics2D.Raycast(new Vector2(rayPos.x, rayPos.y), Vector2.right, 20, LayerMask.GetMask("Blocks"));
        noHit = Physics2D.Raycast(new Vector2(rayPos.x, rayPos.y + 0.2f), Vector2.right, 20, LayerMask.GetMask("Blocks"));

        Debug.DrawRay(new Vector2(rayPos.x, rayPos.y), Vector3.right * 20, Color.green);
        if (noHit.collider)
        {
            //lastHeight = rayPos.y;
            rayPos.y += 0.1f;
            gameObject.transform.position = rayPos;
            blockSpawner.transform.position = new Vector3(0f, rayPos.y + spawnerHeight, 0f);

            /*if(hit.collider)
            {
                detectedBlock = hit.transform.name;
                Debug.Log("highpoint found " + detectedBlock);
            }*/
        }
        else if (!hit.collider && !noHit.collider)
        {
            if (rayPos.y > startHeight)
            {
                rayPos.y -= 0.1f;
                gameObject.transform.position = rayPos;
                blockSpawner.transform.position = new Vector3(0f, rayPos.y + spawnerHeight, 0f);
            }
        }

        Hight = blockSpawner.transform.position.y;
        //Debug.Log(Hight);

        if (Hight >= flag)                                      //過特定高度
        {
            toohigh = true;
            T_counter += Time.deltaTime;
            if (T_counter > 10.0f && !Generate)                  //超過3秒 且 未生成過 player
            {
                Instantiate(player);
                for (int i = -5; i < 6; i += 2)
                {

                    Instantiate(Fish, new Vector3(i, Hight - flag + 1.0f, 0.0f), Quaternion.identity);
                }
                Generate = true;
            }
        }
        else
        {
            T_counter = 0.0f;
            toohigh = false;
        }

    }

    void LateUpdate()
    {
        float recycler_y = blockRecycler.transform.position.y + BRcollider.points[0].y;
        if (rayPos.y + spawnerHeight + 25 >= recycler_y)
        {
            Vector2[] newPoints = BRcollider.points;
            newPoints[0].y += 25;
            newPoints[1].y += 25;
            newPoints[4].y += 25;
            newPoints[5].y += 25;
            BRcollider.points = newPoints;
            //Debug.Log("Recycler border updated.");
        }
    }
}
