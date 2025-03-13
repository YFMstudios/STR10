using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class ClickKingdomPanels : MonoBehaviour//Son Projenin Dosyasý
{
    //Týklana bölgenin özelliklerini gösteren paneldeki deðerleri deðiþtiren script.
    public Image FlagImage;
    public Image WarIcon;
    public Image ObservationImage;//Gözetleme Iconu
    public Sprite warSprite;
    public Sprite observationSprite;
    public TMP_Text owner;//sahibi
    public TMP_Text kingdom;//krallýk
    public TMP_Text civilization;//medeniyet
    public TMP_Text numberOfSoldier;//Asker Sayýsý
    int selectedKingdom = GetVariableFromHere.currentSpriteNum;

    // Start is called before the first frame update

    void Start()
    {
        createDefaultPanel();
        Renderer renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
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
                    Debug.Log("Lexiona Týklandý.");
                    FlagImage.sprite = Kingdom.Kingdoms[4].Flag;
                    if (isYourKingdoms("Lexion") == true)
                    {
                        WarIcon.enabled = false;
                        ObservationImage.enabled = false;
                    }
                    else
                    {
                        WarIcon.enabled = true;
                        ObservationImage.enabled = true;
                        WarIcon.sprite = warSprite;
                        ObservationImage.sprite = observationSprite;
                    }
                    owner.text = "Sahibi: " + findOwner("Lexion");
                    kingdom.text = "Krallýk:Lexion";
                    civilization.text = "Medeniyet:Elf";
                    numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[4].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Alfgard")
                {
                    Debug.Log("Alfgarda Týklandý.");
                    FlagImage.sprite = Kingdom.Kingdoms[1].Flag;
                    if (isYourKingdoms("Alfgard") == true)
                    {
                        WarIcon.enabled = false;
                        ObservationImage.enabled = false;
                    }
                    else
                    {
                        WarIcon.enabled = true;
                        ObservationImage.enabled = true;
                        WarIcon.sprite = warSprite;
                        ObservationImage.sprite = observationSprite;
                    }
                    owner.text = "Sahibi: " + findOwner("Alfgard");
                    kingdom.text = "Krallýk:Alfgard";
                    civilization.text = "Medeniyet:Büyücü";
                    numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[1].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Zephrion")
                {
                    Debug.Log("Zephriona Týklandý.");
                    FlagImage.sprite = Kingdom.Kingdoms[5].Flag;
                    WarIcon.enabled = true;
                    ObservationImage.enabled = true;
                    WarIcon.sprite = warSprite;
                    ObservationImage.sprite = observationSprite;
                    owner.text = "Sahibi: Bilgisayar";
                    kingdom.text = "Krallýk:Zephyrion";
                    civilization.text = "Medeniyet:Ölüler";
                    numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[5].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Arianopol")
                {
                    Debug.Log("Arianopole Týklandý.");
                    FlagImage.sprite = Kingdom.Kingdoms[0].Flag;
                    if (isYourKingdoms("Arianopol") == true)
                    {
                        WarIcon.enabled = false;
                        ObservationImage.enabled = false;
                    }
                    else
                    {
                        WarIcon.enabled = true;
                        ObservationImage.enabled = true;
                        WarIcon.sprite = warSprite;
                        ObservationImage.sprite = observationSprite;
                    }
                    owner.text = "Sahibi: " + findOwner("Arianopol");
                    kingdom.text = "Krallýk:Arianopol";
                    civilization.text = "Medeniyet:Ýnsan";
                    numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[0].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Dhamuron")
                {
                    Debug.Log("Dhamurona Týklandý.");
                    FlagImage.sprite = Kingdom.Kingdoms[3].Flag;
                    if (isYourKingdoms("Dhamuron") == true)
                    {
                        WarIcon.enabled = false;
                        ObservationImage.enabled = false;
                    }
                    else
                    {
                        WarIcon.enabled = true;
                        ObservationImage.enabled = true;
                        WarIcon.sprite = warSprite;
                        ObservationImage.sprite = observationSprite;
                    }
                    owner.text = "Sahibi: " + findOwner("Dhamuron");
                    kingdom.text = "Krallýk:Dhamuron";
                    civilization.text = "Medeniyet:Cüce";
                    numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[3].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Akhadzria")
                {
                    Debug.Log("Akhadzria'ya Týklandý.");
                    FlagImage.sprite = Kingdom.Kingdoms[2].Flag;
                    if (isYourKingdoms("Akhadzria") == true)
                    {
                        WarIcon.enabled = false;
                        ObservationImage.enabled = false;
                    }
                    else
                    {
                        WarIcon.enabled = true;
                        ObservationImage.enabled = true;
                        WarIcon.sprite = warSprite;
                        ObservationImage.sprite = observationSprite;
                    }
                    owner.text = "Sahibi: " + findOwner("Akhadzria");
                    kingdom.text = "Krallýk: Akhadzria";
                    civilization.text = "Medeniyet: Ork";
                    numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[2].SoldierAmount.ToString();
                }
            }
        }
    }
    public bool isYourKingdoms(string name)
    {
        foreach (Kingdom kingdom in Kingdom.Kingdoms)
        {
            if (kingdom.Owner == 1 && kingdom.Name == name)
            {
                return true;
            }
        }
        return false;
    }

    public string findOwner(string name)
    {


        foreach (Kingdom kingdom in Kingdom.Kingdoms)
        {
            if (kingdom.Owner == 1 && kingdom.Name == name)
            {
                return "Player";
            }
        }
        return "Bilgisayar";

    }


    public void createDefaultPanel()
    {
        if (selectedKingdom == 2)
        {
            FlagImage.sprite = Kingdom.Kingdoms[2].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallýk: Akhadzria";
            civilization.text = "Medeniyet: Ork";
            numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[2].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 3)
        {
            FlagImage.sprite = Kingdom.Kingdoms[1].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallýk:Alfgard";
            civilization.text = "Medeniyet:Büyücü";
            numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[1].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 4)
        {
            FlagImage.sprite = Kingdom.Kingdoms[0].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallýk:Arianopol";
            civilization.text = "Medeniyet:Ýnsan";
            numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[0].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 5)
        {
            FlagImage.sprite = Kingdom.Kingdoms[3].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallýk:Dhamuron";
            civilization.text = "Medeniyet:Cüce";
            numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[3].SoldierAmount.ToString();
        }
        else
        {
            FlagImage.sprite = Kingdom.Kingdoms[4].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallýk:Lexion";
            civilization.text = "Medeniyet:Elf";
            numberOfSoldier.text = "Asker Sayýsý: " + Kingdom.Kingdoms[4].SoldierAmount.ToString();
        }
    }

}
