using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTransections : MonoBehaviour
{

    public GameObject[] pages;
    public GameObject rightButton, leftButton, contiuneButton;
    public int currentPage = 1;


    void Start()
    {
        pages = new GameObject[7]
        {
            GameObject.Find("page1"),
            GameObject.Find("page2"),
            GameObject.Find("page3"),
            GameObject.Find("page4"),
            GameObject.Find("page5"),
            GameObject.Find("page6"),
            GameObject.Find("page7"),
        };

        rightButton = GameObject.Find("ButtonRight");
        leftButton = GameObject.Find("ButtonLeft");
        contiuneButton = GameObject.Find("SelectButton");
        for(int i = 1; i < pages.Length;i++)
        {
            pages[i].SetActive(false);
        }
    }

    public void Update()
    {
        if(currentPage == 1 || currentPage == 7) 
        {
            contiuneButton.SetActive(false);    
        }
        else
        {
            contiuneButton.SetActive(true);
        }
    }

    public void clickRight()
    {
        if(currentPage == 7)
        {
            rightButton.SetActive(false);
        }
        else
        {
            rightButton.SetActive(true);
            currentPage++;
            foreach (GameObject page in pages)
            {
                if (page == pages[currentPage - 1])
                {
                    page.SetActive(true);
                }
                else
                {
                    page.SetActive(false);
                }
            }
        }
        
    }
    public void clickLeft()
    {
        if(currentPage == 1)
        {
            leftButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
            currentPage--;
            foreach (GameObject page in pages)
            {
                if (page == pages[currentPage - 1])
                {
                    page.SetActive(true);
                }
                else
                {
                    page.SetActive(false);
                }
            }
        }
    }


}
