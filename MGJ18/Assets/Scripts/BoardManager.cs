using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public static float spacing = 2f;
	public static readonly Vector2[] directions = {
		new Vector2 (spacing, 0f),
		new Vector2 (-spacing, 0f),
		new Vector2 (0f, spacing),
		new Vector2 (0f, -spacing),
	};
	public AudioSource levelStartSfx;

	// ═══╣ properties ╠═══
	List<Node> m_allNodes = new List<Node> ();
	public List<Node> AllNodes { get { return m_allNodes; } }

	Node m_playerNode;
	public Node PlayerNode { get { return m_playerNode; } }

	Node m_goalNode;
	public Node GoalNode { get { return m_goalNode; } }

	// ═══╣ privates ╠═══
	PlayerMover m_player;

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_player = Object.FindObjectOfType<PlayerMover> ();
		FillNodeList ();

		m_goalNode = FindGoalNode ();
	}

	/// <summary>
	/// Get all the nodes in the scene (level) and stores them in the m_allNodes property
	/// </summary>
	public void FillNodeList ()
	{
		Node[] nList = GameObject.FindObjectsOfType<Node> ();
		m_allNodes = new List<Node> (nList);

		if (levelStartSfx)
		{
			levelStartSfx.Play ();
		}
	}

	Node FindGoalNode ()
	{
		return m_allNodes.Find (n => n.isExit);
	}

	public Node GetNodeOnPos (Vector2 targetPos)
	{
		return m_allNodes.Find (n => n.Coordinate == Utility.Vector2Round (targetPos));
	}

	public Node FindPlayerNode ()
	{
		if (m_player != null && !m_player.isMoving)
		{
			return GetNodeOnPos (m_player.CurrentPos);
		}
		return null;
	}

	public void UpdatePlayerNode ()
	{
		m_playerNode = FindPlayerNode ();
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		if (m_playerNode != null)
		{
			Gizmos.DrawWireSphere (m_playerNode.transform.position, 1f);
		}
	}
}