using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core;
using UnityEngine;
using View;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Views")]
    public PauseMenuView PauseMenuView;
    public RestartMenuView RestartMenuView;
    public SelectLevelMenuView SelectLevelMenuMenuView;
    public MainMenuManager MainMenuManager;
    private Stack<IMenuView> _thisStackView = new Stack<IMenuView>();
    
    [Header("Buttons")]
    public KeyCode PauseButton;
    public KeyCode BackButton;

    
    private void Awake()
    {
        PauseMenuView.PauseMenuManager = this;
        SelectLevelMenuMenuView.PauseMenuManager = this;
        RestartMenuView.PauseMenuManager = this;
    }

    //пауза
    public void Pause()
    {
        if (MainMenuManager.IsActive())
        {
            //если открыто главное меню, паузу не открываем
            return;
        }
        Time.timeScale = 0;
        AddStackView(PauseMenuView);
        PauseMenuView.Open();
    }

    //возобновить время
    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void MainMenuOpen()
    {
        Back();
        MainMenuManager.Open();
    }

    //закрыть текущее меню
    public void Back()
    {
        _thisStackView.Peek().Back();
    }

    //закрыть все меню
    public void AllBack()
    {
        while (_thisStackView.Count > 0)
        {
            Back();
        }
    }

    //текущее открытое меню
    public IView GetView()
    {
        return _thisStackView.Peek();
    }

    //добавить меню в стек
    public void AddStackView(IMenuView view)
    {
        _thisStackView.Push(view);
    }

    //удалить меню из стека
    public void RemoveStackView()
    {
        _thisStackView?.Pop();
    }
    public void Update()
    {
        if (Input.GetKeyDown(PauseButton) && _thisStackView.Count == 0) 
        {
            Pause();
        }

        if (Input.GetKeyDown(BackButton) &&  _thisStackView.Count != 0)
        {
            Back();
        }
    }
}
