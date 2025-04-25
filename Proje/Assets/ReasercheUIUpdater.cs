using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReasercheUIUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] seviyeImages = new Image[18];

    public ResearchController researchController ;
    public Image[] lockItems = new Image[18];

    public Button[] buttons = new Button[18];


    
  void Start()
{
    for (int i = 0; i < ResearchResetter.isResearched.Length; i++)
    {
        if (ResearchResetter.isResearched[i])
        {
            // Araştırılmışsa, seviye görselini aktif et
            if (seviyeImages[i] != null)
            {
                seviyeImages[i].enabled = true;
                 seviyeImages[i].color = new Color32(255, 255, 255, 255); // Tam beyaz ve tam opak
            }
            // Butonu yok et
            if (buttons[i] != null)
            {
                 Destroy(buttons[i].gameObject);
            }
            if (lockItems[i] != null)
            {
                Destroy(lockItems[i]);
            }
        }
    }

      for (int i = 0; i < ResearchResetter.isResearched.Length; i++)
        {
            // Eğer o araştırma yapılmamışsa, next iteration
            if (!ResearchResetter.isResearched[i])
                continue;

                switch (i)
                    {
                    case 0:
                        researchController.OpenTwoAndThreeLevels();
                        break;
                    case 1:
                        researchController.OpenFourLevel();
                        break;
                    case 2:
                        researchController.OpenFiveLevel();
                        break;
                    case 3:
                        researchController.controlBuildLevelTwoResearches();
                        break;
                    case 4:
                        researchController.controlBuildLevelTwoResearches();
                        break;
                    case 5:
                        researchController.control9And10Levels();
                        break;
                    case 6:
                        researchController.control9And10Levels();
                        break;
                    case 7:
                        researchController.control9And10Levels();
                        break;
                    case 8:
                        researchController.control11And12And13Levels();
                        break;
                    case 9:
                        researchController.control11And12And13Levels();
                        break;
                    case 10:
                        researchController.controlBuildLevelThreeResearches();
                        break;
                    case 11:
                        researchController.controlBuildLevelThreeResearches();
                        break;
                    case 12:
                        researchController.controlBuildLevelThreeResearches();
                        break;
                    case 13:
                        researchController.control16And17Levels();
                        break;
                    case 14:
                        researchController.control16And17Levels();
                        break;
                    case 15:
                        researchController.level18Control();
                        break;
                    case 16:
                        researchController.level18Control();
                        break;
                    case 17:
                        // Araştırma 18 tamamlandı
                        break;
                    default:
                        break;
                     }
            }

}


}
