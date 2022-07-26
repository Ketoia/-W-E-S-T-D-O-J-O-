using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public GameObject Enemy;

    public int TimeFrame = 0;
    public float TimeFrameRate = 1;
    private float dTimeFrame = 0;

    private System.Guid UpdateGuid;

    void Awake()
    {
        if (instance == null) instance = this;
        UpdateGuid = EventsTransport.GenerateSeededGuid(2);
    }

    void Update()
    {
        if(dTimeFrame > TimeFrameRate)
        {
            TimeFrame++;
            dTimeFrame = 0;
            EventManager.TriggerEvent(UpdateGuid, TimeFrame);
        }
        else
        {
            dTimeFrame += Time.deltaTime;
        }
        SpawnEnemies();
    }

    //Enemies
    public int KilledEnemies = 0;
    float Killdtime = 0;
    void SpawnEnemies()
    {
        float pizza = Killdtime / ((KilledEnemies * 0.2f) + 1);
        for (int i = 0; i < 1; i++)
        {

        }
    }

}
