using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CheckScene : MonoBehaviour
{
    // Krallık bilgisini burada tutuyoruz
    public static string selectedKingdom = "";

    // Bu fonksiyon Continue butonuna basıldığında çalışacak
    public void OnContinueButtonClicked()
    {
        // GetVariableFromHere scriptinden currentSpriteNum değerini al
        int currentSpriteNumber = GetVariableFromHere.currentSpriteNum;

        // Krallık ismini al
        selectedKingdom = GetKingdomName(currentSpriteNumber);

        // Zorluk bilgisi kontrolü
        string previousScreen = PlayerPrefs.GetString("PreviousScreen", "");

        if (previousScreen == "Simple" || previousScreen == "Mid" || previousScreen == "Hard")
        {
            // Eğer zorluk ekranından gelmişse, 6. ekrana yönlendir
            SceneManager.LoadScene(6); // 6. ekranı, zorluk ekranı sonrası için kullanıyoruz
        }
        else
        {
            // Zorluk seçme ekranından gelmediyse, odasına yönlendir
            SceneManager.LoadScene("room");
        }
    }

    // CurrentSpriteNumber'a göre krallığı al
    string GetKingdomName(int spriteNum)
    {
        switch (spriteNum)
        {
            case 2: return "Akhadzria";
            case 3: return "Alfgard";
            case 4: return "Arianopol";
            case 5: return "Dhamuron";
            case 6: return "Lexion";
            case 7: return "Zephyrion";
            default: return "Alfgard"; // Varsayılan olarak Alfgard
        }
    }
}
