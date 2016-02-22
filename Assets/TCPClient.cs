﻿using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;

public class TCPClient : MonoBehaviour {

	TcpClient client;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TCPClient(){
		client = new TcpClient ();
		Thread mThread = new Thread(new ThreadStart(ConnectAsClient));
		mThread.Start ();


	}

	public void ConnectAsClient(){

		client.Connect(IPAddress.Parse(WelcomeScreen.ipInput.text), 30000);

		NetworkStream stream = client.GetStream ();
		Paquete msg = new Paquete ();

		msg.identificadorPaquete = Paquete.Identificador.conectar;
		sendMessage (msg);

		while (true) {
			receiveMessage();
		
		}
	}


	public void sendMessage(Paquete p){

		NetworkStream stream = client.GetStream ();
		String s = p.GetDataStream ();
		byte[] message = Encoding.ASCII.GetBytes (s);
		stream.Write (message, 0,message.Length);


	}
	public void closeConnection(){
		client.GetStream ().Close ();
		client.Close ();

	
	}

	public void receiveMessage(){
		NetworkStream stream = client.GetStream ();
		byte[] bb=new byte[1024];
		stream.Read (bb,0,bb.Length);
		Paquete p = new Paquete(GetString(bb));
		Paquete.Identificador accion = p.identificadorPaquete;
		if (accion == Paquete.Identificador.jugadorListo) {
			
			
		} else if (accion == Paquete.Identificador.moverAbajo) {
			
		} else if (accion == Paquete.Identificador.moverArriba) {
			
			
		} else if (accion == Paquete.Identificador.moverIzquierda) {
			
		} else if (accion == Paquete.Identificador.moverDerecha) {
			
		} else if (accion == Paquete.Identificador.disparar) {
			
		} else if (accion == Paquete.Identificador.colision) {
			
		} else if (accion == Paquete.Identificador.accesoAutorizado) {
			Debug.Log ("Ya me autorizaron :)");
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
