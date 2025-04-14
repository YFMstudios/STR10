using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GetPlayerData getPlayerData;

    public GameObject trapOneObject;
    public GameObject trapTwoObject;
    public GameObject trapThreeObject;

    private  TrapController trapController;

    void Start()
    {

        trapOneObject.SetActive(false);
        trapTwoObject.SetActive(false);
        trapThreeObject.SetActive(false);
        
        if (getPlayerData.TrapOneIsBuilded && trapOneObject != null)
        {
            trapOneObject.SetActive(true);
            Debug.Log("Trap1 aktif edildi.");
            Debug.Log("Trap Hasarı:" + trapController.damageAmount);
            Debug.Log("Trap Etki Alanı:" + trapController.activationDistance);
            
        }

        if (getPlayerData.TrapTwoIsBuilded && trapTwoObject != null)
        {
            trapTwoObject.SetActive(true);
            Debug.Log("Trap2 aktif edildi.");
            trapController.damageAmount=30;
            trapController.activationDistance=2.5f;
            Debug.Log("Trap Hasarı:" + trapController.damageAmount);
            Debug.Log("Trap Etki Alanı:" + trapController.activationDistance);
            
        }

        if (getPlayerData.TrapThreeIsBuilded && trapThreeObject != null)
        {
            trapThreeObject.SetActive(true);
            Debug.Log("Trap3 aktif edildi.");
            trapController.damageAmount=50;
            trapController.activationDistance=3f;
            Debug.Log("Trap Hasarı:" + trapController.damageAmount);
            Debug.Log("Trap Etki Alanı:" + trapController.activationDistance);

        }
    }

    public void SetTrapController(TrapController trap_Controller){
            trapController = trap_Controller;
    }
    
}