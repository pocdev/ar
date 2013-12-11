using UnityEngine;
using System.Collections;


public class SocialNetworkingEventListener : MonoBehaviour
{
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		// Twitter
		SocialNetworkingManager.twitterLogin += twitterLogin;
		SocialNetworkingManager.twitterLoginFailed += twitterLoginFailed;
		SocialNetworkingManager.twitterPost += twitterPost;
		SocialNetworkingManager.twitterPostFailed += twitterPostFailed;
		SocialNetworkingManager.twitterHomeTimelineReceived += twitterHomeTimelineReceived;
		SocialNetworkingManager.twitterHomeTimelineFailed += twitterHomeTimelineFailed;
		SocialNetworkingManager.twitterRequestDidFinishEvent += twitterRequestDidFinishEvent;
		SocialNetworkingManager.twitterRequestDidFailEvent += twitterRequestDidFailEvent;
		
		// Facebook
		SocialNetworkingManager.facebookLogin += facebookLogin;
		SocialNetworkingManager.facebookLoginFailed += facebookLoginFailed;
		SocialNetworkingManager.facebookDidLogoutEvent += facebookDidLogoutEvent;
		SocialNetworkingManager.facebookDidExtendTokenEvent += facebookDidExtendTokenEvent;
		SocialNetworkingManager.facebookSessionInvalidatedEvent += facebookSessionInvalidatedEvent;
		SocialNetworkingManager.facebookReceivedUsername += facebookReceivedUsername;
		SocialNetworkingManager.facebookUsernameRequestFailed += facebookUsernameRequestFailed;
		SocialNetworkingManager.facebookPost += facebookPost;
		SocialNetworkingManager.facebookPostFailed += facebookPostFailed;
		
		SocialNetworkingManager.facebookReceivedFriends += facebookReceivedFriends;
		SocialNetworkingManager.facebookFriendRequestFailed += facebookFriendRequestFailed;
		SocialNetworkingManager.facebookDialogCompleted += facebokDialogCompleted;
		SocialNetworkingManager.facebookDialogCompletedWithUrl += facebookDialogCompletedWithUrl;
		SocialNetworkingManager.facebookDialogDidntComplete += facebookDialogDidntComplete;
		SocialNetworkingManager.facebookDialogFailed += facebookDialogFailed;
		SocialNetworkingManager.facebookReceivedCustomRequest += facebookReceivedCustomRequest;
		SocialNetworkingManager.facebookCustomRequestFailed += facebookCustomRequestFailed;
	}

	
	void OnDisable()
	{
		// Remove all the event handlers
		// Twitter
		SocialNetworkingManager.twitterLogin -= twitterLogin;
		SocialNetworkingManager.twitterLoginFailed -= twitterLoginFailed;
		SocialNetworkingManager.twitterPost -= twitterPost;
		SocialNetworkingManager.twitterPostFailed -= twitterPostFailed;
		SocialNetworkingManager.twitterHomeTimelineReceived -= twitterHomeTimelineReceived;
		SocialNetworkingManager.twitterHomeTimelineFailed -= twitterHomeTimelineFailed;
		SocialNetworkingManager.twitterRequestDidFinishEvent -= twitterRequestDidFinishEvent;
		SocialNetworkingManager.twitterRequestDidFailEvent -= twitterRequestDidFailEvent;
		
		// Facebook
		SocialNetworkingManager.facebookLogin -= facebookLogin;
		SocialNetworkingManager.facebookLoginFailed -= facebookLoginFailed;
		SocialNetworkingManager.facebookDidLogoutEvent -= facebookDidLogoutEvent;
		SocialNetworkingManager.facebookDidExtendTokenEvent -= facebookDidExtendTokenEvent;
		SocialNetworkingManager.facebookSessionInvalidatedEvent -= facebookSessionInvalidatedEvent;
		SocialNetworkingManager.facebookReceivedUsername -= facebookReceivedUsername;
		SocialNetworkingManager.facebookUsernameRequestFailed -= facebookUsernameRequestFailed;
		SocialNetworkingManager.facebookPost -= facebookPost;
		SocialNetworkingManager.facebookPostFailed -= facebookPostFailed;
		
		SocialNetworkingManager.facebookReceivedFriends -= facebookReceivedFriends;
		SocialNetworkingManager.facebookFriendRequestFailed += facebookFriendRequestFailed;
		SocialNetworkingManager.facebookDialogCompleted -= facebokDialogCompleted;
		SocialNetworkingManager.facebookDialogCompletedWithUrl -= facebookDialogCompletedWithUrl;
		SocialNetworkingManager.facebookDialogDidntComplete -= facebookDialogDidntComplete;
		SocialNetworkingManager.facebookDialogFailed -= facebookDialogFailed;
		SocialNetworkingManager.facebookReceivedCustomRequest -= facebookReceivedCustomRequest;
		SocialNetworkingManager.facebookCustomRequestFailed -= facebookCustomRequestFailed;
	}

	
	// Twitter events
	void twitterLogin()
	{
		Debug.Log( "Successfully logged in to Twitter" );
	}
	
	
	void twitterLoginFailed( string error )
	{
		Debug.Log( "Twitter login failed: " + error );
	}
	

	void twitterPost()
	{
		Debug.Log( "Successfully posted to Twitter" );
	}
	

	void twitterPostFailed( string error )
	{
		Debug.Log( "Twitter post failed: " + error );
	}


	void twitterHomeTimelineFailed( string error )
	{
		Debug.Log( "Twitter HomeTimeline failed: " + error );
	}
	
	
	void twitterHomeTimelineReceived( ArrayList result )
	{
		Debug.Log( "received home timeline with tweet count: " + result.Count );
	}
	
	
	void twitterRequestDidFailEvent( string error )
	{
		Debug.Log( "twitterRequestDidFailEvent: " + error );
	}
	
	
	void twitterRequestDidFinishEvent( object result )
	{
		if( result != null )
			Debug.Log( "twitterRequestDidFinishEvent: " + result.GetType().ToString() );
		else
			Debug.Log( "twitterRequestDidFinishEvent with no data" );
	}
	
	
	
	// Facebook events
	void facebookLogin()
	{
		Debug.Log( "Successfully logged in to Facebook" );
	}
	
	
	void facebookLoginFailed( string error )
	{
		Debug.Log( "Facebook login failed: " + error );
	}
	
	
	void facebookDidLogoutEvent()
	{
		Debug.Log( "facebookDidLogoutEvent" );
	}
	
	
	void facebookDidExtendTokenEvent( System.DateTime newExpiry )
	{
		Debug.Log( "facebookDidExtendTokenEvent: " + newExpiry );
	}
	
	
	void facebookSessionInvalidatedEvent()
	{
		Debug.Log( "facebookSessionInvalidatedEvent" );
	}
	

	void facebookReceivedUsername( string username )
	{
		Debug.Log( "Facebook logged in users name: " + username );
	}
	
	
	void facebookUsernameRequestFailed( string error )
	{
		Debug.Log( "Facebook failed to receive username: " + error );
	}
	
	
	void facebookPost()
	{
		Debug.Log( "Successfully posted to Facebook" );
	}
	

	void facebookPostFailed( string error )
	{
		Debug.Log( "Facebook post failed: " + error );
	}


	void facebookReceivedFriends( ArrayList result )
	{
		Debug.Log( "received total friends: " + result.Count );
	}
	
	
	void facebookFriendRequestFailed( string error )
	{
		Debug.Log( "FfacebookFriendRequestFailed: " + error );
	}
	
	
	void facebokDialogCompleted()
	{
		Debug.Log( "facebokDialogCompleted" );
	}
	
	
	void facebookDialogCompletedWithUrl( string url )
	{
		Debug.Log( "facebookDialogCompletedWithUrl: " + url );
	}
	
	
	void facebookDialogDidntComplete()
	{
		Debug.Log( "facebookDialogDidntComplete" );
	}
	
	
	void facebookDialogFailed( string error )
	{
		Debug.Log( "facebookDialogFailed: " + error );
	}
	
	
	void facebookReceivedCustomRequest( object obj )
	{
		Debug.Log( "facebookReceivedCustomRequest" );
	}
	
	
	void facebookCustomRequestFailed( string error )
	{
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
	}
	
}
