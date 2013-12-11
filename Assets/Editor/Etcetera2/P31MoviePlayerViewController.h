//
//

#import <UIKit/UIKit.h>
#import <MediaPlayer/MediaPlayer.h>


@protocol P31MoviePlayerViewControllerDelegate;

@interface P31MoviePlayerViewController : UIViewController 
{
@private
	MPMoviePlayerController *_moviePlayer;
	id<P31MoviePlayerViewControllerDelegate> _delegate;
	BOOL _supportLandscape;
	BOOL _supportPortrait;
}
@property (nonatomic, assign) id<P31MoviePlayerViewControllerDelegate>delegate;
@property (nonatomic) BOOL supportLandscape;
@property (nonatomic) BOOL supportPortrait;


- (id)initWithVideoFilePath:(NSString*)moveFilePath;

- (id)initWithVideoURL:(NSString*)movieUrl;

- (void)startPlaybackShowingControls:(BOOL)showControls;

@end




@protocol P31MoviePlayerViewControllerDelegate <NSObject>
- (void)moviePlayerControllerDidFinish:(P31MoviePlayerViewController*)player;
@end
