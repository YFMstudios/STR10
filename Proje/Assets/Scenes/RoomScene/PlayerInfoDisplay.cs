using TMPro;
using UnityEngine;

public class PlayerInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI KingdomText;

    public void UpdateInfo(string name, string kingdom)
    {
        NameText.text = name;
        KingdomText.text = kingdom;
    }
}
