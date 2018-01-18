using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksChangePage : MonoBehaviour {

    List<GameObject> pageList = new List<GameObject>();

    public enum BUTTONTYPE
    {
        LEFT,
        RIGHT
    }



    public BUTTONTYPE type;
    // Use this for initialization
    void Start()
    {
        foreach (Transform page in transform.parent.GetChild(2))
        {
            pageList.Add(page.gameObject);
        }       
    }

    void Update()
    {
        if (type == BUTTONTYPE.LEFT)
        {
            if (GetCurrentPage() == 0)
            {
                transform.GetComponent<Image>().enabled = false;
            }
            else
            {
                transform.GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            if (GetCurrentPage() == pageList.Count - 1)
            {
                transform.GetComponent<Image>().enabled = false;
            }
            else
            {
                transform.GetComponent<Image>().enabled = true;
            }
        }
    }


    public void OnClick()
    {
        int currentPage = GetCurrentPage();
        if (currentPage >= 0)
        {
            if (type == BUTTONTYPE.LEFT)
            {
                if (currentPage - 1 >= 0)
                {
                    pageList[currentPage].SetActive(false);
                    pageList[currentPage - 1].SetActive(true);
                }
            }
            else
            {
                if (currentPage + 1 < pageList.Count)
                {
                    pageList[currentPage].SetActive(false);
                    pageList[currentPage + 1].SetActive(true);
                }
            }
        }
    }

    int GetCurrentPage()
    {
        for (int i = 0;i < pageList.Count;++i)
        {
            if (pageList[i].activeSelf)
            {
                return i;
            }
        }

        return -1;
    }
}
