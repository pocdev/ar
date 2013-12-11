using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


// Any methods that Obj-C calls back using UnitySendMessage should be present here
public class SocialNetworkingManager : MonoBehaviour
{
	// Twitter
	// Fired after a successful login attempt was made
	public static event Action twitterLogin;
	
	// Fired when an error occurs while logging in
	public static event Action<string> twitterLoginFailed;

	// Fired after successfully sending a status update
	public static event Action twitterPost;

	// Fired when a status update fails
	public static event Action<string> twitterPostFailed;

	// Fired when the home timeline is received
	public static event Action<ArrayList> twitterHomeTimelineReceived;

	// Fired when a request for the home timeline fails
	public static event Action<string> twitterHomeTimelineFailed;

	// Fired when a custom request completes
	public static event Action<object> twitterRequestDidFinishEvent;

	// Fired when a custom request fails
	public static event Action<string> twitterRequestDidFailEvent;


	// Facebook
	// Fired after a successful login attempt was made
	public static event Action facebookLogin;

	// Fired when an error occurs while logging in
	public static event Action<string> facebookLoginFailed;
	
	// Fired when the user logs out
	public static event Action facebookDidLogoutEvent;
	
	// Fired when the access token is extended. Returns the date that the new token will expire
	public static event Action<DateTime> facebookDidExtendTokenEvent;
	
	// Fired when the session is invalidated
	public static event Action facebookSessionInvalidatedEvent;

	// Fired after requesting the logged in users name
	public static event Action<string> facebookReceivedUsername;

	// Fired when failing to get a logged in users name
	public static event Action<string> facebookUsernameRequestFailed;

	// Fired after successfully sending a status update
	public static event Action facebookPost;

	// Fired when a status update fails
	public static event Action<string> facebookPostFailed;

	// Fired when a friend request finishes
	public static event Action<ArrayList> facebookReceivedFriends;

	// Fired when a friend request fails
	public static event Action<string> facebookFriendRequestFailed;

	// Fired when the post message or custom dialog completes
	public static event Action facebookDialogCompleted;

	// Fired when the post message or custom dialog fails
	public static event Action<string> facebookDialogFailed;

	// Fired when the post message or a custom dialog does not complete
	public static event Action facebookDialogDidntComplete;
	
	// Fired when a custom dialog completes with the url passed back from the dialog
	public static event Action<string> facebookDialogCompletedWithUrl;

	// Fired when a custom graph request finishes
	public static event Action<object> facebookReceivedCustomRequest;

	// Fired when a custom graph request fails
	public static event Action<string> facebookCustomRequestFailed;
	
	

    void Awake()
    {
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
    }
	
	
	#region Twitter
	
	// Twitter
	public void twitterLoginSucceeded( string empty )
	{
		if( twitterLogin != null )
			twitterLogin();
	}
	
	
	public void twitterLoginDidFail( string error )
	{
		if( twitterLoginFailed != null )
			twitterLoginFailed( error );
	}
	
	
	public void twitterPostSucceeded( string empty )
	{
		if( twitterPost != null )
			twitterPost();
	}
	
	
	public void twitterPostDidFail( string error )
	{
		if( twitterPostFailed != null )
			twitterPostFailed( error );
	}
	
	
	public void twitterHomeTimelineDidFail( string error )
	{
		if( twitterHomeTimelineFailed != null )
			twitterHomeTimelineFailed( error );
	}
	
	
	public void twitterHomeTimelineDidFinish( string results )
	{
		if( twitterHomeTimelineReceived != null )
		{
			ArrayList resultList = (ArrayList)MiniJSON.jsonDecode( results );
			twitterHomeTimelineReceived( resultList );
		}
	}
	
	
	public void twitterRequestDidFinish( string results )
	{
		if( twitterRequestDidFinishEvent != null )
			twitterRequestDidFinishEvent( MiniJSON.jsonDecode( results ) );
	}
	
	
	public void twitterRequestDidFail( string error )
	{
		if( twitterRequestDidFailEvent != null )
			twitterRequestDidFailEvent( error );
	}
	
	#endregion;
	
	
	#region Facebook

	// Facebook
	public void facebookLoginSucceeded( string empty )
	{
		if( facebookLogin != null )
			facebookLogin();
	}
	
	
	public void facebookLoginDidFail( string error )
	{
		if( facebookLoginFailed != null )
			facebookLoginFailed( error );
	}
	
	
	public void facebookDidLogout( string empty )
	{
		if( facebookDidLogoutEvent != null )
			facebookDidLogoutEvent();
	}
	
	
	public void facebookDidExtendToken( string secondsSinceEpoch )
	{
		if( facebookDidExtendTokenEvent != null )
		{
			var seconds = double.Parse( secondsSinceEpoch );
			var intermediate = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
			var date = intermediate.AddSeconds( seconds );
			facebookDidExtendTokenEvent( date );
		}
	}
	
	
	public void facebookSessionInvalidated( string empty )
	{
		if( facebookSessionInvalidatedEvent != null )
			facebookSessionInvalidatedEvent();
	}
	
	
	public void facebookDidReceiveUsername( string username )
	{
		if( facebookReceivedUsername != null )
			facebookReceivedUsername( username );
	}
	
	
	public void facebookUsernameRequestDidFail( string error )
	{
		if( facebookUsernameRequestFailed != null )
			facebookUsernameRequestFailed( error );
	}
	
	
	public void facebookPostSucceeded( string empty )
	{
		if( facebookPost != null )
			facebookPost();
	}
	
	
	public void facebookPostDidFail( string error )
	{
		if( facebookPostFailed != null )
			facebookPostFailed( error );
	}


	public void facebookDidReceiveFriends( string jsonResult )
	{
		if( facebookReceivedFriends != null )
		{
			Hashtable friendList = (Hashtable)MiniJSON.jsonDecode( jsonResult );
			
			if( friendList.Contains( "data" ) )
				facebookReceivedFriends( (ArrayList)friendList["data"] );
			else
				facebookReceivedFriends( new ArrayList() );
		}
	}
	
	
	public void facebookFriendRequestDidFail( string error )
	{
		if( facebookFriendRequestFailed != null )
			facebookFriendRequestFailed( error );
	}
	
	
	public void facebookDialogDidComplete( string empty )
	{
		if( facebookDialogCompleted != null )
			facebookDialogCompleted();
	}
	
	
	public void facebookDialogDidCompleteWithUrl( string url )
	{
		if( facebookDialogCompletedWithUrl != null )
			facebookDialogCompletedWithUrl( url );
	}


	public void facebookDialogDidNotComplete( string empty )
	{
		if( facebookDialogDidntComplete != null )
			facebookDialogDidntComplete();
	}
	
	
	public void facebookDialogDidFailWithError( string error )
	{
		if( facebookDialogFailed != null )
			facebookDialogFailed( error );
	}


	public void facebookDidReceiveCustomRequest( string result )
	{
		if( facebookReceivedCustomRequest != null )
		{
			object obj = MiniJSON.jsonDecode( result );
			facebookReceivedCustomRequest( obj );
		}
	}
	
	
	public void facebookCustomRequestDidFail( string error )
	{
		if( facebookCustomRequestFailed != null )
			facebookCustomRequestFailed( error );
	}
	
	
	#endregion;

}