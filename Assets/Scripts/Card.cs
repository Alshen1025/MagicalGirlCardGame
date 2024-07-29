using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public Sprite front;
    public Sprite back;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    [SerializeField]
    public bool isSet = false;
    private bool isFront = false;
    public bool IsInField { get; set; }
    public bool isBoard = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        ShowBack();
        Vector2 spriteSize = spriteRenderer.size;
        boxCollider.size = spriteSize;
    }
    public void SetCard(bool value)
    {
        isSet = value;
    }

    public bool IsSet
    {
        get { return isSet; }
    }
    void ShowFront()
    {
        spriteRenderer.sprite = front;
    }

    // 뒷면을 표시하는 메서드
    void ShowBack()
    {
        spriteRenderer.sprite = back;
    }


    // 카드를 뒤집는 메서드
    public void Flip()
    {
    if (isFront)
    {
        transform.DOScaleX(0, 0.08f).OnComplete(() =>
        {
            ShowBack();
            transform.DOScaleX(0.08f, 0.08f); // 원래 크기로 복원
        });
    }
    else
    {
        transform.DOScaleX(0, 0.08f).OnComplete(() =>
        {
            ShowFront();
            transform.DOScaleX(0.08f, 0.08f);
        });
    }

    isFront = !isFront;
    }
}