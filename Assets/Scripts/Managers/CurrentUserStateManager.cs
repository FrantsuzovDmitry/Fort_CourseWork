using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using static Assets.Scripts.Constants;

public static class CurrentUserStateManager
{
	private static readonly List<Character> selectedCharacters = new List<Character>();
	public static Character SelectedCharacter { get; private set; }
	public static bool IsCreatingGroupInProgress { get; private set; } = false;
	public static bool IsSelectingCardToGiveInProgress;
	public static byte SelectedFortIDToAttack { get; private set; }

	public static GroupOfCharacters GetAttackersGroup() => new GroupOfCharacters(selectedCharacters);

	public static bool CanAttackFortress => selectedCharacters.Count > 0;

	public static void StartCreatingOfGroupOfCharacters()
	{
		IsCreatingGroupInProgress = true;
		UIManager.instance.DebugNotification("Start creating of group");
	}

	public static void RememberUserFortSelection(byte numberOfFortToAttack)
	{
		if (numberOfFortToAttack >= MIN_FORT_RATE && numberOfFortToAttack <= MAX_FORT_RATE)
			SelectedFortIDToAttack = numberOfFortToAttack;
		else
			throw new Exception("Incorrect number of fort!");
	}

	public static void StopCreatingOfGroupOfCharacters()
	{
		IsCreatingGroupInProgress = false;
		SelectedFortIDToAttack = MIN_FORT_RATE - 1;
		ClearGroup();
		UIManager.instance.DebugNotification("Stop creating of group");
	}

	public static void RemoveCharacterFromGroup(Character character)
	{
		selectedCharacters.Remove(character);
	}

	public static void AddCharacterToGroup(Character character)
	{
		selectedCharacters.Add(character);
	}

	public static void ClearGroup()
	{
		selectedCharacters.Clear();
	}

	public static void RememberUserCharacterSelection(Character character)
	{
		if (character != null)
			SelectedCharacter = character;
		else
			throw new Exception("Nullable character!");
	}
}
