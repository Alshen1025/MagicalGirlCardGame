using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class PlayerBoard : MonoBehaviour
{
    public Transform hiddenPosition; // 화면 밖 위치
    public Transform visiblePosition; // 화면 안 위치
    public float transitionDuration = 1f; // 이동 시간
    private bool isVisible = false;
    private bool isLocalPlayer = false;

    // 로컬 플레이어로 설정
    public void SetAsLocalPlayer()
    {
        isLocalPlayer = true;
    }

    // 보드 이동 메서드
    public void ToggleBoard()
    {
        if (isVisible)
        {
            HideBoard();
        }
        else
        {
            ShowBoard();
        }
    }

    // 보드를 화면 밖으로 이동
    public void HideBoard()
    {
        transform.DOMove(hiddenPosition.position, transitionDuration).SetEase(Ease.InOutQuad);
        isVisible = false;
    }

    // 보드를 화면 안으로 이동
    public void ShowBoard()
    {
        transform.DOMove(visiblePosition.position, transitionDuration).SetEase(Ease.InOutQuad);
        isVisible = true;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // 스페이스 키를 누르면 보드를 토글
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleBoard();
        }
    }
}