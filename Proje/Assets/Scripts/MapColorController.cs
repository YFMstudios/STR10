using System.Collections.Generic;
using UnityEngine;

public class MapColorController : MonoBehaviour
{
    public class Kingdoms
    {
        public GameObject Kingdom { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }

        public Kingdoms(GameObject gameObject, string name, Color color)
        {
            Kingdom = gameObject;
            Name = name;
            Color = color;
        }
    }

    public class Map
    {
        public List<Kingdoms> Kingdoms { get; set; }

        public Map()
        {
            Kingdoms = new List<Kingdoms>();
        }

        public void AddKingdom(GameObject gameObject, string name, Color color)
        {
            Kingdoms newKingdom = new Kingdoms(gameObject, name, color);
            Kingdoms.Add(newKingdom);
        }
    }

    private Renderer myRenderer;
    private Renderer controlRenderer;
    private Map gameMap;
    private string[] kingdomNames = { "Lexion", "Arianopol", "Dhamuron", "Akhadzria", "Alfgard" };
    private Color[] kindomColours = {
        new Color32(0xEF, 0xCC, 0x9E, 0xFF),
        new Color32(0xF1, 0xFA, 0x97, 0xFF),
        new Color32(0xDD, 0xDE, 0xD6, 0xFF),
        new Color32(0xE9, 0xAF, 0xB5, 0xFF),
        new Color32(0xCB, 0xD3, 0xF6, 0xFF)
    };
    private int coloredRelCount = 0;
    private GameObject lastColored;

    void Start()
    {
        gameMap = new Map();
        for (int i = 0; i < 5; i++)
        {
            GameObject kingdomObject = GameObject.Find(kingdomNames[i]);
            gameMap.AddKingdom(kingdomObject, kingdomObject.name, kindomColours[i]);
            Debug.Log(gameMap.Kingdoms[i].Name);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (coloredRelCount == 0)
                {
                    controlRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (controlRenderer.material.color == Color.white)
                    {
                        for (int i = 0; i < gameMap.Kingdoms.Count; i++)
                        {
                            if (hit.collider.gameObject == gameMap.Kingdoms[i].Kingdom)
                            {
                                myRenderer = gameMap.Kingdoms[i].Kingdom.GetComponent<Renderer>();
                                myRenderer.material.color = gameMap.Kingdoms[i].Color;
                                lastColored = gameMap.Kingdoms[i].Kingdom;
                                coloredRelCount++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        controlRenderer.material.color = Color.white;
                    }
                }
                else
                {
                    controlRenderer = lastColored.GetComponent<Renderer>();
                    controlRenderer.material.color = Color.white;
                    coloredRelCount--;

                    for (int i = 0; i < gameMap.Kingdoms.Count; i++)
                    {
                        if (hit.collider.gameObject == gameMap.Kingdoms[i].Kingdom)
                        {
                            myRenderer = gameMap.Kingdoms[i].Kingdom.GetComponent<Renderer>();
                            myRenderer.material.color = gameMap.Kingdoms[i].Color;
                            lastColored = gameMap.Kingdoms[i].Kingdom;
                            coloredRelCount++;
                            break;
                        }
                    }
                }
            }
        }
    }
}