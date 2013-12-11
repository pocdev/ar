//
//  EtceteraTwoManager.h
//  Unity-iPhone
//
//  Created by Mike on 4/8/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "P31MoviePlayerViewController.h"
#import "ExternalScreenManager.h"


@interface EtceteraTwoManager : NSObject <P31MoviePlayerViewControllerDelegate, ExternalScreenManagerDelegate>
{

}
@property (nonatomic, retain) ExternalScreenManager *screenManager;


+ (EtceteraTwoManager*)sharedManager;


- (void)startListeningForExternalScreensWithFPS:(int)fps;

- (void)stopExternalScreenMirroring;

- (void)setExternalScreenMirorringBackgroundColor:(uint)color;

- (void)setExternalScreenMirorringScale:(CGFloat)scale;

- (void)scheduleNotificationOn:(NSDate*)fireDate
						  text:(NSString*)text
						action:(NSString*)action
						 sound:(NSString*)soundfileName
				   launchImage:(NSString*)launchImage
					badgeCount:(int)badgeCount;

- (void)cancelAllLocalNotifications;

- (void)playMovieAtUrl:(NSString*)url showControls:(BOOL)showControls supportLandscape:(BOOL)landscape supportPortrait:(BOOL)portrait;

// Crack detection
- (BOOL)isJailBroken;

- (BOOL)isInfoPlistPatched;

- (BOOL)isCracked:(long)filesize;

- (long)infoPlistFileSize;

- (BOOL)addSkipBackupAttributeToItemAtURL:(NSURL*)URL;

@end
