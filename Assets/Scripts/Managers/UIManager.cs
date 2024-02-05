using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIButtons : byte
{
	StartAttackButton,
	AttackConfirmationButton
}

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public GameObject winnerPanel;

	[SerializeField] private TextMeshProUGUI winnerName;
	[SerializeField] private Button startAttackButton;
	[SerializeField] private Button attackConfirmationButton;

	private List<Button> _UIbuttons;

	private void Awake()
	{
		instance = this;
		_UIbuttons = new List<Button>() { startAttackButton, attackConfirmationButton };

		startAttackButton.onClick.AddListener(() =>
		{
			Mediator.OnCreatingAttackersGroupStarted();
		});
		attackConfirmationButton.onClick.AddListener(() =>
		{
			Mediator.OnFortressTriedAttacked();
		});
	}

	// And maybe this function is better to be in GameplayUIController/Manager OR Move methods from the controller here
	public void ShowWinnerPanel()
	{
		var winner = PlayerManager.instance.DefineWinner();
		winnerPanel.SetActive(true);
		winnerName.text = $"Player {winner.ID + 1} has won!";
	}

	public void ShowButton(UIButtons button)
	{
		foreach (Button btn in _UIbuttons)
		{
			btn.gameObject.SetActive(false);
		}

		switch (button)
		{
			case UIButtons.StartAttackButton:
				startAttackButton.gameObject.SetActive(true);
				break;
			case UIButtons.AttackConfirmationButton:
				attackConfirmationButton.gameObject.SetActive(true);
				break;
		}
	}

	// Probably should remember active button and set inactive only it
	public void HideCurrentButtons() 
	{
		foreach (Button btn in _UIbuttons)
		{
			btn.gameObject.SetActive(false);
		}
	}

	public void DebugNotification(string message)
	{
		Debug.Log(message);
	}

	public void ShowWarningMessage(string message)
	{
		// TODO: make more cute. Panel for example.
		DebugNotification(message);
	}
}
