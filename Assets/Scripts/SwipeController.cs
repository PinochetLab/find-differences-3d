using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour {

	[SerializeField] Transform spinPoint;
	[SerializeField] float sensitivity = 20;

	private Vector2 tapPosition;
	private Vector2 swipeDelta;

	private float deadZone = 80;

	private bool isSwiping;
	private bool isMobile;

	private float xRot = 0;
	private float yRot = 0;

	private void Start() {
		isMobile = Application.isMobilePlatform;
	}

	private void Update() {
		if ( !isMobile ) {
			if ( Input.GetMouseButtonDown(0) ) {
				isSwiping = true;
				tapPosition = Input.mousePosition;
			}else if ( Input.GetMouseButtonUp(0) ) {
				ResetSwipe();
			}
		}
		else {
			if ( Input.touchCount > 0 ) {
				if ( Input.GetTouch(0).phase == TouchPhase.Began ) {
					isSwiping = true;
					tapPosition = Input.GetTouch(0).position;
				}
				else if ( Input.GetTouch(0).phase == TouchPhase.Canceled
					|| Input.GetTouch(0).phase == TouchPhase.Ended ) {
					ResetSwipe();
				}
			}
		}

		CheckSwipe();
	}

	private void CheckSwipe() {
		swipeDelta = Vector2.zero;

		if ( isSwiping ) {
			if ( !isMobile && Input.GetMouseButton(0) ) {
				swipeDelta = (Vector2)Input.mousePosition - tapPosition;
				tapPosition = Input.mousePosition;
				Rotate(swipeDelta);
			}
			else if ( Input.touchCount > 0 ) {
				swipeDelta = Input.GetTouch(0).position - tapPosition;
				tapPosition = Input.GetTouch(0).position;
				Rotate(swipeDelta);
			}
		}
	}

	private void ResetSwipe() {
		isSwiping = false;
		tapPosition = Vector2.zero;
		swipeDelta = Vector2.zero;
	}

	private void Rotate(Vector2 move) {
		/*Vector3 axis = Vector3.Cross(move, Vector3.forward).normalized;
		float magnitude = move.magnitude * sensitivity;
		Vector3 rot = axis * magnitude;

		Vector3 forward = spinPoint.forward;

		//rot = rot - forward * Vector3.Dot(rot, forward);

		spinPoint.Rotate(rot, Space.World);

		spinPoint.up = Vector3.up;*/

		float x = move.y * sensitivity;
		float y = -move.x * sensitivity;
		Vector3 lastEulerAngles = spinPoint.eulerAngles;
		spinPoint.Rotate(Vector3.up * y, Space.Self);
		spinPoint.Rotate(Vector3.right * x, Space.World);

		if (spinPoint.up.y < 0 ) {
			spinPoint.eulerAngles = lastEulerAngles;
		}
	}
}
