using System.Collections;
using System.Collections.Generic;
using Controllers;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public interface IMenuView:IView
    {
        PauseMenuManager PauseMenuManager { get; set; }

        void Back();
    }
}
