using System.Collections.Generic;
using UnityEngine;

public class CombatMediator : MonoBehaviour
{
    [field: SerializeField]
    public GameObject HUD;

    private HUD _hud;
    private EventTitle _eventTitles;

    readonly private List<Participant> _participants = new List<Participant>();
    private int _currentParticipantIndex = 0;
    private Participant _currentParticipant => _participants[_currentParticipantIndex];

    private void Start()
    {
        _hud = HUD.GetComponent<HUD>();
        _eventTitles = HUD.GetComponent<EventTitle>();
    }

    public void OnCombatInitiated(GameObject pc, GameObject npc)
    {
        _participants.Add(new Participant() { Type = ParticipantType.PC, Name = pc.name, Stats = pc.GetComponent<Stats>() });
        _participants.Add(new Participant() { Type = ParticipantType.NPC, Name = npc.name, Stats = npc.GetComponent<Stats>() });
        _currentParticipantIndex = 0;

        // Freeze participants
        pc.SendMessage("Freeze");
        _hud.EnableCombatActions();
        npc.SendMessage("Freeze");

        _eventTitles.Queue(EventTitle.Type.Negative, "Attack Mode");
        _hud.AddCombatLogEntry($"Attacking {npc.name}");
        _hud.AddCombatLogEntry("Your turn. Use Melee Attack or Magic Missile.");
    }

    public void OnPlayerMeleeAttack()
    {
        if (_currentParticipant.Type != ParticipantType.PC) return;

        // TODO: Don't reference participants like this
        var enemy = _participants[1];
        MeleeAttack(_currentParticipant, enemy);

        EndTurn();
    }

    public void OnPlayerMagicAttack()
    {
        if (_currentParticipant.Type != ParticipantType.PC) return;

        // TODO: Don't reference participants like this
        var enemy = _participants[1];
        MagicAttack(_currentParticipant, enemy);

        EndTurn();
    }

    private void MeleeAttack(Participant attacker, Participant defender)
    {
        var attack = Dice.Roll(1, Dice.D20);
        _hud.AddCombatLogEntry($" > AC check: Rolled {attack} against {defender.Stats.AC}");

        if (attack >= defender.Stats.AC)
        {
            _hud.AddCombatLogEntry(" > Hit. Rolling for damage...");
            var damageRoll = attacker.Stats.MeleeDamage;
            var damage = Dice.Roll(damageRoll.Item1, damageRoll.Item2);
            _hud.AddCombatLogEntry($" > Struck {defender.Name} for {damage} HP.");
        }
        else
        {
            _hud.AddCombatLogEntry(" > Missed");
        }
    }

    private void MagicAttack(Participant attacker, Participant defender)
    {
        _hud.AddCombatLogEntry(" > Rolling for damage...");
        var damageRoll = attacker.Stats.MagicDamage;
        var damage = Dice.Roll(damageRoll.Item1, damageRoll.Item2);
        _hud.AddCombatLogEntry($" > Struck {defender.Name} for {damage} HP.");
    }

    private void EndTurn()
    {
        _hud.AddCombatLogEntry("Turn ended.");
        int next = _currentParticipantIndex + 1;
        if (next >= _participants.Count) next = 0;

        _currentParticipantIndex = next;

        _hud.AddCombatLogEntry($"{_currentParticipant.Name}'s turn");
    }

    private void EndCombat()
    {

    }

    public void FixedUpdate()
    {
        if (_participants.Count == 0) return;

        var participant = _participants[_currentParticipantIndex];

        switch (participant.Type)
        {
            case ParticipantType.PC:
                // noop, wait for player to act
                break;
            case ParticipantType.NPC:
                // TODO: Don't reference participants like this
                var player = _participants[1];
                MeleeAttack(participant, player);
                EndTurn();
                break;
        }
    }

    enum ParticipantType
    {
        PC,
        NPC,
    }

    class Participant
    {
        public ParticipantType Type;
        public string Name;
        public Stats Stats;
    }
}
