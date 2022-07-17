using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Foe : MonoBehaviour
{
    [field: SerializeField]
    public int AggroRadius = 3;

    [field: SerializeField]
    public GameObject Health;

    [field: SerializeField]
    public GameObject HealthBar;

    [field: SerializeField]
    public SpriteRenderer AggroSpriteRenderer;

    private BoxCollider2D _collider;

    void Start()
    {
        AggroSpriteRenderer.transform.localScale = new Vector3(AggroRadius, AggroRadius);
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(AggroRadius, AggroRadius);

        Health.SetActive(false);
        HealthBar.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        Health.SetActive(true);
        HealthBar.SetActive(true);
    }

    public void OnStatsInitialised(Stats stats)
    {
        var healthText = Health.GetComponentInChildren<TextMeshProUGUI>();
        var healthSlider = HealthBar.GetComponentInChildren<Slider>();

        healthText.text = stats.Health.ToString();
        healthSlider.maxValue = stats.MaxHealth;
        healthSlider.value = stats.Health;
    }

    public void OnHealthChanged(int value)
    {
        var healthText = Health.GetComponentInChildren<TextMeshProUGUI>();
        var healthSlider = HealthBar.GetComponentInChildren<Slider>();

        healthText.text = value.ToString();
        healthSlider.value = value;
    }

    public void Kill()
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 0.3f);
    }
}
