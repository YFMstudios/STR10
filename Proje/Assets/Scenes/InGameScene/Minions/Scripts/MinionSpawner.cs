using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinionSpawner : MonoBehaviour
{
    public float meleeMinionMoveSpeed;      // Yakın dövüş minion hareket hızı
    public float rangedMinionMoveSpeed;     // Uzak dövüş minion hareket hızı

    public GameObject meleeMinionPrefab;    // Yakın dövüş minion prefabı
    public GameObject rangedMinionPrefab;   // Uzak dövüş minion prefabı
    public Transform[] spawnPoints;         // Minionların doğacağı noktalar
    public float spawnInterval = 20.0f;     // Minion dalgaları arasındaki aralık
    public float delayBetweenMinions;       // Minionlar arası gecikme süresi

    private ObjectPool meleeMinionPool;     // Yakın dövüş minionları için obje havuzu
    private ObjectPool rangedMinionPool;    // Uzak dövüş minionları için obje havuzu

    private int meleeUnitsToSpawn = 0;      // Üretilecek melee birim miktarı
    private int rangedUnitsToSpawn = 0;     // Üretilecek ranged birim miktarı

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;  // ScriptableObject referansı

    private void Start()
    {
        // Obje havuzlarını başlat
        meleeMinionPool = new ObjectPool(meleeMinionPrefab, 10, transform);
        rangedMinionPool = new ObjectPool(rangedMinionPrefab, 10, transform);


        meleeUnitsToSpawn = (int)getPlayerData.currentSoldierAmount;
        rangedUnitsToSpawn = (int)getPlayerData.currentArcherAmount;


        StartCoroutine(SpawnMinions()); // Minionları doğurma işlemini başlat
    }

    private IEnumerator SpawnMinions()
    {
        // Toplam minyon sayısını hesapla
        int totalUnitsToSpawn = meleeUnitsToSpawn + rangedUnitsToSpawn;

        // Asker sayısı 10'dan fazla ise, gruplara bölerek spawn et
        while (totalUnitsToSpawn > 0)
        {
            int meleeToSpawnThisBatch = Mathf.Min(5, meleeUnitsToSpawn);  // Bu dalgada spawn edilecek melee birim sayısı (max 5)
            int rangedToSpawnThisBatch = Mathf.Min(5, rangedUnitsToSpawn);  // Bu dalgada spawn edilecek ranged birim sayısı (max 5)

            // Melee birimleri spawn et
            for (int i = 0; i < meleeToSpawnThisBatch; i++)
            {
                SpawnMinion(meleeMinionPool, meleeMinionMoveSpeed);
                meleeUnitsToSpawn--;
                totalUnitsToSpawn--;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            // Ranged birimleri spawn et
            for (int i = 0; i < rangedToSpawnThisBatch; i++)
            {
                SpawnMinion(rangedMinionPool, rangedMinionMoveSpeed);
                rangedUnitsToSpawn--;
                totalUnitsToSpawn--;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            // Dalga arası bekleme
            if (totalUnitsToSpawn > 0)  // Eğer daha fazla birim varsa, tekrar spawn etmek için bekle
            {
                yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * (meleeToSpawnThisBatch + rangedToSpawnThisBatch));
            }
        }
    }


    private void SpawnMinion(ObjectPool pool, float moveSpeed)
    {
        GameObject minion = pool.Get();                             // Obje havuzundan bir minion al
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];  // Doğum noktasını rastgele seç
        minion.transform.position = spawnPoint.position;           // Minionu doğum noktasına yerleştir
        minion.transform.rotation = spawnPoint.rotation;           // Minionun doğum noktasına göre rotasyonunu ayarla

        UnityEngine.AI.NavMeshAgent minionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        minionAgent.speed = moveSpeed;                              // Minionun hareket hızını ayarla
    }

    // Obje havuzu sınıfı
    private class ObjectPool
    {
        private Queue<GameObject> poolQueue;    // Obje havuzu kuyruğu
        private GameObject prefab;              // Havuzdaki obje prefabı
        private Transform parent;               // Havuzun parentı

        public ObjectPool(GameObject prefab, int initialSize, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            poolQueue = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab, parent);    // Prefabı havuza ekleyerek obje oluştur
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
                GameObject obj = Instantiate(prefab, parent);    // Prefabtan obje oluştur
                obj.SetActive(false);                             // Objeyi etkisiz hale getir
                poolQueue.Enqueue(obj);                           // Objeyi havuza ekle
            }
        }
    }
}
