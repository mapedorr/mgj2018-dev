using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
	// ═══╣ publics ╠═══
	public Vector2 destination;
	public bool isMoving = false;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
	public float moveSpeed = 1.5f;
	public float iTweenDelay = 0f;
	// the layer on which collisions will be checked to know if a space is open to move into
	public LayerMask blockingLayer;
	public AudioSource hitSfx;
	public AudioSource stepSfx;

	// ═══╣ properties ╠═══
	private Vector2 m_currentPos;
	public Vector2 CurrentPos { get { return m_currentPos; } set { m_currentPos = value; } }

	// ═══╣ privates ╠═══
	BoxCollider2D m_boxCollider;
	BoardManager m_board;
	PlayerManager m_playerManager;

	// ═══╣ methods ╠═══
	void Awake ()
	{
		m_board = Object.FindObjectOfType<BoardManager> ();
	}

	// Use this for initialization
	void Start ()
	{
		m_boxCollider = GetComponent<BoxCollider2D> ();
		UpdateBoard ();
	}

	public Node AttemptMove (Vector2 destinationDir)
	{
		RaycastHit2D hit;
		bool canMove = Move (destinationDir, out hit, 0);
		Node currentNode = m_board.GetNodeOnPos (new Vector2 (transform.position.x, transform.position.y));

		if (!canMove)
		{
			if (hit.transform == null)
			{
				StartCoroutine (WallHitFeedback (destinationDir, currentNode.transform.gameObject));
			}
			else
			{
				StartCoroutine (WallHitFeedback (destinationDir, hit.transform.gameObject));
			}

			return null;
		}
		else
		{
			currentNode.Visited = true;
			return currentNode;
		}
	}

	public bool Move (Vector2 destinationDir, out RaycastHit2D hit, float delayTime = 0.15f)
	{
		Vector2 start = transform.position;
		Vector2 end = start + destinationDir * BoardManager.spacing;

		// assure that the casted rays will not hit the collider of the object that is going to be moved
		m_boxCollider.enabled = false;

		// cast a line from the start point to the end point checking collisions in the blockingLayer
		hit = Physics2D.Linecast (start, end, blockingLayer);

		m_boxCollider.enabled = true;

		if (hit.transform == null)
		{
			Node targetNode = m_board.GetNodeOnPos (end);
			if (targetNode != null && !targetNode.Visited && !targetNode.disabled)
			{
				StartCoroutine (MoveRoutine (end, delayTime));
				return true;
			}
		}

		return false;
	}

	IEnumerator MoveRoutine (Vector2 destinationPos, float delayTime)
	{
		isMoving = true;
		// wait before the movement starts
		yield return new WaitForSeconds (delayTime);

		if (stepSfx)
		{
			stepSfx.Play ();
		}

		iTween.MoveTo (gameObject, iTween.Hash (
			"x", destinationPos.x,
			"y", destinationPos.y,
			"delay", iTweenDelay,
			"easetype", easeType,
			"speed", moveSpeed
		));

		while (Vector2.Distance (destinationPos, transform.position) > float.Epsilon)
		{
			// the player hasn't reach her destination yet
			yield return null;
		}

		iTween.Stop (gameObject);
		transform.position = destinationPos;

		isMoving = false;
		UpdateBoard ();

		// notify the end of this event
		if (m_playerManager != null)
		{
			m_playerManager.MovementFinished ();
		}
	}

	IEnumerator WallHitFeedback (Vector2 shakeDir, GameObject shakeTarget)
	{
		isMoving = true;
		Vector3 originalPosition = new Vector3 (transform.position.x, transform.position.y);
		transform.position += new Vector3 (shakeDir.x, shakeDir.y) * 0.08f;

		if (hitSfx)
		{
			hitSfx.Play ();
		}

		iTween.ShakePosition (Camera.main.gameObject, iTween.Hash (
			"x", shakeDir.x * 0.1f,
			"y", shakeDir.y * 0.1f,
			"time", 0.5f,
			"easetype", iTween.EaseType.easeOutBounce
		));

		iTween.ColorTo (shakeTarget, iTween.Hash (
			"r", 1f,
			"g", 0f,
			"b", 0f,
			"time", 0.1f,
			"easetype", iTween.EaseType.easeInOutExpo
		));

		yield return new WaitForSeconds (0.2f);

		transform.position = originalPosition;
		iTween.ColorTo (shakeTarget, iTween.Hash (
			"r", 1f,
			"g", 1f,
			"b", 1f,
			"time", 0.1f
		));
		isMoving = false;

		// notify the end of this event
		if (m_playerManager != null)
		{
			m_playerManager.HitFinished ();
		}

		yield return null;
	}

	void UpdateBoard ()
	{
		CurrentPos = Utility.Vector2Round (new Vector2 (transform.position.x, transform.position.y));

		if (m_board != null)
		{
			m_board.UpdatePlayerNode ();
		}
	}

	public void SetPlayerManager (PlayerManager pm)
	{
		m_playerManager = pm;
	}
}