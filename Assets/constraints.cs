using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class Constraints : MonoBehaviour
{
    const float OFFSET = 0.2f;
    const int MAX_VISIBLE_AREA = 3;
    const float MIN_VISIBLE_AREA = 0.5f;
    const int VIEWING_FRUSTUM_PILE = 3;
    const float VIEWING_FRUSTUM_LINE_SPACE = 0.3f;
    const float VIEWING_FRUSTUM_INVREMENT = 0.2f;

    public struct Relationships {
        public Vector3[] points; // vertices of bounding box
        public float b; // length of diagonal of the bounding box 
        public Vector2[] a; // centres of accessible space rectangles
        public Vector2[] s; // vertices of accessible space rectangles
        public float[] ad; // lengths of diagonals of accessible space rectangles
        public Vector2[] v; // centres of frustum rectangles
        public Vector2[] w; // vertices of frustum rectangles
        public float[] vd; // lengths of diagonals of frustum rectangles   

        public Relationships(float bValue) {
            points = new Vector3[8];
            b = bValue;
            a = new Vector2[4];
            s = new Vector2[4];
            ad = new float[4];
            v = new Vector2[VIEWING_FRUSTUM_PILE];
            w = new Vector2[VIEWING_FRUSTUM_PILE];
            vd = new float[VIEWING_FRUSTUM_PILE];
        } 
    }

    public struct WallRelation {
        public float NearestDistance;
        public float NearestOrient0;
    }

    public struct WindowRelation {
        public Vector3 RelativeTrans;
        public float Orient0;
    }

    public struct PairwiseRelation {
        public GameObject AttachedObj;
        public Vector3 RelativeTrans;
        public float RelativeRotation;
    }

    private GameObject[] allObjects;
    Dictionary<GameObject, Relationships> objectRelationships = new Dictionary<GameObject, Relationships>();
    int n; // total no. of game objects
    Dictionary<GameObject, WallRelation> allObjWallRelation = new Dictionary<GameObject, WallRelation>();
    Dictionary<GameObject, WindowRelation> allObjWinRelation = new Dictionary<GameObject, WindowRelation>();
    Dictionary<GameObject, PairwiseRelation[]> allObjPairwise = new Dictionary<GameObject, PairwiseRelation[]>();

    float visible(GameObject obj1, GameObject obj2) {
        float total = 0;
        Relationships r1 = objectRelationships[obj1]; 
        Relationships r2 = objectRelationships[obj2]; 
        for (int i = 0; i < VIEWING_FRUSTUM_PILE; ++i) {
            float dis = Mathf.Sqrt(Mathf.Pow(r1.v[i].x - obj1.transform.position.x, 2) + Mathf.Pow(r1.v[i].y - obj1.transform.position.y, 2));
            float f = Mathf.Max(1 - dis / (r1.b + r2.vd[i]), 0);
            total += f;
        }
        return total;
    }

    float visibility() {
        float sum = 0.0f;
        for (int obj1 = 0; obj1 < n; ++obj1) {
            Renderer renderer1 = allObjects[obj1].GetComponent<Renderer>();
            if (renderer1 == null) continue;
            Bounds bound1 = renderer1.bounds;
            for (int obj2 = obj1 + 1; obj2 < n; ++obj2) {
                Renderer renderer2 = allObjects[obj2].GetComponent<Renderer>();
                if (renderer2 == null) continue;
                Bounds bound2 = renderer2.bounds;
                if (bound1.Intersects(bound2)) {
                    Debug.Log("Collision detected between " + allObjects[obj1] + " and " + allObjects[obj2]);
                }
                sum += visible(allObjects[obj1], allObjects[obj2]);
            }     
        } 
        return sum;
    }

    float access(GameObject obj1, GameObject obj2) {
        float total = 0.0f;
        Relationships r1 = objectRelationships[obj1]; 
        Relationships r2 = objectRelationships[obj2]; 
        for (int i = 0; i < 4; ++i) {
            float dis = Mathf.Sqrt(Mathf.Pow(r1.a[i].x - obj1.transform.position.x, 2) + Mathf.Pow(r1.a[i].y - obj1.transform.position.y, 2));
            float f = Mathf.Max(1 - dis / (r1.b + r2.ad[i]), 0);
        }
        return total;
    }

    float accessibility() {
        float sum = 0.0f;
        for (int obj1 = 0; obj1 < n; ++obj1) {
            Renderer renderer1 = allObjects[obj1].GetComponent<Renderer>();
            if (renderer1 == null) continue;
            for (int obj2 = obj1 + 1; obj2 < n; ++obj2) {
                Renderer renderer2 = allObjects[obj2].GetComponent<Renderer>();
                if (renderer2 == null) continue;
                sum += access(allObjects[obj1], allObjects[obj2]);
            } 
        } 
        return sum;
    }

    float prior() {
        float[] areaOrient = {-0.5f * Mathf.PI, 0.0f, 0.5f * Mathf.PI, - Mathf.PI};
        float sum = 0.0f;
        GameObject[] windowList = GameObject.FindGameObjectsWithTag("Window"); // get all window GameObjects
        for (int obj = 0; obj < n; ++obj) {
            Renderer renderer = allObjects[obj].GetComponent<Renderer>();
            if (renderer == null) continue;
            // areaDis = []
            // areaDis.append(abs(obj['translate'][0] - room['roomShapeBBox']['min'][0]))
            // areaDis.append(abs(obj['translate'][2] - room['roomShapeBBox']['max'][1]))
            // areaDis.append(abs(obj['translate'][0] - room['roomShapeBBox']['max'][0]))
            // areaDis.append(abs(obj['translate'][2] - room['roomShapeBBox']['min'][1]))
            float minDis2Wall = 0; // TODO: minDis2Wall = min(areaDis)
            // i = areaDis.index(minDis2Wall)
            float orient2Wall = allObjects[obj].transform.rotation.y - areaOrient[0]; // TODO: areaOrient[i]
            if (allObjWallRelation.TryGetValue(allObjects[obj], out WallRelation rWall))
            {
                float deltaMinDis = Mathf.Abs(minDis2Wall - rWall.NearestDistance);
                float deltaOrient = Mathf.Abs(orient2Wall - rWall.NearestOrient0);
                sum += deltaMinDis + deltaOrient;
            }
            else
            {
                Debug.Log($"Key '{allObjects[obj]}' not found in the wall dictionary.");
            }
            if (allObjWinRelation.TryGetValue(allObjects[obj], out WindowRelation rWindow))
            {
                float[] disList = new float[windowList.Length];
                float minDis = Single.MaxValue;
                int minIndex = -1;

                for (int i = 0; i < windowList.Length; ++i) {
                    disList[i] = Mathf.Sqrt(Mathf.Pow(allObjects[obj].transform.position.x - windowList[i].transform.position.x, 2) + 
                                            Mathf.Pow(allObjects[obj].transform.position.z - windowList[i].transform.position.z, 2));
                    if (minDis < disList[i]) {
                        minDis = disList[i];
                        minIndex = i;
                    }
                }  
                GameObject nearestWin = windowList[minIndex];
                Vector3 deltaTrans = nearestWin.transform.position - allObjects[obj].transform.position - rWindow.RelativeTrans; 
                float deltaTransSum = Mathf.Abs(deltaTrans.x) + Mathf.Abs(deltaTrans.y) + Mathf.Abs(deltaTrans.z);
                float deltaRot = Mathf.Abs(nearestWin.transform.rotation.y - allObjects[obj].transform.rotation.y - rWindow.Orient0);
                sum += deltaTransSum + deltaRot;
            }
            else
            {
                Debug.Log($"Key '{allObjects[obj]}' not found in the window dictionary.");
            }
        }
        return sum;
    }

    float pairwise() {
        float sum = 0.0f;
         for (int obj = 0; obj < n; ++obj) {
            Renderer renderer = allObjects[obj].GetComponent<Renderer>();
            if (renderer == null) continue;
            if (allObjPairwise.TryGetValue(allObjects[obj], out PairwiseRelation[] rPairList)) {
                foreach (PairwiseRelation rPair in rPairList) {
                    Vector3 deltaTrans = rPair.AttachedObj.transform.position - allObjects[obj].transform.position - rPair.RelativeTrans;
                    float deltaTransSum = Mathf.Abs(deltaTrans.x) + Mathf.Abs(deltaTrans.y) + Mathf.Abs(deltaTrans.z);
                    float deltaRot = Mathf.Abs(rPair.AttachedObj.transform.rotation.y - allObjects[obj].transform.rotation.y - rPair.RelativeRotation);
                    sum += deltaTransSum + deltaRot;
                }
            }
        }
        return sum;
    }

    float costFunction() {
        float total = 0.0f;
        float v = visibility();
        float a = accessibility();
        float p = prior();
        float pw = pairwise();
        total = 0.01f * v + 0.01f * a + p + pw;
        return total;
    }

    void readSpatialRelationship() {
        string[] lines = File.ReadAllLines("input.txt");
        foreach (string line in lines) {
            string[] parts = line.Split(';');
            if (parts[3] == "story wall") {
                WallRelation w = new WallRelation();
                string wallStr = parts[2];
                string[] tokens = wallStr
                .Split(new[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(token => token.Trim())
                .ToArray();
                if (float.TryParse(tokens[0], out float nd)){
                    w.NearestDistance = nd;
                } else {
                    Debug.Log($"Key '{parts[0]}' nearest distance value error.");
                }
                if (float.TryParse(tokens[2], out float no)){
                    w.NearestOrient0 = no;
                } else {
                    Debug.Log($"Key '{parts[0]}' nearest orient value error.");
                }
                GameObject gameObject = GameObject.Find(parts[0]);
                allObjWallRelation[gameObject] = w;
            } else if (parts[3] == "story pairwise") {
                PairwiseRelation p = new PairwiseRelation();
                string gTrans = parts[2];
                string[] tokens = gTrans
                .Split(new[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(token => token.Trim())
                .ToArray();
                GameObject pairObj = GameObject.Find(tokens[0]);
                if (float.TryParse(tokens[1], out float posX)){
                    p.RelativeTrans.x = posX;
                } else {
                    Debug.Log($"Key '{parts[0]}' translation x value error.");
                }
                if (float.TryParse(tokens[2], out float posY)){
                    p.RelativeTrans.y = posY;
                } else {
                    Debug.Log($"Key '{parts[0]}' translation y value error.");
                }
                if (float.TryParse(tokens[3], out float posZ)){
                    p.RelativeTrans.z = posZ;
                } else {
                    Debug.Log($"Key '{parts[0]}' translation z value error.");
                }
                if (float.TryParse(tokens[4], out float oriY)){
                    p.RelativeRotation = oriY;
                } else {
                    Debug.Log($"Key '{parts[0]}' orientation y value error.");
                }
            } else if (parts[3] == "story wall wind") {
                
            }
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello world");

        allObjects = GameObject.FindObjectsOfType<GameObject>();
        n = allObjects.Length;

        readSpatialRelationship();

        // calculate relationships for each object
        foreach (var obj in allObjects) {
            // check if object is renderer
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null) continue;
            Bounds originalBounds = renderer.bounds;
            Vector3 center = originalBounds.center;
            Vector3 ext = originalBounds.extents;
            Vector2 e = new Vector2(ext.x, ext.z);
            float deltaX = Mathf.Abs(ext.x);
            float deltaY = Mathf.Abs(ext.y);
            float deltaZ = Mathf.Abs(ext.z);
            // length of diagonal of bounding box 
            Relationships r = new Relationships(e.magnitude);
            // vertices of bounding box
            r.points[0] = center + new Vector3(-deltaX, deltaY, -deltaZ);        // up front left (relative to centre)
            r.points[1] = center + new Vector3(deltaX, deltaY, -deltaZ);         // up front right
            r.points[2] = center + new Vector3(deltaX, deltaY, deltaZ);          // up back right
            r.points[3] = center + new Vector3(-deltaX, deltaY, deltaZ);         // up back left
            r.points[4] = center + new Vector3(-deltaX, -deltaY, -deltaZ);       // down front left
            r.points[5] = center + new Vector3(deltaX, -deltaY, -deltaZ);        // down front right
            r.points[6] = center + new Vector3(deltaX, -deltaY, deltaZ);         // down back right
            r.points[7] = center + new Vector3(-deltaX, -deltaY, deltaZ);        // down back left
            // centres of accessible space rectangles
            Bounds expandedBounds = originalBounds;
            expandedBounds.Expand(OFFSET);
            float x = Mathf.Abs(expandedBounds.max.x - center.x) / 2.0f;
            float z = Mathf.Abs(expandedBounds.max.z - center.z) / 2.0f;
            r.a[0] = new Vector2(center.x, center.z + z);
            r.a[1] = new Vector2(center.x - x, center.z);
            r.a[2] = new Vector2(center.x, center.z - z);
            r.a[3] = new Vector2(center.x + x, center.z - z);
            // vertices of accessible space rectangles
            r.s[0] = new Vector2(originalBounds.min.x, expandedBounds.max.y);
            r.s[1] = new Vector2(originalBounds.min.x, expandedBounds.min.y);
            r.s[2] = new Vector2(originalBounds.max.x, expandedBounds.min.y);
            r.s[3] = new Vector2(originalBounds.max.x, expandedBounds.max.y);
            // lengths of diagonals of accessible space rectangles
            r.ad[0] = Mathf.Sqrt(Mathf.Pow((r.a[0].x - r.s[0].x), 2) + Mathf.Pow((r.a[0].y - r.s[0].y), 2));
            r.ad[1] = Mathf.Sqrt(Mathf.Pow((r.a[1].x - r.s[1].x), 2) + Mathf.Pow((r.a[1].y - r.s[1].y), 2));
            r.ad[2] = Mathf.Sqrt(Mathf.Pow((r.a[2].x - r.s[2].x), 2) + Mathf.Pow((r.a[2].y - r.s[2].y), 2));
            r.ad[3] = Mathf.Sqrt(Mathf.Pow((r.a[3].x - r.s[3].x), 2) + Mathf.Pow((r.a[3].y - r.s[3].y), 2));
            // centres of frustum rectangles => v
            // vertices of frustum rectangles => w
            // lengths of diagonals of frustum rectangles => vd
            float width  = r.a[3].x - r.a[1].x;
            for (int i = 0; i < VIEWING_FRUSTUM_PILE; ++i) {
                float vx = r.a[2].x;
                float vy = r.a[2].y + i * VIEWING_FRUSTUM_LINE_SPACE;
                r.v[i] = new Vector2(vx, vy);

                float wx = vx + (width / 2  + i * VIEWING_FRUSTUM_INVREMENT);
                float wy = vy + VIEWING_FRUSTUM_LINE_SPACE / 2;
                r.w[i] = new Vector2(wx, wy);

                r.vd[i] = Mathf.Sqrt(Mathf.Pow(vx - wx, 2) + Mathf.Pow(vx - vy, 2));
            }

            objectRelationships[obj] = r;
        }
        float cost = costFunction();
        Debug.Log(cost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
