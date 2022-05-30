using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using View;

namespace Controllers
{
    public interface IPauseMenuView : IMenuView
    {
        event Action ResumeEvent;
        event Action<IMenuView> ResetEvent;
        event Action MainMenuEvent;
        event Action<IMenuView> SelectLevelEvent;
    }
    public class PauseMenuController : IController<IPauseMenuView>
    {
        private IPauseMenuView _view;

        public void OnOpen(IPauseMenuView view)
        {
            _view = view;
            _view.ResumeEvent += Resume;
            _view.MainMenuEvent += MainMenu;
            _view.ResetEvent += Restart;
            _view.SelectLevelEvent += SelectLevels;
        }

        public void OnClose(IPauseMenuView view)
        {
            _view.ResumeEvent -= Resume;
            _view.MainMenuEvent -= MainMenu;
            _view.ResetEvent -= Restart;
            _view.SelectLevelEvent -= SelectLevels;
            _view = null;
        }
        
        public void Resume()
        {
            _view.PauseMenuManager.Resume();
            _view.PauseMenuManager.RemoveStackView();
            _view.Close(this);
        }
        public void Restart(IMenuView restartView)
        {
            _view.PauseMenuManager.AddStackView(restartView);
            restartView.Open();
            _view.Close(this);
        }

        public void MainMenu()
        {
            //не работает
            /*_view.Back();
            _view.PauseMenuManager.MainMenuManager.Open();*/
        }

        public void SelectLevels(IMenuView selectLvlView)
        {
            _view.PauseMenuManager.AddStackView(selectLvlView);
            selectLvlView.Open();
            _view.Close(this);
        }
    }
}
