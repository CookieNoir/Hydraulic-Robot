using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    public PauseMenuManager PauseMenuManager;
    public GameObject MainMenu;
    public RobotTasks AllTasks;
    public Button firstSelectedButton;
    private CreateListLevel _createList;
    private bool _isActive;

    private void Awake()
    {
        if (PauseMenuManager == null)
            return;
        _createList = PauseMenuManager.SelectLevelMenuMenuView.GetComponent<CreateListLevel>();
    }

    private void OnEnable()
    {
        Open();
    }

    public void Open()
    {
        //InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        firstSelectedButton.Select();
        Time.timeScale = 0;
        _isActive = true;
        MainMenu.SetActive(true);        
    }

    public void Close()
    {
        //InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        Time.timeScale = 1;
        _isActive = false;
        MainMenu.SetActive(false);        
    }

    public bool IsActive()
    {
        return _isActive;
    }

    public void ResumeEducation()
    {
        _createList.SetTasks(PauseMenuManager, AllTasks.Tasks);
        //PauseMenuManager.Pause();
        Close();
    }

    public void NewEducation()
    {
        AllTasks.ResetAllTasks();
        EducationHandler.instance.DropTask();
        _createList.SetTasks(PauseMenuManager, AllTasks.Tasks);
        //PauseMenuManager.Pause();
        Close();
    }

    /*
    public void ResearchesMode()
    {
        Task[] task = { AllTasks.ResearchTask };
        _createList.SetTasks(PauseMenuManager, task);
        Close();
    }

    public void TrainingMode()
    {
        _createList.SetTasks(PauseMenuManager, AllTasks.RandomTasks);
        PauseMenuManager.Pause();
        Close();
    }
    */
}