//
//
//  Created by Mike on 10/3/10.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>


@protocol ExternalScreenManagerDelegate;

@interface ExternalScreenManager : NSObject
{
@private
	BOOL running;
}
@property (nonatomic, assign) id<ExternalScreenManagerDelegate> delegate;
@property (nonatomic, retain) UIColor *backgroundColor;
@property (nonatomic, assign) int targetFPS;
@property (nonatomic, assign) CGFloat scale;


- (void)listenForExternalScreens;

- (void)stop;


@end



@protocol ExternalScreenManagerDelegate <NSObject>
- (void)screenManagerDidStartMirroring;
- (void)screenManagerDidStopMirroring;
@end
