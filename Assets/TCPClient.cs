using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;

public class TCPClient{
	
	public TcpClient client;
	static string ip;
	bool connect;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public TCPClient(){
		connect = true;
		client = new TcpClient ();
		//client.Connect(IPAddress.Parse(WelcomeScreen.ipInput.text.ToString()), 30000);
		Thread mThread = new Thread(new ThreadStart(ConnectAsClient));
		mThread.Start ();
		
		
	}
	
	public void ConnectAsClient(){
		ip = WelcomeScreen.ipInput.text.ToString ();
		
		
		client.Connect(IPAddress.Parse(ip), 30000);
		Paquete msg = new Paquete ();
		msg.identificadorPaquete = Paquete.Identificador.conectar;
		sendMessage (msg);

		while (connect) {
			receiveMessage();
			
		}
	}
	
	
	public void sendMessage(Paquete p){
		
		NetworkStream stream = client.GetStream ();
		String s = p.GetDataStream ();
		byte[] message = GetBytes (s);
		stream.Write (message, 0,message.Length);
		Debug.Log("TCP Client: Enviado mensaje!");
		
	}
	public void CloseConnection(){
		client.GetStream ().Close ();
		client.Close ();
		connect = false;
		
	}
	
	public void receiveMessage(){
		Debug.Log ("TCP CLIENT: En metodo lectura");
		NetworkStream stream = client.GetStream ();
		byte[] bb=new byte[1024];
		stream.Read (bb,0,bb.Length);
		string s = GetString (bb);
		Paquete p = new Paquete(s);
		Debug.Log ("TCP Client: El XML recibido" + p.GetDataStream ()); 
		Paquete.Identificador accion = p.identificadorPaquete;
		if (accion == Paquete.Identificador.jugadorListo) {
			GameController.controller.opponentReady = true;
			
		} else if (accion == Paquete.Identificador.moverAbajo) {
			//mover el server abajo
			GameController.controller.ship1.rDown = true;
			
		} else if (accion == Paquete.Identificador.moverArriba) {
			//mover el server arriba
			GameController.controller.ship1.rUp = true;
			
		} else if (accion == Paquete.Identificador.moverIzquierda) {
			//mover el server izquierda
			GameController.controller.ship1.rLeft = true;
			
		} else if (accion == Paquete.Identificador.moverDerecha) {
			//mover el server derecha
			GameController.controller.ship1.rRight = true;
			
		} else if (accion == Paquete.Identificador.disparar) {
			GameController.controller.ship1.fireB = true;
		} else if (accion == Paquete.Identificador.colision) {
			
		} else if (accion == Paquete.Identificador.accesoAutorizado) {
			Debug.Log ("TCP: Ya me autorizaron :)");
			GameController.controller.connected = true;
		} else if (accion == Paquete.Identificador.jugadorGana) {
			if (p.jugador == 1) {
				GameController.controller.p1Wins = true;

			} else if (p.jugador == 2) {
				GameController.controller.p2Wins = true;

			}

		
		} else if (accion == Paquete.Identificador.desconectar) {
			GameController.controller.mm2 = true;
			
		} 

		
	}
	/*-------------------------------------FIN AREA SERVER------------------------------------------------------*/
	// Helper Methods
	static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}
	
	static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}
}
