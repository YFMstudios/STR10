using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GetPlayerData getPlayerData;

    public GameObject trapOneObject;
    public GameObject trapTwoObject;
    public GameObject trapThreeObject;

    void Start()
    {

        trapOneObject.SetActive(false);
        trapTwoObject.SetActive(false);
        trapThreeObject.SetActive(false);
        
        if (getPlayerData.TrapOneIsBuilded && trapOneObject != null)
        {
            trapOneObject.SetActive(true);
            Debug.Log("Trap1 aktif edildi.");
        }

        if (getPlayerData.TrapTwoIsBuilded && trapTwoObject != null)
        {
            trapTwoObject.SetActive(true);
            Debug.Log("Trap2 aktif edildi.");
        }

        if (getPlayerData.TrapThreeIsBuilded && trapThreeObject != null)
        {
            trapThreeObject.SetActive(true);
            Debug.Log("Trap3 aktif edildi.");
        }
    }
}