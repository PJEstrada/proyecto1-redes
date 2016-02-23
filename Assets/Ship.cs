using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public string facing;
	public float timeBetweenMove;
	public float speed;
	public GameObject bullet;
	public int player;
	public KeyCode moveUp,moveDown,moveLeft,moveRight,fireBtn;

	// Use this for initialization
	void Start () {

	}
	public void startMoving(){
		
		Invoke("MoveShip",timeBetweenMove);
	}
	// Update is called once per frame
	void Update () {
		if (GameController.controller.gameOn==true) {
			if (Input.GetKeyDown (moveLeft)) {
				rotateLeft ();
				
			} else if (Input.GetKeyDown (moveDown)) {
				rotateDown ();
			} else if (Input.GetKeyDown (moveUp)) {
				rotateFront ();
			} else if (Input.GetKeyDown (moveRight)) {
				rotateRight ();
			} else if (Input.GetKeyDown (fireBtn)) {
				fire ();
			}		
		
		
		}



	}
	void MoveShip(){
		if (GameController.controller.gameOn==true) {
			if (facing.Equals("front")) {
				Vector3 v1 = rigidbody2D.velocity;
				v1.x = 0;
				rigidbody2D.velocity = v1;
				Vector3 v = rigidbody2D.velocity;
				v.y = speed;
				rigidbody2D.velocity = v;
			} else if (facing.Equals("right")) {
				Vector3 v1 = rigidbody2D.velocity;
				v1.y = 0;
				rigidbody2D.velocity = v1;
				Vector3 v = rigidbody2D.velocity;
				v.x = speed;
				rigidbody2D.velocity = v;
			} else if (facing.Equals("left")) {
				Vector3 v1 = rigidbody2D.velocity;
				v1.y = 0;
				rigidbody2D.velocity = v1;
				Vector3 v = rigidbody2D.velocity;
				v.x = -speed;
				rigidbody2D.velocity = v;
			} else if (facing.Equals("down")) {
				Vector3 v1 = rigidbody2D.velocity;
				v1.x = 0;
				rigidbody2D.velocity = v1;
				Vector3 v = rigidbody2D.velocity;
				v.y = -speed;
				rigidbody2D.velocity = v;
			}
			
			Invoke("MoveShip",GameController.controller.timeBetweenSpawn);		
		
		
		}

	}

	void rotateLeft(){
	
		if (!facing.Equals("right") && !facing.Equals("left")) {
			if(facing.Equals("front")){
			
				transform.Rotate (0,0,90);
			}
			else{

				transform.Rotate (0,0,-90);
			}
			facing = "left";
		}

		//Mandamos mensaje (Decidir si antes de pintar o despues)

	}

	void rotateRight(){
		if (!facing.Equals("right") && !facing.Equals("left")) {
			if(facing.Equals("front")){

				transform.Rotate (0,0,-90);
			}
			else{

				transform.Rotate (0,0,90);
			}


			facing = "right";
		}	
		//Mandamos Mensaje (Decidir si antes de pintar o despues)
	}

	void rotateFront(){
		if (!facing.Equals("front") && !facing.Equals("down")) {
			if(facing.Equals("right")){

				transform.Rotate (0,0,90);
			}
			else{
			
				transform.Rotate (0,0,-90);
			}
			facing = "front";
		
			//Mandamos Mensaje (Decidir si antes de pintar o despues)
		}		
	}

	void rotateDown(){
		if (!facing.Equals("down") && !facing.Equals("front")) {
			if(facing.Equals("right")){

				transform.Rotate (0,0,-90);
			}
			else{

				transform.Rotate (0,0,90);
			}
			facing = "down";
		}	
	}


	public void fire(){
		GameObject barrel = getChildGameObject (gameObject, "Barrel");
		GameObject go = (GameObject)Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
		go.GetComponent<BulletObject> ().direction = facing;

		//Mandamos Mensaje (Decidir si antes de pintar o despues)

	}
	public void stopMoving(){
		Vector3 v1 = rigidbody2D.velocity;
		v1.x = 0;
		v1.y = 0;
		rigidbody2D.velocity = v1;

	}
	//Choque de la nave
	void OnTriggerEnter2D(Collider2D other)
	{
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		if (this.player == 1) {
			GameController.controller.playerWins (2);
		} else {
			GameController.controller.playerWins (1);
		}
	}


	void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log("Still colliding with trigger object " + other.name);
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log(gameObject.name + " and trigger object " + other.name + " are no longer colliding");
	}
	
	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		//Author: Isaac Dart, June-13.
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
}
