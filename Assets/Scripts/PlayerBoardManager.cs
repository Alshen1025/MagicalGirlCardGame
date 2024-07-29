using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBoardManager : MonoBehaviourPunCallbacks
{
    public GameObject Board; // UI 요소를 참조합니다.
    public KeyCode toggleKey = KeyCode.E; // UI를 토글할 키를 지정합니다.
    private bool isVisible = true; // UI 요소의 현재 상태를 추적합니다.
    private CardManager cardManager;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
    }

    void Update()
    {
        // 특정 키가 눌렸는지 확인합니다.
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleVisibility();
        }
    }

    void ToggleVisibility()
    {
        // UI 요소의 활성화 상태를 토글합니다.
        if(isVisible)
        {
            Board.SetActive(true);
        }
        else
        {
            Board.SetActive(false);
        }
        isVisible = !isVisible;
    }

}