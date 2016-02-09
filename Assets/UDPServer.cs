using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
public class UDPServer {


	
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
	byte[] dataStream = new byte[1024];
	// Status delegate
	delegate void UpdateStatusDelegate(string status);
		//UpdateStatusDelegate updateStatusDelegate = null;

	public UDPServer()
	{
		try
		{
			// Iniciando array de clientes conectados
			this.listaClientes = new ArrayList();
			
			// Inicializando el delegado para actualizar estado
			//this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateStatus);
			
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
			
			// Empezar a escuhar datos entrantes
			serverSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveData), epSender);
			
			GameController.controller.messages.enabled = true;
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
			byte[] data	;
			// Paquete para almacenar la data recibida
			Paquete receivedData = new Paquete(GetString(this.dataStream));
			// Initialise the IPEndPoint for the clients
			IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
			// Initialise the EndPoint for the clients
			EndPoint epSender = (EndPoint)clients;
			// Receive all data
			serverSocket.EndReceiveFrom(asyncResult, ref epSender);


			// Actualizamos el mundo del server...Pendiente
			if(receivedData.identificadorPaquete == Paquete.Identificador.conectar){
				// Empezamos a crear el paquete a ser enviado
				Paquete sendData = new Paquete();
				sendData.identificadorPaquete = Paquete.Identificador.accesoAutorizado;
				GameController.controller.connected = true;

				// Obtenemos los bytes del paquete
				data = GetBytes(sendData.GetDataStream());
				/*Se envia el paquete a todos los clientes*/
				foreach (Cliente client in this.listaClientes)
				{
					if (client.endPoint != epSender)
					{
						// Enviar a todos los clientes
						serverSocket.BeginSendTo(data, 0, data.Length, SocketFlags.None, client.endPoint, new AsyncCallback(this.enviarData), client.endPoint);
					}
				}

			}

			
			// Volvemos a escuchar conexiones nuevamente...
			serverSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, ref epSender, new AsyncCallback(this.ReceiveData), epSender);
			// Actualizamos el estado con un delegate
			//this.Invoke(this.updateStatusDelegate, new object[] { sendData.ChatMessage });
		}
		catch (Exception ex)
		{
			Debug.Log("ReceiveData Error: " + ex.Message+ "UDP Server");
		}
	}


	// Metodo utilizado para procesar los paquetes que recibe el servidor y realizar las acciones adecuadas en cada caso.
	public void procesarPaquete(){



	}

	private void UpdateStatus(string status)
	{
		Debug.Log( status + Environment.NewLine);
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

