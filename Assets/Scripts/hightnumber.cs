using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class hightnumber : MonoBehaviour
{
    private GameObject hightpoint;

    public Text hightText;

    public float hight;

    // Start is called before the first frame update
    void Start()
    {
        hightpoint = GameObject.Find("blockSpawner");
        hight = 0.0f;
        hightText.text = hight.ToString("0.0");
    }

    // Update is called once per frame
    void Update()
    {
        hight = hightpoint.transform.position.y - 11.1f;
        hightText.text = hight.ToString("0.0");
        //Debug.Log(hight);
    }
}
