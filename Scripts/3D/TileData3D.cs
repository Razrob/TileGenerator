using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileData3D : MonoBehaviour
{
    [SerializeField] private int tileSize;

    Dictionary<string, Vector2Int[]> colorsData = new Dictionary<string, Vector2Int[]>();

    public string temp;

    void Awake()
    {
        CalculateColorData();
    }




    public Vector2Int[] GetColorDataOnSide(string sideName)
    {
       // Debug.Log(colorsData.ContainsKey(sideName));
        return colorsData[sideName];
    }

    public void CalculateColorData()
    {
        colorsData.Add("backSide", new Vector2Int[tileSize * tileSize]);
        colorsData.Add("forwardSide", new Vector2Int[tileSize * tileSize]);
        colorsData.Add("rightSide", new Vector2Int[tileSize * tileSize]);
        colorsData.Add("leftSide", new Vector2Int[tileSize * tileSize]);
        colorsData.Add("topSide", new Vector2Int[tileSize * tileSize]);
        colorsData.Add("bottomSide", new Vector2Int[tileSize * tileSize]);

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        //for back side
        int counter = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                Ray ray = new Ray(mesh.bounds.min + transform.position + new Vector3(0.05f, 0.05f, -0.05f) + new Vector3(x * 0.1f, y * 0.1f, 0), Vector3.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.1f))
                {
                    //Debug.DrawRay(mesh.bounds.min + transform.position + new Vector3(0.05f, 0.05f, -0.05f) + new Vector3(x * 0.1f, y * 0.1f, 0), Vector3.forward * 0.1f, Color.blue, 20);
                    colorsData["backSide"][counter] = new Vector2Int(Convert.ToInt32(hit.textureCoord.x * 256), Convert.ToInt32(hit.textureCoord.y * 256));
                    //  colorsData["backSide"][counter] = mesh.uv[mesh.triangles[hit.triangleIndex * 3]];
                }
                else
                {
                    colorsData["backSide"][counter] = new Vector2Int(999, 999);
                }
                counter++;
            }
        }
        //for forward side 
        counter = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                Ray ray = new Ray(mesh.bounds.min + transform.position + new Vector3(0, 0, tileSize * 0.1f) + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(x * 0.1f, y * 0.1f, 0), -Vector3.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.1f))
                {
                    //colorsData["forwardSide"][counter] = mesh.uv[mesh.triangles[hit.triangleIndex * 3]];
                    colorsData["forwardSide"][counter] = new Vector2Int(Convert.ToInt32(hit.textureCoord.x * 256), Convert.ToInt32(hit.textureCoord.y * 256));
                }
                else
                {
                    colorsData["forwardSide"][counter] = new Vector2Int(999, 999);
                }
                counter++;

            }
        }

        //for right side  
        counter = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                Ray ray = new Ray(mesh.bounds.min + transform.position + new Vector3(tileSize * 0.1f, 0, 0) + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(0, y * 0.1f, x * 0.1f), -Vector3.right);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.1f))
                {
                   // Debug.DrawRay(mesh.bounds.min + transform.position + new Vector3(tileSize * 0.1f, 0, 0) + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(0, y * 0.1f, x * 0.1f), -Vector3.right * 0.1f, Color.blue, 20);
                    //colorsData["rightSide"][counter] = mesh.uv[mesh.triangles[hit.triangleIndex * 3]];
                    colorsData["rightSide"][counter] = new Vector2Int(Convert.ToInt32(hit.textureCoord.x * 256), Convert.ToInt32(hit.textureCoord.y * 256));
                }
                else
                {
                    colorsData["rightSide"][counter] = new Vector2Int(999, 999);
                }
                counter++;
            }
        }

        //for left side  
        counter = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                Ray ray = new Ray(mesh.bounds.min + transform.position + new Vector3(-0.05f, 0.05f, 0.05f) + new Vector3(0, y * 0.1f, x * 0.1f), Vector3.right);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.1f))
                {
                    //colorsData["leftSide"][counter] = mesh.uv[mesh.triangles[hit.triangleIndex * 3]];
                    colorsData["leftSide"][counter] = new Vector2Int(Convert.ToInt32(hit.textureCoord.x * 256), Convert.ToInt32(hit.textureCoord.y * 256));
                }
                else
                {
                    colorsData["leftSide"][counter] = new Vector2Int(999, 999);
                }
                counter++;
            }
        }
        //for top side  
        counter = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int z = 0; z < tileSize; z++)
            {
                Ray ray = new Ray(mesh.bounds.min + transform.position + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(x * 0.1f, tileSize * 0.1f, z * 0.1f), Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.1f))
                {
                    //colorsData["leftSide"][counter] = mesh.uv[mesh.triangles[hit.triangleIndex * 3]];
                    colorsData["topSide"][counter] = new Vector2Int(Convert.ToInt32(hit.textureCoord.x * 256), Convert.ToInt32(hit.textureCoord.y * 256));
                }
                else
                {
                    colorsData["topSide"][counter] = new Vector2Int(999, 999);
                }
                // Debug.DrawRay(mesh.bounds.min + transform.position + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(x * 0.1f, tileSize * 0.1f, z * 0.1f), Vector3.down * 0.1f, Color.blue, 20);
                counter++;
            }
        }


        //for bottom side  
        counter = 0;
        for (int x = 0; x < tileSize; x++)
        {
            for (int z = 0; z < tileSize; z++)
            {
                Ray ray = new Ray(mesh.bounds.min + transform.position + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(x * 0.1f, -0.1f, z * 0.1f), Vector3.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.1f))
                {
                    //colorsData["leftSide"][counter] = mesh.uv[mesh.triangles[hit.triangleIndex * 3]];
                    colorsData["bottomSide"][counter] = new Vector2Int(Convert.ToInt32(hit.textureCoord.x * 256), Convert.ToInt32(hit.textureCoord.y * 256));
                }
                else
                {
                    colorsData["bottomSide"][counter] = new Vector2Int(999, 999);
                }
               // Debug.DrawRay(mesh.bounds.min + transform.position + new Vector3(0.05f, 0.05f, 0.05f) + new Vector3(x * 0.1f, -0.1f, z * 0.1f), Vector3.up * 0.1f, Color.blue, 20);
                counter++;
            }
        }




        if (string.IsNullOrEmpty(temp)) return;
        string temp1 = ""; 

        for (int i = 0; i < 64; i++)
        {
            temp1 += colorsData[temp][i];
        }
        Debug.Log(temp1);
    
    }
}
