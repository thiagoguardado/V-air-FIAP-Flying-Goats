using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHTTP;

public class monitor : MonoBehaviour {

    string result = "[]";
	string[] id_list = {"1","2","3","4"};
	public string name, flight_number, age, sex;

	bool stop = false;

	public Text data;

	// Use this for initialization
	void Start () {
		StartCoroutine (Standby ());
	}

	//Faz a requisição de verificação, buscando pelo ID
	void Get (string id){
		Request verify = new Request( "get", string.Format("http://notepadsaas.herokuapp.com/nota/{0}", id));
		verify.Send( ( request ) => {
			result = request.response.Text;
		});
	}

	//Esse coroutine monitora se a API possui algum
	//json com um dos valores informados no id_list.

	//podemos colocar qualquer ID, basta enviar o mesmo ID
	//na requisição de envio (veja o active_user).
	//Também é necessário atribuir os valores aos usuários.
	IEnumerator Standby () {

		foreach(string id in id_list){
			Get (id);
			yield return new WaitForSeconds (2.0f);
			if (result != "[]") {
				stop = true;
				requester.Delete ();
				SearchForUser (id);
				break;
			} else {
				print ("waiting for player");
			}
		}
		if (!stop){
			StartCoroutine (Standby ());
		}

	}

	//Caso a pesquisa na API retorne algo,
	//o Standby guardará o ID atual e chamará
	//essa função para fazer um case e encontrar
	//o usuário com base no ID.

	//Podemos usar um banco local, não vejo necessidade.
	void SearchForUser (string id){
		print (id);
		switch (id) {
		case "1":
			name = "Rodrigo Rebelatto";
			age = "19";
			sex = "M";
			flight_number = "165";
			break;
		case "2":
			name = "Isabela Crush";
			age = "25";
			sex = "M";
			flight_number = "165";
			break;
		case "3":
			name = "Henrique Onox";
			age = "23";
			sex = "M";
			flight_number = "459";
			break;
		case "4":
			name = "Thiago Guardado";
			age = "25";
			sex = "M";
			flight_number = "362";
			break;
		}
		data.text = string.Format("{0}\n{1} anos\nVoo: {2}", name, age, flight_number);

	}
		
}
