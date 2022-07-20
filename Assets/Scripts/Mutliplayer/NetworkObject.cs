using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

public class NetworkObject : MonoBehaviour
{
    public Guid ParentGuid;
    public List<NetworkBehaviour> NetworkBehaviours = new List<NetworkBehaviour>();

    public bool check = false;

    public void Update()
    {
        if (check)
        {
            if (Input.GetKeyDown(KeyCode.K)) DuplicateOnNetworkObject(this);
            if (Input.GetKeyDown(KeyCode.J)) AddMyComponent("NetworkBehaviour");
        }
    }

    public void AddMyComponent(string ComponentName)
    {
        Type type = Type.GetType(ComponentName);

        if (typeof(NetworkBehaviour) != type) 
        {
            Debug.LogError("There is nothing like: " + ComponentName);
            return;
        }
        NetworkBehaviour netobj = (NetworkBehaviour)gameObject.AddComponent(type);
        netobj.OnBehaviourAdd();
        //netobj.OnBehaviourAdd()
        NetworkBehaviours.Add(netobj);
    }

    //public static void DeleteNetworkedComponent(NetworkObject netobj, ComponentsData componentsData)
    //{

    //}

    public static void AddNetworkedComponent(NetworkObject netobj, ComponentsData componentsData)
    {
        NetworkBehaviour netbehaviour = (NetworkBehaviour)netobj.gameObject.AddComponent(Type.GetType(componentsData.TypeAsString));
        netbehaviour.transports = new List<EventsTransport>();

        //foreach(var item in componentsData.MyTransportObjects)
        //{
        //    netbehaviour.transports.Add(new EventsTransport(Activator.CreateInstance(Type.GetType(item.TypeAsString)), item.Id));
        //}

        //for (int i = 0; i < componentsData.TraposrtObjectKeys.Count; i++)
        //{
        //    netbehaviour.transports.Add(new EventsTransport(Activator.CreateInstance(Type.GetType(componentsData.TraposrtObjectTypesAsString[i])), componentsData.TraposrtObjectKeys[i]));
        //}
    }

    public static void AddNetworkedComponent(NetworkObject netobj, List<ComponentsData> componentsDatas)
    {
        foreach (var item in componentsDatas)
        {
            NetworkBehaviour netbehaviour =  (NetworkBehaviour)netobj.gameObject.AddComponent(Type.GetType(item.TypeAsString));
            netbehaviour.transports = new List<EventsTransport>();

            for (int i = 0; i < item.MyTransportObjects.TypeAsString.Count; i++)
            {
                netbehaviour.transports.Add(new EventsTransport(MyDataDefaultVariables.GetDefaultVariable(item.MyTransportObjects.TypeAsString[i]), item.MyTransportObjects.Id[i]));
            }
        }
        
    }

    public static void DuplicateOnNetworkObject(NetworkObject netobj)
    {
        ComponentsDataList comp = new ComponentsDataList();
        comp.Key = netobj.ParentGuid;
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

        EventsTransport.SoloEventTransport(comp.Value, 3);
    }
}
