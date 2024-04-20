using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum NeedToBeSelected : byte
{
    YES, NO
}

public class CardController : MonoBehaviour, IPointerDownHandler
{
    public Card Card;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image illustration, standardEmission, specialEmission, currentEmission, cardBack;

    public bool Selected { get; private set; } = false;

    private Transform parentPosition;
    private Vector3 Position => transform.position;

    public void Initialize(Card card)
    {
        this.Card = card;
        this.Card.OwnerID = card.OwnerID;
        illustration.sprite = card.logo;
        cardName.text = card.cardName;

        cardBack.SetInactive();

        parentPosition = transform.parent;

        currentEmission = standardEmission;
    }

    /// Turn ON card back
    public void Hide()
    {
        cardBack.SetActive();
    }

    /// Turn OFF card back
    public void Show()
    {
        cardBack.SetInactive();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Card.ProcessOnClick(this) == NeedToBeSelected.YES)
            MakeSelected();
        else
            MakeUnselected();
    }

    public bool IsOneOfTheCardsThatCanBeGiven()
    {
        return parentPosition.name ==
                $"CardsToSelectionPanel";
    }

    public void SetStdEmission()
    {
        currentEmission = standardEmission;
    }

    public void SetSpecialEmission()
    {
        currentEmission = specialEmission;
    }

    public void MakeSelected()
    {
        currentEmission.SetActive();
        transform.position = new Vector3(Position.x, Position.y, -5);   // set in front
        Selected = true;
    }

    public void MakeUnselected()
    {
        currentEmission.SetInactive();
        transform.position = new Vector3(Position.x, Position.y, 0);    // default value
        Selected = false;
    }

    public bool IsCardInPlayerHand()
    {
        return parentPosition.name ==
                $"Player{TurnManager.instance.CurrentPlayerTurn + 1}Hand";
    }

    public bool IsCardIsPlayersOwn()
    {
        return parentPosition.name ==
                $"Player{TurnManager.instance.CurrentPlayerTurn + 1}Forts";
    }

    public bool IsCardInTheMidOfTable() { return parentPosition.name == "PlayArea"; }

    public void ChangePosition(Transform parent)
    {
        this.gameObject.transform.SetParent(parent);
        parentPosition = parent;
        this.gameObject.transform.localRotation = Quaternion.identity;
    }
}
