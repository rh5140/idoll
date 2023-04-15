using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

public class TilemapManager : MonoBehaviour
{
    // Tilemaps
    //private Tilemap roomTilemap; 
    private Tilemap lightTilemap;

    // Light Prefabs
    [SerializeField]
    private List<GameObject> lightPrefabs;

    // Created GameObjects
    private List<GameObject> lights;

    void Start()
    {
        //roomTilemap = GameObject.Find("Rooms").GetComponent<Tilemap>();
        lightTilemap = GameObject.Find("Lights").GetComponent<Tilemap>();

        InitializeLights();
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
}
