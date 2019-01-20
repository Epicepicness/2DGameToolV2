//using Types;
using UnityEngine;
using UnityEditor;


public class LevelCreator : EditorWindow {

    public Texture2D myMap;
    public ColorToPrefab[] colorMappings;

    public Color color;

    public GameObject Generator;
    //public GameObject MyPrefab;

    public Texture2D TheMap;

    [MenuItem("Tool/LevelGenerator")]
    public static void ShowWindow()
    {
        GetWindow<LevelCreator>("LevelGenerator");

    }

    void Start()
    {
        TheMap =  myMap;

    }
    
    void OnGUI()
    {

        

        //Fortnite GUI Button Skins
        var style = new GUIStyle(GUI.skin.button);
        style.hover.textColor = Color.red;
        style.normal.textColor = Color.red;
        style.active.textColor = Color.green;


        //style.fontSize = 18;
        if (colorMappings != null)
        {
            EditorGUILayout.BeginVertical();
            for (int x = 0; x < colorMappings.Length; x++)
            {
                colorMappings[x].prefab = (GameObject)EditorGUILayout.ObjectField("GameObject", colorMappings[x].prefab, typeof(GameObject), true);

            }
            EditorGUILayout.EndVertical();
        }


        //window code

        //Mapje
        GUILayout.Space(20);
        GUILayout.Label("Please select your map", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Map");
        myMap = (Texture2D)EditorGUILayout.ObjectField(myMap, typeof(Texture2D),true);
        GUILayout.EndHorizontal();

        //LevelSpawner
        GUILayout.Space(20);
        GUILayout.Label("Now insert the spawner", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("LevelSpawn");
        Generator = (GameObject)EditorGUILayout.ObjectField(Generator, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        ////ColorPicker
        //GUILayout.Space(20);
        //GUILayout.Label("Define your prefab color", EditorStyles.boldLabel);
        //color = EditorGUILayout.ColorField("Color", color);
        //GUILayout.Space(20);

        ////Prefab slot
        //GUILayout.Label("Last but not least, insert your prefab", EditorStyles.boldLabel);
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Prefab");
        //MyPrefab = (GameObject)EditorGUILayout.ObjectField(MyPrefab, typeof(GameObject), true);
        //GUILayout.EndHorizontal();

        GUILayout.Space(70);

        if (GUILayout.Button("Insert prefabs", GUILayout.Width(130))) {
            ScriptableWizard.DisplayWizard<WizardTest>("Prefab select", "Create", "Apply");
        }



        GUILayout.Space(20);
        // Generate Level Button
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Level", GUILayout.Width(100)))
        {
            if (myMap != null)
            {
                if (Generator != null)
                {

                    //if (MyPrefab != null)
                    //{
                        GenerateLevel();
                        
                    //}
                    //else if (Help.HasHelpForObject(MyPrefab))
                    //{
                    //    Help.ShowHelpForObject(MyPrefab);
                    //}
                    //else
                    //{
                    //    ShowNotification(new GUIContent("No Prefab selected")); 
                    //}
                }
                else if (Help.HasHelpForObject(Generator))
                {
                    Help.ShowHelpForObject(Generator);
                }
                else {
                    ShowNotification(new GUIContent("No Spawn object selected"));
                }
            }
            else if (Help.HasHelpForObject(myMap))
            {
                Help.ShowHelpForObject(myMap);
            }
            else {
                ShowNotification(new GUIContent("No Map selected"));
            }
        }

        //GUILayout.Space(10);
        // Delete Level Button
        if (GUILayout.Button("Delete prefabs", style, GUILayout.Width(100)))
        {
            Wipe();
        }
        GUILayout.EndHorizontal();

    }




    void GenerateLevel()
    {
            for (int x = 0; x < myMap.width; x++)
            {
                for (int y = 0; y < myMap.height; y++)
                {
                    GenerateTile(x, y);
                }
            }
    }




    [ContextMenu("Kill Kids")]
    public void Wipe()
    {
        int childs = Generator.transform.childCount;
        for (int i = childs - 1; i >=0; i--)
        {
            GameObject.DestroyImmediate(Generator.transform.GetChild(i).gameObject);
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = myMap.GetPixel(x, y);
        
        if (pixelColor.a == 0)
        {
            return;
        }
        if (color.Equals(pixelColor))
        {
            Vector2 position = new Vector2(x, y);
            PrefabUtility.InstantiatePrefab(Generator);
            GameObject temp = Instantiate(Generator, position, Quaternion.identity);
            temp.transform.parent = Generator.transform;
        }

        //foreach (ColorToPrefab colorMapping in colorMappings)
        //{

        //    Debug.Log("yo");
        //    if (colorMapping.color.Equals(pixelColor))
        //    {
        //        Vector2 position = new Vector2(x, y);
        //        //Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
        //        GameObject temp = Instantiate(Object1, position, Quaternion.identity);
        //        temp.transform.parent = Generator.transform;
        //        Debug.Log("Done did it to em");
        //    }
        //}
    }
}
