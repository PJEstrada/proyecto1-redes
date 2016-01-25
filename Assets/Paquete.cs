﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Paquete")]
public class Paquete {

	//El identificador del paquete. Aqui ira la accion que se esta enviando en el juego 
	public enum Identificador
	{
		moverIzquierda,
		moverDerecha,
		moverArriba,
		moverAbajo,
		avanzar,
		disparar,
		conectar,
		Null
	}
	public Identificador identificadorPaquete;
	public int jugador; 

	[XmlArray("B"),XmlArrayItem("Bullet")]
	public List<Bullet> bullets = new List<Bullet> ();






	// Default Constructor
	public Paquete()
	{
		this.identificadorPaquete = Identificador.Null;
		this.jugador = -1;
	    this.bullets = new List<Bullet> ();
	}
	
	
	// Convierte un paquete a un data stream para enviar y recibir datos
	//Tambien lo debemos modificar para que se acople a las acciones del juego
	public string GetDataStream()
	{
		string result = "";
		XmlSerializer serializer = new XmlSerializer (typeof(Paquete));
		StringWriter writer = new StringWriter ();
		serializer.Serialize (writer, this);
		result = writer.ToString ();
		Debug.Log ("El xml: "+result);
		return result;
		
		
	}



	/*
	 * Esta parte la podriamos cambiar a un JSON o algo parecido para parsear la data mas facil
	 */
	public Paquete(string dataStream)
	{

		XmlSerializer serializer = new XmlSerializer (typeof(Paquete));
		StringReader reader = new StringReader (dataStream);
		Paquete p = (Paquete)serializer.Deserialize(reader);

		/*Debug.Log ("Jugador: "+p.jugador);
		for (int i =0; i<p.bullets.Count; i++) {
			Debug.Log("Bala: "+p.bullets[i].id);
		}*/
		this.jugador = p.jugador;
		this.bullets = p.bullets;
		this.identificadorPaquete = p.identificadorPaquete;


		

	}

}
