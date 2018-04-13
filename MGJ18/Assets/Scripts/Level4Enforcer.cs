using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Enforcer : MonoBehaviour
{
	GameManager m_gameManager;
	PlayerManager m_playerManager;
	BoardManager m_boardManager;
	LevelManager m_levelManager;
	float m_startTime;
	float m_endTime;
	int m_lastNodeValue;

	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();
		m_boardManager = Object.FindObjectOfType<BoardManager> ();
		m_playerManager = Object.FindObjectOfType<PlayerManager> ();
		m_levelManager = transform.GetComponent<LevelManager> ();
	}

	public void PlayStartedReaction ()
	{
		if (m_gameManager)
		{
			if (m_gameManager.Emotion == Emotion.JOY)
			{
				m_levelManager.Shout ("GO!!!");
				m_startTime = Time.fixedTime;
			}
		}
	}

	public void HitReaction ()
	{
		if (m_gameManager && m_boardManager)
		{
			if (m_gameManager.Emotion == Emotion.ANGER)
			{
				Node playerNode = m_boardManager.FindPlayerNode ();
				if (playerNode.Hits <= playerNode.nodeValue)
				{
					// only when the player hits the walls, points are added
					m_lastNodeValue = playerNode.nodeValue;
					m_gameManager.UpdatePoints (1);
				}
				else
				{
					m_levelManager.Shout ("You think I'm stupid!!!???");
				}
			}
		}
	}

	public void UndoReaction () { }

	public void RestartReaction ()
	{
		if (m_gameManager.Emotion == Emotion.JOY)
		{
			m_startTime = Time.fixedTime;
		}
	}

	public void EndMovementReaction ()
	{
		if (m_gameManager)
		{
			if (m_gameManager.Emotion == Emotion.ANGER)
			{
				List<Node> steps = m_playerManager.GetSteps ();
				m_lastNodeValue = steps[steps.Count - 1].nodeValue;
				m_gameManager.UpdatePoints (-m_lastNodeValue);
			}
		}
	}

	public void EndReachedReaction ()
	{
		if (m_gameManager.Emotion == Emotion.ANGER)
		{
			if (!m_gameManager.GoalAchieved ())
			{
				m_levelManager.Shout ("Kill 'em!!!");
				m_playerManager.Restart ();
			}
		}
		else if (m_gameManager.Emotion == Emotion.JOY)
		{
			m_endTime = Time.fixedTime;
			if ((m_endTime - m_startTime) > m_levelManager.level.expectedTime)
			{
				m_gameManager.UpdatePoints (-m_gameManager.Points);
				m_levelManager.Shout ("You're late!!!");
				m_playerManager.Restart ();
			}
		}
		else if (m_gameManager.Emotion == Emotion.SURPRISE)
		{
			// compare the expected steps with the steps made by the player
			int[] stepsNumbers = new int[m_playerManager.GetSteps ().Count];
			int counter = 0;
			foreach (var node in m_playerManager.GetSteps ())
			{
				stepsNumbers[counter++] = int.Parse (node.name.Substring (5));
			}

			bool failed = false;
			if (m_levelManager.level.expectedPath.Length == stepsNumbers.Length)
			{
				for (int i = 0; i < m_levelManager.level.expectedPath.Length; i++)
				{
					if (m_levelManager.level.expectedPath[i] != stepsNumbers[i])
					{
						failed = true;
						break;
					}
				}
			}
			else
			{
				failed = true;
			}

			if (failed)
			{
				m_gameManager.UpdatePoints (-m_gameManager.Points);
				m_levelManager.Shout ("You took the wrong path!!!");
				m_playerManager.Restart ();
			}
		}
	}
}