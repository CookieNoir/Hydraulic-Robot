using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using View;

namespace Controllers
{
    public interface ISelectLevelMenuView : IMenuView
    {
        event Action BackEvent;
        //event Action<IController<IPauseMenuView>> OpenPauseEvent;
    }
    public class SelectLevelMenuController : IController<ISelectLevelMenuView>
    {
        private ISelectLevelMenuView _view;
        
        public void OnOpen(ISelectLevelMenuView menuView)
        {
            _view = menuView;
            _view.BackEvent += Back;
        }

        public void OnClose(ISelectLevelMenuView menuView)
        {
            _view.BackEvent -= Back;
            _view = null;
        }

        public void Back()
        {
            _view.PauseMenuManager.RemoveStackView();
            _view.PauseMenuManager.GetView().Open();
            _view.Close(this);
        }
        
    }
}
