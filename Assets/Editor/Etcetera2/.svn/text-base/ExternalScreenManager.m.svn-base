//
//
//  Created by Mike on 10/3/10.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//


#import <QuartzCore/QuartzCore.h>
#import <OpenGLES/ES1/glext.h>
#import "ExternalScreenManager.h"



@interface ExternalScreenManager()
@property (nonatomic, retain) UIWindow *originalWindow;
@property (nonatomic, retain) UIWindow *externalWindow;
@property (nonatomic, retain) UIImageView *imageView;
@property (nonatomic, retain) NSTimer *timer;
@property (nonatomic, assign) UIView *eaglView;
@end



@implementation ExternalScreenManager

@synthesize delegate, backgroundColor = _backgroundColor, targetFPS = _targetFPS, scale = _scale, originalWindow, externalWindow, imageView, timer, eaglView;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

- (id)init
{
    if( ( self = [super init] ) )
	{
		self.backgroundColor = [UIColor blackColor];
		_targetFPS = 15;
		self.scale = 1.0f;
		
		// locate the EAGLView and keep a reference to it (assigned!)
		// we dont have access to the EAGLView class, so create it from a string
		Class eaglClass = NSClassFromString( @"EAGLView" );
		if( !eaglClass )
			return nil;
		
		for( UIView *view in [UIApplication sharedApplication].keyWindow.subviews )
		{
			if( [view isKindOfClass:eaglClass] )
			{
				eaglView = view;
				break;
			}
		}
		
		// no eaglView, no TV!
		if( !eaglView )
			return nil;
    }
    return self;
}


- (void)dealloc
{
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	
	// just in case we are still running.  this will also free the timer
	[self stop];
	
	[imageView release];
	[originalWindow release];
	[externalWindow release];
	[_backgroundColor release];
	
	[super dealloc];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (CGRect)rotatedWindowBounds
{
	UIDeviceOrientation orientation = [[UIDevice currentDevice] orientation];
	if( orientation == UIDeviceOrientationFaceUp || orientation == UIDeviceOrientationFaceDown )
		orientation = [UIApplication sharedApplication].statusBarOrientation;
	
	if( orientation == UIDeviceOrientationLandscapeLeft || orientation == UIDeviceOrientationLandscapeRight )
	{
		CGRect windowBounds = imageView.window.bounds;
		return CGRectMake( 0, 0, windowBounds.size.height, windowBounds.size.width );
	}
	
	return imageView.window.bounds;
}


- (CGAffineTransform)transformForDeviceOrientation:(UIDeviceOrientation)orient
{
	CGAffineTransform transform;
	
	if( orient == UIDeviceOrientationLandscapeLeft )
		transform = CGAffineTransformRotate( CGAffineTransformIdentity, M_PI * 1.5 );
	else if( orient == UIDeviceOrientationLandscapeRight )
		transform = CGAffineTransformRotate( CGAffineTransformIdentity, M_PI * -1.5 );
	else if( orient == UIDeviceOrientationPortraitUpsideDown )
		transform = CGAffineTransformRotate( CGAffineTransformIdentity, M_PI );
	else
		transform = CGAffineTransformIdentity;
	
	if( _scale != 1.0 )
		transform = CGAffineTransformScale( transform, _scale, _scale );
	
	return transform;
}


- (UIImage*)snapshot
{
    GLint backingWidth, backingHeight;
	
    // Get the size of the backing CAEAGLLayer
    glGetRenderbufferParameterivOES( GL_RENDERBUFFER_OES, GL_RENDERBUFFER_WIDTH_OES, &backingWidth );
    glGetRenderbufferParameterivOES( GL_RENDERBUFFER_OES, GL_RENDERBUFFER_HEIGHT_OES, &backingHeight );
	
    NSInteger x = 0, y = 0, width = backingWidth, height = backingHeight;
    NSInteger dataLength = width * height * 4;
    GLubyte *data = (GLubyte*)malloc( dataLength * sizeof(GLubyte) );
	
    // Read pixel data from the framebuffer
    glPixelStorei( GL_PACK_ALIGNMENT, 4 );
    glReadPixels( x, y, width, height, GL_RGBA, GL_UNSIGNED_BYTE, data );
	
    // Create a CGImage with the pixel data
    // If your OpenGL ES content is opaque, use kCGImageAlphaNoneSkipLast to ignore the alpha channel
    // otherwise, use kCGImageAlphaPremultipliedLast
    CGDataProviderRef ref = CGDataProviderCreateWithData( NULL, data, dataLength, NULL );
    CGColorSpaceRef colorspace = CGColorSpaceCreateDeviceRGB();
    CGImageRef iref = CGImageCreate( width, height, 8, 32, width * 4, colorspace, kCGBitmapByteOrder32Big | kCGImageAlphaPremultipliedLast,
                                    ref, NULL, true, kCGRenderingIntentDefault );
	
    // OpenGL ES measures data in PIXELS
    // Create a graphics context with the target size measured in POINTS
    NSInteger widthInPoints, heightInPoints;
    if( NULL != UIGraphicsBeginImageContextWithOptions )
	{
        // On iOS 4 and later, use UIGraphicsBeginImageContextWithOptions to take the scale into consideration
        // Set the scale parameter to your OpenGL ES view's contentScaleFactor
        // so that you get a high-resolution snapshot when its value is greater than 1.0
        CGFloat scale = eaglView.contentScaleFactor;
        widthInPoints = width / scale;
        heightInPoints = height / scale;
        UIGraphicsBeginImageContextWithOptions( CGSizeMake( widthInPoints, heightInPoints ), NO, scale );
    }
    else
	{
        // On iOS prior to 4, fall back to use UIGraphicsBeginImageContext
        widthInPoints = width;
        heightInPoints = height;
        UIGraphicsBeginImageContext( CGSizeMake( widthInPoints, heightInPoints ) );
    }
	
    CGContextRef cgcontext = UIGraphicsGetCurrentContext();
	
    // UIKit coordinate system is upside down to GL/Quartz coordinate system
    // Flip the CGImage by rendering it to the flipped bitmap context
    // The size of the destination area is measured in POINTS
    CGContextSetBlendMode( cgcontext, kCGBlendModeCopy );
    CGContextDrawImage( cgcontext, CGRectMake( 0.0, 0.0, widthInPoints, heightInPoints ), iref );
	
    // Retrieve the UIImage from the current context
    UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
	
    UIGraphicsEndImageContext();
	
    // Clean up
    free( data );
    CFRelease( ref );
    CFRelease( colorspace );
    CGImageRelease( iref );
	
    return image;
}


- (void)updateImageView
{
	imageView.image = [self snapshot];
}


- (void)start
{
	if( running )
		return;
	
	NSArray *screens = [UIScreen screens];
	if( screens.count < 2 )
	{
		NSLog( @"no external screens detected!" );
		return;	
	}
	
	running = YES;
	[delegate screenManagerDidStartMirroring];
	
	// save our main device window for later making it key when we are done
	self.originalWindow = [[UIApplication sharedApplication] keyWindow];
	
	CGFloat largestWidth = 0;;
	UIScreenMode *screenMode = nil;
	UIScreen *externalScreen = [screens objectAtIndex:1];
	for( UIScreenMode *mode in [externalScreen availableModes] )
	{
		if( mode.size.width > largestWidth )
		{
			largestWidth = mode.size.width;
			screenMode = mode;
		}
	}
	externalScreen.currentMode = screenMode;
	
	UIDeviceOrientation orient = [UIDevice currentDevice].orientation;
	
	// create our window at the same size as our current mode
	externalWindow = [[UIWindow alloc] initWithFrame:externalScreen.bounds]; //CGRectMake( 0, 0, screenMode.size.width, screenMode.size.height )
	externalWindow.userInteractionEnabled = NO;
	externalWindow.screen = externalScreen;
	externalWindow.backgroundColor = _backgroundColor;
	
	// create an imageView to display the screen captures
	imageView = [[UIImageView alloc] initWithFrame:externalWindow.frame];
	imageView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
	imageView.contentMode = UIViewContentModeScaleAspectFit;
	[externalWindow addSubview:imageView];
	
	// make our external window visible
	externalWindow.hidden = NO;
	[externalWindow makeKeyAndVisible];

	// set the initial transform of the imageView so it is displayed properly
	imageView.bounds = [self rotatedWindowBounds];
	imageView.transform = [self transformForDeviceOrientation:orient];
	
	// give key status back to our originalWindow so that it can display and receive touches
	[originalWindow makeKeyAndVisible];

	// kick off pseudo rendering
	self.timer = [NSTimer scheduledTimerWithTimeInterval:( 1.0 / _targetFPS )
												  target:self
												selector:@selector(updateImageView)
												userInfo:nil
												 repeats:YES];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSNotifications

- (void)screenDidConnect:(NSNotification*)notification
{
	[self start];
}


- (void)screenDidDisconnect:(NSNotification*)notification
{
	[self stop];
}


- (void)screenModeDidChange:(NSNotification*)notification
{
	[self start];
}


- (void)deviceOrientationDidChange:(NSNotification*)notification
{
	// early out if we arent up and running yet
	if( imageView == nil )
		return;
	
	UIDeviceOrientation orient = [UIDevice currentDevice].orientation;
	
	// early out for crap orientaitons
	if( orient == UIDeviceOrientationUnknown || orient == UIDeviceOrientationFaceDown || orient == UIDeviceOrientationFaceUp )
		return;
	
	// reorient the imageView
	[UIView beginAnimations:nil context:nil];
	imageView.bounds = [self rotatedWindowBounds];
	imageView.transform = [self transformForDeviceOrientation:orient];
	[UIView commitAnimations];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)listenForExternalScreens
{
	// listen to screen change notifications
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(screenDidConnect:)
												 name:UIScreenDidConnectNotification
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(screenDidDisconnect:)
												 name:UIScreenDidDisconnectNotification
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(screenModeDidChange:)
												 name:UIScreenModeDidChangeNotification
											   object:nil];
	
	// listen to device orientation changes so we can rotate the image on the second screen
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(deviceOrientationDidChange:)
												 name:UIDeviceOrientationDidChangeNotification
											   object:nil];
	[[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
	
	// if we already have an external screen ready, start it up
	if( [UIScreen screens].count > 1 )
		[self start];
}


- (void)stop
{
	if( timer )
	{
		[timer invalidate];
		self.timer = nil;
	}

	// clean up
	self.externalWindow = nil;
	self.imageView = nil;
	
	running = NO;
	[delegate screenManagerDidStopMirroring];
}


- (void)setBackgroundColor:(UIColor*)color
{
	[_backgroundColor release];
	_backgroundColor = [color retain];
	
	if( externalWindow )
	{
		externalWindow.backgroundColor = _backgroundColor;
	}
}


- (void)setScale:(CGFloat)newScale
{
	if( newScale < 0.4 || newScale > 1.5 )
		newScale = 1.0f;
	_scale = newScale;
	
	if( imageView )
	{
		[UIView beginAnimations:nil context:nil];
		imageView.transform = [self transformForDeviceOrientation:[UIDevice currentDevice].orientation];
		[UIView commitAnimations];
	}
}


- (void)setTargetFPS:(int)newFPS
{
	if( _targetFPS != newFPS )
	{
		_targetFPS = newFPS;
		
		if( self.timer && [self.timer isValid] )
		{
			[self.timer invalidate];
			self.timer = nil;
			
			self.timer = [NSTimer scheduledTimerWithTimeInterval:( 1.0 / _targetFPS )
														  target:self
														selector:@selector(updateImageView)
														userInfo:nil
														 repeats:YES];
		}
	}
}


@end
