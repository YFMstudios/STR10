// TimerController.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class TimerController : MonoBehaviour
{
    [Header("UI (Inspector'dan atayacaksın)")]
    [Tooltip("Zamanı yazdırmak için sahnedeki TextMeshProUGUI bileşenini buraya sürükle")]
    public TextMeshProUGUI timerText;

    [Header("Global Time Data")]
    [Tooltip("ScriptableObject Asset'in (TimeController.asset) referansını buraya at")]
    public TimeController timeController;

    private static TimerController instance;

     private void Awake()
    {
        // Singleton + kalıcı obje
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Sahne yüklendiğinde callback
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Hata kontrolleri
        if (timeController == null)
            Debug.LogError("[TimerController] TimeController asset atanmamış!", this);

        if (timerText == null)
            Debug.LogError("[TimerController] timerText atanmamış!", this);
    }
    
    private void OnDestroy()
    {
        // Leak olmasın diye unbind
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Yeni eklenen metot:
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Örnek: Tag kullanarak
    var go = GameObject.FindWithTag("TimerText");
    if (go != null)
    {
        timerText = go.GetComponent<TextMeshProUGUI>();
        UpdateTimerDisplay();
    }
    else
    {
        Debug.LogWarning($"[TimerController] Bu sahnede TimerText objesi bulunamadı: {scene.name}");
        // önceki timerText olduğu gibi kalacak
    }
}


    void Start()
    {
        timeController.isTimerRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        // Süre artışı
        if (timeController.isTimerRunning)
            timeController.elapsedTime += Time.deltaTime;

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = FormatTime(timeController.elapsedTime);
    }

    private string FormatTime(float t)
    {
        int h = Mathf.FloorToInt(t / 3600f);
        int m = Mathf.FloorToInt((t % 3600f) / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        return $"{h:00}:{m:00}:{s:00}";
    }


}
