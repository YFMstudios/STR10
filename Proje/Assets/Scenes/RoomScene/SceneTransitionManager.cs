using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // Bu fonksiyon, "room" sahnesine geçişi takip eder ve kaydeder.
    public static void SetRoomSceneVisited(bool visited)
    {
        // Geçiş bilgisini PlayerPrefs'e kaydediyoruz
        PlayerPrefs.SetInt("RoomSceneVisited", visited ? 1 : 0);
        PlayerPrefs.Save();  // Değişiklikleri kaydediyoruz
    }

    void Start()
    {
        // Eğer şu anki sahne "room" ise, geçiş bilgisini güncelle
        if (SceneManager.GetActiveScene().name == "room")
        {
            SetRoomSceneVisited(true);
        }
    }
}
