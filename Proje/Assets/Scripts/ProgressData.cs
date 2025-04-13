using UnityEngine;

[CreateAssetMenu(fileName = "ProgressData", menuName = "ScriptableObjects/ProgressData", order = 1)]
public class ProgressData : ScriptableObject
{
    public int createdSoldierAmount;  // �u anki melee birim say�s�
    public int createdArcherAmount;   // �u anki ranged birim say�s�

    public bool IsCastleUpgraded = false;
}
