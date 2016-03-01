using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
public class GameController : MonoBehaviour {


	public static GameController controller;
	//Server

	public bool onGame; //Variable para saber si ya ha iniciado el juego
	public bool isServer; //Variable para saber si estamos en el cliente(false) o en el servidor (true)
	public TCPServer tcpServer;
	public TCPClient tcpClient;
	public UDPClient clientUDP;//Client 
	public UDPServer serverUDP;
	public Text messages; //Mensajes de conexion en pantalla de inicion
	public bool mm2;
	/*--------------------Variables del juego-----------------------*/
	public GameObject player1;
	public GameObject player2;
	public Ship ship1;
	public Ship ship2;
	public float timeBetweenSpawn=0.05f;
	public GameObject borderLeft, borderRight, borderTop, borderDown;
	public bool gameOn,connected;
	public GameObject jugarDeNuevo,salir,listoText;
	public GameObject ready;
	public Boolean imReady,opponentReady;
	public MainGame mainGame;
	public bool p1Wins,p2Wins;

	void Awake(){
		Debug.Log ("NUEVO AWAKE");
		if (controller == null) {

			controller = this;
			DontDestroyOnLoad (gameObject);
		
		} else if(controller != this) {
			Debug.Log ("Destruyo el nuevo controller");
			Destroy(gameObject);
		}


	}
	// Use this for initialization
	void Start () {
		p1Wins = false;
		p2Wins = false;
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
		
		GameController.controller.gameOn = false;
		GameController.controller.connected = false;
		//Cerramos conexiones con cliente y servidor
		if (this.isServer == true) {
			Paquete p = new Paquete();
			p.identificadorPaquete = Paquete.Identificador.desconectar;
			GameController.controller.serverUDP.serverSocket.Close();
			GameController.controller.tcpServer.sendMessage(p);
			GameController.controller.tcpServer.CloseConnection();
			GameController.controller.tcpServer = null;
		} 
		else {
			Paquete p = new Paquete();
			p.identificadorPaquete = Paquete.Identificador.desconectar;
			GameController.controller.clientUDP.clientSocket.Close();
			GameController.controller.tcpClient.sendMessage(p);
			GameController.controller.tcpClient.CloseConnection();
			GameController.controller.tcpClient = null;
		
		}

		
		Application.LoadLevel (0);
	}
	public void MainMenu2(){
		
		gameOn = false;
		connected = false;
		if (this.isServer == true) {

			GameController.controller.serverUDP.serverSocket.Close();
			GameController.controller.tcpServer.CloseConnection();
			GameController.controller.tcpServer = null;
		} 
		else {

			GameController.controller.clientUDP.clientSocket.Close();
			GameController.controller.tcpClient.CloseConnection();
			GameController.controller.tcpClient = null;
		}
		Application.LoadLevel (0);
	}

	public void playerWins(int num){
		GameController.controller.imReady = false;
		GameController.controller.opponentReady = false;
		if (gameOn) {
			controller.gameOn = false;
			if(num==1){

				GameObject.Find("Player1").GetComponent<Ship>().stopMoving();
				GameObject.Find("Player2").GetComponent<Ship>().stopMoving();
			}
			else{
				GameObject.Find("Player2").GetComponent<Ship>().stopMoving();
				GameObject.Find("Player1").GetComponent<Ship>().stopMoving();
			}


			GameObject temp =GameObject.Find 	("Win");
			Text text = temp.GetComponent<Text> ();
			text.text = "Jugador "+num+" Gano!";
			text.enabled = true;
			jugarDeNuevo.SetActive (true);
			salir.SetActive (true);		
		
		}
		Paquete p = new Paquete ();
		p.identificadorPaquete = Paquete.Identificador.jugadorGana;
		p.jugador = num;
		GameController.controller.tcpServer.sendMessage (p);
	}
	public void playerWins2(int num){
		GameController.controller.imReady = false;
		GameController.controller.opponentReady = false;
		if (gameOn) {
			controller.gameOn = false;
			if(num==1){
				GameObject.Find("Player1").GetComponent<Ship>().stopMoving();
				GameObject.Find("Player2").GetComponent<Ship>().stopMoving();
				GameController.controller.ship2.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
			else{
				GameObject.Find("Player2").GetComponent<Ship>().stopMoving();
				GameObject.Find("Player1").GetComponent<Ship>().stopMoving();
				GameController.controller.ship1.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
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
			GameController.controller.connected=false;
			GameController.controller.gameOn=false;
		}	
		if (GameController.controller.imReady == true && GameController.controller.opponentReady == true) {
			Debug.Log("JUGADORES LISTOS");
			GameController.controller.mainGame.startGame();
			GameController.controller.imReady =false;
			GameController.controller.opponentReady = false;

		}
		if (mm2 == true) {
			MainMenu2();
			mm2 =false;
		}
		if (p1Wins == true) {
			GameController.controller.playerWins2(1);
			p1Wins = false;
		}
		if (p2Wins == true) {
			GameController.controller.playerWins2(2);
			p2Wins = false;
			
		}
	}

	
}
