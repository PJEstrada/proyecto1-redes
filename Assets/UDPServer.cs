using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
public class UDPServer : MonoBehaviour {


	
	// Estructura para almacenar info del cliente
	private struct Cliente
	{
		public EndPoint endPoint;
		public string nombre;
	}
	
	// Listado de clientes
	ArrayList listaClientes;
	
	// Server socket
	Socket serverSocket;
	
	// Stream de datos
	byte[] data = new byte[1024];
	
	// Status delegate
	delegate void UpdateStatusDelegate(string status);
		UpdateStatusDelegate updateStatusDelegate = null;
	

	public UDPServer()
	{
		//Constructor....
	}
	

	private void Server_Load(object sender, EventArgs e)
	{
		try
		{
			// Iniciando array de clientes conectados
			this.listaClientes = new ArrayList();
			
			// Inicializando el delegado para actualizar estado
			this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateStatus);
			
			// Inicializando el socket
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			
			// Inicializar IP y escuhar puerto 30000
			IPEndPoint server = new IPEndPoint(IPAddress.Any, 30000);
			
			// Asociar socket con el IP dado y el puerto
			serverSocket.Bind(server);
			
			// Inicializar IPEndpoint de los clientes
			IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
			
			// Inicializar Endpoint de clientes
			EndPoint epSender = (EndPoint)clients;
			
			// Empezar a escuhar datos entrantos
			serverSocket.BeginReceiveFrom(this.data, 0, this.data.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveData), epSender);
	
		}
		catch (Exception ex)
		{
			Debug.Log("Error al cargar servidor: " + ex.Message+ " ---UDP ");
		}
	}
	

	public void enviarData(IAsyncResult asyncResult)
	{
		try
		{
			serverSocket.EndSend(asyncResult);
		}
		catch (Exception ex)
		{
			Debug.Log("Error al enviar data: " + ex.Message+ " ---UDP Server");
		}
	}


	private void ReceiveData(IAsyncResult asyncResult)
	{
		try
		{
			byte[] dataR;
			
			// Initialise a packet object to store the received data
			Paquete receivedData = new Paquete(this.data);
			
			// Initialise a packet object to store the data to be sent
			Paquete sendData = new Paquete();
			
			// Initialise the IPEndPoint for the clients
			IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
			
			// Initialise the EndPoint for the clients
			EndPoint epSender = (EndPoint)clients;
			
			// Receive all data
			serverSocket.EndReceiveFrom(asyncResult, ref epSender);
			
			// Start populating the packet to be sent
			sendData.identificadorPaquete = receivedData.identificadorPaquete;
			
			switch (receivedData.identificadorPaquete)
			{
			case Paquete.Identificador.moverIzquierda:
				//bla bla
				break;
				
			case Paquete.Identificador.moverDerecha:
				// bla bla

				break;
				
			case Paquete.Identificador.moverArriba:
				// bla bla

				break;
			case Paquete.Identificador.moverAbajo:
				// bla bla
				
				break;

			case Paquete.Identificador.disparar:
				// bla bla
				
				break;

			case Paquete.Identificador.avanzar:
				// bla bla
				
				break;

			}
			
			// Get packet as byte array
			dataR = sendData.GetDataStream();


			/*Se envia el pacquete a todos los clientes*/
			foreach (Cliente client in this.listaClientes)
			{
				if (client.endPoint != epSender)
				{
					// Broadcast to all logged on users
					serverSocket.BeginSendTo(dataR, 0, dataR.Length, SocketFlags.None, client.endPoint, new AsyncCallback(this.enviarData), client.endPoint);
				}
			}
			
			// Listen for more connections again...
			serverSocket.BeginReceiveFrom(this.data, 0, this.data.Length, SocketFlags.None, ref epSender, new AsyncCallback(this.ReceiveData), epSender);
			
			// Update status through a delegate
			//this.Invoke(this.updateStatusDelegate, new object[] { sendData.ChatMessage });
		}
		catch (Exception ex)
		{
			Debug.Log("ReceiveData Error: " + ex.Message+ "UDP Server");
		}
	}
	



	private void UpdateStatus(string status)
	{
		Debug.Log( status + Environment.NewLine);
	}

	/*-------------------------------------FIN AREA SERVER------------------------------------------------------*/
	
	// Use this for initialization
	void Start () {
		
		
		
	}	
	// Update is called once per frame
	void Update () {
		
	}
}

