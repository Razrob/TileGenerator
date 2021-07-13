using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class TilePlacer2D : MonoBehaviour
{

    [SerializeField] private float tileSize;

    public GameObject[] tiles; 



    private List<GameObject> instantiatedTiles = new List<GameObject>();

    public Vector2 mapSize;

    private int tempCheck = 0;

    void Start()
    {
        tileSize *= 0.1f;
        //StartCoroutine(Generate(0.005f));
    }


    void Update()
    {


         
            if (Input.GetMouseButtonDown(0))
        {
            Reset();
        }
    }

    private void Reset()
    {

        TileGrid2D.Clear();
        StopAllCoroutines();
        for (int i = 0; i < instantiatedTiles.Count; i++)
        {
            Destroy(instantiatedTiles[i]);
        }
        instantiatedTiles.Clear();
        //StartCoroutine(Generate(0.005f));
        StartCoroutine(Generate(Vector3.zero));
    }



    private IEnumerator Generate(Vector3 position)
    {
        yield return new WaitForSeconds(0.01f);
        if (TileGrid2D.GetTileData(position) == null && position.x >= 0 && position.z >= 0 && position.x <= mapSize.x * tileSize && position.z <= mapSize.y * tileSize)
        {

            List<GameObject> suitableTiles = new List<GameObject>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (CheckSuitability(position, tiles[i].GetComponent<TileData2D>()))
                {
                    suitableTiles.Add(tiles[i]);

                }
            }
            if (suitableTiles.Count > 0)
            {
                int randomIndex = Random.Range(0, suitableTiles.Count);
                GameObject temp = Instantiate(suitableTiles[randomIndex], position, suitableTiles[randomIndex].transform.rotation);
                instantiatedTiles.Add(temp);
                TileGrid2D.AddTile(position, temp.GetComponent<TileData2D>());
            }
            else
            {

                int randomIndex = Random.Range(0, tiles.Length);
                GameObject temp = Instantiate(tiles[randomIndex], position, tiles[randomIndex].transform.rotation);
                instantiatedTiles.Add(temp);
                TileGrid2D.AddTile(position, temp.GetComponent<TileData2D>());
            }



            StartCoroutine(Generate(position + Vector3.forward * tileSize));
            StartCoroutine(Generate(position - Vector3.forward * tileSize));
            StartCoroutine(Generate(position + Vector3.right * tileSize));
            StartCoroutine(Generate(position - Vector3.right * tileSize));
        }

    }



    private IEnumerator dGenerate(float delay)
    { 

        List<GameObject> suitableTiles = new List<GameObject>();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.y; z++)
            {
                suitableTiles.Clear();
                Vector3 pos = new Vector3(x, 0, z) * tileSize;
                
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (CheckSuitability(pos, tiles[i].GetComponent<TileData2D>())) suitableTiles.Add(tiles[i]);
                }

                if (suitableTiles.Count == 0) continue;


                int randomIndex = Random.Range(0, suitableTiles.Count);
                GameObject temp = Instantiate(suitableTiles[randomIndex], pos, suitableTiles[randomIndex].transform.rotation);
                instantiatedTiles.Add(temp);
                TileGrid2D.AddTile(pos, temp.GetComponent<TileData2D>());

               // TileGrid.AddTile(pos, tiles[randomIndex].GetComponent<TileData>());
              //  Instantiate(suitableTiles[randomIndex], pos, suitableTiles[randomIndex].transform.rotation); 
                
                yield return new WaitForSeconds(delay);
            }
        } 
    } 

    private bool CheckSuitability(Vector3 tilePosition, TileData2D tileData)
    { 

        if (TileGrid2D.GetTileData(tilePosition - Vector3.forward * tileSize) != null) if(!TileGrid2D.GetTileData(tilePosition - Vector3.forward * tileSize).GetColorDataOnSide("forwardSide").SequenceEqual(tileData.GetColorDataOnSide("backSide"))) return false;
        if (TileGrid2D.GetTileData(tilePosition + Vector3.forward * tileSize) != null) if(!TileGrid2D.GetTileData(tilePosition + Vector3.forward * tileSize).GetColorDataOnSide("backSide").SequenceEqual(tileData.GetColorDataOnSide("forwardSide"))) return false;
        if (TileGrid2D.GetTileData(tilePosition - Vector3.right * tileSize) != null) if(!TileGrid2D.GetTileData(tilePosition - Vector3.right * tileSize).GetColorDataOnSide("rightSide").SequenceEqual(tileData.GetColorDataOnSide("leftSide"))) return false;
        if (TileGrid2D.GetTileData(tilePosition + Vector3.right * tileSize) != null) if (!TileGrid2D.GetTileData(tilePosition + Vector3.right * tileSize).GetColorDataOnSide("leftSide").SequenceEqual(tileData.GetColorDataOnSide("rightSide"))) return false;

        return true;  
    }

}
