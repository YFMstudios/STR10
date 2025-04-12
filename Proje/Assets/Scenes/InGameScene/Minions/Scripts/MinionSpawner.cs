using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviourPunCallbacks
{
    public float meleeMinionMoveSpeed;    // Yakın dövüş minion hareket hızı
    public float rangedMinionMoveSpeed;   // Uzak dövüş minion hareket hızı

    private const string MELEE_MINION_PREFAB = "Minions/MeleeMinion";
    private const string RANGED_MINION_PREFAB = "Minions/RangedMinion";

    public Transform[] spawnPoints;
    public float spawnInterval = 20.0f;   // Dalga arası bekleme süresi
    public float delayBetweenMinions;     // Her minyon arasında bekleme süresi

    private int meleeUnitsToSpawn = 0;
    private int rangedUnitsToSpawn = 0;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;

    private void Start()
    {
        // Minyon sayısını başlangıç değerinin 3 katına çıkarıyoruz
        meleeUnitsToSpawn = (int)getPlayerData.currentSoldierAmount * 3;
        rangedUnitsToSpawn = (int)getPlayerData.currentArcherAmount * 3;

        // Toplam minyon sayısını 60 ile sınırlıyoruz
        int total = meleeUnitsToSpawn + rangedUnitsToSpawn;
        if (total > 60)
        {
            float ratio = 60f / total;
            meleeUnitsToSpawn = Mathf.FloorToInt(meleeUnitsToSpawn * ratio);
            rangedUnitsToSpawn = Mathf.FloorToInt(rangedUnitsToSpawn * ratio);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        int totalUnitsToSpawn = meleeUnitsToSpawn + rangedUnitsToSpawn;

        // 6 dalga oluşturmak için toplam minyonları bölüyoruz
        int waves = 6;
        int meleePerWave = Mathf.CeilToInt((float)meleeUnitsToSpawn / waves);
        int rangedPerWave = Mathf.CeilToInt((float)rangedUnitsToSpawn / waves);

        for (int wave = 0; wave < waves; wave++)
        {
            int meleeToSpawnThisWave = Mathf.Min(meleePerWave, meleeUnitsToSpawn);
            int rangedToSpawnThisWave = Mathf.Min(rangedPerWave, rangedUnitsToSpawn);

            // Melee minyonları spawn et
            for (int i = 0; i < meleeToSpawnThisWave; i++)
            {
                SpawnMinionForAll(true, meleeMinionMoveSpeed);
                meleeUnitsToSpawn--;
                totalUnitsToSpawn--;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            // Ranged minyonları spawn et
            for (int i = 0; i < rangedToSpawnThisWave; i++)
            {
                SpawnMinionForAll(false, rangedMinionMoveSpeed);
                rangedUnitsToSpawn--;
                totalUnitsToSpawn--;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            // Dalga tamamlandıysa bir sonraki dalgaya kadar bekle
            if (wave < waves - 1)
            {
                float waitTime = spawnInterval - delayBetweenMinions * (meleeToSpawnThisWave + rangedToSpawnThisWave);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    private void SpawnMinionForAll(bool isMelee, float moveSpeed)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenPoint = spawnPoints[spawnIndex];

        string prefabName = isMelee ? MELEE_MINION_PREFAB : RANGED_MINION_PREFAB;

        if (!PhotonNetwork.IsMasterClient) return;

        GameObject minion = PhotonNetwork.Instantiate(
            prefabName,
            chosenPoint.position,
            chosenPoint.rotation
        );

        var agent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
    }
}