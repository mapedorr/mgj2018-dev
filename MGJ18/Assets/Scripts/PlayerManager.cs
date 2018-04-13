using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// in order to function, the PlayerManager will need the PlayerMover and the
// PlayerInput. With RequireComponent both classes are attached to the PlayerManager's class.
[RequireComponent (typeof (PlayerMover))]
[RequireComponent (typeof (PlayerInput))]
public class PlayerManager : MonoBehaviour
{

	// ═══╣ publics ╠═══
	public PlayerMover playerMover;
	public PlayerInput playerInput;
	public MaskableGraphic leftButton;
	public MaskableGraphic rightButton;
	public MaskableGraphic upButton;
	public MaskableGraphic downButton;
	public MaskableGraphic undoButton;
	public MaskableGraphic restartButton;
	public AudioSource undoSfx;
	public AudioSource restartSfx;
	// events for broadcasting
	public UnityEvent endMovementEvent;
	public UnityEvent hitEvent;
	public UnityEvent undoEvent;
	public UnityEvent restartEvent;
	public UnityEvent endReachedEvent;

	// ═══╣ privates ╠═══
	GameManager m_gameManager;
	BoardManager m_boardManager;
	List<Node> m_steps = new List<Node> ();

	Color m_buttonColor;
	public Color ButtonColor
	{
		get { return m_buttonColor; }
		set { m_buttonColor = value; }
	}

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_gameManager = Object.FindObjectOfType<GameManager> ();
		m_boardManager = Object.FindObjectOfType<BoardManager> ();
		playerMover = GetComponent<PlayerMover> ();
		playerInput = GetComponent<PlayerInput> ();
		playerInput.InputEnabled = true;
		m_buttonColor = m_gameManager.DeveloperColor;
	}

	void Start ()
	{
		if (playerMover)
		{
			playerMover.SetPlayerManager (this);
		}
	}

	void Update ()
	{
		if (m_gameManager.IgnoreInput ())
		{
			ToggleGUIActions (false);
			return;
		}
		else
		{
			ToggleGUIActions (true);
		}

		// check each frame for player input to make sure there won't be a delay
		// beteween the action and the game's response
		if (playerMover.isMoving)
		{
			return;
		}

		playerInput.GetKeyInput ();

		int horizontal = (int) playerInput.H;
		int vertical = (int) playerInput.V;

		if (horizontal != 0) vertical = 0;

		if (horizontal != 0 || vertical != 0)
		{
			MoveInDirection (Utility.Vector2Round (new Vector2 (horizontal, vertical)));
		}
		else if (playerInput.ButtonZ)
		{
			Undo ();
		}
		else if (playerInput.ButtonR)
		{
			Restart ();
		}
	}

	public void MoveLeft ()
	{
		MoveInDirection (Utility.Vector2Round (new Vector2 (-1f, 0f)));
	}

	public void MoveRight ()
	{
		MoveInDirection (Utility.Vector2Round (new Vector2 (1f, 0f)));
	}

	public void MoveDown ()
	{
		MoveInDirection (Utility.Vector2Round (new Vector2 (0f, -1f)));
	}

	public void MoveUp ()
	{
		MoveInDirection (Utility.Vector2Round (new Vector2 (0f, 1f)));
	}

	public void Undo ()
	{
		if (m_steps.Count > 0)
		{
			if (undoSfx)
			{
				undoSfx.Play ();
			}

			StartCoroutine (Utility.HighlightGUIObject (undoButton, m_buttonColor));

			Node currentNode = m_boardManager.FindPlayerNode ();
			if (currentNode.Hits >= 1)
			{
				currentNode.Hits = 0;
				m_gameManager.UpdatePoints (-1);
			}

			// get the previous node
			int last = m_steps.Count - 1;
			Node previousNode = m_steps[last];

			// move the player to the previous node, mark it as not visited and
			// update the game score
			transform.position = previousNode.Coordinate;
			playerMover.UpdateBoard ();
			previousNode.Visited = false;

			// remove the node from the steps list
			m_steps.RemoveAt (last);
		}

		if (undoEvent != null)
		{
			undoEvent.Invoke ();
		}
	}

	public void Restart ()
	{
		// if (m_steps.Count > 0)
		// {
		if (restartSfx)
		{
			restartSfx.Play ();
		}

		StartCoroutine (Utility.HighlightGUIObject (restartButton, m_buttonColor));

		if (m_steps.Count > 0)
		{
			transform.position = m_steps[0].Coordinate;
			m_steps = new List<Node> ();
		}

		foreach (var node in m_boardManager.AllNodes)
		{
			if (!node.disabled)
			{
				node.Visited = false;
			}
		}
		playerMover.UpdateBoard ();
		m_gameManager.RestartLevel ();
		// }

		if (restartEvent != null)
		{
			restartEvent.Invoke ();
		}
	}

	void MoveInDirection (Vector2 direction)
	{
		if (playerMover.isMoving || m_gameManager.IgnoreInput ())
		{
			return;
		}

		if (direction.x < 0)
		{
			StartCoroutine (Utility.HighlightGUIObject (leftButton, m_buttonColor));
		}
		else if (direction.x > 0)
		{
			StartCoroutine (Utility.HighlightGUIObject (rightButton, m_buttonColor));
		}
		else if (direction.y < 0)
		{
			StartCoroutine (Utility.HighlightGUIObject (downButton, m_buttonColor));
		}
		else if (direction.y > 0)
		{
			StartCoroutine (Utility.HighlightGUIObject (upButton, m_buttonColor));
		}

		Node previousNode = playerMover.AttemptMove (direction);
		if (previousNode)
		{
			// store the previous node
			m_steps.Add (previousNode);
		}
	}

	// void OnTriggerEnter2D (Collider2D other)
	// {
	// 	if (other.tag == "Exit")
	// 	{
	// 		m_gameManager.ExitReached ();
	// 		if (endReachedEvent != null)
	// 		{
	// 			endReachedEvent.Invoke ();
	// 		}
	// 	}
	// }

	void ToggleGUIActions (bool interactable)
	{
		leftButton.GetComponentInParent<Button> ().interactable = interactable;
		rightButton.GetComponentInParent<Button> ().interactable = interactable;
		upButton.GetComponentInParent<Button> ().interactable = interactable;
		downButton.GetComponentInParent<Button> ().interactable = interactable;
		undoButton.GetComponentInParent<Button> ().interactable = interactable;
		restartButton.GetComponentInParent<Button> ().interactable = interactable;
	}

	public void MovementFinished ()
	{
		if (endMovementEvent != null)
		{
			endMovementEvent.Invoke ();
		}

		// check if the current node is the exit
		if (m_boardManager)
		{
			if (m_boardManager.FindPlayerNode ().isExit)
			{
				if (endReachedEvent != null)
				{
					endReachedEvent.Invoke ();
				}
				m_gameManager.ExitReached ();
			}

		}

	}

	public void HitFinished ()
	{
		if (hitEvent != null)
		{
			hitEvent.Invoke ();
		}
	}

	public List<Node> GetSteps ()
	{
		return m_steps;
	}
}