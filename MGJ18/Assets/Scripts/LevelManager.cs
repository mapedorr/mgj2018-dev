using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class in charge of displaying the texts for the current emotion, set the
/// level title and handle some behaviors between The game and the level
/// </summary>
public class LevelManager : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public Level level;
	public Text titleText;
	public Text dialogueText;
	public MaskableGraphic talkButton;
	public AudioSource talkSound;

	// ═══╣ privates ╠═══
	GameManager m_gameManager;
	BoardManager m_boardManager;
	PlayerManager m_playerManager;
	string[][] m_allDialogues;
	string[] m_dialogue;
	int m_emotionId;
	Color m_emotionColor;
	int m_counter;
	bool m_talkingIntro;
	bool m_talkingEnd;

	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();
		m_boardManager = Object.FindObjectOfType<BoardManager> ();
		m_playerManager = Object.FindObjectOfType<PlayerManager> ();
		m_allDialogues = new string[3][];
		m_emotionId = -1;
		m_emotionColor = Color.white;
		m_counter = 0;
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.E))
		{
			TalkAction ();
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (m_gameManager)
		{
			if (!m_gameManager.HasPickedEmotion ())
			{
				m_gameManager.PickEmotion ();
			}

			m_emotionId = m_gameManager.Emotion.GetHashCode ();
			m_emotionColor = m_gameManager.GetCurrentColor ();
		}
		else
		{
			// without Game Manager, there's nothing to do
			return;
		}

		if (level)
		{
			m_allDialogues[0] = level.introAnger;
			m_allDialogues[1] = level.introJoy;
			m_allDialogues[2] = level.introSurprise;
		}
		else
		{
			// intro level, let the player play
			StartPlay ();
			return;
		}

		if (m_emotionId >= 0 && m_emotionColor != Color.white)
		{
			// setup the objects on the scene based on the current emotion
			if (titleText && level)
			{
				titleText.text = level.title[m_emotionId];
				titleText.color = m_emotionColor;
			}

			if (dialogueText)
			{
				dialogueText.color = m_emotionColor;
			}

			if (m_playerManager)
			{
				m_playerManager.ButtonColor = m_emotionColor;
			}

			if (m_boardManager)
			{
				foreach (var node in m_boardManager.AllNodes)
				{
					if (!node.disabled)
					{
						node.VisitedColor = m_emotionColor;
					}
				}
			}

			// get the texts for the dialogue
			m_dialogue = m_allDialogues[m_emotionId];
			if (m_dialogue != null && m_dialogue.Length > 0)
			{
				m_counter = 0;
				dialogueText.text = "";

				m_talkingIntro = true;
				Talk ();
			}
			else
			{
				StartPlay ();
			}
		}
	}

	void StartPlay ()
	{
		m_gameManager.HasLevelStarted = true;
	}

	public void TalkAction ()
	{
		if (!talkButton)
		{
			return;
		}

		if (talkSound != null)
		{
			talkSound.Play ();
		}

		StartCoroutine (Utility.HighlightGUIObject (talkButton, m_emotionColor));
		if (m_dialogue != null && m_dialogue.Length > 0)
		{
			Talk ();
		}
	}

	void Talk ()
	{
		if (m_counter <= m_dialogue.Length - 1)
		{
			dialogueText.text += m_dialogue[m_counter++] + "\n";

			if (m_counter == m_dialogue.Length)
			{
				if (m_talkingIntro)
				{
					m_talkingIntro = false;

					// no more intro lines, let the player play
					StartPlay ();
				}
			}
		}
		else if (m_talkingEnd)
		{
			m_talkingEnd = false;

			// no more end lines, let the player pass to the next level
			EndLevel ();
		}
	}

	public void HitReaction () { }
	public void UndoReaction () { }
	public void RestartReaction () { }

	public void EndReachedReaction ()
	{
		// look if the game want to say something before it loads the next level
		if (level)
		{
			m_allDialogues[0] = level.exitReachedAnger;
			m_allDialogues[1] = level.exitReachedJoy;
			m_allDialogues[2] = level.exitReachedSurprise;
		}

		// get the texts for the dialogue
		m_dialogue = m_allDialogues[m_emotionId];
		if (m_dialogue != null && m_dialogue.Length > 0)
		{
			m_counter = 0;
			dialogueText.text = "";

			m_talkingEnd = true;

			// show the texts
			Talk ();
		}
		else
		{
			EndLevel ();
		}
	}

	void EndLevel ()
	{
		Debug.Log ("fuuuuck");
		// make the next level to be loaded
		m_gameManager.HasLevelFinished = true;
	}
}