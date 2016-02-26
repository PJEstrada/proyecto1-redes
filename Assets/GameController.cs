﻿using UnityEngine;
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
		if (this.isServer == true) {
			GameController.controller.serverUDP.serverSocket.Close();
		
		} 
		else {
			GameController.controller.clientUDP.clientSocket.Close();
		
		}
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
			GameController.controller.connected=false;
			GameController.controller.gameOn=false;
		}	
		if (GameController.controller.imReady == true && GameController.controller.opponentReady == true) {
			Debug.Log("JUGADORES LISTOS");
			GameController.controller.mainGame.startGame();
			GameController.controller.imReady =false;
			GameController.controller.opponentReady = false;

		}

	}

	
}
