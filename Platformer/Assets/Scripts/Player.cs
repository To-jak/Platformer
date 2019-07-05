using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour {

	Vector3 velocity;
	int jumpCount;
	float velocityXSmoothing;

	[SerializeField]
	Vector2 wallJump;

	[SerializeField]
	float moveSpeed = 6f;
	[SerializeField]
	float jumpStart = 20f;
	[SerializeField]
	float gravity = -50f;
	[SerializeField]
	float jumpForce = 2f;
	[SerializeField]
	float decayRate = 5f;
	[SerializeField]
	int maxJumpCount = 2;
	[SerializeField]
	float wallSlideSpeedMax = 3f;

	Controller2D controller;
	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();
		jumpCount = 0;
	}

	IEnumerator DoJump(){

		velocity.y = jumpStart;

		float jumpAdd = jumpForce;

		while(Input.GetButton("Jump") && jumpAdd > 0)
		{
			jumpAdd -= Mathf.Exp(decayRate * Time.deltaTime);
			velocity.y += jumpAdd * Time.deltaTime;
			yield return null;
		}

		yield return null;
	}

	void Update() {

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;
			if (velocity.y < -wallSlideSpeedMax)
				velocity.y = -wallSlideSpeedMax;
		}

		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}

		if (controller.collisions.below)
			jumpCount = 0;

		if (Input.GetButtonDown ("Jump") && jumpCount > 0 && jumpCount < maxJumpCount && !wallSliding)
		{
			StartCoroutine (DoJump ());
			jumpCount += 1;
		}

		if (Input.GetButtonDown("Jump"))
		{
			if (wallSliding) {
				velocity.x = -wallDirX * wallJump.x;
				velocity.y = wallJump.y;
			}
			if (controller.collisions.below) {
				StartCoroutine (DoJump ());
				jumpCount += 1;
			}
		}





		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 0.1f);
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}