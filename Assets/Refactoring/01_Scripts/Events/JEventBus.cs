using System;
using System.Collections.Generic;
using UnityEngine;

public class JEventBus
{
    #region VARIABLES
    private static Dictionary<Type, Delegate> _eventDict = new Dictionary<Type, Delegate>();
    #endregion





    #region FUNCTIONS
    public static void Subscribe<T>(Action<T> callback)
    {
        if(_eventDict.TryGetValue(typeof(T), out var del) == true)
        {
            _eventDict[typeof(T)] = Delegate.Combine(del, callback);
        }
        else
        {
            _eventDict[typeof(T)] = callback;
        }
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        if(_eventDict.ContainsKey(typeof(T))== true)
        {
            _eventDict[typeof(T)] = Delegate.Remove(_eventDict[typeof(T)], handler);

            if (_eventDict[typeof(T)] == null)
            {
                _eventDict.Remove(typeof(T));
            }
        }
    }

    public static void SendEvent<T>(T eventData)
    {
        if(_eventDict.TryGetValue(typeof(T), out var del))
        {
            ((Action<T>)del)?.Invoke(eventData);
        }
    }
    #endregion
}
