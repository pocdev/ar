using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;


public class SocialNetworkingGUIManager : MonoBehaviour
{
	public bool useTweetSheet = false; // requires iOS 5, set to true to enable the iOS 5 Tweet sheet buttons
	private string screenshotFilename = "someScreeny.png";
	
	
	void Start()
	{
		// Sample of how to get to the data available in the tweets
		SocialNetworkingManager.twitterHomeTimelineReceived += delegate( ArrayList result )
		{
			ResultLogger.logArraylist( result );
		};
		
		// Sample of how to get the results of a custom facebook graph request
		SocialNetworkingManager.facebookReceivedCustomRequest += delegate( object result )
		{
			ResultLogger.logObject( result );
		};
		
		// grab a scren shot for use in the post status update below
		Application.CaptureScreenshot( screenshotFilename );
	}
		
		
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 960 || Screen.height >= 960 ) ? 320 : 160;
		float height = ( Screen.width >= 960 || Screen.height >= 960 ) ? 70 : 30;
		float heightPlus = height + 10.0f;
		
		
		// Facebook
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Initialize" ) )
		{
			FacebookBinding.init( "YOUR_APP_ID_HERE" );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Logged In?" ) )
		{
			bool isLoggedIn = FacebookBinding.isLoggedIn();
			Debug.Log( "Facebook is logged in: " + isLoggedIn );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Login" ) )
		{
			FacebookBinding.login();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Logout" ) )
		{
			FacebookBinding.logout();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get User's Name" ) )
		{
			FacebookBinding.getLoggedinUsersName();
		}

				
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
			FacebookBinding.postImage( pathToImage, "im an image posted from iOS" );
		}
		
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus * 2, width, height ), "More Facebook..." ) )
		{
			Application.LoadLevel( "SocialNetworkingtestSceneTwo" );
		}

		
		
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		// Twitter
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Initialize" ) )
		{
			TwitterBinding.init( "INSERT_YOUR_INFO_HERE", "INSERT_YOUR_INFO_HERE" );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Logged In?" ) )
		{
			bool isLoggedIn = TwitterBinding.isLoggedIn();
			Debug.Log( "Twitter is logged in: " + isLoggedIn );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Logged in Username" ) )
		{
			string username = TwitterBinding.loggedInUsername();
			Debug.Log( "Twitter username: " + username );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Login with Oauth" ) )
		{
			TwitterBinding.showOauthLoginDialog();
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Logout" ) )
		{
			TwitterBinding.logout();
		}
		
		
		if( !useTweetSheet ) // we can post either iOS 5 style or not
		{
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Status Update" ) )
			{
				TwitterBinding.postStatusUpdate( "im posting this from Unity: " + Time.deltaTime );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Status Update + Image" ) )
			{
				var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
				TwitterBinding.postStatusUpdate( "I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage );
			}
		}
		else
		{
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Can User Tweet?" ) )
			{
				Debug.Log( "Can the user tweet using the tweet sheet? " + TwitterBinding.canUserTweet() );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Status Update + Image" ) )
			{
				var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
				TwitterBinding.showTweetComposer( "I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage );
			}
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom Request" ) )
		{
			var dict = new Dictionary<string,string>();
			dict.Add( "status", "word up with a boogie boogie update" );
			TwitterBinding.performRequest( "POST", "/statuses/update.json", dict );
		}
	}


}
