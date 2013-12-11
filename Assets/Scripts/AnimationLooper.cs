using UnityEngine;
using System.Collections;

public class AnimationLooper : MonoBehaviour {

	void Start () 
	{
		Animation _anim = GetComponent<Animation>();
		_anim["Base Stack"].wrapMode = WrapMode.Loop;
	}
}
