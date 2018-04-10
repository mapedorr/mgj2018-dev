using System.Collections;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
	[System.Serializable]
	public struct dialogueLine
	{
		public string[] line;
		public string[] buttonText;
	}

	public dialogueLine[] rows;
}