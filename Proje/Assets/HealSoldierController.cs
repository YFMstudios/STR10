using UnityEngine;
using Photon.Pun;

public class HealSoldierController : MonoBehaviour
{
    public WarController      warController;
    public MinionSpawner      minionSpawner;       // attacker
    public EnemyMinionSpawner enemyMinionSpawner;  // defender

    public void HealCagirma()
    {
        PhotonView pv = warController.GetComponent<PhotonView>();

        pv.RPC("SendCasualtiesToHealController",
               RpcTarget.All,
               minionSpawner.SpawlananArcherCount,
               minionSpawner.SpawlananSoldierCount,
               enemyMinionSpawner.SpawlananArcherCount,
               enemyMinionSpawner.SpawlananSoldierCount);
    }
}
