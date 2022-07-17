using UnityEngine;

public class CombatMediator : MonoBehaviour
{
    [field: SerializeField]
    public GameObject HUD;

    private HUD _hud;
    private EventTitle _eventTitles;

    private bool _initiated = false;

    private Participant _friend;
    private Participant _foe;

    private void Start()
    {
        _hud = HUD.GetComponent<HUD>();
        _eventTitles = HUD.GetComponent<EventTitle>();
    }

    public void OnCombatInitiated(GameObject friend, GameObject foe)
    {
        _initiated = true;
        _friend = new Participant() { Name = friend.name, Stats = friend.GetComponent<Stats>() };
        _foe = new Participant() { Name = foe.name, Stats = foe.GetComponent<Stats>() };

        // Freeze participants
        friend.GetComponent<Selectable>().Freeze();
        friend.GetComponent<Moveable>().Freeze();
        foe.GetComponent<Moveable>().Freeze();

        _eventTitles.Queue(EventTitle.Type.Negative, "Attack Mode");
        _hud.AddCombatLogEntry($"Attacking {_foe.Name}");
    }

    class Participant
    {
        public string Name;
        public Stats Stats;
    }
}
