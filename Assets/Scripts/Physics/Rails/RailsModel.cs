using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RailsModel : CubicBezierCurve
{
    private Bounds triggerBounds;

    private void Awake()
    {
        triggerBounds = GetComponent<Collider>().bounds;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform top = other.transform;
            RobotController controller = top.GetComponent<RobotController>();
            while (!controller)
            {
                top = top.parent;
                controller = top.GetComponent<RobotController>();
            }
            ArticulationBodyMovement movement = controller.movement;
            if (movement && movement.GetType() == typeof(RailsMovement))
            {
                RailsMovement railsMovement = (RailsMovement)movement;
                if (railsMovement && !railsMovement.OnRails())
                {
                    railsMovement.SetOnRails(this);
                }
            }
        }
    }

    public bool IsPointOnRails(Vector3 point, ref Vector3 projection)
    {
        float shortestDistance;
        float t = GetProjectionAndParameterValueOfPoint(point, out shortestDistance, ref projection);
        return triggerBounds.Contains(point) && (t >= 0f && t < 1f);
    }
}