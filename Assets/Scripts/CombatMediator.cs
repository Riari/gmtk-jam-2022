using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMediator : MonoBehaviour
{
    [field: SerializeField]
    public Events.CombatEndedEvent OnCombatEnded;

    [field: SerializeField]
    public GameObject HUD;

    [field: SerializeField]
    public AudioClip PlayerMeleeAttackSound;

    [field: SerializeField]
    public AudioClip EnemyMeleeAttackSound;

    private HUD _hud;
    private EventTitle _eventTitles;

    readonly private List<Participant> _participants = new List<Participant>();
    private int _currentParticipantIndex = 0;
    private Participant _currentParticipant => _participants[_currentParticipantIndex];

    private AudioSource _audio;

    private void Start()
    {
        _hud = HUD.GetComponent<HUD>();
        _eventTitles = HUD.GetComponent<EventTitle>();
        _audio = GetComponent<AudioSource>();
    }

    public void OnCombatInitiated(GameObject pc, GameObject npc)
    {
        _participants.Add(new Participant(ParticipantType.PC, pc));
        _participants.Add(new Participant(ParticipantType.NPC, npc));
        _currentParticipantIndex = 0;

        // Freeze participants
        pc.SendMessage("Freeze");
        npc.SendMessage("Freeze");

        _hud.EnableCombatActions();

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

        _audio.Play();

        if (attack >= defender.Stats.AC)
        {
            _hud.AddCombatLogEntry(" > Hit. Rolling for damage...");
            var damageRoll = attacker.Stats.MeleeDamage;
            var damage = Dice.Roll(damageRoll.Item1, damageRoll.Item2);
            defender.Obj.SendMessage("Damage", damage);
            _hud.AddCombatLogEntry($" > Struck {defender.Name} for {damage} HP.");

            if (defender.Stats.Health == 0)
            {
                EndCombat(attacker, defender);
                return;
            }
        }
        else
        {
            _hud.AddCombatLogEntry(" > Missed");
        }

        EndTurn();
    }

    private void MagicAttack(Participant attacker, Participant defender)
    {
        _hud.AddCombatLogEntry(" > Rolling for damage...");
        var damageRoll = attacker.Stats.MagicDamage;
        var damage = Dice.Roll(damageRoll.Item1, damageRoll.Item2);
        _hud.AddCombatLogEntry($" > Struck {defender.Name} for {damage} HP.");

        if (defender.Stats.Health == 0)
        {
            EndCombat(attacker, defender);
            return;
        }

        EndTurn();
    }

    private void EndTurn()
    {
        if (_currentParticipant.Obj.tag == "Player")
        {
            _audio.clip = EnemyMeleeAttackSound;
        }
        else
        {
            _hud.EnableCombatActions();
            _audio.clip = PlayerMeleeAttackSound;
        }

        _hud.AddCombatLogEntry("Turn ended.");
        int next = _currentParticipantIndex + 1;
        if (next >= _participants.Count) next = 0;

        _currentParticipantIndex = next;

        _hud.AddCombatLogEntry($"{_currentParticipant.Name}'s turn");
    }

    private void EndCombat(Participant winner, Participant loser)
    {
        if (winner.Obj.tag == "Player")
        {
            _eventTitles.Queue(EventTitle.Type.Positive, $"{loser.Name} defeated");
        }
        else
        {
            _hud.DeathScreen.SetActive(true);
        }

        _participants.Clear();
        _currentParticipantIndex = 0;
        winner.Obj.SendMessage("Unfreeze");
        loser.Obj.SendMessage("Kill");
        OnCombatEnded.Invoke(winner.Obj, loser.Obj);
    }

    public void FixedUpdate()
    {
        if (_participants.Count == 0) return;

        var participant = _participants[_currentParticipantIndex];

        switch (participant.Type)
        {
            case ParticipantType.PC:
                // noop - wait for player action
                break;
            case ParticipantType.NPC:
                // TODO: Don't reference participants like this
                _hud.DisableCombatActions();
                var player = _participants[0];
                StartCoroutine(DelayMeleeAttack(participant, player));
                break;
        }
    }

    IEnumerator DelayMeleeAttack(Participant attacker, Participant defender)
    {
        yield return new WaitForSeconds(0.5f);
        MeleeAttack(attacker, defender);
    }

    enum ParticipantType
    {
        PC,
        NPC,
    }

    class Participant
    {
        public ParticipantType Type;
        public GameObject Obj;
        public string Name;
        public Stats Stats;

        public Participant(ParticipantType type, GameObject obj)
        {
            Type = type;
            Obj = obj;
            Name = obj.name;
            Stats = Obj.GetComponent<Stats>();
        }
    }
}
