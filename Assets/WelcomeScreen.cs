using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
public class WelcomeScreen : MonoBehaviour {

	public Toggle checkbox; //Checkbox de la pantalla de incicio
	public static InputField ipInput; //Input de la pantalla de inicio (para la ip)
	public Button iniciar;
	public Text waitingMessage;
	// Use this for initialization
	void Start () {
		ipInput = GameObject.Find ("InputField").GetComponent<InputField> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//Funcion que se llamara al presionar el boton de inicio en el menu principal
	// Creara el server ( o el cliente ) e inicia el juego
	public void startGame(){
		
		//Verificamos contenido del checkbox
		if (checkbox.isOn) {
			GameController.controller.isServer = true;

		} 
		else {
			GameController.controller.isServer = false;
			Debug.Log ("SOY CLIENTE");
		}
		if (GameController.controller.isServer == true) {
			//Creamos servidor
			GameController.controller.server = new UDPServer();
			GameController.controller.tcpServer = new TCPServer();
			Debug.Log ("CREADO EL SERVER");
			iniciar.interactable = false;
			waitingMessage.enabled = true;	
			
		} 
		else {
			try{
				GameController.controller.client = new UDPClient(ipInput.text);
				GameController.controller.tcpClient = new TCPClient();

				waitingMessage.text = "Esperando respuesta del servidor.";
				waitingMessage.enabled =true;
			}
			catch (FormatException e){
				waitingMessage.text = "La ip ingresada no es correcta. Intenta con otra IP.";
				waitingMessage.enabled =true;
				Debug.LogException(e);

			}
			catch(SocketException e){
				waitingMessage.text = "Error de conexion al servidor, verifica que el servidor este corriendo.";
				waitingMessage.enabled =true;
				Debug.LogException(e);
			}
		}
		
	}



}
