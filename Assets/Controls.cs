using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public GameObject target;

	private Vector2 lastFingerPosition = new Vector2 ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
			lastFingerPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}

		if (Input.GetKey(KeyCode.Mouse0)) {
			Vector2 rotation = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastFingerPosition;	
			if (rotation.magnitude > 0) {
				target.transform.Rotate(rotation.y, -rotation.x, 0, Space.World);
			}
			lastFingerPosition = Input.mousePosition;
		}

		if (Input.GetKey(KeyCode.Mouse1)) {
			Vector2 translation = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastFingerPosition;	
			if (translation.magnitude > 0) {
				transform.Translate(-translation.x * Time.deltaTime * 2, translation.y  * Time.deltaTime  * 2, 0, Space.Self);
			}
			lastFingerPosition = Input.mousePosition;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			target.transform.Translate (0, 0, -5 * Time.deltaTime, Space.World);
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			target.transform.Translate (0, 0, 5 * Time.deltaTime, Space.World);
		}

		transform.LookAt (new Vector3());

		foreach (Touch touch in Input.touches) {

			Debug.Log ("Click");

			switch ( touch.phase ) {
				case TouchPhase.Began : {
					lastFingerPosition = touch.position;
				}
				break;

				case TouchPhase.Moved : {
					Vector2 rotation = touch.position - lastFingerPosition;	
					target.transform.Rotate (rotation.x, rotation.y, 0);
					lastFingerPosition = touch.position;
				}
				break;

				case TouchPhase.Ended : 
					
					break;
			}

		}
	}
}
