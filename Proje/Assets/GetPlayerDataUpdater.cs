using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class GetPlayerDataUpdater : MonoBehaviour
{
    private float timer = 0f;
    private float updateInterval = 2f;
    private GetPlayerData playerData;

    private void Start()
    {
        // Resources klasöründen GetPlayerData.asset dosyasını buluyoruz
        playerData = Resources.Load<GetPlayerData>("GetPlayerData");

        if (playerData == null)
        {
            Debug.LogError("[GetPlayerDataUpdater] GetPlayerData bulunamadı! Resources klasöründe GetPlayerData.asset olduğuna emin olun.");
        }
    }

    private void Update()
    {
        if (playerData == null) return;

        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            UpdatePhotonProperties();
            timer = 0f;
        }
    }

    private void UpdatePhotonProperties()
    {
        if (PhotonNetwork.LocalPlayer == null) return;

        Hashtable props = new Hashtable();
        props["SoldierCount"] = playerData.currentSoldierAmount;
        props["ArcherCount"] = playerData.currentArcherAmount;

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log($"[Güncelleme] Soldier: {playerData.currentSoldierAmount}, Archer: {playerData.currentArcherAmount}");
    }
}
