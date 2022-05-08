using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Main logic for Memory Game
/// </summary>
public class MemCardController : MonoBehaviour
{
    [SerializeField]
    float ReflipAnimationTime = 1f;

    [SerializeField]
    ObjectFieldGenerator ObjectField;

    [SerializeField]
    List<FlipCard> SolvedCards = new List<FlipCard>();

    public delegate void CardCountChange(int failedCount, int solvedCount);
    public delegate void WinGame(int failedCount);

    public CardCountChange OnCardCountChange;
    public WinGame OnWinGame;



    Camera cam;
    FlipCard PrevCard;
    FlipCard CurrentCard;
    int FailCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        GlobalSelectedCard.OnCardChanged += CheckCard;
    }

    void CheckCard()
    {
        if (GlobalSelectedCard.Get())
        {
            if (PrevCard && PrevCard.transform.Equals(GlobalSelectedCard.Get().transform))
            {
                // Card is going to flip
                ResetCards();
                return;
            }

            CurrentCard = GlobalSelectedCard.Get();
            CompareCards();
        }
    }

    void CompareCards()
    {
        // One Card is flipped
        if (PrevCard == null)
        {
            PrevCard = CurrentCard;
            CurrentCard = null;
            return;
        }

        // Two Cards are flipped so compare those cards
        if (!PrevCard.BackText.text.Equals(CurrentCard.BackText.text))
        {
            FailCount++;

            if(OnCardCountChange != null)
                OnCardCountChange(FailCount, SolvedCards.Count);

            // Disable Cards
            PrevCard.SetCardDisabled();
            CurrentCard.SetCardDisabled();

            // Reset Cards
            StartCoroutine(ReFlipCard(PrevCard, CurrentCard));
            ResetCards();
            return;
        }
        else
        {
            // Disable Cards
            PrevCard.SetCardDisabled();
            CurrentCard.SetCardDisabled();
            StartCoroutine(SolveCards(PrevCard, CurrentCard));
            ResetCards();
        }
    }

    IEnumerator ReFlipCard(FlipCard firstCard, FlipCard secondCard)
    {
        yield return new WaitForSeconds(ReflipAnimationTime);

        firstCard.OnCardFlip();
        secondCard.OnCardFlip();
        firstCard.SetCardDisabled(false);
        secondCard.SetCardDisabled(false);
    }

    IEnumerator SolveCards(FlipCard firstCard, FlipCard secondCard)
    {
        yield return new WaitForSeconds(ReflipAnimationTime);

        // Add Cards to solved Pile
        SolvedCards.Add(firstCard);
        firstCard.transform.DOJump(new Vector3(transform.position.x, transform.position.y + 0.01f * SolvedCards.Count, transform.position.z), 5f, 1, 0.25f);

        SolvedCards.Add(secondCard);
        secondCard.transform.DOJump(new Vector3(transform.position.x, transform.position.y + 0.01f * SolvedCards.Count, transform.position.z), 5f, 1, 0.25f);

        if(OnCardCountChange != null)
            OnCardCountChange(FailCount, SolvedCards.Count);

        CheckWin();
    }

    void ResetCards()
    {
        PrevCard = null;
        CurrentCard = null;
    }

    void CheckWin()
    {
        if (SolvedCards.Count == ObjectField?.ObjectList.Count)
        {
            if(OnWinGame != null) 
                OnWinGame(FailCount);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        GlobalSelectedCard.OnCardChanged -= CheckCard;
    }
}
