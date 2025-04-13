using UnityEngine;

public class WarController : MonoBehaviour
{
    public static WarController Instance;

    public GetPlayerData getPlayerData;
    public string Attacker;
    public string Defender;
    public int playerkalanokçu;
public int playerkalansavasçı;
public int enemykalanokçu;
public int enemykalansavasçı;

    public bool kaleyikildimi;
    public bool playerOlduMu;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private bool gameEnded = false;

void Update()
{
    if (gameEnded) return;

    // Durum 1: Kale yıkıldıysa -> saldıran kazandı
    if (kaleyikildimi)
    {
        Debug.Log($"🏰 Kale yıkıldı! Saldıran KAZANDI ✅ -> {Attacker}\nSavunan KAYBETTİ ❌ -> {Defender}");
        gameEnded = true;
        return;
    }

    // Durum 2: Player öldüyse ve ally minyon kalmadıysa
    if (playerOlduMu && playerkalansavasçı == 0 && playerkalanokçu == 0)
    {
        Debug.Log($"☠️ Oyuncu öldü ve minyon kalmadı. Savunan KAZANDI ✅ -> {Defender}\nSaldıran KAYBETTİ ❌ -> {Attacker}");
        gameEnded = true;
        return;
    }
}

}
