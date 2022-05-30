using UnityEngine;

public class HitDetection : MonoBehaviour
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterAction();
    }

    protected void OnCollisionEnterAction()
    {
        NotificationSystem.instance?.Notify(NotificationSystem.NotificationTypes.alert, "Удар");
    }
}
