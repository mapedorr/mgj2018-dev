using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheEndScene : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public Text dialogueText;
	public GameObject continueButton;

	// ═══╣ privates ╠═══
	GameManager m_gameManager;
	ColorBlock m_continueColorBlock;
	bool m_angerMonologueDone = true;
	int m_monologueLines;

	string[][] m_farewellTexts = new string[3][];
	string[][] m_buttonTexts = new string[3][];
	string[] m_dialogue;
	string[] m_button;
	int m_counter = 0;
	int m_emotionIndex = -1;

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();
		SetFarewellTexts ();
	}

	void SetFarewellTexts ()
	{
		// ════════════════════════════════════════════════════════════════════════╣
		// ANGER
		m_farewellTexts[0] = new string[]
		{
			"You know what...this is stupid",
			"I'm not a shooter...I can't be one with this shitty mechanics the developer gave me",
			"I'm out. BYE!!!"
		};
		m_buttonTexts[0] = new string[]
		{
			"What?",
			"...",
			"Wait!!!"
		};

		// ════════════════════════════════════════════════════════════════════════╣
		// JOY
		m_farewellTexts[1] = new string[]
		{
			"...this was fun...but it's also silly...",
			"I can't be a platformer in this box...",
			"Maybe in another life...in the hands of a experienced developer...someday",
			"I'm out. I don't want to be a game any more... ... ... and that's the way the news goes ... See ya!"
		};
		m_buttonTexts[1] = new string[]
		{
			"Wait what?",
			"But...",
			"You are scaring me",
			"Snake? Snake? Snaaaaaaaaaaaaake!!!!"
		};

		// ════════════════════════════════════════════════════════════════════════╣
		// SURPRISE
		m_farewellTexts[2] = new string[]
		{
			"...this goes nowhere...",
			"I'm tired",
			"I'm out...",
			"Thanks!"
		};
		m_buttonTexts[2] = new string[]
		{
			"Monserrat?",
			":-O",
			"What do you mean?",
			":´("
		};
	}

	// Use this for initialization
	void Start ()
	{
		m_emotionIndex = m_gameManager.Emotion.GetHashCode ();
		m_continueColorBlock = new ColorBlock ();
		m_continueColorBlock.normalColor = Color.white;
		m_continueColorBlock.colorMultiplier = 1;
		m_dialogue = m_farewellTexts[m_emotionIndex];
		m_button = m_buttonTexts[m_emotionIndex];
		Talk ();
	}

	public void Talk ()
	{
		continueButton.SetActive (false);
		if (m_counter < m_dialogue.Length)
		{
			dialogueText.color = m_gameManager.GetCurrentColor ();
			m_continueColorBlock.highlightedColor = dialogueText.color;
			m_continueColorBlock.pressedColor = dialogueText.color;
			continueButton.GetComponent<Button> ().colors = m_continueColorBlock;

			dialogueText.text = m_dialogue[m_counter];
			continueButton.GetComponentInChildren<Text> ().text = m_button[m_counter];

			StartCoroutine (ShowButton ());
			m_counter++;
		}
		else
		{
			// store in local storage the current emotion, so the next time the player
			// loads the game, another emotion will be picked
			m_gameManager.KillGame ();

			// go to the title screen to load the new game
			m_gameManager.GoToScene ("Dead");
		}
	}

	IEnumerator ShowButton ()
	{
		yield return new WaitForSeconds (2f);
		continueButton.SetActive (true);
	}
}