using UnityEngine;
using System.Collections;

public class BulletObject : MonoBehaviour {
	
	public string direction;
	public float speed;
	// Use this for initialization
	void Start () {

		this.speed = 800.0f;
		//transform.position = new Vector3 (ship.GetComponent<Transform> ().position.x, ship.GetComponent<Transform> ().position.y, 0);
		MoveBullet ();
		//Invoke("MoveBullet",0.5f);
	}


	public void  MoveBullet(){
		if (this.direction=="front") {
			Vector3 v1 = rigidbody2D.velocity;
			v1.x = 0;
			rigidbody2D.velocity = v1;
			Vector3 v = rigidbody2D.velocity;
			v.y = speed;
			rigidbody2D.velocity = v;
		} 
		else if (this.direction=="right") {
			Vector3 v1 = rigidbody2D.velocity;
			v1.y = 0;
			rigidbody2D.velocity = v1;
			Vector3 v = rigidbody2D.velocity;
			v.x = speed;
			rigidbody2D.velocity = v;
		} 
		else if (this.direction=="left") {
			Vector3 v1 = rigidbody2D.velocity;
			v1.y = 0;
			rigidbody2D.velocity = v1;
			Vector3 v = rigidbody2D.velocity;
			v.x = -speed;
			rigidbody2D.velocity = v;
		} 
		else if (this.direction=="down") {
			Vector3 v1 = rigidbody2D.velocity;
			v1.x = 0;
			rigidbody2D.velocity = v1;
			Vector3 v = rigidbody2D.velocity;
			v.y = -speed;
			rigidbody2D.velocity = v;
		}
		Invoke("MoveBullet",0.5f);

	}
	
	//Choque de la nave
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(this.gameObject);
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log("Still colliding with trigger object " + other.name);
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		//Debug.Log(gameObject.name + " and trigger object " + other.name + " are no longer colliding");
	}


}
