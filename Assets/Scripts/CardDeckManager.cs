using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class CardManager : MonoBehaviour
{
    public Transform DEdeckPosition; // 카드가 모일 덱 위치
    public Transform CdeckPosition;
    public Transform DMdeckPosition;
    public Transform EdeckPosition;
    public Transform IdeckPosition;
    public Transform MCdeckPosition;
    public Transform MdeckPosition;
    public Transform QdeckPosition;

    public float randomMoveDuration = 1f; // 랜덤 위치로 이동하는 시간
    public float moveToDeckDuration = 1f; // 덱 위치로 이동하는 시간
    public float moveToHandDuration = 1f;
    public Transform[] DEcardPositions;
    public Transform[] CcardPositions;
    public Transform[] DMcardPositions;
    public Transform[] EcardPositions;
    public Transform[] IcardPositions;
    public Transform[] MCcardPositions;
    public Transform[] McardPositions;
    public Transform[] QcardPositions;

    private List<Card> DEcards = new List<Card>(); //카드 덱 숫자
    public GameObject[] DEcardPrefabs;
    private List<Card> Ccards = new List<Card>();
    public GameObject[] CcardPrefabs;
    private List<Card> DMcards = new List<Card>();
    public GameObject[] DMcardPrefabs;
    private List<Card> Ecards = new List<Card>();
    public GameObject[] EcardPrefabs;
    private List<Card> Icards = new List<Card>();
    public GameObject[] IcardPrefabs;
    private List<Card> MCcards = new List<Card>();
    public GameObject[] MCcardPrefabs;
    private List<Card> Mcards = new List<Card>();
    public GameObject[] McardPrefabs;
    private List<Card> Qcards = new List<Card>();
    public GameObject[] QcardPrefabs;

    public IEnumerator ResetCards()
    {
        yield return StartCoroutine(MoveToDeck(Ecards, EdeckPosition, EcardPositions, EcardPrefabs));
        yield return StartCoroutine(MoveToDeck(Mcards, MdeckPosition, McardPositions, McardPrefabs));
        yield return StartCoroutine(MoveToDeck(Icards, IdeckPosition, IcardPositions, IcardPrefabs));
        yield return StartCoroutine(MoveToDeck(MCcards, MCdeckPosition, MCcardPositions, MCcardPrefabs));
        yield return StartCoroutine(MoveToDeck(Qcards, QdeckPosition, QcardPositions, QcardPrefabs));
    }

    void GenerateCards(List<Card> cards, GameObject[] prefabs, Transform deckPosition)
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject cardObject = Instantiate(prefab, deckPosition.position, Quaternion.identity);
            cardObject.transform.localScale = new Vector3(0.08f, 0.08f, 1f);
            cards.Add(cardObject.GetComponent<Card>());
        }
    }
    Vector3 RandomPosition()
    {
    // 카메라의 뷰포트 크기 가져오기
    Camera camera = Camera.main;

    Vector3 min = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
    Vector3 max = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
    
    return new Vector3(
        Random.Range(min.x, max.x),
        Random.Range(min.y, max.y),
        0
    );
    }

    private IEnumerator MoveToDeck(List<Card> cards, Transform deckPosition,Transform[] Cardpositions, GameObject[] prefabs)
    {
        GenerateCards(cards, prefabs,deckPosition);
        yield return new WaitForSeconds(randomMoveDuration);
        yield return new WaitForSeconds(randomMoveDuration);
        foreach (var card in cards)
        {
            card.SetCard(false);
            Vector3 randomPosition = RandomPosition();
            card.Flip();
            card.transform.DOMove(randomPosition, randomMoveDuration).SetEase(Ease.InOutQuad);

        }

        yield return new WaitForSeconds(randomMoveDuration + 1f);
        
        foreach (var card in cards)
        {
            card.transform.DOMove(deckPosition.position, moveToDeckDuration).SetEase(Ease.InOutQuad);
        }

        // 앞면인 카드를 뒷면으로 만들기
        foreach (var card in cards)
        {
            card.Flip();
        }

        yield return new WaitForSeconds(2f); 

        // 무작위로 5장의 카드를 선택하여 화면에 배치
        MoveRandomCardsToField(cards, Cardpositions);

        yield return new WaitForSeconds(3f); 
    }

    private void MoveRandomCardsToField(List<Card> cards, Transform[] positions)
    {
        ShuffleCards(cards);
        for (int i = 0; i < positions.Length && i < cards.Count; i++)
        {
            Card card = cards[i];
            card.transform.DOMove(positions[i].position, moveToHandDuration).SetEase(Ease.InOutQuad)
                .OnComplete(() => StartCoroutine(FlipCardWithDelay(card)));
            card.SetCard(true);
            card.IsInField = true;
        }
    }

    private void ShuffleCards<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
    }

    private IEnumerator FlipCardWithDelay(Card card)
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        card.Flip();
    }

    public void RemoveCardFromField(Card card)
    {
        if (Ecards.Contains(card))
        {
            int cardIndex = Ecards.IndexOf(card);
            Ecards.Remove(card);
            AddCardToField(Ecards, EcardPositions[cardIndex]);
        }
        // 다른 덱에 대해서도 동일한 처리
        if (Mcards.Contains(card))
        {
            int cardIndex = Mcards.IndexOf(card);
            Mcards.Remove(card);
            AddCardToField(Mcards, McardPositions[cardIndex]);
        }
        if (Icards.Contains(card))
        {
            int cardIndex = Icards.IndexOf(card);
            Icards.Remove(card);
            AddCardToField(Icards, IcardPositions[cardIndex]);
        }
        if (MCcards.Contains(card))
        {
            int cardIndex = MCcards.IndexOf(card);
            MCcards.Remove(card);
            AddCardToField(MCcards, MCcardPositions[cardIndex]);
        }
        if (Qcards.Contains(card))
        {
            int cardIndex = Qcards.IndexOf(card);
            Qcards.Remove(card);
            AddCardToField(Qcards, QcardPositions[cardIndex]);
        }
        // 다른 덱에 대해서도 동일한 처리
    }

    private void AddCardToField(List<Card> deck, Transform position)
    {
        if (deck.Count > 0)
        {
            Card cardToMove = deck[Random.Range(0, deck.Count)];
            cardToMove.transform.position = position.position;
            cardToMove.transform.localScale = new Vector3(0.08f, 0.08f, 1f);
            cardToMove.transform.DOMove(position.position, randomMoveDuration).SetEase(Ease.InOutQuad);
            cardToMove.Flip();
            cardToMove.SetCard(true);
            cardToMove.IsInField = true;
        }
    }
}