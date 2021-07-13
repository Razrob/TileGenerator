using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AdvancedTileGrid2D
{ 
    private static Dictionary<string, List<TileData2D>> possibleTiles = new Dictionary<string, List<TileData2D>>();
    private static Dictionary<string, bool> tileIsStay = new Dictionary<string, bool>();




    public static bool AddOnlyTile(Vector3 position, TileData2D tileData)
    { 
        if(possibleTiles.ContainsKey(position.ToString()))
        {
            List<TileData2D> tiles = new List<TileData2D>();
            tiles.Add(tileData);
            possibleTiles[position.ToString()] = tiles;
            tileIsStay[position.ToString()] = true;
            return true;
        }
        return false;
    }

    public static bool CellIsStay(Vector3 position)
    {
        if (tileIsStay.ContainsKey(position.ToString()))
        {
            return tileIsStay[position.ToString()];
        }
        return false;
    }


    public static bool ChangeTileCell(Vector3 position, List<TileData2D> tileData)
    {
        //Debug.Log(position.ToString());
        if (!possibleTiles.ContainsKey(position.ToString())) return false;

        possibleTiles[position.ToString()] = tileData;
        return true;
    }

    public static int GetTileNumberInCell(Vector3 position)
    {
        if (!possibleTiles.ContainsKey(position.ToString())) return -1;
        return possibleTiles[position.ToString()].Count;
    }

    public static List<TileData2D> GetTileData(Vector3 position)
    {
        if (possibleTiles.ContainsKey(position.ToString())) return possibleTiles[position.ToString()];
        return null;
    } 

    public static void AddTiles(Vector3 position, List<TileData2D> tileData)
    {
        possibleTiles.Add(position.ToString(), tileData);
        tileIsStay.Add(position.ToString(), false);
    } 

    public static void Clear()
    {
        possibleTiles.Clear();
        tileIsStay.Clear();
    }

    public static Vector3 GetDirection(this Vector3 startPosition, Direction direction, float tileSize)
    {
        switch (direction)
        {
            case Direction.right: 
                return startPosition + Vector3.right * tileSize; 
            case Direction.right_forward:
                return startPosition + Vector3.right * tileSize + Vector3.forward * tileSize; 
            case Direction.forward:
                return startPosition + Vector3.forward * tileSize; 
            case Direction.left_forward:
                return startPosition - Vector3.right * tileSize + Vector3.forward * tileSize; 
            case Direction.left:
                return startPosition - Vector3.right * tileSize; 
            case Direction.left_back:
                return startPosition - Vector3.right * tileSize - Vector3.forward * tileSize; 
            case Direction.back:
                return startPosition - Vector3.forward * tileSize;
            case Direction.right_back:
                return startPosition + Vector3.right * tileSize - Vector3.forward * tileSize; 
        }
        return startPosition;
    } 
    
}
public enum Direction
{
    right,
    right_forward,
    forward,
    left_forward,
    left,
    left_back,
    back,
    right_back
}