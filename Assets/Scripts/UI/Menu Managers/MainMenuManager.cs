using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public PauseMenuManager PauseMenuManager;
    public GameObject MainMenu;
    public AllTasks AllTasks;
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
        firstSelectedButton.Select();
        Time.timeScale = 0;
        _isActive = true;
        MainMenu.SetActive(true);
    }

    public void Close()
    {
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
        _createList.SetTasks(PauseMenuManager, AllTasks.Tasks, TaskMode.Education);
        //PauseMenuManager.Pause();
        Close();
    }

    public void NewEducation()
    {
        AllTasks.ResetTask();
        EducationHandler.instance.DropTask();
        _createList.SetTasks(PauseMenuManager, AllTasks.Tasks, TaskMode.Education);
        //PauseMenuManager.Pause();
        Close();
    }

    public void ResearchesMode()
    {
        Task[] task = { AllTasks.ResearchTask };
        _createList.SetTasks(PauseMenuManager, task, TaskMode.Education);
        Close();
    }

    public void TrainingMode()
    {
        _createList.SetTasks(PauseMenuManager, AllTasks.RandomTasks, TaskMode.Training);
        PauseMenuManager.Pause();
        Close();
    }
}