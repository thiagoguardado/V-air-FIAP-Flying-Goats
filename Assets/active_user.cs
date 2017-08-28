using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class active_user : MonoBehaviour {


	public string id = "";

	void Start(){
		StartCoroutine(postID ());
	}


	//Esse coroutine verifica se a variável id
	//possui algo. Se não possuir, eu envio um delete
	//para a API apagar todos os dados salvos.
	//Esse delte garante que a gente não guarde dois IDs
	//de uma vez, ou que a API fique sobrecarregada por
	//requests de outras pessoas (eu deleto tudo dela).

	//Se o id tiver algo, ele faz um POST para API
	//criar um objeto com esse ID.
	IEnumerator postID (){

		if (id == "") {
			requester.Delete ();
			yield return new WaitForSeconds (1.0f);
			StartCoroutine (postID ());

		} else {
			requester.Post (id);
		}
	}
}
