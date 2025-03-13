using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenProcessPanelWithAnim : MonoBehaviour
{
    public GameObject Panel;
    public static bool isPanelActive = false;
    public PanelManager panelManager;
    public void OpenPanel()
    {
        if(Panel != null)
        {
            Animator animator = Panel.GetComponent<Animator>();

            if (animator != null )
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
                isPanelActive = !isOpen;
            }
            else
            {
                Debug.Log("Animator = null");
            }
        }
        else
        {
            Debug.Log("Panel = null");
        }
    }

    public void changeStatus()
    {
        panelManager.ChangeStatus();
    }
}
