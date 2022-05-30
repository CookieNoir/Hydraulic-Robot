using UnityEngine;

public class Marker : MonoBehaviour
{
    public bool lookAtCamera = true;
    public Vector3 offset;

    [Header("Динамика стрелки-указателя")]
    public bool dynamicArrow = false;
    public Transform arrow;
    public float dynamicLength = 1f;
    public float speed = 1f;

    private float step = 0f;
    private float direction = 1f;

    public void UpdatePosition(GameObject target)
    {
        Collider collider = target.GetComponent<Collider>();
        if (collider)
        {
            transform.position = collider.bounds.center + new Vector3(0, collider.bounds.extents.y, 0) + offset;
        }
        else
        {
            transform.position = target.transform.position + offset;
        }
    }

    private void DynamicArrow()
    {
        step += direction * Time.deltaTime * speed;
        if (step > dynamicLength)
        {
            direction = -1f;
            step = 2f * dynamicLength - step;
        }
        else if (step < 0)
        {
            direction = 1f;
            step *= -1f;
        }
        arrow.localPosition = new Vector3(0, step, 0);
    }

    private void Update()
    {
        if (lookAtCamera) transform.LookAt(Camera.main.transform);
        if (dynamicArrow) DynamicArrow();
    }
}
