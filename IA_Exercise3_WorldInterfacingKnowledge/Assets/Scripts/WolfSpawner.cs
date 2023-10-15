using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    public GameObject wolfPrefab;
    public int numWolves = 5;
    public GameObject[] wolfArray;
    public Vector3 spawnLimits = new Vector3(5, 5, 5);

    public GameObject target;
    public bool detection = false;

    private float waitingTime;
    private float actualTime;
    private bool activate = false;
    private bool timerOut = false;

    // Start is called before the first frame update
    void Start()
    {
        waitingTime = 20f;
        actualTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        actualTime += Time.deltaTime;

        if (actualTime > waitingTime && !timerOut)
        {
            activate = true;
            timerOut = true;
        }

        if (activate)
        {
            wolfArray = new GameObject[numWolves];

            for (int i = 0; i < numWolves; i++)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-spawnLimits.x, spawnLimits.x),
                                                               Random.Range(-spawnLimits.y, spawnLimits.y),
                                                               Random.Range(-spawnLimits.z, spawnLimits.z));

                wolfArray[i] = Instantiate(wolfPrefab, pos, Quaternion.identity);
                wolfArray[i].GetComponent<AIVision>().wolf.target = target;

            }

            activate = false;
        }

        foreach (GameObject obj in wolfArray)
        {
            // I tried to do Unity Messaging's BroadcastMessage method here but it doesn't worked properly,
            // so I used an equivalent way to communicate between wolves.

            if (obj.GetComponent<AIVision>().playerDetected && !detection) 
            {
                detection = true;
            }

            if (detection)
            {
                obj.GetComponent<AIVision>().SetStateToSeek();
            }

        }

    }

}
