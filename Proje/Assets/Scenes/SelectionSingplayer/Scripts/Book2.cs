using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Book2 : MonoBehaviour
{
    public GetVariableFromHere2 getVariable;
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages;
    int index = -1;
    bool rotate = false;
    [SerializeField] GameObject ButtonLeft;
    [SerializeField] GameObject ButtonRight;
    int k = 0;
    GameObject[] pageObjects = new GameObject[6];
    public int currentSpriteNumber = 1;
    [SerializeField]GameObject SelectButton;


    private void Start()
    {
        GetVariableFromHere.currentSpriteNum = currentSpriteNumber;
        ButtonLeft.SetActive(false);
        SelectButton = GameObject.Find("SelectButton");

        // PageSprite2'den ba�layarak GameObject'leri bul ve devre d��� b�rak
        for (int i = 0; i < pageObjects.Length; i++)
        {
            string objectName = "PageSprite" + (i + 2);
            pageObjects[i] = GameObject.Find(objectName);

            if (pageObjects[i] != null)
            {
                pageObjects[i].SetActive(false);
            }
            else
            {
                Debug.LogError(objectName + " not found!");
            }
        }
    }

    public void Update()
    {
        if((currentSpriteNumber == 1 && SelectButton != false )|| (currentSpriteNumber ==7 && SelectButton != false))
        {
            SelectButton.SetActive(false);
        }
        else
        {
            SelectButton.SetActive(true);
        }
    }

    public void RotateNext()
    {
        currentSpriteNumber++;
        GetVariableFromHere.currentSpriteNum = currentSpriteNumber;
        if(currentSpriteNumber == 7)
        {
            ButtonRight.SetActive(false);
        }

            for (int i = 0; i < pageObjects.Length; i++)
            {
                if (pageObjects[i].name == "PageSprite" + currentSpriteNumber)
                {
                    pageObjects[i].SetActive(true);
                }
                else
                {
                    pageObjects[i].SetActive(false);
                }
            }

            if (rotate == true)
            {
                return;
            }

            index++;
            float angle = 0;
            ForwardButtonActions();
            pages[index].SetAsLastSibling();
            StartCoroutine(Rotate(angle, true));

            

    }

    public void ForwardButtonActions()
    {
        if (!ButtonLeft.activeInHierarchy)
        {
            ButtonLeft.SetActive(true);
        }

        if (index == pages.Count - 1)
        {
            ButtonRight.SetActive(false);
        }
    }

    public void RotateBack()
    {

        currentSpriteNumber--;
        GetVariableFromHere.currentSpriteNum = currentSpriteNumber;
        for (int i = 0; i < pageObjects.Length; i++)
        {
            if (pageObjects[i].name == "PageSprite" + currentSpriteNumber)
            {
                pageObjects[i].SetActive(true);
            }
            else
            {
                pageObjects[i].SetActive(false);
            }
        }

        if (rotate == true)
        {
            return;
        }

        float angle = 0;
        pages[index].SetAsLastSibling();
        BackButtonActions();
        StartCoroutine(Rotate(angle, false));
    }

    public void BackButtonActions()
    {
        if (!ButtonRight.activeInHierarchy)
        {
            ButtonRight.SetActive(true);
        }

        if (index - 1 == -1)
        {
            ButtonLeft.SetActive(false);
        }
    }

    IEnumerator Rotate(float angle, bool forward)
    {
        float value = 0f;
        while (true)
        {
            rotate = true;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            value += Time.deltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);
            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if (angle1 < 0.1f)
            {
                if (!forward)
                {
                    index--;
                }
                rotate = false;
                break;
            }
            yield return null;
        }
    }

    }


