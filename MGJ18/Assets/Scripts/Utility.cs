using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utility
{
	/// <summary>
	/// Keep of any weird fractions from operations with 2D vectors
	/// </summary>
	/// <param name="inputVector"></param>
	/// <returns></returns>
	public static Vector2 Vector2Round (Vector2 inputVector)
	{
		return new Vector2 (Mathf.Round (inputVector.x), Mathf.Round (inputVector.y));
	}

	public static IEnumerator HighlightGUIObject (MaskableGraphic button, Color color)
	{
		if (button)
		{
			button.color = color;
			yield return new WaitForSeconds (0.2f);
			button.color = Color.white;
		}

		yield return null;
	}
}