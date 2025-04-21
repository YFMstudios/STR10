using UnityEngine;

public class MenuTimerResetter : MonoBehaviour
{
    [Header("Global Time Data")]
    public TimeController timeController;

    private void Awake()
    {
        if (timeController == null)
        {
            Debug.LogError("MenuTimerResetter: TimeController asset atanmamış!", this);
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Menü sahnesine girildiğinde timer'ı sıfırla
        timeController.ResetTimer();
        Debug.Log("MenuTimerResetter: Zamanlayıcı sıfırlandı.");
    }
}
