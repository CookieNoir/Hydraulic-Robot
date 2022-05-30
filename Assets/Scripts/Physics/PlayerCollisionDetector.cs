using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    protected bool detected = false;
    protected Vector3 positionOnDetection;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detected = true;
            positionOnDetection = transform.position;
        }
    }

    public void DropValues()
    {
        detected = false;
    }

    public float GetOffsetSqrMagnitude()
    {
        return detected ? (transform.position - positionOnDetection).sqrMagnitude : 0f;
    }
}