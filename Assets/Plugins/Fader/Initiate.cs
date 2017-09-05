using UnityEngine;
using System.Collections;

public static class Initiate {
    //Create Fader object and assing the fade scripts and assign all the variables
    
	public static Color fadeColor = Color.white;
	public static float damp = 2f;

	public static void FadeDefault(string scene){
		Fade (scene, fadeColor, damp);
	}

	public static void Fade (string scene, Color fadeColor, float damp){
		GameObject init = new GameObject ();
		init.name = "Fader";
		init.AddComponent<Fader> ();
		Fader scr = init.GetComponent<Fader> ();
		scr.fadeDamp = damp;
		scr.fadeScene = scene;
		scr.fadeColor = fadeColor;
		scr.start = true;
	}
}
