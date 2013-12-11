using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class EtceteraTwoEventListener : MonoBehaviour
{
#if UNITY_IPHONE
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		EtceteraTwoManager.screenMirroringDidStartEvent += screenMirroringDidStartEvent;
		EtceteraTwoManager.screenMirroringDidStopEvent += screenMirroringDidStopEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		EtceteraTwoManager.screenMirroringDidStartEvent -= screenMirroringDidStartEvent;
		EtceteraTwoManager.screenMirroringDidStopEvent -= screenMirroringDidStopEvent;
	}



	void screenMirroringDidStartEvent()
	{
		Debug.Log( "screenMirroringDidStartEvent" );
	}


	void screenMirroringDidStopEvent()
	{
		Debug.Log( "screenMirroringDidStopEvent" );
	}
#endif
}


