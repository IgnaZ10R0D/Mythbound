using UnityEngine;
using System.Collections.Generic;

public class GuidePager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private GuideOpener guideOpener;

    private int _currentPage;
    private bool _isActive;

    private void Start()
    {
        ResetGuide();
    }

    private void Update()
    {
        if (!_isActive) return;

        KeyCode shootKey = InputManager.Instance.GetKey("Shoot");

        if (shootKey != KeyCode.None && Input.GetKeyDown(shootKey))
        {
            NextPage();
        }
    }

    public void NextPage()
    {
        _currentPage++;

        if (_currentPage >= pages.Count)
        {
            _isActive = false;
            StartCoroutine(guideOpener.CloseGuide());
            return;
        }

        ShowPage(_currentPage);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Count; i++)
            pages[i].SetActive(i == index);
    }

    public void ResetGuide()
    {
        _currentPage = 0;
        ShowPage(_currentPage);
        _isActive = true;
    }
}
