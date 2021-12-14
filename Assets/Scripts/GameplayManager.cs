using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public GameObject player;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        
    }    
}
