using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public GameObject creditsPanel;
	public Text thanksText;
	public Text madeByText;
	public Text playAgainText;
	public Button exitBtn;
	public Button continueBtn;
	public GameObject emotionColorsPrefab;

	// ═══╣ privates ╠═══
	GameManager m_gameManager;

	void Awake ()
	{
		HideCredits ();
		playAgainText.gameObject.SetActive (false);
		m_gameManager = Object.FindObjectOfType<GameManager> ();
	}

	void HideCredits ()
	{
		thanksText.gameObject.SetActive (false);
		madeByText.gameObject.SetActive (false);
		exitBtn.gameObject.SetActive (false);
		continueBtn.gameObject.SetActive (false);
	}

	void Start ()
	{
		StartCoroutine (ShowEndScene ());
	}

	IEnumerator ShowEndScene ()
	{

		yield return new WaitForSeconds (1f);
		// show the texts
		thanksText.gameObject.SetActive (true);

		yield return new WaitForSeconds (2f);
		madeByText.gameObject.SetActive (true);

		yield return new WaitForSeconds (2f);
		// show the buttons
		exitBtn.gameObject.SetActive (true);

		yield return null;
	}

	public void ContinueGame (bool rejected)
	{
		continueBtn.gameObject.SetActive (false);
		exitBtn.gameObject.SetActive (false);

		m_gameManager.Rejected (rejected);
	}

	public void ShowContinue ()
	{
		continueBtn.gameObject.SetActive (true);
	}
}