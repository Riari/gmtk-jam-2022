using UnityEngine;
using UnityEngine.Events;

public class Foe : MonoBehaviour
{
    [field: SerializeField]
    public int AggroRadius = 3;

    private SpriteRenderer _aggroSpriteRenderer;
    private BoxCollider2D _collider;

    private UnityEvent _onCombatTriggered;

    void Start()
    {
        _aggroSpriteRenderer = GameObject.Find("AggroRadius").GetComponent<SpriteRenderer>();
        _aggroSpriteRenderer.transform.localScale = new Vector3(AggroRadius, AggroRadius);
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(AggroRadius, AggroRadius);

        if (_onCombatTriggered == null)
        {
            _onCombatTriggered = new UnityEvent();
        }
    }

    void Update()
    {
        
    }

    public void RegisterOnCombatTriggeredListener(UnityAction callback)
    {
        _onCombatTriggered.AddListener(callback);
    }
}
