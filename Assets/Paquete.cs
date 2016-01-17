using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
public class Paquete : MonoBehaviour {


	/*
		El identificador del paquete. Aqui ira la accion que se esta enviando en el juego :D
	 */
	public enum Identificador
	{
		moverIzquierda,
		moverDerecha,
		moverArriba,
		moverAbajo,
		avanzar,
		disparar,
		Null
	}

	public Identificador identificadorPaquete; 
	public string jugador; 

	// Default Constructor
	public Paquete()
	{
		this.identificadorPaquete = Identificador.Null;
		this.jugador = null;
	}

	/*
	 * Esta parte la podriamos cambiar a un JSON o algo parecido para parsear la data mas facil
	 */
	public Paquete(byte[] dataStream)
	{
		// Read the data identifier from the beginning of the stream (4 bytes)
		this.identificadorPaquete = (Identificador)BitConverter.ToInt32(dataStream, 0);
		
		// Read the length of the name (4 bytes)
		int nameLength = BitConverter.ToInt32(dataStream, 4);
		
		// Read the length of the message (4 bytes)
		int msgLength = BitConverter.ToInt32(dataStream, 8);
		
		// Read the name field
		if (nameLength > 0)
			this.name = Encoding.UTF8.GetString(dataStream, 12, nameLength);
		else
			this.name = null;
		

	}


	// Convierte un paquete a un data stream para enviar y recibir datos
	//Tambien lo debemos modificar para que se acople a las acciones del juego
	public byte[] GetDataStream()
	{
		List<byte> dataStream = new List<byte>();
		
		// Add the dataIdentifier
		dataStream.AddRange(BitConverter.GetBytes((int)this.identificadorPaquete));

		
		// Add the jugadorv length
		if (this.jugador != null)
			dataStream.AddRange(BitConverter.GetBytes(this.jugador.Length));
		else
			dataStream.AddRange(BitConverter.GetBytes(0));

		
		// Add the jugador
		if (this.jugador != null)
			dataStream.AddRange(Encoding.UTF8.GetBytes(this.jugador));
		
		return dataStream.ToArray();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
