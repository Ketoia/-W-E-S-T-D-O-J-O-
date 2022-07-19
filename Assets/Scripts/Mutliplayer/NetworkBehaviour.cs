using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkBehaviour : MonoBehaviour
{
    public Guid ComponentGuid;

    public MyData.Vector3 Value = new MyData.Vector3(0,0,0);
    public EventsTransport transport;
    protected bool isMaster = true;
    public List<EventsTransport> transports = new List<EventsTransport>();
    
    public void Update()
    {
        if (isMaster && transport != null)
        {
            transform.position += Vector3.up * Time.deltaTime;
            transport.Value = (MyData.Vector3)transform.position;//Value;
        }
    }

    private void Start()
    {
        if(isMaster && transport == null)
        {
            transport = new EventsTransport(Value);

            EventsTransport.SoloEventTransport(transport.GetKey().ToString(), 3);
            //EventsTransport.SoloEventTransport("NetworkObject", 4);

        }
    }

    public void OnBehaviourAdd(/*string Key*/)
    {
        isMaster = false;
        //transport = new EventsTransport(Guid.Parse(Key));

        //EventManager.StartListening(Guid.Parse(Key), OnEventTrigger);
    }

    public void OnEventTrigger(object value)
    {
        UnityEngine.Vector3 pos = (MyData.Vector3)value;
        gameObject.transform.position = pos;
        //Debug.Log("oh");
    }
}
