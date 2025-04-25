using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPropertiesUpdater : MonoBehaviour
{
    public KaynakYoneticisi kaynakYoneticisi;
    // Update is called once per frame
    void Update()
    {
     kaynakYoneticisi.SyncToPhoton();   
    }
}
