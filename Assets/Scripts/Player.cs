using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3S Position;
    new Rigidbody2D rigidbody;
    EventsTransport PositionEvent;

    void Awake()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        
        PositionEvent = new EventsTransport("PlayerPos_" + Steamworks.SteamClient.SteamId);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * 10;
        if(Position != gameObject.transform.position)
        {
            Position = gameObject.transform.position;
            PositionEvent.Object = Position;
            if(SteamManager.instance.host)
                PositionEvent.SendEventToClients();
            else PositionEvent.SendEventToServer();
        }      
    }
}
