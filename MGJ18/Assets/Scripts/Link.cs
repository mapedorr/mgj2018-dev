using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
	public void DrawLink (Vector2 startPos, Vector2 endPos)
	{
		transform.localScale = new Vector3 (BoardManager.spacing, 1f, 1f);
		Vector2 dir = endPos - startPos;
		if (dir.x < 0)
		{
			transform.Rotate (0f, 0f, -180f);
		}
		else if (dir.y != 0)
		{
			if (dir.y > 0)
			{
				transform.Rotate (0f, 0f, 90f);
			}
			else
			{
				transform.Rotate (0f, 0f, -90f);
			}
		}
		transform.position = new Vector3 (startPos.x, startPos.y, 0f);
	}
}