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
	public Image imageNave;
	public Image imageNave2;
	public Text waitingMessage;
	public Sprite spr;
	// Use this for initialization
	void Start () {
		ipInput = GameObject.Find ("InputField").GetComponent<InputField> ();
		imageNave2 = GameObject.Find ("panelNave2").GetComponent<Image> ();
		imageNave2.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void drawShip(){
		imageNave = GameObject.Find ("panelNave").GetComponent<Image> ();
		imageNave2 = GameObject.Find ("panelNave2").GetComponent<Image> ();
		Debug.Log ("hola que hace");
		if (checkbox.isOn) {
			Debug.Log ("hola que hace 1");
			imageNave.enabled = true;
			imageNave2.enabled = false;
		} else {
			Debug.Log ("hola que hace 2");
			imageNave.enabled = false;
			imageNave2.enabled = true;
		}
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
			GameController.controller.serverUDP = new UDPServer();
			GameController.controller.tcpServer = new TCPServer();
			Debug.Log ("CREADO EL SERVER");
			iniciar.interactable = false;
			waitingMessage.enabled = true;	
			
		} 
		else {
			try{
				GameController.controller.clientUDP = new UDPClient(ipInput.text);
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
