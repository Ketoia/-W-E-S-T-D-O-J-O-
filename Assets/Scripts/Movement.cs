using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    DataValues<Vector3S> Position;
    new Rigidbody2D rigidbody;

    GameObject tet;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        Position = new DataValues<Vector3S>("Player_" + Steamworks.SteamClient.SteamId);
        tet = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(tet.GetComponent<BoxCollider>());
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * 10;
        Position.Value = gameObject.transform.position;
        tet.transform.position = Position.Value;
    }
}
