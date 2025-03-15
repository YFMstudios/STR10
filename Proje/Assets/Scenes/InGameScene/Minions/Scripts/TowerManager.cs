using UnityEngine;

public class TowerManager : MonoBehaviour
{
    // Tower ve MinionSpawner eşleştirme (GameObject isimleriyle çalışıyoruz)
    public string[] towerNames = { "EnemyTower1", "EnemyTower2", "EnemyTower3" };
    public string[] spawnerNames = { "EnemyMinionSpawner1", "EnemyMinionSpawner2", "EnemyMinionSpawner3" };

    // Şu anki aktif olan spawner indexi
    private int currentSpawnerIndex = 0;

    void Start()
    {
        // İlk olarak sadece ilk MinionSpawner açık
        for (int i = 0; i < spawnerNames.Length; i++)
        {
            GameObject spawner = GameObject.Find(spawnerNames[i]);
            if (spawner != null)
                spawner.SetActive(i == currentSpawnerIndex); // İlk spawner aktif, diğerleri kapalı
        }
    }

    public void OnTowerDestroyed(string destroyedTowerName)
    {
        // Destroy edilen tower'ın indeksini bul
        int towerIndex = System.Array.IndexOf(towerNames, destroyedTowerName);

        if (towerIndex == -1)
        {
            Debug.LogWarning($"Tower adı bulunamadı: {destroyedTowerName}");
            return;
        }

        // Mevcut MinionSpawner'ı kapat
        if (towerIndex < spawnerNames.Length)
        {
            GameObject spawnerToDisable = GameObject.Find(spawnerNames[towerIndex]);
            if (spawnerToDisable != null)
                spawnerToDisable.SetActive(false);
        }

        // Bir sonraki MinionSpawner'ı aç
        int nextSpawnerIndex = towerIndex + 1;
        if (nextSpawnerIndex < spawnerNames.Length)
        {
            GameObject spawnerToEnable = GameObject.Find(spawnerNames[nextSpawnerIndex]);
            if (spawnerToEnable != null)
            {
                spawnerToEnable.SetActive(true);
                currentSpawnerIndex = nextSpawnerIndex; // Yeni aktif spawner'ı güncelle
            }
        }
    }
}
