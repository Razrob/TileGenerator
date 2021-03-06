using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TileGrid2D
{
    public static Dictionary<string, TileData2D> tilesStorage = new Dictionary<string, TileData2D>();





     public static void AddTile(Vector3 position, TileData2D tileData)
     {
         tilesStorage.Add(position.ToString(), tileData);
     }



     public static TileData2D GetTileData(Vector3 position)
     {
         if(tilesStorage.ContainsKey(position.ToString())) return tilesStorage[position.ToString()];
         return null;
     }

     private static Vector3 Round(this Vector3 vector, int number)
     {
         return new Vector3((float)Math.Round(vector.x, number), (float)Math.Round(vector.y, number), (float)Math.Round(vector.z, number));
     } 

     public static void Clear()
     {
         tilesStorage.Clear();
     }
}
