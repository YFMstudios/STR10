using UnityEngine;

[CreateAssetMenu(fileName = "ProgressData", menuName = "ScriptableObjects/ProgressData", order = 1)]
public class ProgressData : ScriptableObject
{
    public int createdSoldierAmount;  // Þu anki melee birim sayýsý
    public int createdArcherAmount;   // Þu anki ranged birim sayýsý

    public bool IsCastleUpgraded = false;
}
