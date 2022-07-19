using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public MapGeneratorV4 mapGenerator;
    public bool isRandom;
    public int seed;
    public int roomsAmount;
    void Start()
    {
        mapGenerator = gameObject.GetComponent<MapGeneratorV4>();
        
        //mapGenerator
        if (isRandom)
        {
            seed = Random.Range(0, 999999);
        }
        mapGenerator.GenerateWorld(seed,roomsAmount);
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
