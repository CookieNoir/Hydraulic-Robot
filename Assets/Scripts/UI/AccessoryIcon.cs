using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryIcon : MonoBehaviour
{
    [System.Serializable] public class IconProperties
    {
        public Sprite sprite;
        public Vector2 size;
        public Color color;
    }
    public static AccessoryIcon instance;

    [Header("Пары (Icon Name, Icon)")]
    public List<string> iconNames;
    public List<IconProperties> icons;
    [Space(10)]
    public Image backgroundImage;
    public Image foregroundImage;

    public Vector2 backgroundImageSizeAddition;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            TurnOffIcon();
        }
        else
        {
            Debug.Log("Instance of AccessoryIcon " + " already exists");
            Destroy(this);
        }
    }

    private void SetIcon(IconProperties icon)
    {
        if (icon != null)
        {
            foregroundImage.sprite = icon.sprite;
            backgroundImage.rectTransform.sizeDelta = icon.size + backgroundImageSizeAddition;
            foregroundImage.rectTransform.sizeDelta = icon.size;
            foregroundImage.color = icon.color;
        }
    }

    public void SetIconByName(string iconName)
    {
        int index = iconNames.IndexOf(iconName);
        if (index > -1)
        {
            backgroundImage.gameObject.SetActive(true);
            SetIcon(icons[index]);
        }
    }

    public void TurnOffIcon()
    {
        backgroundImage.gameObject.SetActive(false);
    }
}
