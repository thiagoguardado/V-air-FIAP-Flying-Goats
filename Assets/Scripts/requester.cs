using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHTTP;

public class requester : MonoBehaviour {


	public static void Post (string id){
		Hashtable data = new Hashtable ();
		data.Add ("titulo", id);

		Request post = new Request ("post", "http://v-air.herokuapp.com/comm", data);
		post.Send (( request) => {
			print ("ID enviado");
		});
	}

	public static void Delete (){
		Request destroy = new Request ("delete", "http://v-air.herokuapp.com/comm");
		destroy.Send (( request) => {
			print ("Destruindo dados");
		});
	}

}
