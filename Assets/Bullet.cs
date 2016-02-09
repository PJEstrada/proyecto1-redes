using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class BulletMessage  {

	[XmlAttribute("id")]
	public int id;
	[XmlAttribute("px")]
	public float px;
	[XmlAttribute("py")]
	public float py;
	[XmlAttribute("pz")]
	public float pz;
	[XmlAttribute("rx")]
	public float rx;
	[XmlAttribute("ry")]
	public float ry;
	[XmlAttribute("rz")]
	public float rz;

	public BulletMessage(){
		this.px = 0;
		this.id = -1;
		this.py = 0;
		this.pz = 0;
		this.rx = 0;
		this.ry = 0;
		this.rz = 0;	
	}
	public BulletMessage(int id,float px,float py, float pz,float rx,float ry,float rz){
		this.id = id;
		this.px = px;
		this.py = py;
		this.pz = pz;
		this.rx = rx;
		this.ry = ry;
		this.rz = rz;
	}

	
}
