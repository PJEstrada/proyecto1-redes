using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;

public class TCPServer  {
	TcpClient mClient;
	
	private struct Cliente
	{
		public EndPoint endPoint;
		public string nombre;
	}
	ArrayList listaClientes;
	public int entrantPackagesCounter;
	public int sendingPackagesCounter;
	public bool connect;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public TCPServer(){
		IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
		EndPoint epSender = (EndPoint)clients;
		Thread tcpServerRunThread = new Thread(new ThreadStart (TcpServerRun));
		tcpServerRunThread.Start ();
		//TcpServerRun ();
		Debug.Log ("TCP SERVER");
		
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
		
		Debug.Log ("TCP: En handler del server");
		mClient = (TcpClient)client;
		//NetworkStream stream = mClient.GetStream ();
		//byte[] message = new byte[1024];
		Debug.Log ("antes TCP Server:");
		while (connect) {
			NetworkStream stream = mClient.GetStream ();
			byte[] message = new byte[1024];
			Debug.Log ("TCP Server: Esperando Msj...");
			stream.Read (message, 0, message.Length);
			Debug.Log ("TCP Server: Recibido nuevo Mensaje");
			Paquete receivedData = new Paquete (GetString (message));
			Debug.Log ("TCP Server: XML recibido: " + receivedData.GetDataStream ()); 
			Paquete.Identificador accion = receivedData.identificadorPaquete;
			
			if (accion == Paquete.Identificador.conectar) {
				Debug.Log("TCP: Ya me conecte");
				Paquete p = new Paquete();
				p.identificadorPaquete = Paquete.Identificador.accesoAutorizado;
				sendMessage(p);
				GameController.controller.connected = true;
				
				
				
			} else if (accion == Paquete.Identificador.jugadorListo) {
				GameController.controller.opponentReady=true;

				
			} else if (accion == Paquete.Identificador.moverAbajo) {
				Debug.Log ("TCP Server: Recibi mover abajo");
				//mover abajo el cliente
				GameController.controller.ship2.rDown = true;

			} else if (accion == Paquete.Identificador.moverArriba) {
				Debug.Log ("TCP Server: Recibi mover arriba");
				//mover arriba el cliente
				GameController.controller.ship2.rUp = true;
				
			} else if (accion == Paquete.Identificador.moverIzquierda) {
				//mover izquierda el cliente
				Debug.Log ("TCP Server: Recibi mover izquierda");
				GameController.controller.ship2.rLeft = true;
				
			} else if (accion == Paquete.Identificador.moverDerecha) {
				//mover derecha cliente
				Debug.Log ("TCP Server: Recibi mover derecha");
				GameController.controller.ship2.rRight = true;
				
				
			} else if (accion == Paquete.Identificador.disparar) {
				Debug.Log ("TCP Server: Disparar");
				GameController.controller.ship2.fireB=true;
				
				
			} else if (accion == Paquete.Identificador.colision) {
				
				
				
			}		
			else if(accion == Paquete.Identificador.desconectar){
				GameController.controller.mm2 = true;
				
			}			
			
		}
		
		
		
		//stream.Close ();
		//mClient.Close ();
		
	}


	public void CloseConnection(){
		mClient.GetStream ().Close ();
		mClient.Close ();	
		connect = false;
	}
	public void sendMessage(Paquete p){
		
		
		NetworkStream stream = mClient.GetStream ();
		String s = p.GetDataStream ();
		byte[] message = GetBytes (s);
		stream.Write (message, 0,message.Length);
		
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
