using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomshape : MonoBehaviour
{
    // vertexs must be clockwise
    public List<Vector2> roomShapeVertex;
    public Dictionary<string,GameObject> objectDict;
    public List<Prior> priorList;
    // Start is called before the first frame update
    void Start()
    {
        Roomshape2Wall(roomShapeVertex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Roomshape2Wall(List<Vector2> roomShape)
    {
        Vector3 center = new Vector3();
        Vector3 wallShape = new Vector3();
        float wallThickness = 0.3F;
        float wallHeight = 1.5F;
        center.y = wallHeight / 2;
        for(int i = 0; i < roomShape.Count; i++)
        {
            Debug.Log(roomShape[i]);
            int nextIndex = (i + 1) % roomShape.Count;
            Debug.Log(nextIndex);
                // parallel to the axis x
                if(roomShape[i].y == roomShape[nextIndex].y)
                {
                    center.x = (roomShape[i].x + roomShape[nextIndex].x)/2;
                    // bottom wall of the room
                    if(roomShape[i].x > roomShape[nextIndex].x)
                    {
                        center.z = roomShape[i].y - wallThickness;
                    }
                    // up wall of the room
                    else
                    {
                        center.z = roomShape[i].y + wallThickness;
                    }
                    
                    wallShape.x = Math.Abs(roomShape[nextIndex].x - roomShape[i].y);
                    wallShape.y = wallThickness;
                    wallShape.z = wallHeight;
                    
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "Wall"+i.ToString();
                    cube.transform.position = center;
                    cube.transform.localScale = wallShape;
                }
                // parallel to the axis y
                if(roomShape[i].x == roomShape[nextIndex].x)
                {
                    center.z = (roomShape[i].y + roomShape[i+1].y)/2;
                    // left wall of the room
                    if(roomShape[i].y > roomShape[nextIndex].y)
                    {
                        center.x = roomShape[i].x - wallThickness;
                    }
                    // right wall of the room
                    else
                    {
                        center.x = roomShape[i].x + wallThickness;
                    }
                    
                    wallShape.z = Math.Abs(roomShape[nextIndex].y - roomShape[i].y);
                    wallShape.x = wallThickness;
                    wallShape.z = wallHeight;
                    
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "Wall"+i.ToString();
                    cube.transform.position = center;
                    cube.transform.localScale = wallShape;
                }
            
            
        }
    }
    // float Boltzmann-like(List<GameObjcet> objList)
    // {
    //     float
    // }
}

public class Prior
{
    public string MainObj{get; set;}
    public string SubObj{get; set;}
    public float dis{get;set;}
    public float theta{get;set;}
    
}