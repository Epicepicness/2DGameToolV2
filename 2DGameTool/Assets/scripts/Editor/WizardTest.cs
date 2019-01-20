using UnityEditor;
using UnityEngine;

public class WizardTest : ScriptableWizard
{
    public Texture2D map = null;
    public GameObject Generator;

    //LevelCreator assets;
    public ColorToPrefab[] Amount_of_Tiles;
    private LevelCreator assets;
    


    //Saves
    private static Texture2D NewMap;
    private static string TheName;
    private static Color TheColor;
    private static GameObject ThePrefab;

    


    static void CreateWizard()
    {
        //ScriptableWizard.DisplayWizard<WizardTest>("Create Light", "Create");
        

        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    void Start()
    {
        Texture2D map = EditorWindow.GetWindow<LevelCreator>().myMap;
        //map = assets.TheMap;
    }

    void OnWizardCreate()
    {
        GenerateLevel();
    }

    void OnWizardUpdate()
    {
        helpString = "Selecteer je tiles";
        
    }

    // When the user presses the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton() { 


    }


    void GenerateLevel()
    {
            Debug.Log("No kids");
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    GenerateTile(x, y);
                }
            }
    }

   
    //Generate gridmap
    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        //Only accept colors with a an aplha higher than 0
        if (pixelColor.a == 0)
        {
            return;

        }

        //Instantiate a prefab for each color in the grid
        foreach (ColorToPrefab colorMapping in Amount_of_Tiles)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x, y);
                GameObject temp = Instantiate(colorMapping.prefab, position, Quaternion.identity);
                temp.transform.parent = Generator.transform;
            }
        }
    }


}
