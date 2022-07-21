using UnityEngine;
using UnityEngine.UI;

namespace Warfleets.UI{
public class UI_FactionSelectButton : MonoBehaviour
{
    public Button button;
    [SerializeField] Image bg;
    [SerializeField] Image icon;
    Player player;
    FactionData faction;

    public void SetInfo(Player p, FactionData data)
    {
        player = p;
        faction = data;
        bg.color = faction.color_1;
        icon.sprite = faction.icon;

        ColorBlock colorBlock = button.colors;
        colorBlock.highlightedColor = faction.color_1;
        colorBlock.pressedColor = faction.color_1;
        colorBlock.selectedColor = faction.color_1;
        button.colors = colorBlock;

        icon.color = faction.color_2;
    }
}
}