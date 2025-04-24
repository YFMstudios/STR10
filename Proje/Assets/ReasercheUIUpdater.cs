using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReasercheUIUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] seviyeImages = new Image[18];

    
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

            // Kilit simgesini yok et
            if (lockItems[i] != null)
                Destroy(lockItems[i]);

            // Butonu yok et
            if (buttons[i] != null)
{
    Destroy(buttons[i].gameObject);
}
        }
    }
}


}
