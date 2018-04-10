using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	// ═══╣ properties ╠═══
	float m_h;
	public float H { get { return m_h; } }

	float m_v;
	public float V { get { return m_v; } }

	bool m_buttonZ;
	public bool ButtonZ { get { return m_buttonZ; } }

	bool m_buttonR;
	public bool ButtonR { get { return m_buttonR; } }

	// can use this to give power to the game
	bool m_inputEnabled;
	public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

	// ═══╣ methods ╠═══
	public void GetKeyInput ()
	{
		if (m_inputEnabled)
		{
			m_h = Input.GetAxisRaw ("Horizontal");
			m_v = Input.GetAxisRaw ("Vertical");
			m_buttonZ = Input.GetKeyUp (KeyCode.Z);
			m_buttonR = Input.GetKeyUp (KeyCode.R);
		}
		else
		{
			m_h = 0f;
			m_v = 0f;
		}
	}
}