using UnityEngine;

public class RailsTask : Task
{
    [SerializeField] private RailsModel _startingRails;

    protected override void EnableTaskGameObjects() // Этот метод следует наследовать при помощи конструкции base
    {
        _SetOnRails();
    }

    private void _SetOnRails()
    {
        if (robot.movement.GetType() == typeof(RailsMovement))
        {
            RailsMovement movement = (RailsMovement)robot.movement;
            movement.SetOnRails(_startingRails);
        }
        else
        {
            Debug.Log("Этот робот не перемещается по рельсам. Успешное завершение задания может быть невозможно");
        }
    }
}