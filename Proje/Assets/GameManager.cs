using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TMP_Text deðiþkenleri
    public TMP_Text AlfgardTMP;
    public TMP_Text LexionTMP;
    public TMP_Text AkhadzriaTMP;
    public TMP_Text DhamuronTMP;
    public TMP_Text ZephrionTMP;
    public TMP_Text ArianopolTMP;

    private bool gameEnded = false;  // Oyun bitip bitmediðini kontrol eden deðiþken

    // Oyun bitirme fonksiyonu
    void Update()
    {
        if (AlfgardTMP.text == LexionTMP.text &&
            AlfgardTMP.text == AkhadzriaTMP.text &&
            AlfgardTMP.text == DhamuronTMP.text &&
            AlfgardTMP.text == ZephrionTMP.text &&
            AlfgardTMP.text == ArianopolTMP.text)
        {
            if (!gameEnded)
            {
                EndGame();
                gameEnded = true;  // Oyun bitti, tekrar çaðrýlmamasý için true yapýyoruz
            }
        }
    }

    // Oyun bitiþi
    public void EndGame()
    {
        Debug.Log("Game Over! All TMP values are equal.");
        // Panel eklediklerinde burada paneli gösterme iþlemi yapýlabilir
    }
}
