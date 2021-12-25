using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInstantiator : MonoBehaviour
{

    [System.Serializable]
    public class objectPool
    {
        public string tag;
        public GameObject prefab;
        public int initsize = 10;
        public ObjectPool pool;
    }

    public bool SpawnerProcess = true;
    private bool SpawnerPreStatus = false;
    public float SpawnInterval = 5f;
    public List<objectPool> poolList;
    private GameObject poolElement;
    private Transform objParent;
    private CircleCollider2D spawnCheck;
    private Vector3 spawnPoint;
    string objTag;
    bool spawnClear = true;

    public float T_buf;

    public bool delay_stop;


    void Start()
    {
        objParent = GameObject.Find("Blocks").transform;
        spawnCheck = gameObject.GetComponent<CircleCollider2D>();
        foreach (objectPool Pool in poolList)
        {
            Pool.pool = new ObjectPool(Pool.prefab, Pool.initsize, objParent);
        }

        T_buf = 0.0f;
        delay_stop = false;


    }

    void OnTriggerStay2D()
    {
        spawnClear = false;
    }
    void OnTriggerExit2D()
    {
        spawnClear = true;
    }

    void Update()
    {

        if (GameObject.Find("highpointFinder").GetComponent<highpointFinder>().toohigh || delay_stop)                   //block 超過特定高度 暫停生成 block
        {
            SpawnerProcess = false;
            T_buf += Time.deltaTime;
            delay_stop = true;
            if (T_buf >= 7.0f) delay_stop = false;
        }

        else
        {
            SpawnerProcess = true;
            T_buf = 0.0f;
        }

        if (!SpawnerProcess && SpawnerPreStatus)
            SpawnerPreStatus = false;
        else if (SpawnerProcess != SpawnerPreStatus)
        {
            SpawnerPreStatus = SpawnerProcess;
            StartCoroutine(spawnBlock(SpawnInterval));
        }

        Vector2 Offset = spawnCheck.offset;
        Offset.y += spawnClear ? (Offset.y > 0f ? -0.1f : 0f) : 0.1f;
        spawnCheck.offset = Offset;

        //manual spawning
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int rgn = UnityEngine.Random.Range(0, poolList.Count);
            objTag = poolList[rgn].tag;
            poolElement = poolList.Find(x => x.tag == objTag).pool.get();
            poolElement.transform.position = transform.position;
            poolElement.transform.rotation = Quaternion.identity;
            poolElement.tag = "Ground";
            poolElement.SetActive(true);
            poolElement.GetComponent<onBlockSpawn>().enabled = true;

            IPooledObject pooledObj = poolElement.GetComponent<IPooledObject>();
            pooledObj.OnObjectSpawn();
        }
    }
    IEnumerator spawnBlock(float spawnInterval)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(SpawnInterval);
        //WaitUntil waitUntil = new WaitUntil(() => spawnClear);
        while (SpawnerProcess)
        {
            Debug.Log("Spawner Running...");
            int rgn = UnityEngine.Random.Range(0, poolList.Count);
            objTag = poolList[rgn].tag;
            poolElement = poolList.Find(x => x.tag == objTag).pool.get();

            yield return new WaitUntil(() => spawnClear);
            poolElement.transform.position = transform.position + new Vector3(spawnCheck.offset.x, spawnCheck.offset.y, 0f);
            poolElement.tag = "Ground";
            poolElement.transform.rotation = Quaternion.identity;
            poolElement.SetActive(true);
            poolElement.GetComponent<onBlockSpawn>().enabled = true;

            IPooledObject pooledObj = poolElement.GetComponent<IPooledObject>();
            pooledObj.OnObjectSpawn();

            yield return waitForSeconds;
        }
    }
}