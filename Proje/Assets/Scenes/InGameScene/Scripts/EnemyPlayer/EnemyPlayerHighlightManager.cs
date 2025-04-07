using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public class EnemyPlayerHighlightManager : MonoBehaviourPun
{
    private Transform highlightedObj;    // Üzerine gelinen obje
    private Transform selectedObj;       // Seçilen obje
    public LayerMask selectableLayer;    // Seçilebilir objelerin katman maskesi

    private Outline highlightOutline;    // Kontur bileşeni
    private RaycastHit hit;              // Raycast sonucu

    void Update()
    {
        if (!photonView.IsMine)
            return;

        HoverHighlight();
    }

    void HoverHighlight()
    {
        if (highlightedObj != null)
        {
            if (highlightOutline != null)
                highlightOutline.enabled = false;

            highlightedObj = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
        {
            highlightedObj = hit.transform;

            if ((highlightedObj.CompareTag("Player") || highlightedObj.CompareTag("AllyMinion") || highlightedObj.CompareTag("AllyTurret")) && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>();

                if (highlightOutline != null)
                    highlightOutline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
    }

    public void SelectedHighlight()
    {
        if (highlightedObj != null && (highlightedObj.CompareTag("Player") || highlightedObj.CompareTag("AllyMinion") || highlightedObj.CompareTag("AllyTurret")))
        {
            if (selectedObj != null && selectedObj.GetComponent<Outline>() != null)
                selectedObj.GetComponent<Outline>().enabled = false;

            selectedObj = highlightedObj;
            selectedObj.GetComponent<Outline>().enabled = true;

            if (highlightOutline != null)
                highlightOutline.enabled = true;

            highlightedObj = null;
        }
    }

    public void DeselectHighlight()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;
            selectedObj = null;
        }
    }
}
