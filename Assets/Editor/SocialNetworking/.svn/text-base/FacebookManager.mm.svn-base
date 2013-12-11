//
//  FacebookManager.m
//  Facebook
//
//  Created by Mike on 9/13/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "FacebookManager.h"
#import <objc/runtime.h>


NSString* const kFacebookAppIdKey = @"kFacebookAppIdKey";
NSString* const kFacebookLibAccessToken = @"kFacebookLibAccessToken";
NSString* const kFacebookLibAccessTokenExpirationDate = @"kFacebookLibAccessTokenExpirationDate";


void UnitySendMessage( const char * className, const char * methodName, const char * param );

void UnityPause( bool pause );


@implementation FacebookManager

@synthesize appId = _appId;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (FacebookManager*)sharedManager
{
	static FacebookManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[FacebookManager alloc] init];
	
	return sharedSingleton;
}


- (id)init
{
	if( ( self = [super init] ) )
	{
		[self setCredentialsIfAvailable];
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (BOOL)handleOpenURL:(NSURL*)url
{
    BOOL res = [_facebook handleOpenURL:url];
	
	if( [_facebook isSessionValid] )
		[self saveCredentials];
    
    NSLog(@"***********\n*************\n************\n****************%@",url);
    
    UnitySendMessage("AdditiveLoader", "launchViaURL", [[url absoluteString] cStringUsingEncoding:NSASCIIStringEncoding]);
    
	return res;
}


- (void)setCredentialsIfAvailable
{
	// if we have an appId tucked away, set it now
	if( [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookAppIdKey] )
	{
		self.appId = [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookAppIdKey];
		_facebook = [[Facebook alloc] initWithAppId:_appId andDelegate:self];
	}
	
	if( [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookLibAccessTokenExpirationDate] )
		_facebook.expirationDate = [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookLibAccessTokenExpirationDate];
	
	if( [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookLibAccessToken] )
		_facebook.accessToken = [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookLibAccessToken];
}


- (void)saveCredentials
{
    [[NSUserDefaults standardUserDefaults] setObject:_facebook.accessToken forKey:kFacebookLibAccessToken];
    [[NSUserDefaults standardUserDefaults] setObject:_facebook.expirationDate forKey:kFacebookLibAccessTokenExpirationDate];
    [[NSUserDefaults standardUserDefaults] synchronize];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)setAppId:(NSString*)newAppId
{
	[_appId release];	
	_appId = [newAppId copy];
	
	// tuck this sucker in the NSUserDefaults to make SSO more robust
	[[NSUserDefaults standardUserDefaults] setObject:newAppId forKey:kFacebookAppIdKey];
	
	// create the Facebook object if we dont have it already
	if( !_facebook )
		_facebook = [[Facebook alloc] initWithAppId:_appId andDelegate:self];
}


- (BOOL)isLoggedIn
{
	return [_facebook isSessionValid];
}


- (NSString*)accessToken
{
    return _facebook.accessToken;
}


- (void)extendAccessToken
{
	[_facebook extendAccessToken];
}


- (void)login
{
	[self loginWithRequestedPermissions:[NSArray arrayWithObject:@"publish_stream"]];
}


- (void)loginWithRequestedPermissions:(NSArray*)permissions
{
	if( [self isLoggedIn] )
	{
		UnitySendMessage( "SocialNetworkingManager", "facebookLoginSucceeded", "" );
		return;
	}
	
	UnityPause( true );
	[_facebook authorize:permissions];
}


- (void)logout
{
	[_facebook logout];
}


- (void)getLoggedInUsername
{
	[_facebook requestWithGraphPath:@"me" andDelegate:self];
}


- (void)postMessage:(NSString*)message
{
	[self postMessage:message link:nil linkName:nil];
}


- (void)postMessage:(NSString*)message link:(NSString*)link linkName:(NSString*)linkName
{
	[self postMessage:message link:link linkName:linkName linkToImage:nil caption:nil];
}
	 
	 
 // Allowed post params: message, picture (link), link, name (of link), caption, description (of link)
 - (void)postMessage:(NSString*)message link:(NSString*)link linkName:(NSString*)linkName linkToImage:(NSString*)linkToImage caption:(NSString*)caption
{
	NSMutableDictionary *params = [NSMutableDictionary dictionaryWithObjectsAndKeys:message, @"message", nil];
	
	if( link )
		[params setObject:link forKey:@"link"];
	
	if( linkName )
		[params setObject:linkName forKey:@"name"];
	
	if( linkToImage )
		[params setObject:linkToImage forKey:@"picture"];
	
	if( caption )
		[params setObject:caption forKey:@"caption"];
	
	[_facebook requestWithGraphPath:@"me/feed" andParams:params andHttpMethod:@"POST" andDelegate:self];
}


- (void)postPhoto:(NSString*)path caption:(NSString*)caption
{
	if( ![[NSFileManager defaultManager] fileExistsAtPath:path] )
	{
		NSLog( @"image does not exist: %@", path );
		return;
	}
	
	NSURL *url  = [NSURL fileURLWithPath:path];
	NSData *data = [NSData dataWithContentsOfURL:url];
	UIImage *img  = [[UIImage alloc] initWithData:data];
	
	NSMutableDictionary *params = [NSMutableDictionary dictionaryWithObjectsAndKeys:
									img, @"picture",
								   caption, @"message",
									nil];
	[_facebook requestWithGraphPath:@"me/photos" andParams:params andHttpMethod:@"POST" andDelegate:self];
	[img release];  
}


- (void)postPhotoInAlbum:(NSString*)path caption:(NSString*)caption albumId:(NSString*)albumId
{
	if( ![[NSFileManager defaultManager] fileExistsAtPath:path] )
	{
		NSLog( @"image does not exist: %@", path );
			  return;
	}
			  
	NSURL *url  = [NSURL fileURLWithPath:path];
	NSData *data = [NSData dataWithContentsOfURL:url];
	UIImage *img  = [[UIImage alloc] initWithData:data];

	NSMutableDictionary *params = [NSMutableDictionary dictionaryWithObjectsAndKeys:
								   img, @"picture",
								   caption, @"message", nil];
	[_facebook requestWithGraphPath:albumId andParams:params andHttpMethod:@"POST" andDelegate:self];
	[img release];
}


- (void)showPostMessageDialog
{
	NSMutableDictionary *params = [NSMutableDictionary dictionaryWithObjectsAndKeys:_appId, @"api_key", nil];
    [_facebook dialog:@"stream.publish" andParams:params andDelegate:self];
}


- (void)postMessageDialogWithLink:(NSString*)link linkName:(NSString*)linkName linkToImage:(NSString*)linkToImage caption:(NSString*)caption
{
	NSMutableDictionary *params = [NSMutableDictionary dictionaryWithObjectsAndKeys:_appId, @"api_key", nil];

	if( link )
		[params setObject:link forKey:@"link"];
	
	if( linkName )
		[params setObject:linkName forKey:@"name"];
	
	if( linkToImage )
		[params setObject:linkToImage forKey:@"picture"];
	
	if( caption )
		[params setObject:caption forKey:@"caption"];
	
	[self postMessageDialogWithDict:params];
}


- (void)postMessageDialogWithDict:(NSMutableDictionary*)dict
{
	[_facebook dialog:@"stream.publish" andParams:dict andDelegate:self];
}


- (void)showDialog:(NSString*)dialogType withParms:(NSMutableDictionary*)dict
{
	// add the apiKey
	if( !dict )
		dict = [NSMutableDictionary dictionary];

	[dict setObject:_appId forKey:@"api_key"];
	[_facebook dialog:dialogType andParams:dict andDelegate:self];
}


- (void)getFriends
{
	[_facebook requestWithGraphPath:@"me/friends" andDelegate:self];
}


- (void)requestWithGraphPath:(NSString*)path httpMethod:(NSString*)method params:(NSMutableDictionary*)params
{
	[_facebook requestWithGraphPath:path andParams:params andHttpMethod:method andDelegate:self];
}


- (void)requestWithRestMethod:(NSString*)restMethod httpMethod:(NSString*)method params:(NSMutableDictionary*)params
{
	[_facebook requestWithMethodName:restMethod andParams:params andHttpMethod:method andDelegate:self];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark FBSessionDelegate

// Called when the dialog successful log in the user
- (void)fbDidLogin
{
    [self saveCredentials];

    UnityPause( false );
	UnitySendMessage( "SocialNetworkingManager", "facebookLoginSucceeded", "" );
}


// Called when the user dismiss the dialog without login
- (void)fbDidNotLogin:(BOOL)cancelled
{
    [[NSUserDefaults standardUserDefaults] removeObjectForKey:kFacebookLibAccessToken];
    [[NSUserDefaults standardUserDefaults] removeObjectForKey:kFacebookLibAccessTokenExpirationDate];
    [[NSUserDefaults standardUserDefaults] synchronize];

	UnityPause( false );
	UnitySendMessage( "SocialNetworkingManager", "facebookLoginDidFail", "" );
}


- (void)fbDidLogout
{
    [[NSUserDefaults standardUserDefaults] removeObjectForKey:kFacebookLibAccessToken];
    [[NSUserDefaults standardUserDefaults] removeObjectForKey:kFacebookLibAccessTokenExpirationDate];
    [[NSUserDefaults standardUserDefaults] synchronize];
	
	UnitySendMessage( "SocialNetworkingManager", "facebookDidLogout", "" );
}


- (void)fbDidExtendToken:(NSString*)accessToken expiresAt:(NSDate*)expiresAt
{
	NSString *expires = [NSString stringWithFormat:@"%d", [expiresAt timeIntervalSince1970]];
	UnitySendMessage( "SocialNetworkingManager", "facebookDidExtendToken", expires.UTF8String );
}


- (void)fbSessionInvalidated
{
	UnitySendMessage( "SocialNetworkingManager", "facebookSessionInvalidated", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark FBRequestDelegate

// Called when an error prevents the request from completing successfully.
- (void)request:(FBRequest*)request didFailWithError:(NSError*)error
{
	NSLog( @"error description: %@", [error description] );
	NSLog( @"error userInfo: %@", [error userInfo] );
	
	// figure out what kind of post we are dealing with
	if( [request.url hasSuffix:@"me/friends"] ) // friend request
	{
		UnitySendMessage( "SocialNetworkingManager", "facebookFriendRequestDidFail", [[error localizedDescription] UTF8String] );
	}
	else if( [request.url hasSuffix:@"me"] ) // username request: special case can be both custom and username
	{
		UnitySendMessage( "SocialNetworkingManager", "facebookUsernameRequestDidFail", [[error localizedDescription] UTF8String] );
		UnitySendMessage( "SocialNetworkingManager", "facebookCustomRequestDidFail", [[error localizedDescription] UTF8String] );
	}
	else if( [request.url hasSuffix:@"me/feed"] ) // post to my wall request
	{
		UnitySendMessage( "SocialNetworkingManager", "facebookPostDidFail", [[error localizedDescription] UTF8String] );
	}
	else // custom request
	{
		UnitySendMessage( "SocialNetworkingManager", "facebookCustomRequestDidFail", [[error localizedDescription] UTF8String] );
	}
}


/**
 * Called when a request returns and its response has been parsed into an object.
 *
 * The resulting object may be a dictionary, an array, a string, or a number, depending
 * on thee format of the API response.
 */
- (void)request:(FBRequest*)request didLoad:(id)result
{
	// figure out what kind of post we are dealing with
	if( [request.url hasSuffix:@"me/friends"] ) // friend request
	{
		NSString *json = [[[NSString alloc] initWithData:request.responseText encoding:NSUTF8StringEncoding] autorelease];
		UnitySendMessage( "SocialNetworkingManager", "facebookDidReceiveFriends", [json UTF8String] );
	}
	else if( [request.url hasSuffix:@"me"] ) // username request: special case can be both custom and username
	{
		NSString *name = [result objectForKey:@"name"];
		UnitySendMessage( "SocialNetworkingManager", "facebookDidReceiveUsername", [name UTF8String] );

		NSString *json = [[[NSString alloc] initWithData:request.responseText encoding:NSUTF8StringEncoding] autorelease];
		UnitySendMessage( "SocialNetworkingManager", "facebookDidReceiveCustomRequest", [json UTF8String] );
	}
	else if( [request.url hasSuffix:@"me/feed"] ) // post to my wall request
	{
		UnitySendMessage( "SocialNetworkingManager", "facebookPostSucceeded", "" );
	}
	else // custom request
	{
		NSString *json = [[[NSString alloc] initWithData:request.responseText encoding:NSUTF8StringEncoding] autorelease];
		UnitySendMessage( "SocialNetworkingManager", "facebookDidReceiveCustomRequest", [json UTF8String] );
	}
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark FBDialogDelegate

/**
 * Called when the dialog succeeds and is about to be dismissed.
 */
- (void)dialogDidComplete:(FBDialog*)dialog
{
	UnitySendMessage( "SocialNetworkingManager", "facebookDialogDidComplete", "" );
}


- (void)dialogCompleteWithUrl:(NSURL*)url
{
	UnitySendMessage( "SocialNetworkingManager", "facebookDialogDidCompleteWithUrl", url.absoluteString.UTF8String );
}


/**
 * Called when the dialog is cancelled and is about to be dismissed.
 */
- (void)dialogDidNotComplete:(FBDialog*)dialog
{
	UnitySendMessage( "SocialNetworkingManager", "facebookDialogDidNotComplete", "" );
}


/**
 * Called when dialog failed to load due to an error.
 */
- (void)dialog:(FBDialog*)dialog didFailWithError:(NSError*)error
{
	NSLog( @"error description: %@", [error description] );
	NSLog( @"error userInfo: %@", [error userInfo] );
	
	UnitySendMessage( "SocialNetworkingManager", "facebookDialogDidFailWithError", [[error localizedDescription] UTF8String] );
}


@end





#import "AppController.h"


@implementation AppController(FacebookURLHandler)

- (BOOL)application:(UIApplication*)application handleOpenURL:(NSURL*)url
{
	BOOL res = [[FacebookManager sharedManager] handleOpenURL:url];

	return res;
}

@end


