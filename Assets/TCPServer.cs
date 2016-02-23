using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;

public class TCPServer : MonoBehaviour {
	TcpClient mClient;

	private struct Cliente
	{
		public EndPoint endPoint;
		public string nombre;
	}
	ArrayList listaClientes;
	public int entrantPackagesCounter;
	public int sendingPackagesCounter;

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
		NetworkStream stream = mClient.GetStream ();
		byte[] message = new byte[1024];
		Debug.Log ("antes TCPClient:");
		stream.Read (message, 0, message.Length);
		Debug.Log ("despues TCPClient:");
		Paquete receivedData = new Paquete (GetString (message));
		Debug.Log ("Client: el xml tcp:" + receivedData.GetDataStream ()); 
		Paquete.Identificador accion = receivedData.identificadorPaquete;

		if (accion == Paquete.Identificador.conectar) {
			Debug.Log("TCP: Ya me conecte");
			Paquete p = new Paquete();
			p.identificadorPaquete = Paquete.Identificador.accesoAutorizado;
			sendMessage(p);
		
		} else if (accion == Paquete.Identificador.jugadorListo) {
			GameController.controller.opponentReady=true;
		
		} else if (accion == Paquete.Identificador.moverAbajo) {
		
		} else if (accion == Paquete.Identificador.moverArriba) {
		
		
		} else if (accion == Paquete.Identificador.moverIzquierda) {
		
		} else if (accion == Paquete.Identificador.moverDerecha) {
		
		} else if (accion == Paquete.Identificador.disparar) {
		
		} else if (accion == Paquete.Identificador.colision) {
		
		}
		else if( accion == Paquete.Identificador.conectar){
			byte[] data	;
			// Empezamos a crear el paquete a ser enviado
			Paquete sendData = new Paquete();
			sendData.id = this.sendingPackagesCounter;
			sendData.identificadorPaquete = Paquete.Identificador.accesoAutorizado;
			GameController.controller.connected = true;
			Cliente client2 = new Cliente();
			// Initialise the IPEndPoint for the clients
			IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
			// Initialise the EndPoint for the clients
			EndPoint epSender = (EndPoint)clients;
			client2.endPoint = epSender;
			client2.nombre = "Player2";
			
			// Add client to list
			this.listaClientes.Add(client2);

			/*Se envia el paquete a  los clientes*/
			sendMessage(sendData);


		}

		//stream.Close ();
		//mClient.Close ();
	
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
