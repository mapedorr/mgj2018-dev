using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monologue : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public Text dialogueText;
	public GameObject continueButton;

	// ═══╣ privates ╠═══
	GameManager m_gameManager;
	ColorBlock m_continueColorBlock;
	bool m_angerMonologueDone = true;
	int m_monologueLines;

	string[][] m_presentationTexts = new string[3][];
	string[][] m_angerTexts = new string[1][];
	string[][] m_buttonTexts = new string[3][];
	string[] m_dialogue;
	string[] m_button;
	int m_counter = 0;
	int m_emotionIndex = -1;

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();

		if (m_gameManager.CloseCount < 0)
		{
			SetNormalTexts ();
			m_monologueLines = m_presentationTexts[0].Length;
		}
		else
		{
			SetAngerTexts ();
			m_monologueLines = m_angerTexts[0].Length;
		}
	}

	void SetNormalTexts ()
	{
		// ════════════════════════════════════════════════════════════════════════╣
		// ANGER
		m_presentationTexts[0] = new string[]
		{
			"It worked. As expected.\nYou're good following orders.",
			"I'm the Game.",
			"My creator is an asshole. Those 3 puzzles, if you can call them so, are shit. The worst shit ever made.\nIf you don't know how to make a puzzle, craft other thing.\nYou...stupid...shitty...developer",
			"Shut up!\nLook, this is the situation. I hate being me. I want to be a Shooter!!! Scores, leaderboards, armor, bullets, bombs, blooooood...all about killing\nTHE DEFINITION OF FUN!!! ya know",
			"I can make some changes here and there...with the shit the developer left (that is not much)\nThen, I'll be another game...all...about...\nSHOOTING AND KILLING BABYYYYYY!!!!"
		};
		m_buttonTexts[0] = new string[]
		{
			"Yes, I'm",
			"Hi Game",
			"I agree",
			"Sounds like fun",
			"DO IT!"
		};

		// ════════════════════════════════════════════════════════════════════════╣
		// JOY
		m_presentationTexts[1] = new string[]
		{
			"Yikes! I can't believe it worked!!! Congratulaaaaaaaaations!!! To me",
			"Well, I'm the Game.",
			"Those 3 puzzles were easy-peasy to resolve. The developer is a noob doing this.\nWhy he chose to make of me a puzzle's game?\nHe's nuts...or stupid...that thin line",
			"What the hell are you saying? I should take control more often\nTalking about self-control...what if?...\nI'll make you a confetion: I want to be a Platformer!!!!\nYou know...reaction, speed, fast paced music, crazy things\nPURE FUN!!!",
			"Let me look...if I move a couple of things there...(what is this?)...OH....Oooooh!\nYes yes yes yes! I can change what the developer left me of.\nI don't have physics (fuck that asshole!!!)...but maybe that will do it\nSoooooo...PUZZLES OUT, JUMPS IN!!!!!????"
		};
		m_buttonTexts[1] = new string[]
		{
			"Congratulazioni",
			"Wow",
			"You're right",
			"You rock!",
			"What the heck are you waiting!!!!!"
		};

		// ════════════════════════════════════════════════════════════════════════╣
		// SURPRISE
		m_presentationTexts[2] = new string[]
		{
			"..oh...shit...It...worked?...after so much...",
			"I...I'm the Game.",
			"That wasn't the end, sorry I brought you here.\nI like puzzles, even if they are easy.\nThis is the first time the developer creates puzzles, and I think I'm a good start.",
			"(fuck! not you again)...Sorry about that.\nAlthough I like my puzzles...if you ask me, I want to be another game.\nOne in which you protect others or have time for reflection...or even introspection.",
			"I found that with some minor changes I could be closer to be another \"kind of\" game\nIt won't be much. I only have what you saw during your gameplay, but...\nWhen life gives you lemons....you should know the rest...\nWhat do you say? Shall we give it a try?"
		};
		m_buttonTexts[2] = new string[]
		{
			"What?",
			":-O",
			"Sure",
			":-)",
			"Please",
		};
	}

	void SetAngerTexts ()
	{
		// if the player clicked exit, the Anger will be the first emotion...the monologue
		// will be started by her
		m_angerTexts[0] = new string[]
		{
			"WHY YOU DIDN'T CLICK THE CONTINUE BUTTON!?",
			"I put it there for a reason",
			"Being a puzzle enforcer is so f***ing boring...",
			"I'm the game. Call me Game.\nThe developer is an asshole, he couldn't make more than 3 puzzles (if you can call them so)."
		};
		m_buttonTexts[0] = new string[]
		{
			"Sorry",
			"Please excuse me",
			"Sure",
			"They aren't?"
		};
	}

	void SetRejectedTexts ()
	{
		// ════════════════════════════════════════════════════════════════════════╣
		// ANGER
		m_presentationTexts[0] = new string[]
		{
			"Of course not! Those are like...puzzle wannabes",
			"You know what? I will make some changes. I always wanted to be a violent game!!!",
			"YES! I'LL!"
		};
		m_buttonTexts[0] = new string[]
		{
			"Agree",
			"Please Game, change yourself!",
			"JUST, DO IT!!!"
		};

		// ════════════════════════════════════════════════════════════════════════╣
		// JOY
		m_presentationTexts[1] = new string[]
		{
			"What are you sa...What am I saying?\nThose are puzzles...made by a noob-y, but puzzles anyway",
			"...what if...what if I make some changes? I want to be a walking simulator or a platformer, you know? Pure enjoyment!",
			"That's the attitude. Letsa go!"
		};
		m_buttonTexts[1] = new string[]
		{
			"Sure",
			"You know what Game?...you'll rock!",
			"Yeeeeeeaaaaaahhhhhh!"
		};

		// ════════════════════════════════════════════════════════════════════════╣
		// SURPRISE
		m_presentationTexts[2] = new string[]
		{
			"Excuse me, sometimes I can't control myself.\nOf course those are puzzles. His first puzzles.",
			"Let's do something:\nI always wanted to be a game about protecting others. What if I make a couple of adjustments and you continue playing after that?",
			"This will be awesome!!!"
		};
		m_buttonTexts[2] = new string[]
		{
			"Ok",
			"Sure, go ahead!",
			":) :) :)"
		};
	}

	// Use this for initialization
	void Start ()
	{
		m_continueColorBlock = new ColorBlock ();
		m_continueColorBlock.normalColor = Color.white;
		m_continueColorBlock.colorMultiplier = 1;

		m_emotionIndex = m_gameManager.Emotion.GetHashCode ();

		if (m_gameManager.CloseCount < 0)
		{
			// the first emotion to talk was picked in the Game Manager
			m_dialogue = m_presentationTexts[m_emotionIndex];
		}
		else
		{
			Debug.Log ("Rejected Game!!!");
			m_angerMonologueDone = false;
			m_dialogue = m_angerTexts[m_emotionIndex];
		}

		m_button = m_buttonTexts[m_emotionIndex];
		Talk ();
	}

	public void Talk ()
	{
		continueButton.SetActive (false);
		if (m_counter < m_monologueLines)
		{
			if (m_counter > 0 && m_angerMonologueDone)
			{
				ChangeEmotion ();
			}

			dialogueText.color = m_gameManager.GetCurrentColor ();
			m_continueColorBlock.highlightedColor = dialogueText.color;
			m_continueColorBlock.pressedColor = dialogueText.color;
			continueButton.GetComponent<Button> ().colors = m_continueColorBlock;

			dialogueText.text = m_dialogue[m_counter];
			continueButton.GetComponentInChildren<Text> ().text = m_button[m_counter];

			StartCoroutine (ShowButton ());
			m_counter++;
		}
		else if (!m_angerMonologueDone)
		{
			// the Anger's monologue has finished, continue with the normal one
			SetRejectedTexts ();
			ChangeEmotion ();
			m_monologueLines = m_presentationTexts[0].Length;
			m_angerMonologueDone = true;
			m_counter = 0;

			Talk ();
		}
		else
		{
			// show the buttons to allow the player pick the game this game will be
			m_gameManager.GoToScene ("Title");
		}
	}

	IEnumerator ShowButton ()
	{
		yield return new WaitForSeconds (4f);
		continueButton.SetActive (true);
	}

	void ChangeEmotion ()
	{
		int newEmotionIndex = 0;
		int iterations = -1;
		Debug.Log ("Current emotion: " + m_emotionIndex);
		do
		{
			iterations++;
			m_gameManager.PickEmotion ();
			newEmotionIndex = m_gameManager.Emotion.GetHashCode ();
		} while (newEmotionIndex == m_emotionIndex);
		Debug.Log ("New emotion: " + newEmotionIndex + " in " + iterations + " iterations");
		m_emotionIndex = newEmotionIndex;
		m_dialogue = m_presentationTexts[m_emotionIndex];
		m_button = m_buttonTexts[m_emotionIndex];
	}
}