using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

public class NetworkObject : MonoBehaviour
{
    public Guid ParentGuid;
    public List<NetworkBehaviour> NetworkBehaviours = new List<NetworkBehaviour>();

    public bool IsMaster = true;
    public bool IsInSync = false;

    private void Start()
    {
        if(IsMaster)
        {
            ParentGuid = Guid.NewGuid();
            DuplicateOnNetworkObject();
        }
        IsInSync = true;
    }

    private void OnDestroy()
    {
        if (IsMaster)
            DeleteNetworkObject();
    }

    private void RecalculateNetworkBehaviours()
    {
        NetworkBehaviours.Clear();
        NetworkBehaviours.AddRange(GetComponents<NetworkBehaviour>());

        foreach(NetworkBehaviour networkBehaviour in NetworkBehaviours)
        {
            networkBehaviour.Recalculate();
        }
    }

    public void AddMyComponent(NetworkBehaviour component)
    {
        NetworkBehaviours.Add(component);

        ComponentsDataList comp = new ComponentsDataList();
        comp.ParentID = ParentGuid;
        comp.Value = new List<ComponentsData>();

        ComponentsData componentsData = new ComponentsData();
        componentsData.TypeAsString = component.GetType().Name;
        componentsData.ComponentID = component.ComponentID;
        componentsData.MyTransportObjects = new MyTransportObject();
        componentsData.MyTransportObjects.Id = new List<Guid>();
        componentsData.MyTransportObjects.TypeAsString = new List<string>();
        
        foreach (var transportitem in component.transports)
        {
            componentsData.MyTransportObjects.Id.Add(transportitem.GetKey());
            componentsData.MyTransportObjects.TypeAsString.Add(transportitem.Value.GetType().Name);
        }
        
        comp.Value.Add(componentsData);

        EventsTransport.SingleEventTransport(comp, (int)SingleEventsSeed.AddComponent);
    }

    public void DeleteMyComponent(NetworkBehaviour component)
    {
        ComponentsDataList componentsDataList = new ComponentsDataList();
        componentsDataList.ParentID = ParentGuid;
        componentsDataList.Value = new List<ComponentsData>();

        ComponentsData componentsData = new ComponentsData();
        componentsData.MyTransportObjects = new MyTransportObject();
        componentsData.ComponentID = component.ComponentID;
        componentsData.MyTransportObjects.TypeAsString = new List<string>();
        componentsData.MyTransportObjects.Id = new List<Guid>();

        componentsDataList.Value.Add(componentsData);

        EventsTransport.SingleEventTransport(componentsDataList, (int)SingleEventsSeed.DeleteComponent);

        NetworkBehaviours.Remove(component);
        Destroy(component);
    }

    #region Main network functions

    public void AddNetworkedComponent(List<ComponentsData> componentsDatas)
    {
        foreach (var item in componentsDatas)
        {
            NetworkBehaviour netbehaviour = (NetworkBehaviour)gameObject.gameObject.AddComponent(Type.GetType(item.TypeAsString));
            netbehaviour.transports = new List<EventsTransport>();

            for (int i = 0; i < item.MyTransportObjects.TypeAsString.Count; i++)
            {
                netbehaviour.transports.Add(new EventsTransport(MyDataDefaultVariables.GetDefaultVariable(item.MyTransportObjects.TypeAsString[i]), item.MyTransportObjects.Id[i]));
            }
            netbehaviour.Recalculate();
            NetworkBehaviours.Add(netbehaviour);
        }
    }

    public static void AddNetworkedComponent(NetworkObject netobj, List<ComponentsData> componentsDatas)
    {
        foreach (var item in componentsDatas)
        {
            NetworkBehaviour netbehaviour =  (NetworkBehaviour)netobj.gameObject.AddComponent(Type.GetType(item.TypeAsString));
            netbehaviour.transports = new List<EventsTransport>();
            netbehaviour.ComponentID = item.ComponentID;

            for (int i = 0; i < item.MyTransportObjects.TypeAsString.Count; i++)
            {
                netbehaviour.transports.Add(new EventsTransport(MyDataDefaultVariables.GetDefaultVariable(item.MyTransportObjects.TypeAsString[i]), item.MyTransportObjects.Id[i]));
            }
            netbehaviour.Recalculate();
            netobj.NetworkBehaviours.Add(netbehaviour);
        }
    }

    public void DeleteNetworkedComponent(NetworkBehaviour networkBehaviour)
    {
        NetworkBehaviours.Remove(networkBehaviour);
        Destroy(networkBehaviour);
    }

    public static void DeleteNetworkedComponent(NetworkObject netobj, List<ComponentsData> componentsDatas)
    {
        List<NetworkBehaviour> coponents = netobj.NetworkBehaviours;
        List<NetworkBehaviour> todeleteComponents = new List<NetworkBehaviour>();
        
        foreach (var componentDatas in componentsDatas)
        {
            todeleteComponents.AddRange(coponents.FindAll(e => e.ComponentID == componentDatas.ComponentID));
        }

        foreach (var item in todeleteComponents)
        {
            netobj.NetworkBehaviours.Remove(item);
            Destroy(item);
        }
    }

    public void DuplicateOnNetworkObject()
    {
        ComponentsDataList comp = new ComponentsDataList();
        comp.ParentID = ParentGuid;
        comp.Value = new List<ComponentsData>();

        foreach (var item in NetworkBehaviours)
        {
            ComponentsData componentsData = new ComponentsData();
            componentsData.TypeAsString = item.GetType().Name;
            componentsData.ComponentID = item.ComponentID;
            componentsData.MyTransportObjects = new MyTransportObject();
            componentsData.MyTransportObjects.Id = new List<Guid>();
            componentsData.MyTransportObjects.TypeAsString = new List<string>();

            foreach (var transportitem in item.transports)
            {
                componentsData.MyTransportObjects.Id.Add(transportitem.GetKey());
                componentsData.MyTransportObjects.TypeAsString.Add(transportitem.Value.GetType().Name);
            }

            comp.Value.Add(componentsData);
        }

        EventsTransport.SingleEventTransport(comp, (int)SingleEventsSeed.DuplicateNetworkObject);
    }

    public static void DuplicateOnNetworkObject(NetworkObject netobj)
    {
        ComponentsDataList comp = new ComponentsDataList();
        comp.ParentID = netobj.ParentGuid;
        comp.Value = new List<ComponentsData>();

        foreach (var item in netobj.NetworkBehaviours)
        {
            ComponentsData componentsData = new ComponentsData();
            componentsData.TypeAsString = item.GetType().Name;
            componentsData.ComponentID = item.ComponentID;
            componentsData.MyTransportObjects = new MyTransportObject();
            componentsData.MyTransportObjects.Id = new List<Guid>();
            componentsData.MyTransportObjects.TypeAsString = new List<string>();
            
            foreach (var transportitem in item.transports)
            {
                componentsData.MyTransportObjects.Id.Add(transportitem.GetKey());
                componentsData.MyTransportObjects.TypeAsString.Add(transportitem.Value.GetType().Name);
            }

            comp.Value.Add(componentsData);
        }

        EventsTransport.SingleEventTransport(comp, (int)SingleEventsSeed.DuplicateNetworkObject);
    }

    public void DeleteNetworkObject()
    {
        ComponentsDataList componentsDataList = new ComponentsDataList();
        componentsDataList.ParentID = ParentGuid;
        componentsDataList.Value = new List<ComponentsData>();

        EventsTransport.SingleEventTransport(componentsDataList, (int)SingleEventsSeed.DeleteNetworkObject);

        Destroy(this);
    }
    //todo
    public static void DeleteNetworkObject(NetworkObject netobj)
    {

    }

    #endregion
}
