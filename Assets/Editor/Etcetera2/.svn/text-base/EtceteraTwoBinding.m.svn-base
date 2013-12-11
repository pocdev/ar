//
//  EtceteraTwoBinding.m
//  Unity-iPhone
//
//  Created by Mike on 4/8/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#include "EtceteraTwoManager.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


// Starts listening for any VGA cables plugged into the iDevice.  Set a reasonable targetFPS (~10 - 15) to keep performance decent.
void _etceteraTwoStartListeningForExternalScreens( int targetFPS )
{
	[[EtceteraTwoManager sharedManager] startListeningForExternalScreensWithFPS:targetFPS];
}


// Stops listening for VGA cable input and turns off mirroring if it is on
void _etceteraTwoStopScreenMirroring()
{
	[[EtceteraTwoManager sharedManager] stopExternalScreenMirroring];
}


// Sets the background color of any empty space due to aspect ratio differences of the VGA device
void _etceteraTwoSetExternalScreenMirroringBackgroundColor( uint color )
{
	[[EtceteraTwoManager sharedManager] setExternalScreenMirorringBackgroundColor:color];
}


// Sets the scale of the mirrored screen.  Some old TV's clip a small portion on the top and bottom and dropping the scale to 0.9 fixes this.
void _etceteraTwoSetExternalScreenMirroringScale( float scale )
{
	[[EtceteraTwoManager sharedManager] setExternalScreenMirorringScale:scale];
}


// Schedulds a local notification.  Text is the text in the alert prompt, action is the button text, sound is an audio file in your ap bundle and launchImage is a different default.png to use when launching.
void _etceteraTwoScheduleLocalNotification( int secondsFromNow, const char * text, const char * action, int badgeCount, const char * sound, const char * launchImage )
{
	NSDate *date = [NSDate dateWithTimeIntervalSinceNow:secondsFromNow];
	[[EtceteraTwoManager sharedManager] scheduleNotificationOn:date
														  text:GetStringParam( text )
														action:GetStringParam( action )
														 sound:GetStringParam( sound )
												   launchImage:GetStringParam( launchImage )
													badgeCount:badgeCount];
}


// Cancels all scheduled notifications
void _etceteraTwoCancelAllLocalNotifications()
{
	[[EtceteraTwoManager sharedManager] cancelAllLocalNotifications];
}


// Plays a movie optionally showing the movie controls.  Specify supported orientations with the bools.
void _etceteraTwoPlayMovie( const char * urlOrFilename, bool showControls, bool supportLandscape, bool supportPortrait )
{
	[[EtceteraTwoManager sharedManager] playMovieAtUrl:GetStringParam( urlOrFilename )
										  showControls:showControls
									  supportLandscape:supportLandscape
									   supportPortrait:supportPortrait];
}


BOOL _etceteraTwoIsJailBroken()
{
	return [[EtceteraTwoManager sharedManager] isJailBroken];
}


BOOL _etceteraTwoIsInfoPlistPatched()
{
	return [[EtceteraTwoManager sharedManager] isInfoPlistPatched];
}


BOOL _etceteraTwoIsCracked( long filesize )
{
	return [[EtceteraTwoManager sharedManager] isCracked:filesize];
}


void _etceteraTwoLogInfoPlistFileSize()
{
	[[EtceteraTwoManager sharedManager] infoPlistFileSize];
}


const char * _etceteraTwoGetInfoPlistValue( const char * key )
{
	NSString *bundleKey = GetStringParam( key );
	
	id value = [[[NSBundle mainBundle] infoDictionary] objectForKey:bundleKey];
	if( value && [value isKindOfClass:[NSString class]] )
		return MakeStringCopy( value );
	
	return MakeStringCopy( @"" );
}


BOOL _etceteraTwoAddSkipBackupAttribute( const char * filePath )
{
	NSString *path = GetStringParam( filePath );
	
	if( ![[NSFileManager defaultManager] fileExistsAtPath:path] )
	{
		NSLog( @"file does not exist at path: %@", path );
		return NO;
	}
	
	return [[EtceteraTwoManager sharedManager] addSkipBackupAttributeToItemAtURL:[NSURL URLWithString:path]];
}


