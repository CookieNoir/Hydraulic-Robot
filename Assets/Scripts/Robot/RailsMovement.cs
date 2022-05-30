using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsMovement : ArticulationBodyMovement
{
    [SerializeField] private Transform _offsetPoint;
    private RailsModel _rails;
    private Vector3 _projection = Vector3.zero;
    private bool _onRails;
    private const float _ALLOWED_COS = 0.99f;
    private const float _MIN_FORCE = 0.001f;

    public override void Move(Vector3 forceDirection, float forceMultiplier, Vector3 torqueDirection, float torqueMultiplier)
    {
        if (_onRails && Mathf.Abs(forceMultiplier) > _MIN_FORCE)
        {
            float shortestDistance;
            Vector3 currentPoint = _rails.GetProjectionOfPoint(_offsetPoint.position, out shortestDistance);
            Vector3 newPoint = _rails.GetProjectionOfPoint(_offsetPoint.position + currentLinearSpeed * forceMultiplier * forceDirection, out shortestDistance);
            Vector3 difference = newPoint - currentPoint;
            if (forceMultiplier < 0f) difference *= -1f;
            float yAngle = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg + 90f;
            articulationBody.TeleportRoot((articulationBody.transform.position - _offsetPoint.position) + newPoint,
                Quaternion.Euler(articulationBody.transform.rotation.x, yAngle, articulationBody.transform.rotation.z));
        }
    }

    public bool OnRails()
    {
        return _onRails;
    }

    private void _GetProjectionAndParameter()
    {
        if (_rails)
        {
            _onRails = _rails.IsPointOnRails(_offsetPoint.position, ref _projection);
            if (!_onRails) _rails = null;
        }
    }

    private void FixedUpdate()
    {
        _GetProjectionAndParameter();
    }

    public void SetOnRails(RailsModel newRails)
    {
        if (!_onRails)
        {
            float shortestDistance;
            Vector3 projection0 = new Vector3();
            float t = newRails.GetProjectionAndParameterValueOfPoint(_offsetPoint.position, out shortestDistance, ref projection0);
            if (t >= 0f && t < 1f)
            {
                Vector3 railsSecondPoint = newRails.GetPointByParameter(t + 0.01f);
                Vector3 projection1 = new Vector3();
                t = newRails.GetProjectionAndParameterValueOfPoint(_offsetPoint.position + 0.01f * _offsetPoint.forward, out shortestDistance, ref projection1);
                Vector3 vector1 = projection1 - projection0;
                vector1.y = 0f;
                Vector3 vector2 = railsSecondPoint - projection0;
                vector2.y = 0f;
                float cos = Mathf.Abs(Vector3.Dot(vector1, vector2) / (vector1.magnitude * vector2.magnitude));
                if (cos > _ALLOWED_COS)
                {
                    _rails = newRails;
                    _onRails = true;
                }
            }        
        }
    }
}