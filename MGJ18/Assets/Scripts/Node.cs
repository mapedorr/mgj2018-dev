using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public GameObject linkPrefab;
	public GameObject nodeValuePrefab;
	public int nodeValue = 1;
	public bool isExit = false;
	public bool disabled = false;

	// ═══╣ properties ╠═══
	Vector2 m_coordinate;
	public Vector2 Coordinate { get { return Utility.Vector2Round (m_coordinate); } }

	List<Node> m_neighborNodes = new List<Node> ();
	public List<Node> NeighborNodes { get { return m_neighborNodes; } }

	List<Node> m_linkedNodes = new List<Node> ();
	public List<Node> LinkedNodes { get { return m_linkedNodes; } }

	Color m_visitedColor;
	Color m_nodeValuePrefabColor;
	public Color VisitedColor
	{
		get { return m_visitedColor; }
		set { m_visitedColor = value; }
	}

	bool m_visited;
	public bool Visited
	{
		get { return m_visited; }
		set
		{
			m_visited = value;
			if (SpriteRenderer)
			{
				if (m_visited)
				{
					SpriteRenderer.color = m_visitedColor;
					m_nodeValueText.color = m_visitedColor;
				}
				else
				{
					SpriteRenderer.color = Color.white;
					m_nodeValueText.color = m_nodeValuePrefabColor;
				}
			}
			if (!isExit)
			{
				// update the amount of points for the player
				if (m_visited)
				{
					m_gameManager.UpdatePoints (nodeValue);
				}
				else
				{
					m_gameManager.UpdatePoints (nodeValue * -1);
				}
			}
		}
	}

	SpriteRenderer m_spriteRenderer;
	public SpriteRenderer SpriteRenderer { get { return m_spriteRenderer; } }

	// ═══╣ privates ╠═══
	GameManager m_gameManager;
	BoardManager m_board;
	Text m_nodeValueText;

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();
		m_board = Object.FindObjectOfType<BoardManager> ();
		m_coordinate = new Vector2 (transform.position.x, transform.position.y);
		m_spriteRenderer = GetComponent<SpriteRenderer> ();
		m_visitedColor = m_gameManager.DeveloperColor;

		if (disabled)
		{
			SpriteRenderer.color = new Color (1f, 1f, 1f, 0.5f);
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (m_board != null)
		{
			m_neighborNodes = FindNeighbors (m_board.AllNodes);
			foreach (Node neighborNode in m_neighborNodes)
			{
				if (!disabled && !neighborNode.disabled && !m_linkedNodes.Contains (neighborNode))
				{
					LinkNode (neighborNode);
				}
			}
		}

		if (nodeValuePrefab != null && !disabled)
		{
			nodeValuePrefab = Instantiate (nodeValuePrefab);
			nodeValuePrefab.transform.SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);
			nodeValuePrefab.transform.position = Camera.main.WorldToScreenPoint (transform.position + Vector3.up + Vector3.left * 0.5f);
			m_nodeValueText = nodeValuePrefab.GetComponent<Text> ();
			m_nodeValuePrefabColor = m_nodeValueText.color;

			if (isExit)
			{
				m_nodeValueText.text = "0/" + nodeValue;
				m_nodeValueText.color = Color.magenta;
			}
			else
			{
				m_nodeValueText.text = "" + nodeValue;
			}
		}
	}

	public List<Node> FindNeighbors (List<Node> nodes)
	{
		List<Node> nList = new List<Node> ();

		foreach (Vector2 dir in BoardManager.directions)
		{
			// by using the property Coordinate, we guarantee only rounded values will
			// be used
			Node foundNeighbor = nodes.Find (n => n.Coordinate == Coordinate + dir);

			if (foundNeighbor != null && !foundNeighbor.disabled && !nList.Contains (foundNeighbor))
			{
				nList.Add (foundNeighbor);
			}
		}

		return nList;
	}

	void LinkNode (Node targetNode)
	{
		if (linkPrefab != null)
		{
			GameObject linkInstance = Instantiate (linkPrefab, transform.position, Quaternion.identity);
			linkInstance.transform.parent = transform;

			Link link = linkInstance.GetComponent<Link> ();
			if (link != null)
			{
				link.DrawLink (Coordinate, targetNode.Coordinate);
			}

			if (!m_linkedNodes.Contains (targetNode))
			{
				m_linkedNodes.Add (targetNode);
			}

			if (!targetNode.LinkedNodes.Contains (this))
			{
				targetNode.LinkedNodes.Add (this);
			}
		}
	}

	public void UpdatePoints (int points)
	{
		m_nodeValueText.text = points + "/" + nodeValue;
	}
}