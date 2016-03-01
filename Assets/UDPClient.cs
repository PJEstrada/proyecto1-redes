using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class UDPClient {

	public Socket clientSocket;
	public EndPoint epServer;
	public byte[] dataStream;
	public int entrantPackagesCounter;
	public int sendingPackagesCounter;
	public  float lastSynchronizationTime = 0f;
	public float syncDelay = 0f;
	public  float syncTime = 0f;
	public  Vector3 syncStartPosition = Vector3.zero;
	public  Vector3 syncEndPosition = Vector3.zero;
	Vector3 syncPosition = Vector3.zero;
	Vector3 syncVelocity = Vector3.zero;
	public UDPClient(string serverIP) {
		//Paquete sendData = new Paquete ();
		//sendData.id = 0;
		//sendData.identificadorPaquete = Paquete.Identificador.conectar;
		entrantPackagesCounter = 0;
		sendingPackagesCounter = 0;
		//Creamos conexion
		this.clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		//Inicializamos IP del servidor
		try{
			IPAddress ipServer = IPAddress.Parse (serverIP);
			IPEndPoint server = new IPEndPoint (ipServer, 30001);
			
			epServer = (EndPoint)server;
			//Debug.Log("Enviando data de inicio de conexion: ");
			//string  data = sendData.GetDataStream();
			//byte[] dataBytes = GetBytes (data);
			//Enviar solicitud de conexion al servidor
			//clientSocket.BeginSendTo (dataBytes,0,dataBytes.Length,SocketFlags.None,epServer,new System.AsyncCallback(this.SendData),null);
			// Inicializando el dataStream
			this.dataStream = new byte[1024];
			// Empezamos a escuhar respuestas del servidor
			clientSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, ref epServer, new AsyncCallback(this.ReceiveData), null);

		}
		catch(FormatException e){
		
			throw e;
		}
		catch(SocketException e){
			
			throw e;
		}
	
	}

	public void sendMessage(Paquete p){
		byte[] data	;
		p.id = this.sendingPackagesCounter;
		sendingPackagesCounter++;
		// Obtenemos los bytes del paquete
		data = GetBytes(p.GetDataStream());		
		// Enviar al servidor
		clientSocket.BeginSendTo(data, 0, data.Length, SocketFlags.None, epServer, new System.AsyncCallback(this.SendData), null);	
	}

	private void SendData(IAsyncResult ar)
	{
		//Debug.Log("UDP Client: Enviando data -Counter Send = "+this.sendingPackagesCounter);
		try
		{
			clientSocket.EndSend(ar);


		}
		catch (Exception ex)
		{
			//Debug.Log("Send Data: " + ex.Message+ "UDP Client");
		}
	}
	private void ReceiveData(IAsyncResult ar)
	{
		try
		{
			//Debug.Log("UDP Client: Recibiendo data : ");

			// Recibimos toda la data
			this.clientSocket.EndReceive(ar);
			// Inicializamos un paquete para almacenar la data 
			Paquete receivedData = new Paquete(GetString(this.dataStream));
			//Debug.Log("UDP Client: Received data num : "+receivedData.id);
			//Debug.Log ("UDP Client: Client Counter: "+this.entrantPackagesCounter);
			if(receivedData.id<this.entrantPackagesCounter){
				//Debug.Log("UDP Client: Paquete Descartado : ");
				return; //Descartamos el paquete
				
			}
			else if(receivedData.id > this.entrantPackagesCounter){
				this.entrantPackagesCounter = receivedData.id;
			}
			else{
				this.entrantPackagesCounter++;
			}			




			//Actualizamos el Mundo del Juego

			if(receivedData.identificadorPaquete == Paquete.Identificador.nuevaPos){



				if(GameController.controller.ship1.posx >= receivedData.x +60.0 || GameController.controller.ship1.posy >= receivedData.y +60.0){
					//Corregimos la posicion
					//Debug.Log ("UDP CLIENT: DESFASE DE SERVER!! Corrigiendo...");
					float newx = receivedData.x;
					float newy = receivedData.y;
					GameController.controller.ship1.newx  = newx;
					GameController.controller.ship1.newy = newy;
					GameController.controller.ship1.setPos = true;
					GameController.controller.ship1.transform.position = new Vector3(newx,newy,0);
					
					
				}
				else{
					/*//Actualizamos tiempos y transforms para prediccion e interpolacion
					GameController.controller.player1.transform.position = new Vector3(receivedData.x,receivedData.y,0);
					syncVelocity = GameController.controller.player1.rigidbody2D.velocity;
					
					syncTime = 0f;
					syncDelay = Time.time -lastSynchronizationTime;
					lastSynchronizationTime = Time.time;
					
					syncEndPosition = syncPosition+syncVelocity*syncDelay;
					syncStartPosition = new Vector3(receivedData.x,receivedData.y,0);*/

				}






			}
			else if (receivedData.identificadorPaquete == Paquete.Identificador.corregirPos){
				//Corregimos la posicion
				//Debug.Log ("UDP CLIENT: Recibi correccion de pos del server!! Corrigiendo...");
				float newx = receivedData.x;
				float newy = receivedData.y;
			}


				

			// Reset data stream
			this.dataStream = new byte[1024];
			// Continue listening for broadcasts
			clientSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, ref epServer, new AsyncCallback(this.ReceiveData), null);
		}
		catch (ObjectDisposedException)
		{ 
		
		}
		catch (Exception ex)
		{
			//Debug.LogException(ex);

			throw ex;
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
