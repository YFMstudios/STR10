using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChatPanelWithAnim : MonoBehaviour
{
    public GameObject Panel;
    public static bool isPanelActive = false;
    public GameObject ScroolView;
    public GameObject Button;
    public GameObject InputField;

    void Start()
    {
        // Baþlangýçta ScroolView, Button ve InputField'i pasif yap
        if (ScroolView != null) ScroolView.SetActive(false);
        if (Button != null) Button.SetActive(false);
        if (InputField != null) InputField.SetActive(false);
    }

    public void OpenPanel()
    {
        Debug.Log("Týkaldýn");

        if (Panel != null)
        {
            Animator animator = Panel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
                isPanelActive = !isOpen;

                // Diðer bileþenlerin durumunu deðiþtir
                changeStatusOtherComponents();
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

    public void changeStatusOtherComponents()
    {
        // Panel aktif ise diðer bileþenleri aktif yap, deðilse pasif yap
        if (ScroolView != null) ScroolView.SetActive(isPanelActive);
        if (Button != null) Button.SetActive(isPanelActive);
        if (InputField != null) InputField.SetActive(isPanelActive);
    }
}
