using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class EnemyMinionSpawner : MonoBehaviourPunCallbacks
{
    public float meleeMinionMoveSpeed;
    public float rangedMinionMoveSpeed;

    private const string ENEMY_MELEE_MINION_PREFAB = "Minions/EnemyMeleeMinion";
    private const string ENEMY_RANGED_MINION_PREFAB = "Minions/EnemyRangedMinion";

    public Transform[] spawnPoints;
    public float spawnInterval = 20.0f;
    public int minionsPerWave = 20; // Artık her dalgada 20 minyon olacak
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
        int wavesToSpawn = 6; // Toplamda 6 dalga oluşturuyoruz

        for (int wave = 0; wave < wavesToSpawn; wave++)
        {
            for (int i = 0; i < minionsPerWave; i++)
            {
                bool isMelee = (i < minionsPerWave / 2);
                float speed = isMelee ? meleeMinionMoveSpeed : rangedMinionMoveSpeed;
                SpawnMinionForAll(isMelee, speed);
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            if (wave < wavesToSpawn - 1)
            {
                float waveDelay = spawnInterval - (delayBetweenMinions * minionsPerWave);
                yield return new WaitForSeconds(waveDelay);
            }
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