using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public GameObject developScreen;
	public GameObject angerScreen;
	public GameObject joyScreen;
	public GameObject surpriseScreen;
	public GameObject playButton;
	public GameObject quitButton;

	// ═══╣ privates ╠═══
	GameManager m_gameManager;
	bool m_firstTime;
	Button m_playButton;

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();
		m_playButton = playButton.GetComponent<Button> ();

		// make all the possible title screens inactive by default
		developScreen.SetActive (false);
		angerScreen.SetActive (false);
		joyScreen.SetActive (false);
		surpriseScreen.SetActive (false);
	}

	// Use this for initialization
	void Start ()
	{
		if (m_gameManager)
		{
			// determine which screen might be shown and set the colors for the buttons
			if (m_gameManager.HasPickedEmotion ())
			{
				m_firstTime = false;

				// set the colors for the Play button
				Color emotionColor = m_gameManager.GetCurrentColor ();
				ColorBlock colorBlock = new ColorBlock ();
				colorBlock.normalColor = Color.white;
				colorBlock.colorMultiplier = 1;
				colorBlock.highlightedColor = emotionColor;
				colorBlock.pressedColor = emotionColor;
				m_playButton.colors = colorBlock;

				// change the color in the Web browser
				m_gameManager.ChangeBackgroundInBrowser ();

				switch (m_gameManager.Emotion)
				{
					case Emotion.ANGER:
						angerScreen.SetActive (true);
						break;
					case Emotion.JOY:
						joyScreen.SetActive (true);
						break;
					case Emotion.SURPRISE:
						surpriseScreen.SetActive (true);
						break;
					default:
						break;
				}
			}
			else
			{
				m_firstTime = true;

				// show the developers screen
				developScreen.SetActive (true);
			}
		}
	}

	public void Play ()
	{
		// give feedback to the player

		// determine which scene should be loaded
		if (m_firstTime)
		{
			m_gameManager.GoToScene ("Level 1");
		}
		else
		{
			m_gameManager.GoToScene ("Level 4");
		}
	}

	public void Quit ()
	{
		// give feedback to the player

		// quit the game
	}
}