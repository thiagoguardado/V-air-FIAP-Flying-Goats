using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphatext : MonoBehaviour {

	public Material material;

	// Use this for initialization
	void Start () {
		StartCoroutine (ChangeAlpha ());
	}
	
	// Update is called once per frame
	IEnumerator ChangeAlpha () {
		print (material.color.a);
		if (material.color.a > 0) {
			material.color = new Color (material.color.r, material.color.g, material.color.b, material.color.a - 0.01f);
			yield return new WaitForSeconds (0.05f);
			StartCoroutine (ChangeAlpha ());
		}
	}
}
