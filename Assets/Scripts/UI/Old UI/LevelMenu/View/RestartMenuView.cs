using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core;
using UnityEngine;
using UnityEngine.UI;
using View;

public class RestartMenuView:BaseView<IRestartMenuView>,IRestartMenuView
{
    public PauseMenuManager PauseMenuManager { get; set; }
    public EducationHandler educationHandler { get; set; }
    protected override IRestartMenuView View => this;

    public event Action ResetEvent;
    public event Action BackEvent;
    
    protected override IController<T> CreateController<T>()
    {
        return (IController<T>) new RestartMenuController();
    }

    public void ResetTask()
    {
        ResetEvent?.Invoke();
    }

    public void Back()
    {
        BackEvent?.Invoke();
    }
}
