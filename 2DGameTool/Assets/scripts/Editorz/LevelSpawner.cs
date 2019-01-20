using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelSpawner : EditorWindow {

	private GUIStyle buttonStyle;
	private Texture2D mapToGenerate = null;
	private GameObject parentObject;

	public List<ColorTiles> TilePerColour = new List<ColorTiles> ();


	[MenuItem ("Tool/LevelGenerator2 - Electric Boogaloo")]
	public static void ShowWindow () {
		GetWindow<LevelSpawner> ("LevelGenerator");
	}

	// Called at start to setup the button style
	private void WindowSetup () {
		buttonStyle = new GUIStyle (GUI.skin.button);
		buttonStyle.hover.textColor = Color.red;
		buttonStyle.normal.textColor = Color.red;
		buttonStyle.active.textColor = Color.green;
	}

	// Called to generate the actual level
	private void OnGenerateLevel () {
		Debug.Log ("On Generate Level");
		if (mapToGenerate == null) {
			if (Help.HasHelpForObject (mapToGenerate))
				Help.ShowHelpForObject (mapToGenerate);
			ShowNotification (new GUIContent ("No Map selected"));
			return;
		}
		else if (parentObject == null) {
			if (Help.HasHelpForObject (parentObject))
				Help.ShowHelpForObject (parentObject);
			ShowNotification (new GUIContent ("No Prefab selected"));
			return;
		}
		Debug.Log ("Komt voorbij checks");

		if (parentObject == null)
			parentObject = new GameObject ();
		PrefabUtility.InstantiatePrefab (parentObject);

		int count = 0;

		Debug.Log ("Begin pixel loop");
		for (int x = 0; x < mapToGenerate.width; x++) {
			for (int y = 0; y < mapToGenerate.height; y++) {

				Color pixelColor = mapToGenerate.GetPixel (x, y);
				Debug.Log ("Checking: " + pixelColor + ", at: (" + x + ", " + y + ").");

				if (pixelColor.a < 1) {
					Debug.Log ("Alpha < 1");
					continue;
				}

				foreach (ColorTiles c in TilePerColour) {
					if (c.color == pixelColor) {
						
						Debug.Log ("De kleur komt overeen; spawning: " + c.prefab);
						GameObject newTile = Instantiate (c.prefab, new Vector2 (x, y), Quaternion.identity);
						newTile.transform.parent = parentObject.transform;

						count++;
						break;
					}
					Debug.Log ("Kleur:" + pixelColor + " komt niet overeen met " + c.color);
				}
			}
		}
		Debug.Log ("Spawned: " + count + " total gameObjects");
	}

	// Clears all child objects of the current parentObject
	[ContextMenu ("Kill Kids")]
	public void ClearLevel () {
		if (parentObject == null)
			return;

		int childs = parentObject.transform.childCount;
		for (int i = childs - 1; i >= 0; i--) {
			GameObject.DestroyImmediate (parentObject.transform.GetChild (i).gameObject);
		}
	}

	// ---- OnGUI --------------------------------------------------------------------------------------------------------
	private void OnGUI () {
		if (buttonStyle == null)
			WindowSetup ();

		if (TilePerColour != null) {
			EditorGUILayout.BeginVertical ();
			for (int x = 0; x < TilePerColour.Count; x++) {
				TilePerColour [x].prefab = (GameObject) EditorGUILayout.ObjectField ("GameObject", TilePerColour [x].prefab, typeof (GameObject), true);
			}
			EditorGUILayout.EndVertical ();
		}

		//Mapje
		GUILayout.Space (20);
		GUILayout.Label ("Select your map", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Map Image: ");
		mapToGenerate = (Texture2D) EditorGUILayout.ObjectField (mapToGenerate, typeof (Texture2D), true);
		GUILayout.EndHorizontal ();

		//LevelSpawner
		GUILayout.Space (20);
		GUILayout.Label ("Insert the spawner", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Parent Object: ");
		parentObject = (GameObject) EditorGUILayout.ObjectField (parentObject, typeof (GameObject), true);
		GUILayout.EndHorizontal ();

		GUILayout.Space (50);

		if (GUILayout.Button ("Insert prefabs", GUILayout.Width (130))) {
			ScriptableWizard.DisplayWizard<TileListWizard> ("I am wizard", "Create", "Apply");
			Debug.Log ("Wizard Summoned");
		}

		// ---- Buttons at the Bottom ----
		GUILayout.Space (20);

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Generate Level", GUILayout.Width (100))) {
			OnGenerateLevel ();
		}
		if (GUILayout.Button ("Clear Level", buttonStyle, GUILayout.Width (100))) {
			ClearLevel ();
		}
		GUILayout.EndHorizontal ();
	}

}
