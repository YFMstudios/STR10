// ResearchActions.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchActions", menuName = "ScriptableObjects/ResearchActions")]
public class ResearchActions : ScriptableObject
{
    [Header("Minion Prefabs")]
    public GameObject EnemyMeleeMinion;
    public GameObject EnemyRangedMinion;
    public GameObject MeleeMinion;
    public GameObject RangedMinion;

    public GameObject Kale;

    public GameObject Kule1;

    public GameObject Kule2;

    public GameObject Enemy;
    
    public GameObject Player;

       public GameObject Trap1;

    public GameObject Trap2;

    public GameObject Trap3;




    /// <summary>
    /// Tüm minion referanslarını bir araya topla ve damage değerlerini oku.
    /// </summary>
    public void MinionHasarArttirma()
    {
        // Inspector'da atadığın 4 prefab'ı bu diziye koy:
        GameObject[] minionPrefabs = new GameObject[]
        {
            EnemyMeleeMinion,
            EnemyRangedMinion,
            MeleeMinion,
            RangedMinion,
        };

        foreach (var prefab in minionPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<ObjectiveStats>();
            if (stats != null)
            {
                Debug.Log($"{prefab.name} damage = {stats.damage}");
                stats.damage=10;
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde ObjectiveStats bulunamadı!");
            }
        }
    }

      public void KaleKuleCanArttirma()
    {
       
        GameObject[] BuildPrefabs = new GameObject[]
        {
           Kale,
           Kule1,
           Kule2
        };

        foreach (var prefab in BuildPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<ObjectiveStats>();
            if (stats != null)
            {
                Debug.Log($"{prefab.name} can = {stats.health}");
                stats.health+=500;
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde ObjectiveStats bulunamadı!");
            }
        }
    }

    public void MinionHareketHiziArttirma()
    {
        // Inspector'da atadığın 4 prefab'ı bu diziye koy:
        GameObject[] minionPrefabs = new GameObject[]
        {
            MeleeMinion,
            RangedMinion
        };

        foreach (var prefab in minionPrefabs)
        {
            if (prefab == null) continue;

            var minionAI = prefab.GetComponent<MinionAI>();
            if (minionAI != null)
            {
                Debug.Log($"{prefab.name} hız = {minionAI.rotationSpeed}");
                minionAI.rotationSpeed=7.5f;
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde minionAı bulunamadı!");
            }
        }
         // Inspector'da atadığın 4 prefab'ı bu diziye koy:
        GameObject[] minionDefencePrefabs = new GameObject[]
        {
            EnemyMeleeMinion,
            EnemyRangedMinion
        };

        foreach (var prefab in minionDefencePrefabs)
        {
            if (prefab == null) continue;

            var minionAIDefence = prefab.GetComponent<MinionAIDefence>();
            if (minionAIDefence != null)
            {
                Debug.Log($"{prefab.name} hız = {minionAIDefence.rotationSpeed}");
                minionAIDefence.rotationSpeed=7.5f;
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde minionAıdefence bulunamadı!");
            }
        }

    }

    public void MinionDamageSifirlama(){

             GameObject[] minionPrefabs = new GameObject[]
        {
            MeleeMinion,
            RangedMinion,
            EnemyMeleeMinion,
            EnemyRangedMinion
        };

        foreach (var prefab in minionPrefabs)
        {
            if (prefab == null) continue;

            var objectiveStats = prefab.GetComponent<ObjectiveStats>();
            if (objectiveStats != null)
            {
                objectiveStats.damage=5f;
                Debug.Log($"{prefab.name} damage = {objectiveStats.damage}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde minionAıdefence bulunamadı!");
            }
        }
    }

     public void MinionHareketHiziSifirlama()
    {
        // Inspector'da atadığın 4 prefab'ı bu diziye koy:
        GameObject[] minionPrefabs = new GameObject[]
        {
            MeleeMinion,
            RangedMinion
        };

        foreach (var prefab in minionPrefabs)
        {
            if (prefab == null) continue;

            var minionAI = prefab.GetComponent<MinionAI>();
            if (minionAI != null)
            {
               
                minionAI.rotationSpeed=5f;
                 Debug.Log($"{prefab.name} hız = {minionAI.rotationSpeed}");
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde minionAı bulunamadı!");
            }
        }
         // Inspector'da atadığın 4 prefab'ı bu diziye koy:
        GameObject[] minionDefencePrefabs = new GameObject[]
        {
            EnemyMeleeMinion,
            EnemyRangedMinion
        };

        foreach (var prefab in minionDefencePrefabs)
        {
            if (prefab == null) continue;

            var minionAIDefence = prefab.GetComponent<MinionAIDefence>();
            if (minionAIDefence != null)
            {
                 minionAIDefence.rotationSpeed=5f;
                Debug.Log($"{prefab.name} hız = {minionAIDefence.rotationSpeed}");
               
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde minionAıdefence bulunamadı!");
            }
        }

    }

        public void KaleKuleCanSifirlama()
    {
       
        GameObject[] BuildPrefabs = new GameObject[]
        {
           Kale
        };

        foreach (var prefab in BuildPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<ObjectiveStats>();
            if (stats != null)
            {
                stats.health=1500;
                Debug.Log($"{prefab.name} can = {stats.health}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde ObjectiveStats bulunamadı!");
            }
        }
         GameObject[] TowerPrefabs = new GameObject[]
        {
           Kule1,
           Kule2
        };

        foreach (var prefab in TowerPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<ObjectiveStats>();
            if (stats != null)
            {
                stats.health=1000;
                Debug.Log($"{prefab.name} can = {stats.health}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde ObjectiveStats bulunamadı!");
            }
        }
    }

    public void KarakterCanVeHasarArttirma(){

             GameObject[] KarakterPrefabs = new GameObject[]
        {
           Player,
           Enemy
        };

        foreach (var prefab in KarakterPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<Stats>();
            if (stats != null)
            {
                stats.damage=30f;
                stats.health=250;
                Debug.Log($"{prefab.name} damage = {stats.damage}");
                Debug.Log($"{prefab.name} can = {stats.health}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde stats bulunamadı!");
            }
        }
    }

     public void KarakterCanVeHasarArttirma2(){

             GameObject[] KarakterPrefabs = new GameObject[]
        {
           Player,
           Enemy
        };
        foreach (var prefab in KarakterPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<Stats>();
            if (stats != null)
            {
                stats.damage=35f;
                stats.health=300;
                Debug.Log($"{prefab.name} damage = {stats.damage}");
                Debug.Log($"{prefab.name} can = {stats.health}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde stats bulunamadı!");
            }
        }
    }

     public void KarakterCanVeHasarSifirlama(){

             GameObject[] KarakterPrefabs = new GameObject[]
        {
           Player,
           Enemy
        };

        foreach (var prefab in KarakterPrefabs)
        {
            if (prefab == null) continue;

            var stats = prefab.GetComponent<Stats>();
            if (stats != null)
            {
                stats.damage=25f;
                stats.health=200;
                Debug.Log($"{prefab.name} damage = {stats.damage}");
                Debug.Log($"{prefab.name} can = {stats.health}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde stats bulunamadı!");
            }
        }
    }

    
     public void TrapHasarAttirma(){

             GameObject[] TrapPrefabs = new GameObject[]
        {
           Trap1,
           Trap2,
           Trap3
        };

        foreach (var prefab in TrapPrefabs)
        {
            if (prefab == null) continue;

            var trapController = prefab.GetComponent<TrapController>();
            if (trapController != null)
            {
                trapController.damageAmount=25;
                Debug.Log($"{prefab.name} damage = {trapController.damageAmount}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde trap bulunamadı!");
            }
        }
    }

      public void TrapHasarSifirlama(){

             GameObject[] TrapPrefabs = new GameObject[]
        {
           Trap1,
           Trap2,
           Trap3
        };

        foreach (var prefab in TrapPrefabs)
        {
            if (prefab == null) continue;

            var trapController = prefab.GetComponent<TrapController>();
            if (trapController != null)
            {
                trapController.damageAmount=15;
                Debug.Log($"{prefab.name} damage = {trapController.damageAmount}");
                
            }
            else
            {
                Debug.LogWarning($"{prefab.name} üzerinde trağ bulunamadı!");
            }
        }
    }
   
}
