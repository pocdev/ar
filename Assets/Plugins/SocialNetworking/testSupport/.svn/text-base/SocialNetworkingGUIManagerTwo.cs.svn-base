using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;


public class SocialNetworkingGUIManagerTwo : MonoBehaviour
{
	void Start()
	{
		// Dump friends list to log
		SocialNetworkingManager.facebookReceivedFriends += delegate( ArrayList result )
		{
			ResultLogger.logArraylist( result );
		};
		
		// Dump custom data to log
		SocialNetworkingManager.facebookReceivedCustomRequest += delegate( object result )
		{
			ResultLogger.logObject( result );
		};
	}
	

	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 960 || Screen.height >= 960 ) ? 320 : 160;
		float height = ( Screen.width >= 960 || Screen.height >= 960 ) ? 70 : 30;
		float heightPlus = height + 10.0f;
		
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Post Message" ) )
		{
			FacebookBinding.postMessage( "im posting this from Unity: " + Time.deltaTime );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Message & More" ) )
		{
			FacebookBinding.postMessageWithLinkAndLinkToImage( "link post from Unity: " + Time.deltaTime, "http://prime31.com", "Prime31 Studios", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo" );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Friends" ) )
		{
			FacebookBinding.getFriends();
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Dialog With Options" ) )
		{
			FacebookBinding.showPostMessageDialogWithOptions( "http://prime31.com", "Prime31 Studios", string.Empty, string.Empty );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom Feed Dialog" ) )
		{
			var ht = new Hashtable();
			ht.Add( "link", "http://prime31.com" );
			ht.Add( "picture", "http://prime31.com/assets/images/prime31logo.png" );
			ht.Add( "name", "Name of the link" );
			ht.Add( "caption", "Im the prime31 logo" );
			ht.Add( "description", "some text telling what this is all about" );
			FacebookBinding.showPostMessageDialogWithOptions( ht );
		}
		
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus * 2, width, height ), "Back" ) )
		{
			Application.LoadLevel( "SocialNetworkingtestScene" );
		}
		
		

		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		// Twitter
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Graph Request (me)" ) )
		{
			FacebookBinding.graphRequest( "me", "GET", new Hashtable() );
		}
		

		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom Graph Request" ) )
		{
			FacebookBinding.graphRequest( "platform/posts", "GET", new Hashtable() );
		}
		

		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom REST Request" ) )
		{
			var hash = new Hashtable();
			hash.Add( "query", "SELECT uid,name FROM user WHERE uid=4" );
			FacebookBinding.restRequest( "fql.query", "POST", hash );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom Dialog" ) )
		{
			var parameters = new Dictionary<string,string>()
			{
				{ "message", "Check out this great app!" }
			};
			FacebookBinding.showDialog( "apprequests", parameters );
		}
	}

}
