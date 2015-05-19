using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;
using SimpleJSON;
using System.Text;
using System.ComponentModel;

public class Controls : MonoBehaviour {

	public GameObject target;

	private Vector2 lastFingerPosition = new Vector2 ();

	public delegate void dInstantiate (GameObject g);
	
	// Use this for initialization
	void Start () {
		DirectoryInfo dir = new DirectoryInfo("assets/json");
		FileInfo[] info = dir.GetFiles("*.json");
		foreach (FileInfo f in info)  {
			StartCoroutine(createObjectFromJson("assets/json/" + f.Name, target));
		}
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

		transform.LookAt (target.transform.position);

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

	IEnumerator createObjectFromJson (string file_name, GameObject t) {

		string fText = "";
		string line;
		StreamReader reader = new StreamReader(file_name, Encoding.Default);
		
		using (reader)
		{
			do
			{
				line = reader.ReadLine();
				
				if (line != null)
				{
					fText += line;
				}
			}
			while (line != null);
			
			reader.Close();
		}
		
		JSONArray geometries;
		
		try {
			var jsonObject = JSON.Parse (fText);
			geometries = (JSONArray) jsonObject ["geometries"];

			foreach (JSONNode node in geometries) {
				GameObject target = Instantiate(t);
				
				JSONArray f;
				JSONArray v;
				
				try {
					f = (JSONArray) node["data"]["faces"];
					v = (JSONArray) node["data"]["vertices"];
					
					if (v.Count > 0 && f.Count > 0) {
						Vector3 [] vertices = new Vector3[v.Count / 3];
						int [] faces = new int[f.Count - (f.Count / 4)];
						
						int index = 0;
						int count = 0;
						do {
							vertices [index++] = new Vector3 (
								float.Parse ((string)v [count++]), 
								float.Parse ((string)v [count++]), 
								float.Parse ((string)v [count++])
								);
						} while (count < v.Count);
						
						index = 0;
						count = 0;
						do {
							count++;
							faces [index++] = int.Parse ((string)f [count++]);
							faces [index++] = int.Parse ((string)f [count++]);
							faces [index++] = int.Parse ((string)f [count++]);
						} while (count < f.Count);
						
						Mesh mesh = new Mesh ();
						
						mesh.vertices = vertices;
						
						mesh.triangles = faces;
						
						mesh.RecalculateNormals ();
						
						target.GetComponent<MeshFilter> ().mesh = mesh;
						
					} else {
						Destroy (target);
					}
				} catch (System.Exception e) {
					Debug.Log (e.StackTrace);
					Destroy (target);
				}
			}
		} catch (System.Exception e) {
			Debug.Log (e.StackTrace);
		} 
		
		yield return null;
	}

}
