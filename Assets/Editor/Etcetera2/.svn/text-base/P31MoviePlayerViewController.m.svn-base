

#import "P31MoviePlayerViewController.h"

@implementation P31MoviePlayerViewController

@synthesize supportLandscape = _supportLandscape, supportPortrait = _supportPortrait, delegate = _delegate;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

- (id)initWithVideoFilePath:(NSString*)moveFilePath
{
	// Initialize and create movie URL
	if( ( self = [super init] ) )
	{
		_supportLandscape = YES;
		NSURL *movieURL = [NSURL fileURLWithPath:moveFilePath];
		_moviePlayer = [[MPMoviePlayerController alloc] initWithContentURL:movieURL];
	}
	return self;
}


- (id)initWithVideoURL:(NSString*)movieUrl
{
	// Initialize and create movie URL
	if( ( self = [super init] ) )
	{
		_supportLandscape = YES;
		NSURL *url = [NSURL URLWithString:movieUrl];
		_moviePlayer = [[MPMoviePlayerController alloc] initWithContentURL:url];
	}
	return self;
}


- (void)dealloc
{
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	[_moviePlayer release];
	
	[super dealloc];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark UIViewController

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation
{
	if( _supportLandscape && UIInterfaceOrientationIsLandscape( toInterfaceOrientation ) )
		return YES;
	
	if( _supportPortrait && UIInterfaceOrientationIsPortrait( toInterfaceOrientation ) )
		return YES;

	return NO;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startPlaybackShowingControls:(BOOL)showControls
{
	if( [_moviePlayer respondsToSelector:@selector(setFullscreen:animated:)] )
	{
		// Use the new 3.2 style API
		_moviePlayer.controlStyle = showControls ? MPMovieControlStyleFullscreen : MPMovieControlStyleNone;
		_moviePlayer.shouldAutoplay = YES;
		[self.view addSubview:_moviePlayer.view];
		[_moviePlayer setFullscreen:YES animated:NO];
	}
	else
	{  
		// Use the old 2.0 style API  
		_moviePlayer.movieControlMode = showControls ? MPMovieControlModeDefault : MPMovieControlModeHidden;
		[_moviePlayer play];
	}

	// Register to receive a notification when the movie has finished playing.
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(moviePlayBackDidFinish:)
												 name:MPMoviePlayerPlaybackDidFinishNotification
											   object:nil];
}


#pragma mark NSNotification

- (void)moviePlayBackDidFinish:(NSNotification*)notification
{
	[[UIApplication sharedApplication] setStatusBarHidden:YES];
	
	// If the moviePlayer.view was added to the view, it needs to be removed  
	if( [_moviePlayer respondsToSelector:@selector(setFullscreen:animated:)] )
		[_moviePlayer.view removeFromSuperview];
	
	// Remove observer
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	
	[_delegate moviePlayerControllerDidFinish:self];
}

@end
