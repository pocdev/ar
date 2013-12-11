//
//  EtceteraTwoManager.m
//  Unity-iPhone
//
//  Created by Mike on 4/8/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import "EtceteraTwoManager.h"
#include <sys/xattr.h>


void UnityPause( bool pause );

void UnitySendMessage( const char * className, const char * methodName, const char * param );

UIViewController *UnityGetGLViewController();


UIColor * UIColorFromHex( int hexcolor )
{
	int r = ( hexcolor >> 24 ) & 0xFF;
	int g = ( hexcolor >> 16 ) & 0xFF;
	int b = ( hexcolor >> 8 ) & 0xFF;
	int a = hexcolor & 0xFF;
	
	if( a == 0 )
		a = 1.0f;
	
	return [UIColor colorWithRed:(r/255.0) green:(g/255.0) blue:(b/255.0) alpha:(a/255.0)];
}


@interface EtceteraTwoManager()
@property (nonatomic, retain) UIViewController *placeholderController;
@end




@implementation EtceteraTwoManager

@synthesize screenManager, placeholderController;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (EtceteraTwoManager*)sharedManager
{
	static EtceteraTwoManager *sharedManager = nil;
	
	if( !sharedManager )
		sharedManager = [[EtceteraTwoManager alloc] init];
	
	return sharedManager;
}


- (id)init
{
	if( ( self = [super init] ) )
	{

	}
	return self;
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark P31MoviePlayerViewControllerDelegate

- (void)moviePlayerControllerDidFinish:(P31MoviePlayerViewController*)player
{
	UnityPause( false );
	
	// kill the player
	[player dismissModalViewControllerAnimated:NO];
	
	// kill the placeholder that we launched the player from if we needed it
	if( placeholderController )
	{
		[placeholderController.view removeFromSuperview];
		self.placeholderController = nil;
	}
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark ExternalScreenManagerDelegate

- (void)screenManagerDidStartMirroring
{
	UnitySendMessage( "EtceteraTwoManager", "screenMirroringDidStart", "" );
}


- (void)screenManagerDidStopMirroring
{
	UnitySendMessage( "EtceteraTwoManager", "screenMirroringDidStop", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Crack Detection

- (BOOL)isJailBroken
{
	NSArray *apps = [NSArray arrayWithObjects:@"/Applications/Cydia.app", @"/Applications/limera1n.app", @"/Applications/greenpois0n.app",
					 @"/Applications/blackra1n.app", @"/Applications/blacksn0w.app", @"/Applications/redsn0w.app", nil];
	
	// Now check for known jailbreak apps. If we encounter one, the device is jailbroken.
	for( NSString *app in apps )
	{
		if( [[NSFileManager defaultManager] fileExistsAtPath:app] )
			return YES;	
	}
	
	return NO;
}


- (BOOL)isInfoPlistPatched
{
	char csignid[] = "PfdkboFabkqfqv";
	for( int i = 0; i < strlen( csignid ); i++ )
		csignid[i] = csignid[i] + 3;
	NSString *signIdentity = [[[NSString alloc] initWithCString:csignid encoding:NSUTF8StringEncoding] autorelease];
	
	if( [[[NSBundle mainBundle] infoDictionary] objectForKey:signIdentity] != nil )
		return YES;
	return NO;
}


- (BOOL)isCracked:(long)filesize
{
	if( [self isInfoPlistPatched] )
		return YES;
	
	// only check filesize if it was passed in
	if( filesize > 0 )
	{
		long actualFileSize = [self infoPlistFileSize];
		if( actualFileSize != filesize )
			return YES;
	}

	return NO;
}


- (long)infoPlistFileSize
{
	NSString *path = [[[NSBundle mainBundle] bundlePath] stringByAppendingPathComponent:@"Info.plist"];
	
	NSDictionary *fileAttributes = [[NSFileManager defaultManager] attributesOfItemAtPath:path error:nil];	
	if( fileAttributes != nil )
	{
		NSNumber *fileSize = [fileAttributes objectForKey:NSFileSize];
		NSLog( @"File size: %qi\n", [fileSize unsignedLongLongValue] );
		
		return [fileSize unsignedLongLongValue];
	}
	
	NSLog( @"could not find Info.plist file" );
	
	return 0;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startListeningForExternalScreensWithFPS:(int)fps
{
	// early out if we already exist
	if( screenManager )
		return;
	
	screenManager = [[ExternalScreenManager alloc] init];
	screenManager.targetFPS = fps;
	screenManager.delegate = self;
	[screenManager listenForExternalScreens];
}


- (void)stopExternalScreenMirroring
{
	[screenManager stop];
	self.screenManager = nil;
}


- (void)setExternalScreenMirorringBackgroundColor:(uint)color
{
	if( screenManager )
		screenManager.backgroundColor = UIColorFromHex( color );
}


- (void)setExternalScreenMirorringScale:(CGFloat)scale
{
	if( screenManager )
		screenManager.scale = scale;
}


- (void)scheduleNotificationOn:(NSDate*)fireDate
						  text:(NSString*)text
						action:(NSString*)action
						 sound:(NSString*)soundfileName
				   launchImage:(NSString*)launchImage
					badgeCount:(int)badgeCount
{
	// iOS < 4 doesnt have these suckers so check here
	Class notes = NSClassFromString( @"UILocalNotification" );
	if( !notes )
		return;
	
	UILocalNotification *localNotification = [[UILocalNotification alloc] init];
    localNotification.fireDate = fireDate;
    localNotification.timeZone = [NSTimeZone defaultTimeZone];	
	
    localNotification.alertBody = text;
    localNotification.alertAction = action;	
	
	if( !soundfileName )
		localNotification.soundName = UILocalNotificationDefaultSoundName;
	else
		localNotification.soundName = soundfileName;
	
	localNotification.alertLaunchImage = launchImage;
    localNotification.applicationIconBadgeNumber = badgeCount;			
	
	// Schedule it with the app
    [[UIApplication sharedApplication] scheduleLocalNotification:localNotification];
    [localNotification release];
}


- (void)cancelAllLocalNotifications
{
	[[UIApplication sharedApplication] cancelAllLocalNotifications];
}


- (void)playMovieAtUrl:(NSString*)url showControls:(BOOL)showControls supportLandscape:(BOOL)landscape supportPortrait:(BOOL)portrait
{
	UIWindow *win = [UIApplication sharedApplication].keyWindow;
	
	// are we playing a local file or something on the web?
	BOOL isRemote = [url rangeOfString:@"http"].location == 0;
	
	// Create custom movie player
	P31MoviePlayerViewController *moviePlayer;
	if( isRemote )
	{
		moviePlayer = [[P31MoviePlayerViewController alloc] initWithVideoURL:url];
	}
	else
	{
		// if the url starts with '/' then use it directly else get the bundle path
		if( ![[url substringToIndex:1] isEqualToString:@"/"] )
			url = [[NSBundle mainBundle] pathForResource:url ofType:nil];
		moviePlayer = [[P31MoviePlayerViewController alloc] initWithVideoFilePath:url];
	}
	
	moviePlayer.delegate = self;
	moviePlayer.supportPortrait = portrait;
	moviePlayer.supportLandscape = landscape;
	
	
	UIViewController *vc;
	
	if( &UnityGetGLViewController != NULL )
	{
		vc = UnityGetGLViewController();
	}
	else
	{
		// stick a view controller in there so we can launch modally from it
		placeholderController = [[UIViewController alloc] init];
		placeholderController.view.frame = win.bounds;
		[win addSubview:placeholderController.view];
		placeholderController.view.frame = CGRectZero;
		vc = placeholderController;
	}
	
	// Show the movie player modally
	[vc presentModalViewController:moviePlayer animated:NO];
	
	// Prep and play the movie
	[moviePlayer startPlaybackShowingControls:showControls];
	UnityPause( true );
}


- (BOOL)addSkipBackupAttributeToItemAtURL:(NSURL*)URL
{
    const char* filePath = [[URL path] fileSystemRepresentation];
    const char* attrName = "com.apple.MobileBackup";
    u_int8_t attrValue = 1;
    int result = setxattr( filePath, attrName, &attrValue, sizeof( attrValue ), 0, 0 );
    return result == 0;
}



@end
