using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviourPunCallbacks
{
    public float meleeMinionMoveSpeed;    // Yakın dövüş minion hareket hızı
    public float rangedMinionMoveSpeed;   // Uzak dövüş minion hareket hızı

    // Prefab adlarını kod içinde sabit tanımlıyoruz (Inspector'da görünmeyecek)
    private const string MELEE_MINION_PREFAB = "Minions/MeleeMinion";
    private const string RANGED_MINION_PREFAB = "Minions/RangedMinion";

    public Transform[] spawnPoints;       
    public float spawnInterval = 20.0f;   // Dalga arası bekleme süresi
    public float delayBetweenMinions;     // Her minyon arasında bekleme süresi

    private int meleeUnitsToSpawn = 0;    
    private int rangedUnitsToSpawn = 0;   

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;   // Asker sayısı bilgisi çekmek için

    private void Start()
    {
        meleeUnitsToSpawn = (int)getPlayerData.currentSoldierAmount;
        rangedUnitsToSpawn = (int)getPlayerData.currentArcherAmount;

        // Toplam spawn sayısını kontrol et ve 20'yi aşarsa orantı ile indirgeme yap
        int total = meleeUnitsToSpawn + rangedUnitsToSpawn;
        if (total > 20)
        {
            float ratio = 20f / total;
            meleeUnitsToSpawn = Mathf.FloorToInt(meleeUnitsToSpawn * ratio);
            rangedUnitsToSpawn = Mathf.FloorToInt(rangedUnitsToSpawn * ratio);
            total = meleeUnitsToSpawn + rangedUnitsToSpawn; // Artık toplam 20 veya biraz altında olabilir
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        int totalUnitsToSpawn = meleeUnitsToSpawn + rangedUnitsToSpawn;

        while (totalUnitsToSpawn > 0)
        {
            int meleeToSpawnThisBatch = Mathf.Min(5, meleeUnitsToSpawn);
            int rangedToSpawnThisBatch = Mathf.Min(5, rangedUnitsToSpawn);

            // Melee minyonlar
            for (int i = 0; i < meleeToSpawnThisBatch; i++)
            {
                SpawnMinionForAll(true, meleeMinionMoveSpeed);
                meleeUnitsToSpawn--;
                totalUnitsToSpawn--;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            // Ranged minyonlar
            for (int i = 0; i < rangedToSpawnThisBatch; i++)
            {
                SpawnMinionForAll(false, rangedMinionMoveSpeed);
                rangedUnitsToSpawn--;
                totalUnitsToSpawn--;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            if (totalUnitsToSpawn > 0)
            {
                float waitTime = spawnInterval - delayBetweenMinions * (meleeToSpawnThisBatch + rangedToSpawnThisBatch);
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
