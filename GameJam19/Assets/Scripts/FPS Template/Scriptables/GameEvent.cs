using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author Manuel Fischer
// use in asset/events folder

[CreateAssetMenu]
public class GameEvent : ScriptableObject {

    List<GameEventListener> listeners = new List<GameEventListener>();

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void UnRegisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnEventRaised();
    }
}
