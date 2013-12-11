using UnityEngine;
using System.Collections;

public class MarioLevelAnimController : MonoBehaviour {
	
	
	void animStart()
	{
		if(!Camera.mainCamera.audio.isPlaying)
			Camera.mainCamera.audio.Play();
	}
}
