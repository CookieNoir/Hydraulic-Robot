using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace View
{

    public class PauseMenuView : BaseView<IPauseMenuView>, IPauseMenuView
    {
        public PauseMenuManager PauseMenuManager { get; set; }

        protected override IPauseMenuView View => this;
        
        public event Action ResumeEvent;
        public event Action<IMenuView> ResetEvent;
        public event Action MainMenuEvent;
        public event Action<IMenuView> SelectLevelEvent;



        protected override IController<T> CreateController<T>()
        {
            return (IController<T>) new PauseMenuController();
        }

        public void Resume()
        {
            
            ResumeEvent?.Invoke();
        }

        public void Restart()
        {
            ResetEvent?.Invoke(PauseMenuManager.RestartMenuView);
        }

        public void MainMenu()
        {
            MainMenuEvent?.Invoke();
        }

        public void SelectLevels()
        {
            SelectLevelEvent?.Invoke(PauseMenuManager.SelectLevelMenuMenuView);
        }

        public  void Back()
        {
            Resume();
        }

    }
}
