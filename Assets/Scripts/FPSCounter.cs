using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
    
	private float m_fps;
	
    void Update()
    {
    	m_fps = 1 / Time.deltaTime;
    }
       
	void OnGUI()
	{
		GUI.contentColor = Color.magenta;
     	GUI.Label(new Rect(0, 0, 30, 20), m_fps.ToString());
    }
}