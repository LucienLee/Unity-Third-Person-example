using UnityEngine;
using System.Collections;

public class ThreePersonController : MonoBehaviour {

	public GameObject player;
	public GameObject camera;
	public GameObject cameraCollisionBox;
	public CollisionCounter probe;
	public CollisionCounter cameraProbe;

	public float Maxdistance;
	public float moveSpeed;
	public float verticalRatio;
	

	void CalculateHeight(){
		Vector3 originVector = camera.transform.position;
		Vector3 localVector = camera.transform.localPosition;

		// REMOVE Y FACTOR IN magnitude
		originVector.y = player.transform.position.y;
		
		float y = Maxdistance - (originVector - player.transform.position).magnitude;
		localVector.y=y;
		camera.transform.localPosition = localVector*verticalRatio;
	}
	

	// Update is called once per frame
	void Update () {
		// GET INPUT AXIS
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		// GET DIRECTION
		Vector3 forward = camera.transform.forward;
		Vector3 right = camera.transform.right;

		//KEEP Y AXIX 
		forward.y = 0;
		right.y = 0;
		// GET DIRECTION UNIT VECTOR
		forward.Normalize();
		right.Normalize ();

		//FOR RESET CHARACTOR FLOAT IN AIR
		float Yvelocity = player.rigidbody.velocity.y;
		Vector3 distance = (  camera.transform.position - player.transform.position );
		distance.y = 0;

		if (vertical > 0) {
			player.transform.rotation = Quaternion.Euler(0,camera.transform.rotation.eulerAngles.y,0);
			player.rigidbody.velocity = forward*moveSpeed;

			// chase player 
			if( distance.magnitude > Maxdistance ){
				float originY = cameraCollisionBox.transform.position.y;
				Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
				newPosition.y = originY;
				cameraCollisionBox.transform.position = newPosition;
			}

			this.CalculateHeight();

		}else if( vertical < 0){
			player.transform.rotation = Quaternion.Euler(0,camera.transform.rotation.eulerAngles.y+180,0);
			player.rigidbody.velocity = -forward*moveSpeed;

			if( cameraProbe.counter <= 0 ){
				// chase player 
				float originY = cameraCollisionBox.transform.position.y;
				Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
				newPosition.y = originY;
				cameraCollisionBox.transform.position = newPosition;
			}else{
				// camera up to sky
				this.CalculateHeight();
			}

		}else{
			player.rigidbody.velocity = Vector3.zero;
		}

		if (horizontal > 0){
			player.transform.rotation = Quaternion.Euler(0,camera.transform.rotation.eulerAngles.y+90,0);
			if( probe.counter > 0 ){
				cameraCollisionBox.transform.position += -right*moveSpeed*Time.deltaTime;
			}else{ 
				player.rigidbody.velocity = right*moveSpeed;
			}
		}else if (horizontal < 0){
			player.transform.rotation = Quaternion.Euler(0,camera.transform.rotation.eulerAngles.y-90,0);
			if( probe.counter > 0 ){
				cameraCollisionBox.transform.position += right*moveSpeed*Time.deltaTime;
			}else{
				player.rigidbody.velocity = -right*moveSpeed;
			}
		}

		//RESET CHARACTOR FLOAT IN AIR
		player.rigidbody.velocity = new Vector3 (player.rigidbody.velocity.x, Yvelocity, player.rigidbody.velocity.z);
	}
}
