using System;
using System.Collections.Generic;
using UnityEngine;

public class logMovement : MonoBehaviour
{
    
    
    public List<GameObject> controlPoints;
    public GameObject log;
    
    public int resolution = 100;
    public float timeForMovement = 10f;
    public float rotationSpeed = 10f;

    private void Update()
    {
        log.transform.LookAt(GetBezierPoint((Time.time / timeForMovement) % 1, controlPoints.ConvertAll(p => p.transform.position)));
        log.transform.position = GetBezierPoint((Time.time  / timeForMovement)%1, controlPoints.ConvertAll(p => p.transform.position));
    }

    void OnDrawGizmos()
    {
        if (controlPoints.Count < 2) return;
        List<Vector3> points = new List<Vector3>();
        foreach (GameObject obj in controlPoints)
        {
            points.Add(obj.transform.position);
        }

        // Dessiner la courbe de Bézier
        Gizmos.color = Color.red;
        for (int i = 0; i < resolution; i++)
        {
            float t1 = i / (float)resolution;
            float t2 = (i + 1) / (float)resolution;

            Vector3 point1 = GetBezierPoint(t1, points);
            Vector3 point2 = GetBezierPoint(t2, points);

            Gizmos.DrawLine(point1, point2);
        }
        // dessiner une droite entre les points de 
        
        Gizmos.color = Color.blue;
        
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(controlPoints[i].transform.position, controlPoints[i + 1].transform.position);
        }
    }

    
    Vector3 GetBezierPoint(float t, List<Vector3> points)
    {
        int n = points.Count - 1;
        Vector3 point = Vector3.zero;
        
        for (int i = 0; i <= n; i++)
        {
            float coefficient = BinomialCoefficient(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
            point += coefficient * points[i];
        }

        return point;
    }
    
    int BinomialCoefficient(int n, int k)
    {
        if (k == 0 || k == n)
            return 1;
        return BinomialCoefficient(n - 1, k - 1) + BinomialCoefficient(n - 1, k);
    }
}
    
    


    
