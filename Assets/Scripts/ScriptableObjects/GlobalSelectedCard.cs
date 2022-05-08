using UnityEngine;

[CreateAssetMenu]
public class GlobalSelectedCard : ScriptableObject
{
    private static GlobalSelectedCard instance;

    public delegate void CardChanged();
    public static CardChanged OnCardChanged;

    private static FlipCard currentCard;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        instance = Resources.Load<GlobalSelectedCard>("GlobalSelectedCard");
    }

    public static void Set(ref FlipCard newCard)
    {
        currentCard = newCard;
        OnCardChanged();
    }

    public static FlipCard Get()
    {
        return currentCard;
    }
}
