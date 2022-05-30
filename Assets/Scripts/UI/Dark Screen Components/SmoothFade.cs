using UnityEngine;
using UnityEngine.UI;

public class SmoothFade : MonoBehaviour
{
    public MaskableGraphic maskableGraphic;
    [Header("Dark Screen sets total appearing time")]
    public float appearingDelay;
    public float appearingTime;

    private float appearingDelayClipped;
    private float appearingTimeClippedInversed;
    private float appearingRemainderClipped;
    [Space(10, order = 0)]
    [Header("Dark Screen sets total fading time", order = 1)]
    public float fadingDelay;
    public float fadingTime;

    private float fadingDelayClipped;
    private float fadingTimeClippedInversed;
    private float fadingRemainderClipped;
    private Color color;

    private void Awake()
    {
        color = maskableGraphic.color;
    }

    public void SetTotalAppearingAndFadingTime(float totalAppearingTime, float totalFadingTime)
    {
        appearingDelayClipped = appearingDelay / totalAppearingTime;
        appearingTimeClippedInversed = totalAppearingTime / appearingTime;
        appearingRemainderClipped = (appearingDelay + appearingTime) / totalAppearingTime;

        fadingDelayClipped = 1f - fadingDelay / totalFadingTime;
        fadingTimeClippedInversed = totalFadingTime / fadingTime;
        fadingRemainderClipped = 1f - (fadingDelay + fadingTime) / totalFadingTime;
    }

    public void SetAlpha(float f)
    {
        color.a = SimpleFunctions.Smoothstep(f);
        maskableGraphic.color = color;
    }

    public void SetAlphaOnFading(float f)
    {
        if (f <= appearingDelayClipped) SetAlpha(0f);
        else if (f >= appearingRemainderClipped) SetAlpha(1f);
        else SetAlpha((f - appearingDelayClipped) * appearingTimeClippedInversed);
    }

    public void SetAlphaOnAppearing(float f)
    {
        if (f >= fadingDelayClipped) SetAlpha(1f);
        else if (f <= fadingRemainderClipped) SetAlpha(0f);
        else SetAlpha((f - fadingRemainderClipped) * fadingTimeClippedInversed);
    }
}
