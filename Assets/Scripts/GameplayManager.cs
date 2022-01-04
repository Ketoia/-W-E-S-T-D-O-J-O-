using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public int TimeFrame = 0;
    public float TimeFrameRate = 1;
    private float dTimeFrame = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        if(dTimeFrame > TimeFrameRate)
        {
            TimeFrame++;
            dTimeFrame = 0;
            EventManager.TriggerEvent("TimeFrameRateTick", TimeFrame);
        }
        else
        {
            dTimeFrame += Time.deltaTime;
        }
    }    
}
