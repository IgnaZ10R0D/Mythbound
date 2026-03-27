using UnityEngine;
using System.Collections.Generic;

public class GuidePager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private GuideOpener guideOpener;

    private int currentPage = 0;
    private bool isActive = false;

    private void Start()
    {
        ResetGuide();
    }

    private void Update()
    {
        if (!isActive) return;

        KeyCode shootKey = InputManager.Instance.GetKey("Shoot");

        if (shootKey != KeyCode.None && Input.GetKeyDown(shootKey))
        {
            NextPage();
        }
    }

    private void NextPage()
    {
        currentPage++;

        if (currentPage >= pages.Count)
        {
            isActive = false;
            StartCoroutine(guideOpener.CloseGuide());
            return;
        }

        ShowPage(currentPage);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Count; i++)
            pages[i].SetActive(i == index);
    }

    public void ResetGuide()
    {
        currentPage = 0;
        ShowPage(currentPage);
        isActive = true;
    }
}
