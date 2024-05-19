using UnityEngine;
using UnityEngine.Events;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private float smashThreshold;
    [SerializeField]
    private bool smashIfHitFloor;
    bool isBroken = false;
    public UnityEvent onBroken;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        Debug.Log(collision.collider.tag);
        if (collision.relativeVelocity.magnitude > smashThreshold || smashIfHitFloor && collision.collider.CompareTag("Floor"))
        {
            isBroken = true;
            onBroken.Invoke();
        }
    }
}
