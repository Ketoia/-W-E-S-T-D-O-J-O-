using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    private class MyEvent<T> : UnityEvent<T> { }

    private static Dictionary<string, MyEvent<object>> eventDictionaryWithParameters = new Dictionary<string, MyEvent<object>>();
    private static Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

    public static void StartListening(string eventName, UnityAction<object> listener)
    {
        if (eventDictionaryWithParameters.TryGetValue(eventName, out MyEvent<object> thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MyEvent<object>();
            thisEvent.AddListener(listener);
            eventDictionaryWithParameters.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(string eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventDictionaryWithParameters.TryGetValue(eventName, out MyEvent<object> thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, object obj)
    {
        if (eventDictionaryWithParameters.TryGetValue(eventName, out MyEvent<object> thisEvent))
        {
            thisEvent.Invoke(obj);
        }
    }
    public static void TriggerEvent(string eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}