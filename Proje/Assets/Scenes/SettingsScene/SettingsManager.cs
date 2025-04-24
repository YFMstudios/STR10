using UnityEngine;
using TMPro; // TMP kullanımı
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown aaDropdown;
    public TMP_Dropdown vsyncDropdown;


    void Start()
    {
        SetupWindowMode(); // Yeni eklendi
        SetupQuality();
        SetupAntiAliasing();
        SetupVSync();
    }


    void SetupWindowMode()
    {
        List<string> modes = new List<string> { "Fullscreen", "Windowed", "Borderless" };
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(modes);

        FullScreenMode currentMode = Screen.fullScreenMode;
        int currentIndex = 0;

        switch (currentMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                currentIndex = 0;
                break;
            case FullScreenMode.Windowed:
                currentIndex = 1;
                break;
            case FullScreenMode.FullScreenWindow:
                currentIndex = 2;
                break;
        }

        windowModeDropdown.value = currentIndex;
        windowModeDropdown.RefreshShownValue();
    }



    void SetupQuality()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    void SetupAntiAliasing()
    {
        List<string> aaOptions = new List<string> { "Off", "2x", "4x", "8x" };
        aaDropdown.ClearOptions();
        aaDropdown.AddOptions(aaOptions);

        int currentAA = QualitySettings.antiAliasing;
        int index = aaOptions.IndexOf(currentAA + "x");
        aaDropdown.value = index == -1 ? 0 : index;
        aaDropdown.RefreshShownValue();
    }

    void SetupVSync()
    {
        vsyncDropdown.ClearOptions();
        List<string> options = new List<string> { "Off", "On" };
        vsyncDropdown.AddOptions(options);
        vsyncDropdown.value = QualitySettings.vSyncCount > 0 ? 1 : 0;
        vsyncDropdown.RefreshShownValue();
    }

    // --- Dropdown olaylarında çağrılacak fonksiyonlar ---



    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetAntiAliasing(int index)
    {
        int[] levels = { 0, 2, 4, 8 };
        QualitySettings.antiAliasing = levels[index];
    }

    public void SetWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }


    public void SetVSync(int index)
    {
        QualitySettings.vSyncCount = (index == 1) ? 1 : 0;
    }
}

