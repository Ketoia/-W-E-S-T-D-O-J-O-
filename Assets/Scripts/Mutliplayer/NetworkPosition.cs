using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPosition : NetworkBehaviour
{    
    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (IsMaster)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
            transports[0].Value = (MyData.Vector3)transform.position;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.StopListening(transports[0].GetKey(), OnPositionEvent);
    }

    public override void Recalculate()
    {
        base.Recalculate();
        if (IsMaster) 
            transports.Add(new EventsTransport(new MyData.Vector3(0, 0, 0)));

        EventManager.StartListening(transports[0].GetKey(), OnPositionEvent);
    }

    private void OnPositionEvent(object Value)
    {
        MyData.Vector3 position = (MyData.Vector3)Value;

        transform.position = (Vector3)position;
    }
}
