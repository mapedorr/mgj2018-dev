using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
	// ═══╣ publics ╠═══
	public string[] title;
	// lines where the player clicked on continue
	public string[] introAnger;
	public string[] introJoy;
	public string[] introSurprise;
	// lines where the player clicked on exit
	public string[] introRejectedAnger;
	public string[] introRejectedJoy;
	public string[] introRejectedSurprise;
	// the path the game wants the player to take
	public int[] expectedPath;
	// lines the game will say if the player hits
	// lines the game will say if the player undoes
	// lines the game will say if the player restarts
	// lines the game will say when the player reaches the exit
	public string[] exitReachedAnger;
	public string[] exitReachedJoy;
	public string[] exitReachedSurprise;
}