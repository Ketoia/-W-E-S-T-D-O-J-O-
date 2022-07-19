using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public Guid ParentGuid;
    public List<NetworkBehaviour> NetworkBehaviours = new List<NetworkBehaviour>();

    public void AddMyComponent(string ComponentName)
    {
        Type type = Type.GetType(ComponentName);

        if(!type.IsSubclassOf(typeof(NetworkBehaviour)))
        {
            Debug.LogError("There is nothing like: " + ComponentName);
            return;
        }
        NetworkBehaviour netobj = (NetworkBehaviour)gameObject.AddComponent(type);
        //netobj.OnBehaviourAdd()
        NetworkBehaviours.Add(netobj);
    }
}
