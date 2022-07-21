using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_WeaponItem : MonoBehaviour
{
    public Image icon;
    public Image bg;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI label;
    public float ignoreAlfa = .5f;

    public void Select()
    {
        canvasGroup.alpha = 1f;
    }

    public void Unselect()
    {
        canvasGroup.alpha = ignoreAlfa;
    }
}
