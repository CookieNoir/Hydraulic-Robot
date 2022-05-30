using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkScreen : MonoBehaviour
{
    public static DarkScreen instance;

    public float totalAppearingTime = 2f;
    public float totalFadingTime = 2f;
    public GameObject screen;
    public List<SmoothFade> darkScreenElements;
    private const float timeStep = 0.016666f;
    private IEnumerator darkScreenCoroutine;
    private float _totalAppearingTime;
    private float _totalFadingTime;
    private float f = 1f;
    private float appearingTimeStep;
    private float fadingTimeStep;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            darkScreenCoroutine = DarkScreenCoroutine(1f, null, true);
            _totalAppearingTime = totalAppearingTime;
            _totalFadingTime = totalFadingTime;
            appearingTimeStep = 1f / _totalAppearingTime;
            fadingTimeStep = 1f / _totalFadingTime;
        }
        else
        {
            Debug.Log("Instance of DarkScreen " + " already exists");
            Destroy(this);
        }
    }

    private void Start()
    {
        foreach (SmoothFade element in darkScreenElements)
        {
            element.SetTotalAppearingAndFadingTime(_totalAppearingTime, _totalFadingTime);
        }
        StartCoroutine(darkScreenCoroutine);
    }

    public void ExecuteInDarkScreen(float waitingTime = 1f, Action action = null, bool realtime = false)
    {
        screen.gameObject.SetActive(true);
        StopCoroutine(darkScreenCoroutine);
        darkScreenCoroutine = DarkScreenCoroutine(1f, action, realtime);
        StartCoroutine(darkScreenCoroutine);
    }

    private IEnumerator DarkScreenCoroutine(float waitingTime = 1f, Action action = null, bool isRealtime = false)
    {
        while (f < 1f)
        {
            SetAlphaOnAppearingForElements(f);
            yield return new WaitForSecondsRealtime(timeStep);
            f += timeStep * appearingTimeStep;
        }
        f = 1f;
        SetAlphaForElements(f);
        action?.Invoke();
        if (isRealtime) yield return new WaitForSecondsRealtime(waitingTime);
        else yield return new WaitForSeconds(waitingTime);
        while (f > 0f)
        {
            SetAlphaOnFadingForElements(f);
            yield return new WaitForSecondsRealtime(timeStep);
            f -= timeStep * fadingTimeStep;
        }
        f = 0f;
        SetAlphaForElements(f);
        screen.gameObject.SetActive(false);
    }

    private void SetAlphaOnAppearingForElements(float f)
    {
        foreach (SmoothFade element in darkScreenElements)
        {
            element.SetAlphaOnAppearing(f);
        }
    }

    private void SetAlphaForElements(float f)
    {
        foreach (SmoothFade element in darkScreenElements)
        {
            element.SetAlpha(f);
        }
    }

    private void SetAlphaOnFadingForElements(float f)
    {
        foreach (SmoothFade element in darkScreenElements)
        {
            element.SetAlphaOnFading(f);
        }
    }
}