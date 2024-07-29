using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drag : MonoBehaviour
{
    private Camera mainCamera;
    private Card card;
    private bool isDragging = false;
    private Vector3 offset;
    private PlayerBoardManager playerBoardManager;
    private bool isRightClick = false;
    private int originalOrderInLayer; 
    private int highlightedOrderInLayer = 10;
    private SpriteRenderer spriteRenderer;
    private CardManager cardManager; // CardManager 추가

    void Start()
    {
        mainCamera = Camera.main;
        card = GetComponent<Card>();
        playerBoardManager = FindObjectOfType<PlayerBoardManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalOrderInLayer = spriteRenderer.sortingOrder;
        cardManager = FindObjectOfType<CardManager>(); // CardManager 초기화
    }

    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        // 우클릭 감지
        if (Input.GetMouseButtonDown(1)) // 우클릭 시
        {
            //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            Vector2 direction = Vector2.zero;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, direction);

            Debug.DrawRay(mousePosition, direction, Color.green, 2f); 
            Debug.DrawLine(mousePosition, mousePosition + Vector3.forward * 10, Color.green, 2f);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isRightClick = true; // 우클릭 상태를 true로 설정
                transform.DOScale(0.2f, 0.2f); // 크기 확대
                spriteRenderer.sortingOrder = highlightedOrderInLayer;
            }
        }

        // 우클릭 버튼 떼었을 때
        if (Input.GetMouseButtonUp(1)) // 우클릭 떼었을 때
        {
            if (isRightClick) // 우클릭 상태일 때만
            {
                transform.DOScale(0.08f, 0.2f); // 크기 원래대로 복원
                isRightClick = false; // 우클릭 상태를 false로 설정
                spriteRenderer.sortingOrder = originalOrderInLayer;
            }
        }
    }

    void OnMouseDown()
    {
        if (card.IsSet || card.isBoard)
        {
            isDragging = true;
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
        else
        {
            Debug.Log("Card is not set: " + card.IsSet);
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z) + offset;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            Debug.DrawLine(mousePosition, mousePosition + Vector3.forward * 10, Color.green, 2f);
            Transform boardTransform = GameObject.FindGameObjectWithTag("PlayerBoard")?.transform;

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("PlayerBoard"))
                {
                    Debug.Log("Card dropped on a Board");
                    transform.SetParent(boardTransform);
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    spriteRenderer.sortingOrder = 5;
                    cardManager.RemoveCardFromField(card); // 필드에서 카드를 제거하고 새로운 카드를 추가
                    
                }
                else
                {
                    Debug.Log(hit.collider.tag);
                }
            }
        }
    }
}