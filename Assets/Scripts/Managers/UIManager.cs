using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _tapToStartPanel;

    public void TapToStart()
    {
        _tapToStartPanel.SetActive(false);
    }
}
