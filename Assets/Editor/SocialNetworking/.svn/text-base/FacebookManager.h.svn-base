//
//  FacebookManager.h
//  Facebook
//
//  Created by Mike on 9/13/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "FBConnect.h"



extern NSString *const kFacebookAppIdKey;


@interface FacebookManager : NSObject <FBSessionDelegate, FBRequestDelegate, FBDialogDelegate>
{
	Facebook *_facebook;
	NSString *_appId;
}
@property (nonatomic, copy) NSString *appId;


+ (FacebookManager*)sharedManager;

- (void)setCredentialsIfAvailable;

- (void)saveCredentials;



- (BOOL)isLoggedIn;

- (NSString*)accessToken;

- (void)extendAccessToken;

- (void)login;

- (void)loginWithRequestedPermissions:(NSArray*)permissions;

- (void)logout;

- (void)getLoggedInUsername;

- (void)postMessage:(NSString*)message;

- (void)postMessage:(NSString*)message link:(NSString*)link linkName:(NSString*)linkName;

- (void)postMessage:(NSString*)message link:(NSString*)link linkName:(NSString*)linkName linkToImage:(NSString*)linkToImage caption:(NSString*)caption;

- (void)postPhoto:(NSString*)path caption:(NSString*)caption;

- (void)postPhotoInAlbum:(NSString*)path caption:(NSString*)caption albumId:(NSString*)albumId;

- (void)showPostMessageDialog;

- (void)postMessageDialogWithLink:(NSString*)link linkName:(NSString*)linkName linkToImage:(NSString*)linkToImage caption:(NSString*)caption;

- (void)postMessageDialogWithDict:(NSMutableDictionary*)dict;

- (void)showDialog:(NSString*)dialogType withParms:(NSMutableDictionary*)dict;

- (void)postPhoto:(NSString*)path caption:(NSString*)caption;

- (void)getFriends;

- (void)requestWithGraphPath:(NSString*)path httpMethod:(NSString*)method params:(NSMutableDictionary*)params;

- (void)requestWithRestMethod:(NSString*)restMethod httpMethod:(NSString*)method params:(NSMutableDictionary*)params;

@end
