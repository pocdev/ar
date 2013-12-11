using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class FacebookBinding
{
    [DllImport("__Internal")]
    private static extern void _facebookInit( string applicationId );

	// Initializes the Facebook plugin for you application
    public static void init( string applicationId )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookInit( applicationId );
    }
	
	
    [DllImport("__Internal")]
    private static extern bool _facebookIsLoggedIn();
 
	// Checks to see if there is a currently logged in user
    public static bool isLoggedIn()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _facebookIsLoggedIn();
		return false;
    }
    
    
	[DllImport("__Internal")]
	private static extern string _facebookGetFacebookAccessToken();
	
	// Grabs the current Facebook access token
	public static string getFacebookAccessToken()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
			return _facebookGetFacebookAccessToken();

		return string.Empty;
	}

	
	[DllImport("__Internal")]
	private static extern void _facebookExtendAccessToken();
	
	// Extends the logged in user's access token
	public static void extendAccessToken()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
			_facebookExtendAccessToken();
	}
	

    [DllImport("__Internal")]
    private static extern void _facebookLogin();
 
	// Opens the Facebook single sign on login in Safari or the official Facebook app
    public static void login()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookLogin();
    }

	
    [DllImport("__Internal")]
    private static extern void _facebookLoginWithRequestedPermissions( string perms );
 
	// Opens the Facebook single sign on login in Safari or the official Facebook app with the requested permissions
    public static void loginWithRequestedPermissions( string[] permissions )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			var permissionsString = string.Join( ",", permissions );
			_facebookLoginWithRequestedPermissions( permissionsString );
		}
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookLogout();
 
	// Logs out the current user
    public static void logout()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookLogout();
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookGetLoggedinUsersName();
 
	// Gets the currently logged in users name
    public static void getLoggedinUsersName()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookGetLoggedinUsersName();
    }


    [DllImport("__Internal")]
    private static extern void _facebookPostMessage( string message );
 
	// Posts the message to the user's wall
    public static void postMessage( string message )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookPostMessage( message );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookPostMessageWithLink( string message, string link, string linkName );
 
	// Posts the message to the user's wall with a link and a name for the link
    public static void postMessageWithLink( string message, string link, string linkName )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookPostMessageWithLink( message, link, linkName );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookPostMessageWithLinkAndLinkToImage( string message, string link, string linkName, string linkToImage, string caption );
 
	// Posts the message to the user's wall with a link, a name for the link, a link to an image and a caption for the image
    public static void postMessageWithLinkAndLinkToImage( string message, string link, string linkName, string linkToImage, string caption )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookPostMessageWithLinkAndLinkToImage( message, link, linkName, linkToImage, caption );
    }
	

    [DllImport("__Internal")]
    private static extern void _facebookPostImage( string pathToImage, string caption );
 
	// Posts an image on the user's wall along with a caption.
    public static void postImage( string pathToImage, string caption )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookPostImage( pathToImage, caption );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookPostImageInAlbum( string pathToImage, string caption, string albumId );
 
	// Posts an image to a specific album along with a caption.
    public static void postImage( string pathToImage, string caption, string albumId )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookPostImageInAlbum( pathToImage, caption, albumId );
    }
	

	[DllImport("__Internal")]
    private static extern void _facebookGetFriends();
 
	// Gets the currently logged in users friends
    public static void getFriends()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookGetFriends();
    }


    [DllImport("__Internal")]
    private static extern void _facebookShowPostMessageDialog();
 
	// Shows a dialog allowing a user to compose a message to post to their wall
    public static void showPostMessageDialog()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookShowPostMessageDialog();
    }

	
    [DllImport("__Internal")]
    private static extern void _facebookShowPostMessageDialogWithOptions( string link, string linkName, string linkToImage, string caption );
 
	// Shows a dialog allowing a user to compose a message with a link, a name for the link, a link to an image and a caption for the image.
	// Pass in an empty string for any params you do not want to include
    public static void showPostMessageDialogWithOptions( string link, string linkName, string linkToImage, string caption )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookShowPostMessageDialogWithOptions( link, linkName, linkToImage, caption );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookShowPostMessageDialogWithCustomOptions( string json );
 
	// Shows the Feed Dialog.  The Hashtable can contain any of the available string options from the Facebook docs
    public static void showPostMessageDialogWithOptions( Hashtable options )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookShowPostMessageDialogWithCustomOptions( MiniJSON.jsonEncode( options ) );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _facebookShowDialog( string dialogType, string json );
 
	// Full access to any existing or new Facebook dialogs that get added.  See Facebooks documentation for parameters and dialog types
    public static void showDialog( string dialogType, Dictionary<string,string> options )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookShowDialog( dialogType, MiniJSON.jsonEncode( options ) );
    }
	

    [DllImport("__Internal")]
    private static extern void _facebookGraphRequest( string graphPath, string httpMethod, string jsonDict );
 
	// Allows you to use any available Facebook Graph API method
    public static void graphRequest( string graphPath, string httpMethod, Hashtable keyValueHash )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			// convert the Hashtable to JSON
			string jsonDict = MiniJSON.jsonEncode( keyValueHash );
			
			if( jsonDict != null )
				_facebookGraphRequest( graphPath, httpMethod, jsonDict );
		}
    }

	
    [DllImport("__Internal")]
    private static extern void _facebookRestRequest( string restMethod, string httpMethod, string jsonDict );
 
	// Allows you to use any available Facebook REST API method
    public static void restRequest( string restMethod, string httpMethod, Hashtable keyValueHash )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			// convert the Hashtable to JSON
			string jsonDict = MiniJSON.jsonEncode( keyValueHash );
			
			if( jsonDict != null )
				_facebookRestRequest( restMethod, httpMethod, jsonDict );
		}
    }

}
