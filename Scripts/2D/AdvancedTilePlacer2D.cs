using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class AdvancedTilePlacer2D : MonoBehaviour
{


    [SerializeField] private float tileSize;

    public TileData2D[] tiles;
    private GameObject[] tilesObjects;


    private delegate void ListTileCheckMethods(Vector3 position);


    private List<GameObject> instantiatedTiles = new List<GameObject>();

    public Vector2 mapSize;
     

    void Start()
    {
        tileSize *= 0.1f;
        tilesObjects = new GameObject[tiles.Length];
         

        for (int i = 0; i < tiles.Length; i++) Destroy(tiles[i].GetComponent<TileData2DCreator>());


        //for (int i = 0; i < tiles.Length; i++)
        //{
        //    GameObject obj = new GameObject();
        //    obj.AddComponent<MeshFilter>().sharedMesh = tiles[i].GetComponent<MeshFilter>().sharedMesh; 
        //    obj.AddComponent<MeshRenderer>().material = tiles[i].GetComponent<MeshRenderer>().material;
        //    obj.AddComponent<MeshCollider>().sharedMesh = tiles[i].GetComponent<MeshCollider>().sharedMesh;
        //    tilesObjects[i] = obj;
        //}

    }



    void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            Reset();
            //Test();
        }
    }


    void Test()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.y; z++)
            {
                AdvancedTileGrid2D.AddTiles(new Vector3(x, 0, z) * tileSize, tiles.ToList());
            }
        }
        Vector3 pos = new Vector3(1, 0, 1) * tileSize;
        AdvancedTileGrid2D.AddOnlyTile(pos, tiles[0]);
        CheckNearTiles(pos);
     //   Debug.Log(pos.GetDirection(0, tileSize));
        Debug.Log(AdvancedTileGrid2D.GetTileNumberInCell(pos.GetDirection(Direction.left, tileSize)));



    }


    private void Reset()
    {
        StopAllCoroutines();

        AdvancedTileGrid2D.Clear();

        for (int i = 0; i < instantiatedTiles.Count; i++)
        {
            Destroy(instantiatedTiles[i]);
        }
        instantiatedTiles.Clear();
        StartCoroutine(Generate());

    }

    private IEnumerator Generate()
    {

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.y; z++)
            {
                AdvancedTileGrid2D.AddTiles(new Vector3(x, 0, z) * tileSize, tiles.ToList());
            }
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.y; z++)
            { 
                Vector3 pos = new Vector3(x, 0, z) * tileSize;

                //Debug.Log(AdvancedTileGrid2D.GetTileNumberInCell(pos));
                if (AdvancedTileGrid2D.GetTileNumberInCell(pos) < 1) continue;
                 

                int index = Random.Range(0, AdvancedTileGrid2D.GetTileNumberInCell(pos));

                GameObject obj = Instantiate(AdvancedTileGrid2D.GetTileData(pos)[index].gameObject, pos, AdvancedTileGrid2D.GetTileData(pos)[index].transform.rotation);
                //obj.GetComponent<TileData2D>().enabled = false;
                Destroy(obj.GetComponent<TileData2D>());
                instantiatedTiles.Add(obj);


                AdvancedTileGrid2D.AddOnlyTile(pos, AdvancedTileGrid2D.GetTileData(pos)[index]); 

                CheckNearTiles(pos);

                yield return null;
            }
             
        }
    }


    //private void StartGen()
    //{
    //    for (int x = 0; x < mapSize.x; x++)
    //    {
    //        for (int z = 0; z < mapSize.y; z++)
    //        {
    //            AdvancedTileGrid2D.AddTiles(new Vector3(x, 0, z) * tileSize, tiles.ToList());
    //        }
    //    }
    //    Vector3 pos = Vector3.zero;
          

    //    StartCoroutine(Generate(pos));
    //}



    //private IEnumerator Generate(Vector3 position)
    //{

    //    yield return new WaitForSeconds(0.01f);
    //    if (AdvancedTileGrid2D.GetTileNumberInCell(position) > 1 && position.x >= 0 && position.z >= 0 && position.x <= mapSize.x * tileSize && position.z <= mapSize.y * tileSize)
    //    { 

    //        int index = Random.Range(0, AdvancedTileGrid2D.GetTileNumberInCell(position));
 
    //        AdvancedTileGrid2D.AddOnlyTile(position, AdvancedTileGrid2D.GetTileData(position)[index]);

    //        CheckNearTiles(position);


    //        StartCoroutine(Generate(position + Vector3.forward * tileSize));
    //        StartCoroutine(Generate(position - Vector3.forward * tileSize));
    //        StartCoroutine(Generate(position + Vector3.right * tileSize));
    //        StartCoroutine(Generate(position - Vector3.right * tileSize));
    //    }
    //}




    private bool CheckTrue(Vector3 position, TileData2D tileData)
    {
        if (AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize) != null && AdvancedTileGrid2D.CellIsStay(position - Vector3.forward * tileSize))
        {
            if (!AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize)[0].GetColorDataOnSide("forwardSide").SequenceEqual(tileData.GetColorDataOnSide("backSide"))) return false;
        }
        if (AdvancedTileGrid2D.GetTileData(position + Vector3.forward * tileSize) != null && AdvancedTileGrid2D.CellIsStay(position + Vector3.forward * tileSize))
        {
            if (!AdvancedTileGrid2D.GetTileData(position + Vector3.forward * tileSize)[0].GetColorDataOnSide("backSide").SequenceEqual(tileData.GetColorDataOnSide("forwardSide"))) return false;
        }
        if (AdvancedTileGrid2D.GetTileData(position + Vector3.right * tileSize) != null && AdvancedTileGrid2D.CellIsStay(position + Vector3.right * tileSize))
        {
            if (!AdvancedTileGrid2D.GetTileData(position + Vector3.right * tileSize)[0].GetColorDataOnSide("leftSide").SequenceEqual(tileData.GetColorDataOnSide("rightSide"))) return false;
        }
        if (AdvancedTileGrid2D.GetTileData(position - Vector3.right * tileSize) != null && AdvancedTileGrid2D.CellIsStay(position - Vector3.right * tileSize))
        {
            if (!AdvancedTileGrid2D.GetTileData(position - Vector3.right * tileSize)[0].GetColorDataOnSide("rightSide").SequenceEqual(tileData.GetColorDataOnSide("leftSide"))) return false;
        }


        return true;
    }







    private void CheckNearTiles(Vector3 position)
    {  


        if(AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.forward, tileSize)) > 1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.forward, tileSize)))
        {
            List<TileData2D> newPossibleTileData = new List<TileData2D>();

            for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.forward, tileSize)); i++)
            {
                if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position)[0], "forwardSide", AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.forward, tileSize))[i], "backSide"))
                { 
                    newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.forward, tileSize))[i]);
                }
            }
            if (newPossibleTileData.Count > 0) AdvancedTileGrid2D.ChangeTileCell(position.GetDirection(Direction.forward, tileSize), newPossibleTileData); 
        }

        if (AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.back, tileSize)) > 1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.back, tileSize)))
        {
            List<TileData2D> newPossibleTileData = new List<TileData2D>();

            for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.back, tileSize)); i++)
            {
                if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position)[0], "backSide", AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.back, tileSize))[i], "forwardSide"))
                {
                    newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.back, tileSize))[i]);
                }
            } 
            if (newPossibleTileData.Count > 0) AdvancedTileGrid2D.ChangeTileCell(position.GetDirection(Direction.back, tileSize), newPossibleTileData); 
        }

        if (AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.right, tileSize)) > 1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.right, tileSize)))
        {
            List<TileData2D> newPossibleTileData = new List<TileData2D>();


            for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.right, tileSize)); i++)
            { 
                if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position)[0], "rightSide", AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.right, tileSize))[i], "leftSide"))
                {
                    newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.right, tileSize))[i]);
                }
            }

            if (newPossibleTileData.Count > 0) AdvancedTileGrid2D.ChangeTileCell(position.GetDirection(Direction.right, tileSize), newPossibleTileData); 
        }

        if (AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.left, tileSize)) > 1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.left, tileSize)))
        {
            List<TileData2D> newPossibleTileData = new List<TileData2D>();

            for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.left, tileSize)); i++)
            {
                if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position)[0], "leftSide", AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.left, tileSize))[i], "rightSide"))
                {
                    newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.left, tileSize))[i]);
                }
            } 
            if (newPossibleTileData.Count > 0) AdvancedTileGrid2D.ChangeTileCell(position.GetDirection(Direction.left, tileSize), newPossibleTileData); 
        }




        //if (AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.right_forward, tileSize)) != -1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.right_forward, tileSize)))
        //{
        //    List<TileData2D> newPossibleTileData = new List<TileData2D>();

        //    for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.right_forward, tileSize)); i++)
        //    { 
        //        bool isSuitable1 = false;
        //        bool isSuitable2 = false;

        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.forward, tileSize)); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.forward, tileSize))[f], "rightSide", AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.right_forward, tileSize))[i], "leftSide")) isSuitable1 = true;

        //        }  

        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position.GetDirection(Direction.right, tileSize)); f++)
        //        {
        //            if (!CheckSuitability(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.right, tileSize))[f], "forwardSide", AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.right_forward, tileSize))[i], "backSide")) isSuitable2 = true;
        //        }

        //        if (isSuitable1 && isSuitable2) newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position.GetDirection(Direction.right_forward, tileSize))[i]);
        //    }

        //    AdvancedTileGrid2D.ChangeTileCell(position.GetDirection(Direction.right_forward, tileSize), newPossibleTileData);
        //}
         

        //if (AdvancedTileGrid2D.GetTileNumberInCell(position + Vector3.forward * tileSize - Vector3.right * tileSize) != -1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.left_forward, tileSize)))
        //{
        //    List<TileData2D> newPossibleTileData = new List<TileData2D>();

        //    for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position + Vector3.forward * tileSize - Vector3.right * tileSize); i++)
        //    {
        //        bool isSuitable1 = false;
        //        bool isSuitable2 = false;
        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position + Vector3.forward * tileSize); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position + Vector3.forward * tileSize)[f], "leftSide", AdvancedTileGrid2D.GetTileData(position + Vector3.forward * tileSize - Vector3.right * tileSize)[i], "rightSide")) isSuitable1 = true;
        //        } 

        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.right * tileSize); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position - Vector3.right * tileSize)[f], "forwardSide", AdvancedTileGrid2D.GetTileData(position + Vector3.forward * tileSize - Vector3.right * tileSize)[i], "backSide")) isSuitable2 = true;
        //        }

        //        if (isSuitable1 && isSuitable2) newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position + Vector3.forward * tileSize - Vector3.right * tileSize)[i]);
        //    }

        //    AdvancedTileGrid2D.ChangeTileCell(position + Vector3.forward * tileSize - Vector3.right * tileSize, newPossibleTileData); 
        //}






        //if (AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.forward * tileSize + Vector3.right * tileSize) != -1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.right_back, tileSize)))
        //{
        //    List<TileData2D> newPossibleTileData = new List<TileData2D>();

        //    for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.forward * tileSize + Vector3.right * tileSize); i++)
        //    {
        //        bool isSuitable1 = false;
        //        bool isSuitable2 = false;
        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.forward * tileSize); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize)[f], "rightSide", AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize + Vector3.right * tileSize)[i], "leftSide")) isSuitable1 = true;
        //        } 

        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position + Vector3.right * tileSize); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position + Vector3.right * tileSize)[f], "backSide", AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize + Vector3.right * tileSize)[i], "forwardSide")) isSuitable2 = true;
        //        }

        //        if (isSuitable1 && isSuitable2) newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize + Vector3.right * tileSize)[i]);
        //    }

        //    AdvancedTileGrid2D.ChangeTileCell(position - Vector3.forward * tileSize + Vector3.right * tileSize, newPossibleTileData); 
        //}



        //if (AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.forward * tileSize - Vector3.right * tileSize) != -1 && !AdvancedTileGrid2D.CellIsStay(position.GetDirection(Direction.left_back, tileSize)))
        //{
        //    List<TileData2D> newPossibleTileData = new List<TileData2D>();

        //    for (int i = 0; i < AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.forward * tileSize - Vector3.right * tileSize); i++)
        //    {
        //        bool isSuitable1 = false;
        //        bool isSuitable2 = false;
        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.forward * tileSize); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize)[f], "leftSide", AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize - Vector3.right * tileSize)[i], "rightSide")) isSuitable1 = true;
        //        } 

        //        for (int f = 0; f < AdvancedTileGrid2D.GetTileNumberInCell(position - Vector3.right * tileSize); f++)
        //        {
        //            if (CheckSuitability(AdvancedTileGrid2D.GetTileData(position - Vector3.right * tileSize)[f], "backSide", AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize - Vector3.right * tileSize)[i], "forwardSide")) isSuitable2 = true;
        //        }
        //        if (isSuitable1 && isSuitable2) newPossibleTileData.Add(AdvancedTileGrid2D.GetTileData(position - Vector3.forward * tileSize - Vector3.right * tileSize)[i]);
        //    }

        //    AdvancedTileGrid2D.ChangeTileCell(position - Vector3.forward * tileSize - Vector3.right * tileSize, newPossibleTileData); 
        //}

         

    }

     


    private bool CheckSuitability(TileData2D mainTileData, string mainTileDataSide, TileData2D secondTileData, string secondTileDataSide)
    {
        if (!mainTileData.GetColorDataOnSide(mainTileDataSide).SequenceEqual(secondTileData.GetColorDataOnSide(secondTileDataSide))) return false; 

        return true; 
    }
}
