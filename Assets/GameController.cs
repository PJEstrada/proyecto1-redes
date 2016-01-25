using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
public class GameController : MonoBehaviour {


	public static GameController controller;
	//Server
	public UDPServer server;
	public bool onGame; //Variable para saber si ya ha iniciado el juego
	public bool isServer; //Variable para saber si estamos en el cliente(false) o en el servidor (true)
	public Toggle checkbox; //Checkbox de la pantalla de incicio
	public InputField ipInput; //Input de la pantalla de inicio (para la ip)
	public Text messages; //Mensajes de conexion en pantalla de inicion

	public UDPClient client;//Client  


	void Awake(){
		if (controller == null) {
			controller = this;

		
		} else {
			Destroy(gameObject);
		}


	}
	// Use this for initialization
	void Start () {
		//---------------------Prueba de XML
		/*Paquete p = new Paquete ();
		p.jugador = 6;
		string s;
		p.bullets.Add (new Bullet(0,1,1,1,1,1,1));
		p.bullets.Add (new Bullet (1,2, 2, 2, 2, 2, 2));

		s = p.GetDataStream ();

		Paquete p1 = new Paquete (s);*/
		//---------------------Fin Prueba de XML







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
			this.server = new UDPServer();


		 
		
		} 
		else {


		
		}
		
	}

	void Update () {
	
	}
}
