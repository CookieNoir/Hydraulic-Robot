using UnityEngine;

public class NotificationCaller : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private NotificationSystem.NotificationTypes _type;

    public void Notify()
    {
        NotificationSystem.instance.Notify(_type, _text);
    }
}