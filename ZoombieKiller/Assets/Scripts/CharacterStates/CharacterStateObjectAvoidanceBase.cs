using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStateObjectAvoidanceBase : CharacterStateBase {
	List <Vector3> wayPoints = new List<Vector3>();
	Vector3 lastPos = new Vector3();
	public Vector3 targetPosition;
	float objectAvoidanceTimeCheck = 0.5f;
	protected Vector3 currLocation = new Vector3();
	protected Vector3 moveToPoint = Vector3.zero;
	protected float lookSpeed = 6f;

	public override void startState() {
		wayPoints.Clear();
		lastPos = transform.position;
		CancelInvoke("objectAvoidanceMovement");
		Invoke("objectAvoidanceMovement",objectAvoidanceTimeCheck);
	}
	public override void excuteState() {
		if (isObjectAvoidanceActive) {
			moveToPoint.x = wayPoints[wayPoints.Count-1].x;
			moveToPoint.y = transform.position.y;
			moveToPoint.z = wayPoints[wayPoints.Count-1].z;

			if (Vector3.Distance(transform.position,moveToPoint) > 0.5f) {
				//let's rotate our player
				currLocation.x = transform.position.x;
				currLocation.z = transform.position.z;
				currLocation.y = transform.position.y;
				Vector3 PrevLocation = currLocation;
				currLocation.x = moveToPoint.x;
				currLocation.z = moveToPoint.z;
				if (currLocation != PrevLocation) {
					parentChar.mAnimator.transform.rotation = Quaternion.Lerp (parentChar.mAnimator.transform.rotation,  Quaternion.LookRotation(currLocation - PrevLocation), Time.deltaTime * lookSpeed);		
				}

				//actually applying the movement of the player
				Vector3 diffrence = moveToPoint - transform.position;
				diffrence.Normalize();
				Vector3  moveDirection = diffrence * parentChar.charSpeed;
				parentChar.mRigidBody.velocity = moveDirection;
			} else {
				wayPoints.RemoveAt(wayPoints.Count-1);
			}
		}
	}
	public override void endState() {
		//cancel calling the object avoidance algorithim
		CancelInvoke("objectAvoidanceMovement");
	}

	protected bool isObjectAvoidanceActive {
		get {
			return (wayPoints.Count > 0);
		}
	}

	void objectAvoidanceMovement() {
		if (Vector3.Distance(lastPos,transform.position) < 0.5f) {
			//character is properly stuck, let's add a way point to the left or right, but first best to go a step back
			Vector3 rayCenter = transform.position - parentChar.mAnimator.transform.forward + new Vector3(0,1f,0);
			float distance = Random.Range( 3f, 5f);
			while (distance > 1.5f) {
				Vector3 leftR = rayCenter - (parentChar.mAnimator.transform.right * distance);
				Vector3 rightR = rayCenter + (parentChar.mAnimator.transform.right * distance);

				if (Vector3.Distance(leftR,targetPosition) < Vector3.Distance(rightR,targetPosition)) {
					//left is closer to our target
					if (tryToAddLeftPoint(rayCenter,leftR,distance)) {
						//add a point to move a step back first
						wayPoints.Add(rayCenter);
						// point added, let's get out of this loop
						break;
					}
					//unable to add point to the left
					if (tryToAddRightPoint(rayCenter,rightR,distance)) {
						//add a point to move a step back first
						wayPoints.Add(rayCenter);
						// point added, let's get out of this loop
						break;
					}

				} else {
					//right is closer to our target
					if (tryToAddRightPoint(rayCenter,rightR,distance)) {
						//add a point to move a step back first
						wayPoints.Add(rayCenter);
						// point added, let's get out of this loop
						break;
					}
					//unable to add point to the right
					if (tryToAddLeftPoint(rayCenter,leftR,distance)) {
						//add a point to move a step back first
						wayPoints.Add(rayCenter);
						// point added, let's get out of this loop
						break;
					}

				}

				distance -= 0.5f;
			}
		}
		lastPos = transform.position;
		//recall the methode after another 2 seconds
		CancelInvoke("objectAvoidanceMovement");
		Invoke("objectAvoidanceMovement",objectAvoidanceTimeCheck);
	}

	bool tryToAddRightPoint(Vector3 rayCenter, Vector3 rightR, float distance) {
		//first step, check to my right if there's no obstacle
		Debug.DrawLine(rayCenter, rayCenter + (parentChar.mAnimator.transform.right*distance), Color.yellow, float.MaxValue);
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(rayCenter, parentChar.mAnimator.transform.right, out hit, distance)) {
			Debug.Log("Right hit.transform " + hit.transform.gameObject.name,hit.transform.gameObject);
		} else {

			//second step is checking a path of right
			Debug.DrawLine(rightR, rightR + (parentChar.mAnimator.transform.forward*distance), Color.yellow, float.MaxValue);
			hit = new RaycastHit();
			if(Physics.Raycast(rightR, parentChar.mAnimator.transform.forward, out hit, distance)) {
				Debug.Log("Right hit.transform " + hit.transform.gameObject.name,hit.transform.gameObject);
			} else {
				Debug.Log("Adding waypoint to the right");
				wayPoints.Add(rightR);
				return true;
			}
		}
		return false;
	}

	bool tryToAddLeftPoint(Vector3 rayCenter, Vector3 leftR, float distance) {
		//first step, check to my left if there's no obstacle
		Debug.DrawLine(rayCenter, rayCenter - (parentChar.mAnimator.transform.right*distance), Color.red, float.MaxValue);
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(rayCenter, -parentChar.mAnimator.transform.right, out hit, distance)) {
			Debug.Log("Left hit.transform " + hit.transform.gameObject.name,hit.transform.gameObject);
		} else {
			//second step is checking a path of left
			Debug.DrawLine(leftR, leftR + (parentChar.mAnimator.transform.forward*distance), Color.red, float.MaxValue);
			hit = new RaycastHit();
			if(Physics.Raycast(leftR, parentChar.mAnimator.transform.forward, out hit, distance)) {
				Debug.Log("Left hit.transform " + hit.transform.gameObject.name,hit.transform.gameObject);
			} else {
				Debug.Log("Adding waypoint to the left");
				wayPoints.Add(leftR);
				return true;
			}
		}

		return false;
	}
}
