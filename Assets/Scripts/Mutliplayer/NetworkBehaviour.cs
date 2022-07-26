using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NetworkObject))]
public class NetworkBehaviour : MonoBehaviour
{
    public Guid ComponentID;

    protected bool IsMaster = false;
    public List<EventsTransport> transports = new List<EventsTransport>();

    protected NetworkObject m_NetworkObject;

    protected virtual void Start()
    {
        m_NetworkObject = GetComponent<NetworkObject>();
        if (m_NetworkObject == null) return;

        IsMaster = m_NetworkObject.IsMaster;

        if(IsMaster && m_NetworkObject.IsInSync)
        {
            Recalculate();
            m_NetworkObject.AddMyComponent(this);
        }
    }

    //protected virtual void Update()
    //{
    //    if (IsMaster)
    //    {
    //        transports[0].Value = transform.position.ToString();
    //    }
    //}

    protected virtual void OnDestroy()
    {
        //EventManager.StopListening(transports[0].GetKey(), OnEventTrigger);
        m_NetworkObject.DeleteMyComponent(this);
    }

    public virtual void Recalculate()
    {
        if (IsMaster) 
            ComponentID = Guid.NewGuid();

        //transports.Add(new EventsTransport("123"));
        //transports.Add(new EventsTransport(123));
        //transports.Add(new EventsTransport(124.0f));
        //transports.Add(new EventsTransport(false));

        //EventManager.StartListening(transports[0].GetKey(), OnEventTrigger);
    }

    //protected void OnEventTrigger(object Value)
    //{
    //    string pos = (string)Value;
    //    Debug.Log(pos);
    //}
}
