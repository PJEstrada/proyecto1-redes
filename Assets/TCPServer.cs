using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;

public class TCPServer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TCPServer(){
		Thread tcpServerRunThread = new Thread(new ThreadStart (TcpServerRun));
		tcpServerRunThread.Start ();
		
	
	}
	public void TcpServerRun(){
		TcpListener tcpListener = new TcpListener (IPAddress.Any, 30000);
		tcpListener.Start ();

		while(true){
			TcpClient client = tcpListener.AcceptTcpClient();
			Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
			tcpHandlerThread.Start(client);
		}
	}

	//Receive Message
	public void  tcpHandler(object client){

		TcpClient mClient = (TcpClient)client;
		NetworkStream stream = mClient.GetStream ();
		byte[] message = new byte[1024];
		stream.Read (message, 0, message.Length);
		Paquete receivedData = new Paquete (GetString (message));
		Paquete.Identificador accion = receivedData.identificadorPaquete;

		if (accion == Paquete.Identificador.conectar) {
			Debug.Log("Ya me conecte");
			Paquete p = new Paquete();
			p.identificadorPaquete = Paquete.Identificador.accesoAutorizado;
			sendMessage(p);
		
		} else if (accion == Paquete.Identificador.jugadorListo) {
		
		
		} else if (accion == Paquete.Identificador.moverAbajo) {
		
		} else if (accion == Paquete.Identificador.moverArriba) {
		
		
		} else if (accion == Paquete.Identificador.moverIzquierda) {
		
		} else if (accion == Paquete.Identificador.moverDerecha) {
		
		} else if (accion == Paquete.Identificador.disparar) {
		
		} else if (accion == Paquete.Identificador.colision) {
		
		}

		stream.Close ();
		mClient.Close ();
	
	}

	public void sendMessage(Paquete p){



	
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
