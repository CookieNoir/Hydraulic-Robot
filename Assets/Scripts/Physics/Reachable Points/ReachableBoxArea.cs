using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class ReachableBoxArea : ReachablePoint
{
    [SerializeField] private Vector3 globalScale;
    public int requiredObjectsAmount;
    public int resultValue = 1;

    public int IsEnoughObjects()
    {
        if (objectsAtPoint >= requiredObjectsAmount)
            return resultValue;
        else
            return 0;
    }

    public void SetGlobalScale(Vector3 newScale)
    {
        if (transform.parent)
        {
            transform.localScale = new Vector3(newScale.x / transform.parent.transform.localScale.x,
                                    newScale.y / transform.parent.transform.localScale.y,
                                    newScale.z / transform.parent.transform.localScale.z);
        }
        else
        {
            transform.localScale = globalScale;
        }
    }

    private void OnValidate()
    {
        SetGlobalScale(globalScale);
    }
}
