using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

public class TilemapManager : MonoBehaviour
{
    // Tilemaps
    //private Tilemap roomTilemap;
    private Tilemap floorTilemap;
    private Tilemap lightTilemap;

    // Light Prefabs
    [SerializeField]
    private List<GameObject> lightPrefabs;

    // Created GameObjects
    private List<GameObject> lights;

    // Shadow Rule Tiles
    [SerializeField]
    private TileBase wallShadowRuleTile;

    void Start()
    {
        //roomTilemap = GameObject.Find("Rooms").GetComponent<Tilemap>();
        try
        {
            lightTilemap = GameObject.Find("Lights").GetComponent<Tilemap>();
            InitializeLights();
        }
        catch
        {
            Debug.Log("No Tilemap GameObject named 'Lights' was found");
        }


        try
        {
            floorTilemap = GameObject.Find("Floors").GetComponent<Tilemap>();
            InitializeWallShadows();
        }
        catch
        {
            Debug.Log("No Tilemap GameObject named 'Floors' was found");
        }
    }

    // Spawn a light at each location in the lights tilemap
    private void InitializeLights()
    {
        // Spawn lights based on the Tilemap
        int lightCount = 0;
        foreach (Vector3Int pos in lightTilemap.cellBounds.allPositionsWithin)
        {
            if (lightTilemap.HasTile(pos))
            {
                lightCount++;

                string tileName = lightTilemap.GetTile(pos).name;
                GameObject light;
                 
                for (int lightNumber = 0; lightNumber < lightPrefabs.Count; lightNumber++)
                {
                    if (tileName == "Lights1_" + lightNumber)
                    {
                        light = Instantiate(lightPrefabs[lightNumber]);
                        light.transform.position = pos;
                        // Reposition tilemap for differently-sized light tilemaps
                        light.transform.position = light.transform.position / (1f/ lightTilemap.transform.localScale.x) - new Vector3(lightTilemap.transform.localPosition.x / 2f, lightTilemap.transform.localPosition.y / 2f);
                        break;
                    }
                }

                //var light = new GameObject("Ligh`t" + lightCount);
                //light.AddComponent<Light2D>();
                //GameObject lights = Instantiate(lightPrefabs[0])
                //lights.Add(Instantiate(lightPrefabs[0]));
                //lights[^1].transform.position = pos;
                //lights[^1].AddComponent<Light>();
                //lights.Add(new GameObject("Light"));
                //lights[^1].AddComponent<Light>();

            }
        }

        lightTilemap.ClearAllTiles();
    }

    private void InitializeWallShadows()
    {
        // Potential reasons why wall shadows aren't showing up
        // 1) Shadows Rule Tile needs to be dragged into the TilemapManager in the inspector
        // 2) Room Grid is not tagged with 'CurrentRoom'
        // 3) No Floor tilemap was found
        // 4) Something on the Background sorting layer has an order > 10 (unlikely)

        // Create WallShadows Tilemap
        var wallShadows = new GameObject("WallShadows");
        var tm = wallShadows.AddComponent<Tilemap>();
        var tr = wallShadows.AddComponent<TilemapRenderer>();

        wallShadows.transform.SetParent(GameObject.FindGameObjectWithTag("CurrentRoom").transform);
        tr.sortingLayerName = "Background";
        tr.sortingOrder = 10; // Render shadows on top of all of the background/floor tiles
        wallShadows.transform.localPosition = new Vector2(0f, 0f);

        foreach (Vector3Int pos in floorTilemap.cellBounds.allPositionsWithin)
        {
            if (floorTilemap.HasTile(pos))
            {
                tm.SetTile(pos, wallShadowRuleTile);
            }
        }    
    }
}
