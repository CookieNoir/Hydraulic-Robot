using UnityEngine;

public class GroundMovement : ArticulationBodyMovement
{
    [SerializeField] private float _torqueMultiplier;
    [SerializeField] private Transform _leftCaterpillarAxis;
    [SerializeField] private Transform _rightCaterpillarAxis;

    public override void Move(float axisLeft, float axisRight, float dynamicForceMultiplier)
    {
        float absLeft = Mathf.Abs(axisLeft);
        float absRight = Mathf.Abs(axisRight);
        float influence = 
            absLeft + absRight < 0.001f ?
            0.5f :
            absRight / (absLeft + absRight);
        articulationBody.AddForce(forceMultiplier * dynamicForceMultiplier * Vector3.Lerp(
                axisLeft * _leftCaterpillarAxis.forward,
                axisRight * _rightCaterpillarAxis.forward,
                influence), 
                ForceMode.Acceleration);
        articulationBody.AddTorque(_torqueMultiplier * dynamicForceMultiplier * Vector3.Lerp(
                axisLeft * _leftCaterpillarAxis.up,
                axisRight * _rightCaterpillarAxis.up,
                influence),
                ForceMode.Acceleration);
    }
}