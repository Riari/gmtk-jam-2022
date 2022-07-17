using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [System.Serializable]
    public class CombatStartedEvent : UnityEvent<GameObject, GameObject>
    {
    }

    [System.Serializable]
    public class CombatEndedEvent : UnityEvent<GameObject, GameObject>
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

    [System.Serializable]
    public class CharacterStatsInitialisedEvent : UnityEvent<Stats>
    {
    }

    [System.Serializable]
    public class CharacterHealthChangedEvent : UnityEvent<int>
    {
    }
};