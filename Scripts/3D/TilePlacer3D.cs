using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class TilePlacer3D : MonoBehaviour
{

    [SerializeField] private float tileSize;

    public GameObject[] tiles;



    private List<GameObject> instantiatedTiles = new List<GameObject>();

    public Vector3 mapSize;

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

        TileGrid3D.Clear();
        StopAllCoroutines();
        for (int i = 0; i < instantiatedTiles.Count; i++)
        {
            Destroy(instantiatedTiles[i]);
        }
        instantiatedTiles.Clear();
        StartCoroutine(Generate(0.000000005f));
    }

    private IEnumerator Generate(float delay)
    {

        List<GameObject> suitableTiles = new List<GameObject>();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    suitableTiles.Clear();
                    Vector3 pos = new Vector3(x, y, z) * tileSize;

                    for (int i = 0; i < tiles.Length; i++)
                    {
                        if (CheckSuitability(pos, tiles[i].GetComponent<TileData3D>())) suitableTiles.Add(tiles[i]);
                    }

                    if (suitableTiles.Count == 0) continue;


                    int randomIndex = Random.Range(0, suitableTiles.Count);
                    GameObject temp = Instantiate(suitableTiles[randomIndex], pos, suitableTiles[randomIndex].transform.rotation);
                    instantiatedTiles.Add(temp);
                    TileGrid3D.AddTile(pos, temp.GetComponent<TileData3D>());

                    // TileGrid.AddTile(pos, tiles[randomIndex].GetComponent<TileData>());
                    //  Instantiate(suitableTiles[randomIndex], pos, suitableTiles[randomIndex].transform.rotation); 

                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }

    private bool CheckSuitability(Vector3 tilePosition, TileData3D tileData)
    {

        if (TileGrid3D.GetTileData(tilePosition - Vector3.forward * tileSize) != null) if (!TileGrid3D.GetTileData(tilePosition - Vector3.forward * tileSize).GetColorDataOnSide("forwardSide").SequenceEqual(tileData.GetColorDataOnSide("backSide"))) return false;
        if (TileGrid3D.GetTileData(tilePosition + Vector3.forward * tileSize) != null) if (!TileGrid3D.GetTileData(tilePosition + Vector3.forward * tileSize).GetColorDataOnSide("backSide").SequenceEqual(tileData.GetColorDataOnSide("forwardSide"))) return false;
        if (TileGrid3D.GetTileData(tilePosition - Vector3.right * tileSize) != null) if (!TileGrid3D.GetTileData(tilePosition - Vector3.right * tileSize).GetColorDataOnSide("rightSide").SequenceEqual(tileData.GetColorDataOnSide("leftSide"))) return false;
        if (TileGrid3D.GetTileData(tilePosition + Vector3.right * tileSize) != null) if (!TileGrid3D.GetTileData(tilePosition + Vector3.right * tileSize).GetColorDataOnSide("leftSide").SequenceEqual(tileData.GetColorDataOnSide("rightSide"))) return false;

        if (TileGrid3D.GetTileData(tilePosition + Vector3.up * tileSize) != null) if (!TileGrid3D.GetTileData(tilePosition + Vector3.up * tileSize).GetColorDataOnSide("bottomSide").SequenceEqual(tileData.GetColorDataOnSide("topSide"))) return false;
        if (TileGrid3D.GetTileData(tilePosition - Vector3.up * tileSize) != null) if (!TileGrid3D.GetTileData(tilePosition - Vector3.up * tileSize).GetColorDataOnSide("topSide").SequenceEqual(tileData.GetColorDataOnSide("bottomSide"))) return false;

        return true;
    }

}
