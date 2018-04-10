using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public GameObject exitPrefab;
	public string nextLevel = "0";
	public float changeLevelTime = 1f;
	public static GameManager instance = null;
	public static bool m_isWeb = false;
	public AudioSource deniedSfx;
	public GameObject emotionColorsPrefab;
	public static bool initialCallMade = false;
	public float enableInputDelay = 0f;

	// ═══╣ properties ╠═══
	bool m_hasLevelStarted = false;
	public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }

	bool m_isGamePlaying = false;
	public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }

	bool m_hasLevelFinished;
	public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }

	// bool m_isGameOver;
	// public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

	static int m_closeCount = -1;
	public int CloseCount { get { return m_closeCount; } }

	static Emotion m_emotion;
	public Emotion Emotion { get { return m_emotion; } }

	Color m_developerColor = Color.magenta;
	public Color DeveloperColor { get { return m_developerColor; } }

	// ═══╣ privates ╠═══
	int points;
	Node exitNode;
	bool m_ignoreInput;
	PlayerManager m_player;
	EmotionColors m_emotionColors;
	static bool m_emotionPicked = false;

	// https://docs.unity3d.com/560/Documentation/Manual/webgl-interactingwithbrowserscripting.html
	[DllImport ("__Internal")]
	static extern void CheckRecord ();
	[DllImport ("__Internal")]
	static extern void PutInList (int id, string color);
	[DllImport ("__Internal")]
	static extern void SetBackgroundColor (string color);

	// ═══╣ methods ╠═══
	void Awake ()
	{
		if (instance == null) instance = this;
		else if (instance != this) Destroy (gameObject);

		// DontDestroyOnLoad(...) >> when a Scene is loaded, normally all the game objects in it are destroyed
		//                           this method allow as to prevent that behavior
		// DontDestroyOnLoad (gameObject);

		m_isWeb = true;
		m_ignoreInput = false;
		m_player = Object.FindObjectOfType<PlayerManager> ();
		if (emotionColorsPrefab)
		{
			m_emotionColors = emotionColorsPrefab.GetComponent<EmotionColors> ();
		}

		if (exitPrefab)
		{
			exitNode = exitPrefab.GetComponent<Node> ();
		}

		// if (!m_emotionPicked)
		// {
		// 	PickEmotion ();
		// }
	}

	void Start ()
	{
		try
		{
			if (!initialCallMade)
			{
				CheckRecord ();
			}
		}
		catch (System.EntryPointNotFoundException)
		{
			m_isWeb = false;
		}
		finally
		{
			if (m_player != null)
			{
				// use the game loop only in Level scenes, the others don't need this shit
				StartCoroutine (RunGameLoop ());
			}
		}
	}

	IEnumerator RunGameLoop ()
	{
		yield return StartCoroutine ("StartLevelRoutine");
		yield return StartCoroutine ("PlayLevelRoutine");
		yield return StartCoroutine ("EndLevelRoutine");
	}

	IEnumerator StartLevelRoutine ()
	{
		Debug.Log ("Start level");
		m_player.playerInput.InputEnabled = false;
		m_ignoreInput = true;

		while (!m_hasLevelStarted)
		{
			// wait for the level manager to notify the level is playable
			yield return null;
		}
	}

	IEnumerator PlayLevelRoutine ()
	{
		Debug.Log ("Play level");
		m_isGamePlaying = true;
		// yield return new WaitForSeconds (enableInputDelay);
		m_player.playerInput.InputEnabled = true;
		m_ignoreInput = false;

		// trigger any events related with the play start
		// ...

		while (m_isGamePlaying)
		{
			yield return null;
		}
	}

	// end stage after gameplay is complete (exit reached, required points achieved)
	IEnumerator EndLevelRoutine ()
	{
		Debug.Log ("End level");
		m_player.playerInput.InputEnabled = false;
		m_ignoreInput = true;

		while (!m_hasLevelFinished)
		{
			yield return null;
		}

		if (nextLevel != "0")
		{
			StartCoroutine (ChangeScene ("Level " + nextLevel));
		}
		else
		{
			StartCoroutine (ChangeScene ("FirstEnd"));
		}
	}

	public void Rejected (bool rejected)
	{
		if (rejected)
		{
			// if the player tryed to quit the game, the first emotion to talk will be Anger
			m_closeCount = 1;
			m_emotion = Emotion.ANGER;
			m_emotionPicked = true;
		}
		else
		{
			// pick an emotion between Joy and Surprise
			PickEmotion (1);
		}

		StartCoroutine (ChangeScene ("Monologue"));
	}

	public void UpdatePoints (int value)
	{
		points += value;
		exitNode.UpdatePoints (points);
	}

	public void ExitReached ()
	{
		if (GoalAchieved ())
		{
			m_isGamePlaying = false;
		}
		else if (deniedSfx)
		{
			deniedSfx.Play ();
		}
	}

	public bool GoalAchieved ()
	{
		return points == exitNode.nodeValue;
	}

	IEnumerator ChangeScene (string sceneName)
	{
		yield return new WaitForSeconds (changeLevelTime);
		GoToScene (sceneName);
		yield return null;
	}

	public void GoToScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	public bool IgnoreInput ()
	{
		return m_ignoreInput;
	}

	public void PickEmotion (int start = 0)
	{
		m_emotion = (Emotion) Random.Range (start, System.Enum.GetValues (typeof (Emotion)).Length);
		m_emotionPicked = true;
	}

	public void EndGame ()
	{
		PickEmotion ();
		Color emotionColor = m_emotionColors.getEmotionColor (m_emotion);
		string hexEmotionColor = ColorUtility.ToHtmlStringRGB (emotionColor);
		if (m_isWeb)
		{
			// tanks: https://docs.unity3d.com/560/Documentation/Manual/webgl-interactingwithbrowserscripting.html
			// call the JavaScript function that will store the data in LocalStorage
			// so the next time the player enters the game, she will be aware of it
			PutInList (m_emotion.GetHashCode (), hexEmotionColor);
		}
		else
		{
			StartCoroutine (CloseGame ());
		}
	}

	IEnumerator CloseGame ()
	{
		yield return new WaitForSeconds (5f);
		Debug.Log ("Here's when the game closes");
		Application.Quit ();
	}

	public void RestartLevel ()
	{
		// Scene scene = SceneManager.GetActiveScene ();
		// SceneManager.LoadScene (scene.name);
		points = 0;
		exitNode.UpdatePoints (points);
	}

	public void PlayLevel ()
	{
		m_hasLevelStarted = true;
	}

	public bool IsWeb ()
	{
		return m_isWeb;
	}

	public Color GetCurrentColor ()
	{
		return m_emotionColors.getEmotionColor (m_emotion);
	}

	public bool HasPickedEmotion ()
	{
		return m_emotionPicked;
	}

	public void ChangeBackgroundInBrowser ()
	{
		if (m_isWeb)
		{
			SetBackgroundColor (ColorUtility.ToHtmlStringRGB (GetCurrentColor ()));
		}
	}
}