using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public bool isFoodAvailable;
    GameObject foodTable;
    Vector3 foodTableCoord;
    Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        foodTable = GameObject.FindGameObjectWithTag("FoodTable");
        foodTableCoord = foodTable.transform.position;
        isFoodAvailable = false;
        offSet = new Vector3(0, 0.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFoodAvailable)
        {
            Invoke("spawnFood", 10);
            isFoodAvailable = true;
        }
    }

    void spawnFood()
    {
        Instantiate(foodPrefab, foodTableCoord + offSet, Quaternion.identity);
    }
}
