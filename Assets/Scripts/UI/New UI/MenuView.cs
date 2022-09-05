using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private GameObject _menuGameObject;
    [SerializeField] private Button _firstSelectedButton;
    protected Button firstSelectedButton;
    private Button _selectedButton;

    protected virtual void Awake()
    {
        if (_firstSelectedButton) firstSelectedButton = _firstSelectedButton;
    }

    public void Enable()
    {
        OnMenuViewEnabled();
        _menuGameObject.SetActive(true);
        if (firstSelectedButton)
        {
            SelectButton(firstSelectedButton);
        }
    }

    protected virtual void OnMenuViewEnabled()
    {

    }

    public void Disable()
    {
        _menuGameObject.SetActive(false);
        OnMenuViewDisabled();
    }

    protected virtual void OnMenuViewDisabled()
    {
        
    }

    public void SelectNext()
    {
        _selectNextButton(1);
    }

    public void SelectPrev()
    {
        _selectNextButton(_selectedButton.transform.parent.childCount - 1);
    }

    protected virtual void OnButtonSelected(Button button)
    {
    
    }

    public void SelectButtonByIndex(int index)
    {
        if (_selectedButton)
        {
            Transform parent = _selectedButton.transform.parent;
            index = (index % parent.childCount + parent.childCount) % parent.childCount;
            Button button = parent.GetChild(index).GetComponent<Button>();
            SelectButton(button);
        }
    }

    protected void SelectButton(Button button)
    {
        _selectedButton = button;
        _selectedButton.Select();
        OnButtonSelected(_selectedButton);
    }

    private void _selectNextButton(int offset)
    {
        if (_selectedButton)
        {
            int siblingIndex = _selectedButton.transform.GetSiblingIndex();
            Transform parent = _selectedButton.transform.parent;
            siblingIndex = (siblingIndex + offset) % parent.childCount;
            Button button = parent.GetChild(siblingIndex).GetComponent<Button>();
            while (button && !button.interactable)
            {
                siblingIndex = (siblingIndex + offset) % parent.childCount;
                button = parent.GetChild(siblingIndex).GetComponent<Button>();
            }
            SelectButton(button);
        }
    }
}
