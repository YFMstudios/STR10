using UnityEngine;
using TMPro;
using System.Collections;

public class LogController : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI tmpText;

    private Coroutine activeCoroutine;

    public void ShowLog(string message)
    {
        // Eğer aktif bir coroutine varsa, durdur
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        // Paneli göster ve mesajı ayarla
        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (tmpText != null)
        {
            tmpText.text = message;
        }

        // 5 saniye sonra paneli gizlemek için coroutine başlat
        activeCoroutine = StartCoroutine(HideLogAfterDelay(5f));
    }

    private IEnumerator HideLogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (panel != null)
        {
            panel.SetActive(false);
        }

        activeCoroutine = null;
    }
}