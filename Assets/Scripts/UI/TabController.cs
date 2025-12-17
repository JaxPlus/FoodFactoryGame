using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNum)
    {
        // -1 bo jest craftingPanel ale nie ma do niego taba
        for (int i = 0; i < pages.Length - 1; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        }

        pages[tabNum].SetActive(true);
        tabImages[tabNum].color = Color.green;
        pages[3].SetActive(pages[0].activeSelf);
    }
}
