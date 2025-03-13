using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class EnemyMinionSpawner : MonoBehaviourPunCallbacks
{
    public float meleeMinionMoveSpeed;      
    public float rangedMinionMoveSpeed;     

    // Sabit prefab adları (Inspector'da görünmeyecek)
    private const string ENEMY_MELEE_MINION_PREFAB = "Minions/EnemyMeleeMinion";
    private const string ENEMY_RANGED_MINION_PREFAB = "Minions/EnemyRangedMinion";

    public Transform[] spawnPoints;         
    public float spawnInterval = 20.0f;     
    public int minionsPerWave = 10;         // Her dalgada spawn edilecek minyon sayısı (örneğin 10: 5 melee + 5 ranged)
    public float delayBetweenMinions;       

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        int wavesToSpawn = 2; // Sadece 2 dalga spawn edilecek
        for (int wave = 0; wave < wavesToSpawn; wave++)
        {
            for (int i = 0; i < minionsPerWave; i++)
            {
                bool isMelee = (i < minionsPerWave / 2); // İlk yarısı melee, ikinci yarısı ranged
                float speed = isMelee ? meleeMinionMoveSpeed : rangedMinionMoveSpeed;
                SpawnMinionForAll(isMelee, speed);
                yield return new WaitForSeconds(delayBetweenMinions);
            }
            float waveDelay = spawnInterval - (delayBetweenMinions * minionsPerWave);
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private void SpawnMinionForAll(bool isMelee, float moveSpeed)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        string prefabName = isMelee ? ENEMY_MELEE_MINION_PREFAB : ENEMY_RANGED_MINION_PREFAB;

        if (!PhotonNetwork.IsMasterClient) return;

        GameObject minion = PhotonNetwork.Instantiate(
            prefabName,
            spawnPoint.position,
            spawnPoint.rotation
        );

        var agent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
    }
}
