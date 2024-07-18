using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public Sprite front;
    public Sprite back;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool isSet = false;
    private bool isFront = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ShowBack();
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

    // 앞면을 표시하는 메서드
}