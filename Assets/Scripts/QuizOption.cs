﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;


// This script is for loading scenes from the main menu.
// Each 'button' will be a rendering showing the scene
// that will be loaded and use the SelectionRadial.
public class QuizOption : MonoBehaviour
{
	public event Action<QuizOption> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.

	[SerializeField] private QuizController quizController;
	[SerializeField] private QuizQuestion.QuizAnswer correctAnswer;       // The name of the scene to load.
	[SerializeField] private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
	[SerializeField] private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.


	private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.


	private void OnEnable ()
	{
		m_InteractiveItem.OnOver += HandleOver;
		m_InteractiveItem.OnOut += HandleOut;
		m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
	}


	private void OnDisable ()
	{
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
		m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
	}


	private void HandleOver()
	{
		// When the user looks at the rendering of the scene, show the radial.
		m_SelectionRadial.Show();

		m_GazeOver = true;
	}


	private void HandleOut()
	{
		// When the user looks away from the rendering of the scene, hide the radial.
		m_SelectionRadial.Hide();

		m_GazeOver = false;
	}


	private void HandleSelectionComplete()
	{
		// If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
		if(m_GazeOver)
			StartCoroutine (ActivateButton());
	}


	private IEnumerator ActivateButton()
	{

		// If anything is subscribed to the OnButtonSelected event, call it.
		if (OnButtonSelected != null)
			OnButtonSelected(this);


		// Send 
		quizController.SelectAnswer(correctAnswer);

		yield return null;
	}
}