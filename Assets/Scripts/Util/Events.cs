using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [System.Serializable]
    public class CombatEvent : UnityEvent<GameObject, GameObject>
    {
    }
};