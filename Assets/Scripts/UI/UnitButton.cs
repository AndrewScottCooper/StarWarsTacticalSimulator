using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public Image unitImage;
    public Button button;

    void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        if (unitImage == null)
        {
            unitImage = GetComponentInChildren<Image>();
        }
    }

    public void Setup(Sprite unitSprite, System.Action onClickAction)
    {
        if (unitImage != null)
        {
            unitImage.sprite = unitSprite;
        }
        button.onClick.AddListener(() => onClickAction.Invoke());
    }
}