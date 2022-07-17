using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [System.Serializable]
    public class CombatEvent : UnityEvent<GameObject, GameObject>
    {
    }

    [System.Serializable]
    public class CharacterSelectedEvent : UnityEvent<GameObject>
    {
    }

    [System.Serializable]
    public class CharacterMovingEvent : UnityEvent<GameObject>
    {
    }
};