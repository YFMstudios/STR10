using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrapPanelController : MonoBehaviour
{
    // Trap 1
    public TMP_Text trapOneBuildLevelText;
    public TMP_Text trapOneEtkiAlaniText;
    public TMP_Text trapOneSaldiriGucuText;

    // Trap 2
    public TMP_Text trapTwoBuildLevelText;
    public TMP_Text trapTwoEtkiAlaniText;
    public TMP_Text trapTwoSaldiriGucuText;

    // Trap 3
    public TMP_Text trapThreeBuildLevelText;
    public TMP_Text trapThreeEtkiAlaniText;
    public TMP_Text trapThreeSaldiriGucuText;

    public Button cancelTrapButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;

                          public Button buildTrap1Button;
                          public Button buildTrap2Button;
                            public Button buildTrap3Button;
    private Text buttonText;
    // Trap 1 refresh function
    public void refreshTrapOne()
    {
         TextMeshProUGUI buttonText = buildTrap1Button.GetComponentInChildren<TextMeshProUGUI>();
        if (Trap.trapOneBuildLevel == 1)
        {
            trapOneBuildLevelText.text = "1";
            trapOneEtkiAlaniText.text = "5";
            trapOneSaldiriGucuText.text = "50";
                        buttonText.text = "Yükselt";
        }
        else if (Trap.trapOneBuildLevel == 2)
        {
            trapOneBuildLevelText.text = "2";
            trapOneEtkiAlaniText.text = "7.5";
            trapOneSaldiriGucuText.text = "75";
                        buttonText.text = "Yükselt";
        }
        else if (Trap.trapOneBuildLevel == 3)
        {
            trapOneBuildLevelText.text = "3";
            trapOneEtkiAlaniText.text = "10";
            trapOneSaldiriGucuText.text = "100";
        }
    }

    // Trap 2 refresh function
    public void refreshTrapTwo()
    {
         TextMeshProUGUI buttonText = buildTrap2Button.GetComponentInChildren<TextMeshProUGUI>();
        if (Trap.trapTwoBuildLevel == 1)
        {
            trapTwoBuildLevelText.text = "1";
            trapTwoEtkiAlaniText.text = "5";
            trapTwoSaldiriGucuText.text = "50";
                        buttonText.text = "Yükselt";
        }
        else if (Trap.trapTwoBuildLevel == 2)
        {
            trapTwoBuildLevelText.text = "2";
            trapTwoEtkiAlaniText.text = "7.5";
            trapTwoSaldiriGucuText.text = "75";
                        buttonText.text = "Yükselt";
        }
        else if (Trap.trapTwoBuildLevel == 3)
        {
            trapTwoBuildLevelText.text = "3";
            trapTwoEtkiAlaniText.text = "10";
            trapTwoSaldiriGucuText.text = "100";
        }
    }

    // Trap 3 refresh function
    public void refreshTrapThree()
    {
         TextMeshProUGUI buttonText = buildTrap3Button.GetComponentInChildren<TextMeshProUGUI>();
        if (Trap.trapThreeBuildLevel == 1)
        {
            trapThreeBuildLevelText.text = "1";
            trapThreeEtkiAlaniText.text = "5";
            trapThreeSaldiriGucuText.text = "50";
                        buttonText.text = "Yükselt";
        }
        else if (Trap.trapThreeBuildLevel == 2)
        {
            trapThreeBuildLevelText.text = "2";
            trapThreeEtkiAlaniText.text = "7.5";
            trapThreeSaldiriGucuText.text = "75";
                        buttonText.text = "Yükselt";
        }
        else if (Trap.trapThreeBuildLevel == 3)
        {
            trapThreeBuildLevelText.text = "3";
            trapThreeEtkiAlaniText.text = "10";
            trapThreeSaldiriGucuText.text = "100";
        }
    }

    // Destroy function, all traps are level 3
    public void DestroyComponents()
    {
        if (Trap.trapOneBuildLevel == 3 && Trap.trapTwoBuildLevel == 3 && Trap.trapThreeBuildLevel == 3)
        {
            // All traps are at level 3, perform destruction
            Debug.Log("T�m tuzaklar 3. seviyeye ula�t�. Bile�enler yok ediliyor.");
        }
    }

    // Cancel trap build
    public void cancelTrapBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        panelManager.DestroyPanel("TrapBuildingProcessPanel");
        cancelTrapButton.gameObject.SetActive(false);
    }
}
