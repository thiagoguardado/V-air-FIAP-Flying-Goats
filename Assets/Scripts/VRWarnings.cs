using UnityEngine;
using System.Collections;

public class VRWarnings : MonoBehaviour
{

	public static void VooEmbarcando(){

		string title = "ATENÇÃO, " + PlayerDevice.currentUser.name ;
		string body = "Seu vôo de número " + PlayerDevice.currentUser.flight + " foi adiantado! \n Dirija-se imediatamente ao portão de embarque!";
		string footer = "Pressione VOLTAR para continuar!";

		string fullText = title + "_" + body + "_" + footer;

		VRControl.SetWarning (fullText,null,true,0f);

	}


}

