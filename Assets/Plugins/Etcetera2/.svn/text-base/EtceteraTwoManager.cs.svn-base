using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class EtceteraTwoManager : MonoBehaviour
{
#if UNITY_IPHONE
	// Fired when an external screen is detected and is being mirrored onto
	public static event Action screenMirroringDidStartEvent;
	
	// Fired when external screen mirroring is stopped
	public static event Action screenMirroringDidStopEvent;


	void Awake()
	{
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}


	public void screenMirroringDidStart( string empty )
	{
		if( screenMirroringDidStartEvent != null )
			screenMirroringDidStartEvent();
	}


	public void screenMirroringDidStop( string empty )
	{
		if( screenMirroringDidStopEvent != null )
			screenMirroringDidStopEvent();
	}
#endif
}

