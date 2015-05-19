using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Text;
using System.IO;  

public class JsonParser : MonoBehaviour {

	string file_name = "assets/json/testfile.json";

	public static GameObject createObjectFromJson (string file_name) {
		var JsonObject = parseJson(readFile(file_name)); 

		GameObject gObject = new GameObject ();
		Mesh mesh = new Mesh();

		Vector3 [] newVertices = new Vector3();

	}

	public static JSONNode parseJson (string json) {
		return JSON.Parse(fText);
	}

	public static string readFile (string file_name) {
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

		return fText;
	}

}
