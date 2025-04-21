using UnityEngine;

// Bu script'i sahne geçişlerinde yok olmayacak bir GameObject'e ekleyin
public class BattleTimer : MonoBehaviour
{
    public TimeController timeController;
    private static BattleTimer instance;
    
    private void Awake()
    {
        // Singleton mantığı
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (timeController == null)
        {
            Debug.LogError("TimeController asset atanmamış!", this);
            enabled = false;
        }
    }
    
    void Update()
    {
        if (timeController != null && timeController.isTimerRunning)
        {
            // Time.unscaledDeltaTime ile sayacı timeScale'den bağımsız artır
            timeController.elapsedTime += Time.unscaledDeltaTime;
        }
    }
}
