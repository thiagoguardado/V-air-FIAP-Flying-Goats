using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHTTP;

public class requester : MonoBehaviour {


	public static void Post (string id){
		Hashtable data = new Hashtable ();
		data.Add ("titulo", id);

		Request post = new Request ("post", "http://notepadsaas.herokuapp.com/nota", data);
		post.Send (( request) => {
			print ("ID enviado");
		});
	}

	public static void Delete (){
		Request destroy = new Request ("delete", "http://notepadsaas.herokuapp.com/nota");
		destroy.Send (( request) => {
			print ("Destruindo dados");
		});
	}

}
