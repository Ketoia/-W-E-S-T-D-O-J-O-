using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NetworkObject))]
public class NetworkBehaviour : MonoBehaviour
{
    public Guid ComponentID;
    public List<EventsTransport> transports = new List<EventsTransport>();

    protected bool IsMaster = false;
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

    protected virtual void OnDestroy()
    {
        m_NetworkObject.DeleteMyComponent(this);
    }

    public virtual void Recalculate()
    {
        if (IsMaster) 
            ComponentID = Guid.NewGuid();
    }
}
