using System.Collections;
using UnityEngine;

public class SmoothQuestLine : MonoBehaviour
{
    private Material material;
    private float currentValue1, 
                  currentValue2;
    private IEnumerator setValuesCoroutine;

    public void SetMaterial(in Material material)
    {
        this.material = material;
        setValuesCoroutine = SetValuesCoroutine(0f, 0.001f);
    }

    public void SetStartValues(in float scale, in float value1, in float value2)
    {
        material.mainTextureScale = new Vector2(scale, 1f);
        currentValue1 = value1;
        currentValue2 = value2;
        SetCurrentValues();
    }

    public void SetValues(in float value1, in float value2)
    {
        StopCoroutine(setValuesCoroutine);
        setValuesCoroutine = SetValuesCoroutine(value1, value2);
        StartCoroutine(setValuesCoroutine);
    }

    private void SetCurrentValues()
    {
        material.SetFloat("_Value1", currentValue1);
        material.SetFloat("_Value2", currentValue2);
    }

    private IEnumerator SetValuesCoroutine(float value1, float value2)
    {
        float startValue1 = currentValue1, startValue2 = currentValue2, timer = 0;
        while (timer < 1f)
        {
            currentValue1 = Mathf.Lerp(startValue1, value1, timer);
            currentValue2 = Mathf.Lerp(startValue2, value2, timer);
            SetCurrentValues();
            yield return null;
            timer += Time.deltaTime * 4f;
        }
        currentValue1 = value1;
        currentValue2 = value2;
        SetCurrentValues();
    }
}
