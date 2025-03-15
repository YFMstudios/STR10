using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMinionSpawner : MonoBehaviour
{
    public float meleeMinionMoveSpeed;      // Yakýn dövüþ minion hareket hýzý
    public float rangedMinionMoveSpeed;     // Uzak dövüþ minion hareket hýzý

    public GameObject meleeMinionPrefab;    // Yakýn dövüþ minion prefabý
    public GameObject rangedMinionPrefab;   // Uzak dövüþ minion prefabý
    public Transform[] spawnPoints;         // Minionlarýn doðacaðý noktalar
    public float spawnInterval = 20.0f;     // Minion dalgalarý arasýndaki aralýk
    public int minionsPerWave = 10;         // Her dalga için minion sayýsý (5 melee, 5 ranged)
    public float delayBetweenMinions;       // Minionlar arasý gecikme süresi

    private ObjectPool meleeMinionPool;     // Yakýn dövüþ minionlarý için obje havuzu
    private ObjectPool rangedMinionPool;    // Uzak dövüþ minionlarý için obje havuzu

    private void Start()
    {
        // meleeMinionPool ve rangedMinionPool boyutlarýný 5 yaparak her dalga için yeterli sayýda minion saðlýyoruz
        meleeMinionPool = new ObjectPool(meleeMinionPrefab, 5, transform);
        rangedMinionPool = new ObjectPool(rangedMinionPrefab, 5, transform);

        StartCoroutine(SpawnMinions()); // Minionlarý doðurma iþlemini baþlat
    }

    private IEnumerator SpawnMinions()
    {
        while (true)
        {
            for (int i = 0; i < minionsPerWave; i++)
            {
                // Ýlk 5 minion melee olacak, sonraki 5 minion ranged olacak
                if (i < 5) // Ýlk 5 minion melee
                {
                    SpawnMinion(meleeMinionPool, meleeMinionMoveSpeed);
                }
                else // Sonraki 5 minion ranged
                {
                    SpawnMinion(rangedMinionPool, rangedMinionMoveSpeed);
                }

                yield return new WaitForSeconds(delayBetweenMinions);
            }

            yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * minionsPerWave);
        }
    }

    private void SpawnMinion(ObjectPool pool, float moveSpeed)
    {
        GameObject minion = pool.Get();                             // Obje havuzundan bir minion al
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];  // Doðum noktasýný rastgele seç
        minion.transform.position = spawnPoint.position;           // Minionu doðum noktasýna yerleþtir
        minion.transform.rotation = spawnPoint.rotation;           // Minionun doðum noktasýna göre rotasyonunu ayarla

        UnityEngine.AI.NavMeshAgent minionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        minionAgent.speed = moveSpeed;                              // Minionun hareket hýzýný ayarla
    }

    // Obje havuzu sýnýfý
    private class ObjectPool
    {
        private Queue<GameObject> poolQueue;    // Obje havuzu kuyruðu
        private GameObject prefab;              // Havuzdaki obje prefabý
        private Transform parent;               // Havuzun parentý

        public ObjectPool(GameObject prefab, int initialSize, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            poolQueue = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab, parent);    // Prefabý havuza ekleyerek obje oluþtur
                obj.SetActive(false);                             // Objeyi etkisiz hale getir
                poolQueue.Enqueue(obj);                           // Objeyi havuza ekle
            }
        }

        public GameObject Get()
        {
            if (poolQueue.Count == 0)
            {
                AddObjects(1);          // Havuzda obje kalmazsa yeni objeler ekle
            }

            GameObject obj = poolQueue.Dequeue();    // Kuyruktan bir obje al
            obj.SetActive(true);                    // Objeyi etkin hale getir
            return obj;                             // Objeyi döndür
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);       // Objeyi etkisiz hale getir
            poolQueue.Enqueue(obj);     // Objeyi havuza geri ekle
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab, parent);    // Prefabtan obje oluþtur
                obj.SetActive(false);                             // Objeyi etkisiz hale getir
                poolQueue.Enqueue(obj);                           // Objeyi havuza ekle
            }
        }
    }
}
