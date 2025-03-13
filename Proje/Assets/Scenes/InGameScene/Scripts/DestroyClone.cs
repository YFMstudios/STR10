using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class DestroyClone : MonoBehaviourPun
{
    public float delay = 5f;

    void Start()
    {
        if (photonView.IsMine)
        {
            // Sadece sahibi olan istemci 5 saniye sonra yok etmeyi tetikliyor.
            // Tüm istemcilerde yok etmek için PhotonNetwork.Destroy():
            StartCoroutine(DestroyForAll());
        }
    }

    private IEnumerator DestroyForAll()
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject); // Herkeste siler
    }
}

