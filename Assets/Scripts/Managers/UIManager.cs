using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _tapToStartTMP;
    [SerializeField] private TextMeshProUGUI _rankTMP;

    public void TapToStart()
    {
        _tapToStartTMP.SetActive(false);
    }

    public void ShowRank(int rank)
    {
        _rankTMP.enabled = true;
        _rankTMP.text ="Sýralamanýz : " + rank;
    }
}
