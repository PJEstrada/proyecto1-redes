using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class UDPClient {

	public Socket clientSocket;
	public EndPoint epServer;
	public byte[] dataStream;


	public UDPClient(string serverIP){

		Paquete sendData = new Paquete ();
		sendData.identificadorPaquete = Paquete.Identificador.conectar;
		//Creamos conexion
		this.clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		//Inicializamos IP del servidor
		IPAddress ipServer = IPAddress.Parse (serverIP);
		IPEndPoint server = new IPEndPoint (ipServer, 30000);
		epServer = (EndPoint)server;
		string  data = sendData.GetDataStream();
		byte[] dataBytes = GetBytes (data);
		//Enviar solicitud de conexion al servidor
		clientSocket.BeginSendTo (dataBytes,0,dataBytes.Length,SocketFlags.None,epServer,new System.AsyncCallback(this.SendData),null);
		// Inicializando el dataStream
		this.dataStream = new byte[1024];
		
		// Empezamos a escuhar respuestas del servidor
		clientSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, ref epServer, new AsyncCallback(this.ReceiveData), null);
	
	
	}

	private void SendData(IAsyncResult ar)
	{
		try
		{
			clientSocket.EndSend(ar);

		}
		catch (Exception ex)
		{
			Debug.Log("Send Data: " + ex.Message+ "UDP Client");
		}
	}
	private void ReceiveData(IAsyncResult ar)
	{
		try
		{
			// Recibimos toda la data
			this.clientSocket.EndReceive(ar);
			
			// Inicializamos un paquete para almacenar la data 
			Paquete receivedData = new Paquete(GetString(this.dataStream));
			//Actualizamos el Mundo del Juego

				//...pendiente


			// Reset data stream
			this.dataStream = new byte[1024];
			
			// Continue listening for broadcasts
			clientSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, ref epServer, new AsyncCallback(this.ReceiveData), null);
		}
		catch (ObjectDisposedException)
		{ }
		catch (Exception ex)
		{
			Debug.Log("Receive Data: " + ex.Message+ "UDP Client");
		}
	}



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
