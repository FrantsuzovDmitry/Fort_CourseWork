using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Scripts;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Card card;
    public Image illustration, emission, cardBack;
    public TextMeshProUGUI cardName;

    private Transform parentPosition;
    private Vector3 position => transform.position;

	public void Initialize(Card card, int ownerID)
    {
        this.card = card;
        this.card.ownerID = card.ownerID;
        illustration.sprite = card.illustration;
        cardName.text = card.cardName;

        cardBack.SetInactive();

        parentPosition = transform.parent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    private void ShowCardInfo()
    {
        //TODO:
        /*
         * При наведении (или при долгом нажатии) на карту должна появляться информация про неё.
         * То есть должна дергаться инфа о карте из класса Card. Что-то типа GetCardInformation; 
         * Это сильно потом надо будет сделать.
         * 
         * При обычном нажатии происходит выбор карты
         * */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (card is Sandglass) return;

        // If fortress to attack is selected
        if (TurnManager.instance.NowTheProcessOfCreatingGroupIsUnderway)
        {
            // Player try to take his card
            if (IsCardInPlayerHand())
            {
                var temp = (Character)card;
                if (temp.IsInGroup)
                {
                    CardManager.instance.RemoveCharacterFromGroup(temp);
                    temp.IsInGroup = false;
                    MakeUnselected();
                }
                else
                {
                    CardManager.instance.AddCharacterToGroup(temp);
                    temp.IsInGroup = true;
                    MakeSelected();
				}
			}
            // The card is not his
            else if (card is Fortress)
            {
                //CardManager.instance.AttackToFortress(TurnManager.instance.currentPlayerTurn, (Fortress)card);
                //CardManager.instance.AttackToFortress(TurnManager.instance.currentPlayerTurn, this);
                Observer.onFortressAttacked(this);
                //TurnManager.instance.StopCreatingOfGroup();
            }
            else
            {
                Debug.Log("You cannot attack this card");
            }
        }
        // If it's a Fort on the table
        else if (IsCardInTheMidOfTable())
        {
            MakeSelected();
            UIManager.instance.ShowButton(UIButtons.StartAttackButton);
            //TurnManager.instance.StartCreatingOfGroupOfCharacters();
        }
    }

    private void MakeSelected()
    {
        emission.SetActive();
        transform.position = new Vector3(position.x, position.y, -5);   // set in front
    }

    private void MakeUnselected()
    {
        emission.SetInactive();
		transform.position = new Vector3(position.x, position.y, 0);    // default value
	}

    private bool IsCardInPlayerHand() {
        return parentPosition.name ==
                $"Player{TurnManager.instance.currentPlayerTurn + 1}Hand";
    }

    private bool IsCardInTheMidOfTable() { return parentPosition.name == "PlayArea"; }

	public void changePosition(Transform parent)
    {
        parentPosition = parent;
    }

    private void ReturnToHand()
    {
        //transform.SetParent(originalParent);
        //transform.localPosition = Vector3.zero;
    }
}
