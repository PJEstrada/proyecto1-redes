using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGame : MonoBehaviour {
	
	
	public GameObject salir,listoText,ready,jugarDeNuevo,esperandoOponente;
	
	
	// Use this for initialization
	void Start () {
		GameController.controller.connected=false;
		
		GameController.controller.listoText = listoText;
		GameController.controller.salir = salir;
		GameController.controller.ready = ready;
		GameController.controller.jugarDeNuevo = jugarDeNuevo;
		
		salir.SetActive (false);
		jugarDeNuevo.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void startGame(){
		Debug.Log ("Inciando Juego");
		esperandoOponente.SetActive (false); 
		GameController.controller.gameOn=	true;
		GameController.controller.player1.GetComponent<Ship>().facing ="front";
		GameController.controller.player2.GetComponent<Ship>().facing ="down";
		GameController.controller.player2.transform.position =new Vector3 (-50,0,0);
		GameController.controller.player1.transform.position =new Vector3 (50,0,0);
		GameController.controller.player1.transform.eulerAngles = new Vector3(0,0,0);
		GameController.controller.player2.transform.eulerAngles = new Vector3(0,0,180);
		GameController.controller.player1.GetComponent<Ship> ().startMoving ();
		GameController.controller.player2.GetComponent<Ship> ().startMoving ();
		ready.GetComponent<Button> ().enabled = false;
		ready.GetComponent<Image> ().enabled = false;
		
	}
	public void playerReady(){
		GameController.controller.player1 = GameObject.Find ("Player1");
		GameController.controller.player2 = GameObject.Find ("Player2");
		GameController.controller.ship1 = GameController.controller.player1.GetComponent<Ship> ();
		GameController.controller.ship2 = GameController.controller.player2.GetComponent<Ship> ();
		GameController.controller.ship1.player = 1;
		GameController.controller.ship2.player = 2;
		//Eliminando texto 
		GameController.controller.mainGame = GameObject.Find ("bg").GetComponent<MainGame> ();
		GameObject temp = GameObject.Find ("Win");
		Text text = temp.GetComponent<Text> ();
		text.enabled = false;
		//Escondiendo botones y textos
		listoText.GetComponent<Text> ().enabled = false;
		salir.SetActive (false);
		jugarDeNuevo.SetActive (false);
		//Reactivando Naves
		
		
		GameController.controller.player1.GetComponent<SpriteRenderer> ().enabled = true;
		GameController.controller.player2.GetComponent<SpriteRenderer> ().enabled = true;
		
		esperandoOponente.SetActive (true);
		
		ready.GetComponent<Image> ().color = Color.green;
		
		if (GameController.controller.isServer == true) {
			
			GameController.controller.imReady = true;
			//Enviamos mensaje de jugador listo a cliente
			Paquete p = new Paquete ();
			p.identificadorPaquete = Paquete.Identificador.jugadorListo;
			GameController.controller.tcpServer.sendMessage(p);
			
		} else {
			
			GameController.controller.imReady = true;
			//Enviamos mensaje de jugador listo a cliente
			Paquete p = new Paquete ();
			p.identificadorPaquete = Paquete.Identificador.jugadorListo;
			GameController.controller.tcpClient.sendMessage (p);		
		}
	}
	
}
