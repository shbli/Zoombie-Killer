using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	float maxMoveDistance = 10f;
	Vector2 joyStickCenter;
	Vector3 showPos;
	Vector3 hidePos;
	Vector2 deltaFromCenter;
	Vector2 delta;
	bool isInDragMode = false;
	[SerializeField]
	float deltaMultiplier = 0.05f;

	enum JoystickAction {
		movement = 0,
		shooting = 1
	}

	[SerializeField]
	JoystickAction mJoystickAction;

	// Use this for initialization
	void Start () {
		joyStickCenter = new Vector2(transform.localPosition.x,transform.localPosition.y);
	}

	void Update () {
		if (GameController.Instance.isHeroControllable) {
			if (isInDragMode) {
				//move the hero
				switch (mJoystickAction) {
				case JoystickAction.movement:
					GameController.Instance.GameHero.moveToDirection(deltaFromCenter.y,deltaFromCenter.x);
					break;
				case JoystickAction.shooting:
					GameController.Instance.GameHero.fireToDirection(deltaFromCenter.y,deltaFromCenter.x);
					break;
				default:
					Debug.LogError("Unkowen joystick movement");
					break;
				}
			}
		}
	}

	public void OnBeginDrag(PointerEventData _EventData) {
		//move the joystick with delta amount
		delta = _EventData.delta * deltaMultiplier;
		transform.localPosition = transform.localPosition + (Vector3)delta;
		//set it in drag mode
		isInDragMode = true;
	}

	public void OnDrag(PointerEventData _EventData) {
		//move the joystick with delta amount
		delta = _EventData.delta * deltaMultiplier;
		transform.localPosition = transform.localPosition + (Vector3)delta;

		deltaFromCenter = new Vector2(transform.localPosition.x,transform.localPosition.y) - joyStickCenter;
		float distance = Mathf.Sqrt( (deltaFromCenter.x*deltaFromCenter.x) + (deltaFromCenter.y*deltaFromCenter.y) );
		if (distance > maxMoveDistance) {
			deltaFromCenter.x /= distance;
			deltaFromCenter.y /= distance;

			distance = maxMoveDistance;

			deltaFromCenter.x *= distance;
			deltaFromCenter.y *= distance;

			transform.localPosition = joyStickCenter + deltaFromCenter;
		}
	}

	public void OnEndDrag(PointerEventData _EventData) {
		//reset things to original state as user remove dragging his finger
		transform.localPosition = joyStickCenter;
		isInDragMode = false;
	}
}
