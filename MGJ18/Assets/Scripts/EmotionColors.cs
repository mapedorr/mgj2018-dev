using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionColors : MonoBehaviour
{
	public Color anger;
	public Color joy;
	public Color surprise;

	public Color getEmotionColor (Emotion index)
	{
		switch (index)
		{
			case Emotion.ANGER:
				return anger;
			case Emotion.JOY:
				return joy;
			case Emotion.SURPRISE:
				return surprise;
			default:
				return Color.white;
		}
	}
}