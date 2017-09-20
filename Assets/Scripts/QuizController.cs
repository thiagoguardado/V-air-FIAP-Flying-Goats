using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour {

	// references
	public Renderer question;
	public Renderer succeed;
	public Renderer fail;
	public Renderer answerA;
	public Renderer answerB;
	public Renderer answerC;
	public Renderer answerD;
	private Renderer[] listaRend;

	// questions
	public QuizQuestion[] allQuestions;

	private QuizQuestion.QuizAnswer correctAnswer;

	public FollowCamera panelsFollowCamera;


	void Awake(){

		List<Renderer> lista = new List<Renderer> ();
		lista.Add (question);
		lista.Add (succeed);
		lista.Add (fail);
		lista.Add (answerA);
		lista.Add (answerB);
		lista.Add (answerC);
		lista.Add (answerD);
		listaRend = lista.ToArray ();


		OpaqueAll ();

	}


	void Start(){
	
		LoadQuestion ();
		StartCoroutine (StartQuestion (10.0f,0.5f,1.0f,0.5f));

	}


	public void SelectAnswer(QuizQuestion.QuizAnswer answer){
	
		if (answer != correctAnswer) {
		
			WrongAnswer ();
		
		} else {
		
			CorrectAnswer ();
		
		}

	
	}


	private void LoadQuestion(){

		// select random
		QuizQuestion question = allQuestions [Random.Range (0, allQuestions.Length)];

		// change textures
		answerA.material.SetTexture ("_MainTex", question.answerA);
		answerB.material.SetTexture ("_MainTex", question.answerB);
		answerC.material.SetTexture ("_MainTex", question.answerC);
		answerD.material.SetTexture ("_MainTex", question.answerD);
		correctAnswer = question.correctAnswer;

	}


	void OpaqueAll(){



		for (int i = 0; i < listaRend.Length; i++) {
			Color original = listaRend [i].material.color;
			Color newColor = new Color(original.r,original.g, original.b, 0);
			listaRend [i].material.color = newColor;
		}
			

	}


	IEnumerator StartQuestion(float waitInitial, float questionDuration, float waitAfter, float answersDuration){

		yield return new WaitForSeconds (waitInitial);

		yield return StartCoroutine(FromAlphaToOpaque (question, questionDuration));

		yield return new WaitForSeconds (waitAfter);

		panelsFollowCamera.enabled = false;

		StartCoroutine(FromAlphaToOpaque (answerA, questionDuration));
		StartCoroutine(FromAlphaToOpaque (answerB, questionDuration));
		StartCoroutine(FromAlphaToOpaque (answerC, questionDuration));
		StartCoroutine(FromAlphaToOpaque (answerD, questionDuration));

	}




	IEnumerator FromAlphaToOpaque(Renderer rend, float atime){

		for (float t = 0.0f; t < 1f; t += Time.deltaTime/atime)
		{
			Color newColor = new Color(rend.material.color.r, rend.material.color.g,rend.material.color.b, Mathf.Lerp(0f,1f,t));
			rend.material.color = newColor;
			yield return null;
		}
	
	
	}


	IEnumerator EndScene(float wait){
	
		yield return new WaitForSeconds (wait);

		Initiate.FadeDefault ("VR_MainMenu");
	
	}


	void CorrectAnswer ()
	{
		OpaqueAll ();

		panelsFollowCamera.enabled = true;

		StartCoroutine(FromAlphaToOpaque (succeed, 1f));

		StartCoroutine (EndScene(5f));

	}


	void WrongAnswer ()
	{
		OpaqueAll ();

		panelsFollowCamera.enabled = true;

		StartCoroutine(FromAlphaToOpaque (fail, 1f));

		StartCoroutine (EndScene(5f));
	}


}


[System.Serializable]
public class QuizQuestion {

	[System.Serializable]
	public enum QuizAnswer
	{
		A,
		B,
		C,
		D
	}
	public Texture question;
	public Texture answerA;
	public Texture answerB;
	public Texture answerC;
	public Texture answerD;
	public QuizAnswer correctAnswer;
	public int points;

}
