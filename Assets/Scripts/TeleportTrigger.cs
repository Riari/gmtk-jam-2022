using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Destination;

    private bool _isEnabled = true;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Receive(GameObject obj)
    {
        _isEnabled = false;
        obj.transform.position = transform.position;
        Camera.main.GetComponent<MainCamera>().FocusOn(gameObject);

        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isEnabled) return;

        Destination.GetComponent<TeleportTrigger>().Receive(other.gameObject);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _isEnabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
