using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPosition : NetworkBehaviour
{
    private EventsTransport NetworkPos;
    private EventsTransport NetworkVel;
    private NetworkSpirte networkSpirte;

    [SerializeField]
    private MyData.Vector3 velocity = Vector3.zero;

    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (IsMaster)
        {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime;
            transform.position += velocity;

            NetworkVel.Value = velocity;
            NetworkPos.Value = (MyData.Vector3)transform.position;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.StopListening(NetworkPos.GetKey(), OnPositionEvent);
        EventManager.StopListening(NetworkVel.GetKey(), OnVelocityEvent);
    }

    public override void Recalculate()
    {
        base.Recalculate();
        if (IsMaster)
        {
            NetworkPos = new EventsTransport(new MyData.Vector3(0, 0, 0));
            NetworkVel = new EventsTransport(new MyData.Vector3(0, 0, 0));

            transports.Add(NetworkPos);
            transports.Add(NetworkVel);
        }
        else
        {
            NetworkPos = transports[0];
            NetworkVel = transports[1];
        }

        networkSpirte = new NetworkSpirte(gameObject);
        //networkSpirte.Init(gameObject);

        EventManager.StartListening(NetworkPos.GetKey(), OnPositionEvent);
        EventManager.StartListening(NetworkVel.GetKey(), OnVelocityEvent);
    }

    private void OnPositionEvent(object Value)
    {
        MyData.Vector3 position = (MyData.Vector3)Value;

        transform.position = (Vector3)position;
    }

    private void OnVelocityEvent(object Value)
    {
        velocity = (MyData.Vector3)Value;

        networkSpirte.velocity = velocity;
    }
}
