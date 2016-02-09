using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WelcomeScreen : MonoBehaviour {
	public bool isServer; //Variable para saber si estamos en el cliente(false) o en el servidor (true)
	public Toggle checkbox; //Checkbox de la pantalla de incicio
	public InputField ipInput; //Input de la pantalla de inicio (para la ip)
	public Button iniciar;
	public Text waitingMessage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//Funcion que se llamara al presionar el boton de inicio en el menu principal
	// Creara el server ( o el cliente ) e inicia el juego
	public void startGame(){
		
		//Verificamos contenido del checkbox
		if (checkbox.isOn) {
			this.isServer = true;
		} 
		else {
			this.isServer = false;
		}
		
		
		if (isServer == true) {
			//Creamos servidor
			GameController.controller.server = new UDPServer();
			iniciar.interactable = false;
			waitingMessage.enabled = true;

			
			
		} 
		else {
			GameController.controller.client = new UDPClient(ipInput.text);
			
			
		}




		
		
		
	}



}
