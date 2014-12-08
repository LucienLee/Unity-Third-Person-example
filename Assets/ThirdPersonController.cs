using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {

	public GameObject player;
	public GameObject mainCamera;
	public GameObject cameraCollisionBox;
	public CollisionCounter probe;
	public CollisionCounter cameraProbe;

	public float Maxdistance;
	public float moveSpeed;
	public float verticalRatio;
	

	private bool isHoriMove;
	private bool isVertiMove;
	private Vector3 horiVelocity;
	private Vector3 vertiVelocity;

	//FOR RESET CHARACTOR FLOAT IN AIR
	private float originYVelocity; 
	//FOR Continue Movement
	private float timer;

	void CalculateHeight(){
		Vector3 originVector = mainCamera.transform.position;
		Vector3 localVector = mainCamera.transform.localPosition;

		// REMOVE Y to caculate XZ magnitude
		originVector.y = player.transform.position.y;
		
		float y = Maxdistance - (originVector - player.transform.position).magnitude;
		localVector.y=y;
		mainCamera.transform.localPosition = localVector*verticalRatio;
	}

	void Draw(){
		if( this.isHoriMove && this.isVertiMove ){
			player.rigidbody.velocity = this.horiVelocity + this.vertiVelocity;
		}else if( this.isHoriMove ){
			player.rigidbody.velocity = this.horiVelocity;
		}else if( this.isVertiMove ){
			player.rigidbody.velocity = this.vertiVelocity;
		}
		 
		if (this.isHoriMove || this.isVertiMove) {
			// Caculate the player's degree from velocity
			float rotate = Mathf.Atan2 (player.rigidbody.velocity.x, player.rigidbody.velocity.z);
			player.transform.rotation = Quaternion.Euler (0, rotate / Mathf.PI * 180, 0);

			// Let player move continue a little bit
			this.timer = 0.1f;
		}
		if (this.timer >= 0) {
			this.timer -= Time.deltaTime;
		} else {
			player.rigidbody.velocity = Vector3.zero;
		}
		// Reset Y axis velocity to origin
		player.rigidbody.velocity = new Vector3 (player.rigidbody.velocity.x, this.originYVelocity, player.rigidbody.velocity.z);
	}

	// Update is called once per frame
	void Update () {
		// GET INPUT AXIS
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		// GET DIRECTION
		Vector3 forward = mainCamera.transform.forward;
		Vector3 right = mainCamera.transform.right;
		//Calibrate Y AXIS 
		forward.y = 0;
		right.y = 0;
		// GET DIRECTION UNIT VECTOR
		forward.Normalize();
		right.Normalize ();

		Vector3 distance = ( mainCamera.transform.position - player.transform.position );
		distance.y = 0;

		//RESET ROTATION 
		this.isHoriMove = false;
		this.isVertiMove = false;
		this.horiVelocity = Vector3.zero;
		this.vertiVelocity = Vector3.zero;

		this.originYVelocity = player.rigidbody.velocity.y;

		if (vertical > 0) {
			this.vertiVelocity = forward*moveSpeed;
			//Camera chases the player
			if( distance.magnitude > Maxdistance ){
				float originY = cameraCollisionBox.transform.position.y;
				Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
				newPosition.y = originY;
				cameraCollisionBox.transform.position = newPosition;
			}

			CalculateHeight();
			isVertiMove = true;

		}else if( vertical < 0){
			this.vertiVelocity = -forward*moveSpeed;
			if( cameraProbe.counter <= 0 ){
				//Camera chases the player
				float originY = cameraCollisionBox.transform.position.y;
				Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
				newPosition.y = originY;
				cameraCollisionBox.transform.position = newPosition;
			}else{
				//Camera move higher to see the player
				CalculateHeight();
			}
			isVertiMove = true;
		}

		if (horizontal > 0){
			if( probe.counter > 0 ){
				cameraCollisionBox.transform.position += -right*moveSpeed*Time.deltaTime;
			}
			this.horiVelocity = right*moveSpeed;
			isHoriMove = true;
		}else if (horizontal < 0){
			if( probe.counter > 0 ){
				cameraCollisionBox.transform.position += right*moveSpeed*Time.deltaTime;
			}
			this.horiVelocity = -right*moveSpeed;
			isHoriMove = true;
		}

		// perform the result on charactor
		Draw ();
	}
}
