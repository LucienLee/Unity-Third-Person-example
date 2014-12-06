using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public Transform lookAtObj;
		
	// Update is called once per frame
	void Update () {
		// look at player
		transform.LookAt(lookAtObj);	
	}
}
