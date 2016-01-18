using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {


	public GameObject shipSprite1;
	public GameObject shipSprite2;
	public float timeBetweenSpawn=0.05f;
	public string facing;


	// Use this for initialization
	void Start () {
		facing = "front";
		Invoke("MoveShip",timeBetweenSpawn);
	}
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("left")) {
			rotateLeft ();
		
		} else if (Input.GetKeyDown ("down")) {
			rotateDown();
		}			
		else if (Input.GetKeyDown ("up")) {
			rotateFront();
		}			
		else if (Input.GetKeyDown ("right")) {
			rotateRight();
		}			


	}
	void MoveShip(){
		if (facing.Equals("front")) {
			transform.Translate (0, 5, 0);
		} else if (facing.Equals("right")) {
			transform.Translate (0, 5, 0);
		} else if (facing.Equals("left")) {
			transform.Translate (0, 5, 0);
		} else if (facing.Equals("down")) {
			transform.Translate (0, 5,0);
		}

		Invoke("MoveShip",timeBetweenSpawn);
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
	

}
