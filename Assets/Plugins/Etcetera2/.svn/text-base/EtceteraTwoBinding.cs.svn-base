using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


#if UNITY_IPHONE
public class EtceteraTwoBinding
{
	[DllImport("__Internal")]
	private static extern void _etceteraTwoStartListeningForExternalScreens( int targetFPS );

	// Starts listening for any VGA cables plugged into the iDevice.  Set a reasonable targetFPS (~10 - 15) to keep performance decent.
	public static void startListeningForExternalScreens( int targetFPS )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoStartListeningForExternalScreens( targetFPS );
	}


	[DllImport("__Internal")]
	private static extern void _etceteraTwoStopScreenMirroring();

	// Stops listening for VGA cable input and turns off mirroring if it is on
	public static void stopScreenMirroring()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoStopScreenMirroring();
	}


	[DllImport("__Internal")]
	private static extern void _etceteraTwoSetExternalScreenMirroringBackgroundColor( uint color );

	// Sets the background color of any empty space due to aspect ratio differences of the VGA device
	public static void setExternalScreenMirroringBackgroundColor( uint color )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoSetExternalScreenMirroringBackgroundColor( color );
	}


	[DllImport("__Internal")]
	private static extern void _etceteraTwoSetExternalScreenMirroringScale( float scale );

	// Sets the scale of the mirrored screen.  Some old TV's clip a small portion on the top and bottom and dropping the scale to 0.9 fixes this.
	public static void setExternalScreenMirroringScale( float scale )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoSetExternalScreenMirroringScale( scale );
	}


	[DllImport("__Internal")]
	private static extern void _etceteraTwoScheduleLocalNotification( int secondsFromNow, string text, string action, int badgeCount, string sound, string launchImage );
	
	public static void scheduleLocalNotification( int secondsFromNow, string text, string action )
	{
		scheduleLocalNotification( secondsFromNow, text, action, 0, string.Empty, string.Empty );
	}
	
	// Schedules a local notification.  Text is the text in the alert prompt, action is the button text, sound is an audio file in your app bundle and launchImage is a different default.png to use when launching.
	public static void scheduleLocalNotification( int secondsFromNow, string text, string action, int badgeCount, string sound, string launchImage )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoScheduleLocalNotification( secondsFromNow, text, action, badgeCount, sound, launchImage );
	}


	[DllImport("__Internal")]
	private static extern void _etceteraTwoCancelAllLocalNotifications();

	// Cancels all scheduled notifications
	public static void cancelAllLocalNotifications()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoCancelAllLocalNotifications();
	}


	[DllImport("__Internal")]
	private static extern void _etceteraTwoPlayMovie( string urlOrFilename, bool showControls, bool supportLandscape, bool supportPortrait );

	// Plays a movie optionally showing the movie controls.  Specify supported orientations with the bools.
	public static void playMovie( string urlOrFilename, bool showControls, bool supportLandscape, bool supportPortrait )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_etceteraTwoPlayMovie( urlOrFilename, showControls, supportLandscape, supportPortrait );
	}


	[DllImport("__Internal")]
	private static extern bool _etceteraTwoIsJailBroken();

	// Checks to see if the phone is jailbroken.  Do not use this on it's own to count as a cracked app!
	public static bool isJailBroken()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _etceteraTwoIsJailBroken();
		return false;
	}


	[DllImport("__Internal")]
	private static extern bool _etceteraTwoIsInfoPlistPatched();

	// Checks to see if the info.plist file was patched
	public static bool isInfoPlistPatched()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _etceteraTwoIsInfoPlistPatched();
		return false;
	}


	[DllImport("__Internal")]
	private static extern bool _etceteraTwoIsCracked( long filesize );

	// Checks to see if the app is cracked with an optional info.plist file size check
	public static bool isCracked()
	{
		return isCracked( 0 );
	}
	
	public static bool isCracked( long filesize )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _etceteraTwoIsCracked( filesize);
		return false;
	}


	[DllImport("__Internal")]
	private static extern bool _etceteraTwoLogInfoPlistFileSize();

	// Logs the filesize of the Info.plist file to the console.  The filesize can then be passed to isCracked for an extra check.
	public static bool logInfoPlistFileSize()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _etceteraTwoLogInfoPlistFileSize();
		return false;
	}
	

	[DllImport("__Internal")]
	private static extern string _etceteraTwoGetInfoPlistValue( string key );

	// Grabs the value of an Info.plist key if it exists and is a string
	public static string getInfoPlistValue( string key )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _etceteraTwoGetInfoPlistValue( key );
		return string.Empty;;
	}
	
	
	[DllImport("__Internal")]
	private static extern bool _etceteraTwoAddSkipBackupAttribute( string filePath );

	// Adds the new iOS 5.0.1 skip backup flag allowing you to store files in the Documents directory safely
	public static bool addSkipBackupAttribute( string filePath )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _etceteraTwoAddSkipBackupAttribute( filePath);
		return false;
	}

}
#endif
