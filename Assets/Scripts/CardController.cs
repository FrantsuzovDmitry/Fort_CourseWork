using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Card card;
    public Image illustration, image, cardBack;
    public TextMeshProUGUI cardName;

    private Transform originalParent;

    private void Awake()
    {
        //image = GetComponent<Image>();
    }

    public void Initialize(Card card, int ownerID)
    {
        this.card = card;
        this.card.ownerID = card.ownerID;
        illustration.sprite = card.illustration;
        cardName.text = card.cardName;

        cardBack.gameObject.SetActive(false);

        originalParent = transform.parent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        if (originalParent.name != $"Player{card.ownerID+1}Hand")
            //TurnManager.instance.currentPlayerTurn != card.ownerID)
        {
            Debug.Log("It's not your hand of not your turn!");
        }
        else
        {
            // Parent of the parent (UI in our case)
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }
        */

        // If fortress to attack is selected
        if (TurnManager.instance.isProcessOfCreatingGroup)
        {
            // Player try to take his card
            if (originalParent.name ==
                $"Player{TurnManager.instance.currentPlayerTurn + 1}Hand")
            {
                var temp = (Character)card;
                if (temp.IsInGroup)
                {
                    CardManager.instance.RemoveCharacterFromGroup(temp);
                    temp.IsInGroup = false;
                }
                else
                {
                    CardManager.instance.AddCharacterToGroup(temp);
                    temp.IsInGroup = true;
                }
            }
            // The card is not his
            else
            {
                //CardManager.instance.AttackToFortress(TurnManager.instance.currentPlayerTurn, (Fortress)card);
                //CardManager.instance.AttackToFortress(TurnManager.instance.currentPlayerTurn, this);
                Observer.onFortressAttacked(this);
                //TurnManager.instance.StopCreatingOfGroup();
            }
        }
        // If it's a Fort (OR SANDGLASSES)
        else if (originalParent.name == "PlayArea")
        {
            TurnManager.instance.CreatingOfGroupOfCharacters();
        }
    }

    public void changeParent(Transform parent)
    {
        originalParent = parent;
    }

    private void ReturnRoHand()
    {
        //transform.SetParent(originalParent);
        //transform.localPosition = Vector3.zero;
    }
}
