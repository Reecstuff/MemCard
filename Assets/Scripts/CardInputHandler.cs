using UnityEngine.EventSystems;
using UnityEngine;


/// <summary>
/// Handle Input on Cards based on GraphicRaycaster
/// </summary>
public class CardInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    FlipCard flipCard;

    void Awake()
    {
        flipCard = GetComponentInParent<FlipCard>();
    }

    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        flipCard.OnHoverOver(true);
    }

    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        flipCard.OnHoverOver(false);
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!flipCard.GetCardDisabled())
        {
            flipCard.OnCardFlip();
            GlobalSelectedCard.Set(ref flipCard);
        }
    }

    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
    }

    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
    }
}
