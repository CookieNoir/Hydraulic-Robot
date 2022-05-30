using System.Collections.Generic;
using UnityEngine;

public class RobotState : MonoBehaviour
{
    [System.Serializable]
    public struct SingleRobotState{
        public string name;
        public float value;
    }

    public Transform statePoint;
    public List<SingleRobotState> armStatesValues;
    public List<SingleRobotState> legsStatesValues;
}