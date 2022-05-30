using UnityEngine;

public class NonPlayerCollisionCounter : MonoBehaviour
{
    public bool HasCollisions
    {
        get; private set;
    }
    private int prevCollisionCount = 0;
    private int collisionCount = 0;

    private void FixedUpdate()
    {
        prevCollisionCount = collisionCount;
        HasCollisions = collisionCount > 0;
        collisionCount = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(other.gameObject.CompareTag("Player") || 
            other.gameObject.CompareTag("Accessory") || 
            other.gameObject.CompareTag("Transparent"))) collisionCount++;
    }
}
