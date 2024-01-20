using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCMC : MonoBehaviour
{
    public List<GameObject> objList;
    private List<GameObject> preObjList;
    private float cost;

    public float roomLenMax;
    public float roomLenMin;
    public int num_of_iterations = 1000;


    // Start is called before the first frame update
    void Start()
    {
        preObjList = objList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SimulatedAnnealing()
    {
        
        int t;
        for(int itera = 0;  itera < num_of_iterations; itera ++)
        {
            if(itera < 400)
            {
                t = 1000;
            }
            else if( 400 < itera & itera < 800)
            {
                t = 100;
            }
            else
            {
                t = 10;
            }
        }
        
    }

    void moveOrRoatate(float t)
    {
        int len = objList.Count;
        int i = UnityEngine.Random.Range(0, len);
        int j = UnityEngine.Random.Range(0, 1);
        float dis = UnityEngine.Random.Range(roomLenMin, roomLenMax);
        float angle = UnityEngine.Random.Range(0, 360);
        if (j == 0)
        {
            MoveT(dis, preObjList[i]);
        }
        else
        {
            MoveR(angle, preObjList[i]);
        }
        float newCost = Cost(preObjList);
        if (newCost > cost)
        {
            objList = preObjList;
        }
        else
        {
            float prob = UnityEngine.Random.Range(0, 1);
            float mh = Mathf.Exp(t*(newCost - cost));
            if (prob > mh)
                objList = preObjList;
        }
    }

    float Cost(List<GameObject> objList)
    {
        return 1.0F;
    }

    void MoveT(float dis, GameObject g)
    {
        int m = UnityEngine.Random.Range(0, 2);
        if(m == 0)
        {
            g.transform.Translate(dis, 0, 0);
        }
        else
        {
            g.transform.Translate(0, dis, 0);
        }
    }

    void MoveR(float angle, GameObject g)
    {
        Vector3 v = new Vector3(0f, angle, 0f);
        g.transform.Rotate(v, Space.Self);
    }
}
