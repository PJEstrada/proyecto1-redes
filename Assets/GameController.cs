using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
public class GameController : MonoBehaviour {


	public static GameController controller;
	//Server
	public UDPServer server;
	public bool onGame; //Variable para saber si ya ha iniciado el juego


	public Text messages; //Mensajes de conexion en pantalla de inicion
	public UDPClient client;//Client  
	/*--------------------Variables del juego-----------------------*/
	public GameObject player1;
	public GameObject player2;
	public float timeBetweenSpawn=0.05f;
	public GameObject borderLeft, borderRight, borderTop, borderDown;
	public bool gameOn,connected;
	public GameObject jugarDeNuevo,salir;
	GameObject ready;
	GameObject listoText;
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
	public void MainGame(){
		if (GameController.controller.connected == true) {
			Application.LoadLevel (1);
			
		}

	}
	
	public void MainMenu(){
		
		gameOn = false;
		connected = false;
		//Cerramos conexiones con cliente y servidor
		
		//... To Do
		
		Application.LoadLevel (0);
	}

	public void playerWins(int num){
		if (gameOn) {
			controller.gameOn = false;
			if(num==1){
				GameObject.Find("Player1").GetComponent<Ship>().stopMoving();

			}
			else{
				GameObject.Find("Player2").GetComponent<Ship>().stopMoving();

			}


			GameObject temp =GameObject.Find 	("Win");
			Text text = temp.GetComponent<Text> ();
			text.text = "Jugador "+num+" Gano!";
			text.enabled = true;
			jugarDeNuevo.SetActive (true);
			salir.SetActive (true);		
		
		}

	}

	void Update () {
		if (GameController.controller.connected == true) {
			Application.LoadLevel (1);

			ready = GameObject.Find ("Ready");
			listoText = GameObject.Find ("Listo");
			listoText.SetActive (false);
			jugarDeNuevo = GameObject.Find ("JugarDeNuevo");
			salir = GameObject.Find ("Salir");
			jugarDeNuevo.SetActive (false);
			salir.SetActive (false);			
		}	

	}


	public void playerReady(){
		//Eliminando texto 
		GameObject temp =GameObject.Find 	("Win");
		Text text = temp.GetComponent<Text> ();
		text.enabled = false;
		//Escondiendo botones y textos
		listoText.SetActive (false);
		
		salir.SetActive (false);
		jugarDeNuevo.SetActive (false);
		//Reactivando Naves
		player1.SetActive (true);
		player2.SetActive (true);



		ready.GetComponent<Image>().color = Color.green;
		gameOn = true;
		player1.GetComponent<Ship>().facing ="front";
		player2.GetComponent<Ship>().facing ="down";
		player2.transform.position =new Vector3 (-50,0,0);
		player1.transform.position =new Vector3 (50,0,0);
		player1.transform.eulerAngles = new Vector3(0,0,0);
		player2.transform.eulerAngles = new Vector3(0,0,180);

	

		player1.GetComponent<Ship> ().startMoving ();
		player2.GetComponent<Ship> ().startMoving ();
		ready.SetActive (false);


	}

}
