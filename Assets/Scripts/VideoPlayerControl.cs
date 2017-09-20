using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerControl : MonoBehaviour {


	private VideoPlayer videoPlayer;

	void Awake() {
	
		videoPlayer = GetComponent<VideoPlayer> ();
		videoPlayer.loopPointReached += VideoEndReached;
			
	}


	public void PauseVideo(){
	
	
		videoPlayer.Pause ();
	
	}


	public void UnPauseVideo(){


		videoPlayer.Play ();

	}


	// when video finished
	void VideoEndReached(UnityEngine.Video.VideoPlayer vp) {

		vp.Pause ();


		// back to menu screen
		VRControl.BackToMenu();
		
	}

}
