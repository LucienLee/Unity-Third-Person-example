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

	// Use this for initialization
	void Start () {

	}

	void CalculateHeight(){
		Vector3 originVector = camera.transform.position;
		// REMOVE Y FACTOR IN magnitude
		originVector.y = player.transform.position.y;
		
		float y = Maxdistance - (originVector - player.transform.position).magnitude;
		originVector.y=y;
		originVector.x = 0;
		originVector.z = 0;
		camera.transform.localPosition = originVector*verticalRatio;
	}

	// Update is called once per frame
	void Update () {
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		Vector3 forward = camera.transform.forward;
		Vector3 right = camera.transform.right;

		// confirm camera y 
		forward.y = 0;
		forward.Normalize();
		right.y = 0;
		right.Normalize ();
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

		player.rigidbody.velocity = new Vector3 (player.rigidbody.velocity.x, Yvelocity, player.rigidbody.velocity.z);
	}
}
