using UnityEngine;

public class TowerManager : MonoBehaviour
{
    // Tower ve MinionSpawner eşleştirme (isimlerle çalışıyoruz)
    public string[] towerNames = { "EnemyTower1", "EnemyTower2", "EnemyTower3" };
    public string[] spawnerNames = { "EnemyMinionSpawner1", "EnemyMinionSpawner2", "EnemyMinionSpawner3" };

    private int currentSpawnerIndex = 0;

    void Start()
    {
        // İlk spawner aktif, diğerleri kapalı
        for (int i = 0; i < spawnerNames.Length; i++)
        {
            GameObject spawner = GameObject.Find(spawnerNames[i]);
            if (spawner != null)
                spawner.SetActive(i == currentSpawnerIndex);
        }
    }

    public void OnTowerDestroyed(string destroyedTowerName)
    {
        int towerIndex = System.Array.IndexOf(towerNames, destroyedTowerName);
        if (towerIndex == -1)
        {
            Debug.LogWarning($"Tower adı bulunamadı: {destroyedTowerName}");
            return;
        }

        if (towerIndex < spawnerNames.Length)
        {
            GameObject spawnerToDisable = GameObject.Find(spawnerNames[towerIndex]);
            if (spawnerToDisable != null)
                spawnerToDisable.SetActive(false);
        }

        int nextSpawnerIndex = towerIndex + 1;
        if (nextSpawnerIndex < spawnerNames.Length)
        {
            GameObject spawnerToEnable = GameObject.Find(spawnerNames[nextSpawnerIndex]);
            if (spawnerToEnable != null)
            {
                spawnerToEnable.SetActive(true);
                currentSpawnerIndex = nextSpawnerIndex;
            }
        }
    }
}
