using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Destination;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.position = Destination.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
