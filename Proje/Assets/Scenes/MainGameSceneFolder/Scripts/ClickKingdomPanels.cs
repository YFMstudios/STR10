using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class ClickKingdomPanels : MonoBehaviour//Son Projenin Dosyas�
{
    //T�klana b�lgenin �zelliklerini g�steren paneldeki de�erleri de�i�tiren script.
    public Image FlagImage;
    public Image WarIcon;
    public Image ObservationImage;//G�zetleme Iconu
    public Sprite warSprite;
    public Sprite observationSprite;
    public TMP_Text owner;//sahibi
    public TMP_Text kingdom;//krall�k
    public TMP_Text civilization;//medeniyet
    public TMP_Text numberOfSoldier;//Asker Say�s�
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
                    Debug.Log("Lexiona T�kland�.");
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
                    kingdom.text = "Krall�k:Lexion";
                    civilization.text = "Medeniyet:Elf";
                    numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[4].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Alfgard")
                {
                    Debug.Log("Alfgarda T�kland�.");
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
                    kingdom.text = "Krall�k:Alfgard";
                    civilization.text = "Medeniyet:B�y�c�";
                    numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[1].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Zephrion")
                {
                    Debug.Log("Zephriona T�kland�.");
                    FlagImage.sprite = Kingdom.Kingdoms[5].Flag;
                    WarIcon.enabled = true;
                    ObservationImage.enabled = true;
                    WarIcon.sprite = warSprite;
                    ObservationImage.sprite = observationSprite;
                    owner.text = "Sahibi: Bilgisayar";
                    kingdom.text = "Krall�k:Zephyrion";
                    civilization.text = "Medeniyet:�l�ler";
                    numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[5].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Arianopol")
                {
                    Debug.Log("Arianopole T�kland�.");
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
                    kingdom.text = "Krall�k:Arianopol";
                    civilization.text = "Medeniyet:�nsan";
                    numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[0].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Dhamuron")
                {
                    Debug.Log("Dhamurona T�kland�.");
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
                    kingdom.text = "Krall�k:Dhamuron";
                    civilization.text = "Medeniyet:C�ce";
                    numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[3].SoldierAmount.ToString();
                }
                if (hit.collider.gameObject.name == "Akhadzria")
                {
                    Debug.Log("Akhadzria'ya T�kland�.");
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
                    kingdom.text = "Krall�k: Akhadzria";
                    civilization.text = "Medeniyet: Ork";
                    numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[2].SoldierAmount.ToString();
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
            kingdom.text = "Krall�k: Akhadzria";
            civilization.text = "Medeniyet: Ork";
            numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[2].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 3)
        {
            FlagImage.sprite = Kingdom.Kingdoms[1].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krall�k:Alfgard";
            civilization.text = "Medeniyet:B�y�c�";
            numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[1].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 4)
        {
            FlagImage.sprite = Kingdom.Kingdoms[0].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krall�k:Arianopol";
            civilization.text = "Medeniyet:�nsan";
            numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[0].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 5)
        {
            FlagImage.sprite = Kingdom.Kingdoms[3].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krall�k:Dhamuron";
            civilization.text = "Medeniyet:C�ce";
            numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[3].SoldierAmount.ToString();
        }
        else
        {
            FlagImage.sprite = Kingdom.Kingdoms[4].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krall�k:Lexion";
            civilization.text = "Medeniyet:Elf";
            numberOfSoldier.text = "Asker Say�s�: " + Kingdom.Kingdoms[4].SoldierAmount.ToString();
        }
    }

}
