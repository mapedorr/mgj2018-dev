using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitBtn : MonoBehaviour, IPointerEnterHandler
{
	public GameObject sceneManager;

	public void OnPointerEnter (PointerEventData eventData)
	{
		sceneManager.GetComponent<EndScene> ().ShowContinue ();
	}
}