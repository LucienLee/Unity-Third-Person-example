using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour {

	public Animator animator;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		animator.SetFloat ("velocity", this.rigidbody.velocity.magnitude);
	}
}
