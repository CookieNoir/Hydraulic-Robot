using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : ArticulationBodyCenterOfMass
{
    [Space(10, order = 0), Header("Движение", order = 1)]
    public bool alwaysAllowRotations;
    [Space(10)]
    public NonPlayerCollisionCounter caterpillarLeftCollisionCounter;
    public NonPlayerCollisionCounter caterpillarRightCollisionCounter;
    public List<NonPlayerCollisionCounter> legsCollisionCounters;
    [Space(10)]
    public List<ArticulationBodyXDriveModification> articulationBodyRotations;
    public List<ArticulationBodyXDriveModification> articulationBodyLegs;
    public float criticalAngle = -95f;
    private bool legsTouchSurface;
    public ArticulationBodyMovement movement;
    public InputActionReference leftInputAction, rightInputAction;
    private InputAction leftIA, rightIA;
    private float inputValueLeft, inputValueRight;
    public bool MovementAllowed { get; private set; } = false;
    public bool RotationsAllowed { get; private set; } = false;
    [Space(10, order = 0), Header("Навесное оборудование и перегруз", order = 1)]
    public AccessoryJoinPoint accessoryJoinPoint;
    [Space(10, order = 0), Header("Позиции камер", order = 1)]
    public Transform regularCamerasContainer;
    public Transform specialCamerasContainer;
    [Space(10, order = 0), Header("Состояния", order = 1)]
    public RobotState defaultState;
    public float timeForSettingState = 2f;
    public RobotTasks robotTasks;
    private List<Collider> childrenColliders;
    private bool stateIsSet = true;
    private IEnumerator setStateWithDelayCoroutine;

    protected override void Awake()
    {
        base.Awake();
        childrenColliders = new List<Collider>(transform.GetComponentsInChildren<Collider>());
        setStateWithDelayCoroutine = SetStateWithDelayCoroutine(defaultState);

        if (leftInputAction is not null)
        {
            leftIA = leftInputAction.action;
            if (!leftIA.enabled) leftIA.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));

        if (rightInputAction is not null)
        {
            rightIA = rightInputAction.action;
            if (!rightIA.enabled) rightIA.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));
    }

    void OnDisable()
    {
        if (leftIA.enabled) leftIA.Disable();
        if (rightIA.enabled) rightIA.Disable();
    }

    void OnEnable()
    {
        if (!leftIA.enabled) leftIA.Enable();
        if (!rightIA.enabled) rightIA.Enable();
    }


    private void Update()
    {
        if (stateIsSet)
        {
            legsTouchSurface = LegsTouchSurface();
            RotationsAllowed = alwaysAllowRotations || (legsTouchSurface && accessoryJoinPoint.IsFree);
            AllowRotations(articulationBodyRotations, RotationsAllowed);
            MovementAllowed = accessoryJoinPoint.IsFree
                && (caterpillarLeftCollisionCounter.HasCollisions
                || caterpillarRightCollisionCounter.HasCollisions)
                && !legsTouchSurface;

            inputValueLeft = leftIA.ReadValue<float>();
            inputValueRight = rightIA.ReadValue<float>();
        }
        else MovementAllowed = false;
    }

    private void FixedUpdate()
    {
        if (MovementAllowed)
        {
            movement.Move(
                inputValueLeft,
                inputValueRight,
                accessoryJoinPoint.SpeedModifier
            );
        }
    }

    private void AllowRotations(List<ArticulationBodyXDriveModification> xDriveModifications, bool value)
    {
        foreach (ArticulationBodyXDriveModification xDriveModification in xDriveModifications)
        {
            xDriveModification.AllowRotation(value);
        }
    }

    private bool LegsTouchSurface()
    {
        int legsCollisionsCount = 0;
        bool criticalAngleReached = false;
        for (int i = 0; i < legsCollisionCounters.Count; ++i)
        {
            if (legsCollisionCounters[i].HasCollisions) legsCollisionsCount++;
            if (articulationBodyLegs[i].GetXDriveTarget() < criticalAngle)
            {
                criticalAngleReached = true;
                break;
            }
        }
        return criticalAngleReached || legsCollisionsCount == legsCollisionCounters.Count;
    }

    private void ApplyState(RobotState newState)
    {
        stateIsSet = false;
        MovementAllowed = false;
        RotationsAllowed = false;
        AllowRotations(articulationBodyRotations, stateIsSet);
        AllowRotations(articulationBodyLegs, stateIsSet);
        for (int i = 0; i < articulationBodyRotations.Count; ++i)
        {
            articulationBodyRotations[i].MoveTo(newState.armStatesValues[i].value);
        }
        for (int i = 0; i < articulationBodyLegs.Count; ++i)
        {
            articulationBodyLegs[i].MoveTo(newState.legsStatesValues[i].value);
        }

        articulationBody.TeleportRoot(newState.statePoint.position, newState.statePoint.rotation);
        articulationBody.immovable = true;
        foreach (Collider collider in childrenColliders)
        {
            collider.enabled = false;
        }
    }

    private void UnlockMovement()
    {
        articulationBody.immovable = false;
        foreach (Collider collider in childrenColliders)
        {
            collider.enabled = true;
        }
        AllowRotations(articulationBodyLegs, true);
        stateIsSet = true;
    }

    private IEnumerator SetStateWithDelayCoroutine(RobotState state)
    {
        ApplyState(state);
        yield return new WaitForSeconds(timeForSettingState);
        UnlockMovement();
    }

    private void ApplyStateWithDelay(RobotState state)
    {
        StopCoroutine(setStateWithDelayCoroutine);
        setStateWithDelayCoroutine = SetStateWithDelayCoroutine(state);
        StartCoroutine(setStateWithDelayCoroutine);
    }

    public void SetState(RobotState specialState)
    {
        if (specialState) ApplyStateWithDelay(specialState);
        else ApplyStateWithDelay(defaultState);
    }

    public ArticulationBodyXDriveModification GetXDriveModificationByIndex(int index, bool getFromLegsList = false)
    {
        if (index < 0) return null;
        else
        {
            if (getFromLegsList)
            {
                return (index < articulationBodyLegs.Count) ? articulationBodyLegs[index] : null;
            }
            else
            {
                return (index < articulationBodyRotations.Count) ? articulationBodyRotations[index] : null;
            }
        }
    }

    public void SetEnabled(bool value)
    {
        enabled = value;
        accessoryJoinPoint.SetEnabled(value);
        for (int i = 0; i < articulationBodyRotations.Count; ++i)
        {
            articulationBodyRotations[i].enabled = value;
        }
        for (int i = 0; i < articulationBodyLegs.Count; ++i)
        {
            articulationBodyLegs[i].enabled = value;
        }
    }
}