using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	
	public PlayHardwareMovieClassPro[] movieClass;
	public string[] movieName;

	private int _currentMovieIndex = -1;
	
	void Start()
	{
		Home.onClickedHomeButton += stopMovie;
	}
	
	public void playMovie(int index)
	{
		stopMovie();

		_currentMovieIndex = index;
		
		ForwardiOSMessages.movie.Add(movieClass[index]);
		
		movieClass[index].PlayMovie(movieName[index]);
		movieClass[index].AudioLevel(.75f);
	}
	
	public void stopMovie()
	{
		if(_currentMovieIndex != -1)
		{
			movieClass[_currentMovieIndex].StopMovie();
			movieClass[_currentMovieIndex].AudioLevel(0f);
			
			ForwardiOSMessages.movie.Clear();
			
			_currentMovieIndex = -1;
		}
	}
}