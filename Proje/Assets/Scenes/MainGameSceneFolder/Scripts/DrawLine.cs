using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapColorController;

public class DrawLine : MonoBehaviour
{
    public GameObject[] kingdoms;

    public Sprite akhadzriaWithLine;
    public Sprite alfgardWithLine;
    public Sprite arianopolWithLine;
    public Sprite dhamuronWithLine;
    public Sprite lexionWithLine;
    public Sprite zephrionWithLine;

    public Sprite akhadzriaSprite;
    public Sprite alfgardSprite;
    public Sprite arianopolSprite;
    public Sprite dhamuronSprite;
    public Sprite lexionSprite;
    public Sprite zephrionSprite;


    public void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Lexion")
                {
                    SetPrefabOnTopAndDrawLine(hit.collider.gameObject);
                    closeOthers(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.name == "Alfgard")
                {
                    SetPrefabOnTopAndDrawLine(hit.collider.gameObject);
                    closeOthers(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.name == "Zephrion")
                {
                    SetPrefabOnTopAndDrawLine(hit.collider.gameObject);
                    closeOthers(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.name == "Arianopol")
                {
                    SetPrefabOnTopAndDrawLine(hit.collider.gameObject);
                    closeOthers(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.name == "Dhamuron")
                {
                    SetPrefabOnTopAndDrawLine(hit.collider.gameObject);
                    closeOthers(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.name == "Akhadzria")
                {
                    SetPrefabOnTopAndDrawLine(hit.collider.gameObject);
                    closeOthers(hit.collider.gameObject);
                }
            }
        }
    }

    public void SetPrefabOnTopAndDrawLine(GameObject go)
    {
        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sortingLayerName = "Foreground";
            drawLine(go);

        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }

    public void drawLine(GameObject go)
    {
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        if (go.name == "Lexion")
        {
            spriteRenderer.sprite = lexionWithLine;
        }
        if (go.name == "Akhadzria")
        {
            spriteRenderer.sprite = akhadzriaWithLine;
        }
        if (go.name == "Alfgard")
        {
            spriteRenderer.sprite = alfgardWithLine;
        }
        if (go.name == "Zephrion")
        {
            spriteRenderer.sprite = zephrionWithLine;
        }
        if (go.name == "Arianopol")
        {
            spriteRenderer.sprite = arianopolWithLine;
        }
        if (go.name == "Dhamuron")
        {
            spriteRenderer.sprite = dhamuronWithLine;
        }
    }

    public void closeOthers(GameObject go)
    {

        foreach (GameObject kingdom in kingdoms)
        {
            if (kingdom.name != go.name)
            {
                SpriteRenderer spriteRenderer = kingdom.GetComponent<SpriteRenderer>(); // Diðer krallýklarýn SpriteRenderer bileþenlerine eriþ
                Renderer renderer = kingdom.GetComponent<Renderer>();
                if (kingdom.name == "Lexion")
                {
                    Debug.Log("Buraya Girdim");
                    spriteRenderer.sprite = lexionSprite;
                    renderer.sortingLayerName = "Default";
                }
                else if (kingdom.name == "Akhadzria")
                {
                    spriteRenderer.sprite = akhadzriaSprite;
                    renderer.sortingLayerName = "Default";
                }
                else if (kingdom.name == "Alfgard")
                {
                    spriteRenderer.sprite = alfgardSprite;
                    renderer.sortingLayerName = "Default";
                }
                else if (kingdom.name == "Zephrion")
                {
                    spriteRenderer.sprite = zephrionSprite;
                    renderer.sortingLayerName = "Default";
                }
                else if (kingdom.name == "Arianopol")
                {
                    spriteRenderer.sprite = arianopolSprite;
                    renderer.sortingLayerName = "Default";
                }
                else //Dhamuron kaldý
                {
                    spriteRenderer.sprite = dhamuronSprite;
                    renderer.sortingLayerName = "Default";
                }
            }
        }
    }



}
