/*
Adapted from: https://github.com/vazgriz/DungeonGenerator/blob/master/Assets/Scripts2D/Delaunay2D.cs

Copyright (c) 2019 - 2023 Ryan Vazquez

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT 
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DelaunayTriangulation : MonoBehaviour
{

    //This may need to be changed to work with a list of Vector3, y becomes z
    //Just thinking in two dimensions for now
    public List<Vector2> points; //Input points
    List<Triangle> triangulation; //List of triangles

    private void Start()
    {
        triangulation = BowyerWatson(points);
        foreach (Triangle triangle in triangulation)
        {
            Debug.DrawLine(triangle.e1.p1, triangle.e1.p2, Color.green, 100f);
            Debug.DrawLine(triangle.e2.p1, triangle.e2.p2, Color.green, 100f);
            Debug.DrawLine(triangle.e3.p1, triangle.e3.p2, Color.green, 100f);
            Debug.Log(triangle.e1);
        }
        foreach (Vector2 point in points)
        {
            Debug.DrawLine(new Vector2(0, 0), point, Color.red, 100f);
        }
    }

    List<Triangle> BowyerWatson(List<Vector2> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        //Generating the super triangle (contains all input points)

        //First we locate minimum and maximum x and y values
        float minX = points[0].x, minY = points[0].y;
        float maxX = float.MinValue, maxY = float.MinValue;
        foreach (Vector2 point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.x > maxX) maxX = point.x;
            if (point.y > maxY) maxY = point.y;
        }

        //Calculating differences between max and min
        float dx = maxX - minX;
        float dy = maxY - minY;
        //Calculating Max Delta; determine size of super triangle
        float deltaMax = Mathf.Max(dx, dy) * 2;
        //Calculating midpoint of bounding box around input points
        Vector2 mid = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);

        //Calculating supertriangle vertices
        Vector2 p1 = new Vector2(mid.x - 100 * dx, mid.y - dy);
        Vector2 p2 = new Vector2(mid.x, mid.y + 100 * deltaMax);
        Vector2 p3 = new Vector2(mid.x + 100 * dx, mid.y - dy);
        //p1 is to the left and below midpoint
        //p2 is above the midpoint
        //p3 is to the right and below midpoint
        //multiplying by factor of 20 just ensures triangle covers all points
        //accounts for potential fluctuations in point distribution, maybe unnecessary
        Debug.Log(p1);

        triangles.Add(new Triangle(p1, p2, p3));

        foreach (Vector2 point in points)
        {
            List<Edge> badEdges = new List<Edge>();
            foreach (Triangle triangle in triangles)
            {
                if (triangle.CircumcircleContainsVertex(point))
                {
                    badEdges.Add(triangle.e1);
                    badEdges.Add(triangle.e2);
                    badEdges.Add(triangle.e3);
                }
            }

            //remove duplicate bad edges from list
            badEdges = RemoveDuplicateEdges(badEdges);

            //remove triangles that share vertex with bad edge
            triangles.RemoveAll(triangle => badEdges.Contains(triangle.e1) ||
                                            badEdges.Contains(triangle.e2) ||
                                            badEdges.Contains(triangle.e3));

            //add new triangles by connecting point to vertices of bad edges
            foreach (Edge edge in badEdges)
            {
                triangles.Add(new Triangle(edge.p1, edge.p2, point));
            }
        }

        //remove triangles that contain any of the super triangle vertices
        triangles.RemoveAll(triangle => triangle.ContainsVertex(p1) ||
                                        triangle.ContainsVertex(p2) ||
                                        triangle.ContainsVertex(p3));

        return triangles;
    }

    //using a hashset here since it can't contain duplicates
    //should work if Equals and GetHashCode are properly implemented for Edge
    //class
    List<Edge> RemoveDuplicateEdges(List<Edge> edges)
    {
        HashSet<Edge> edgeSet = new HashSet<Edge>(edges);
        return new List<Edge>(edgeSet);
    }
}

public class Triangle
{
    public Vector2 v1, v2, v3, circumcenter;
    public Edge e1, e2, e3;
    public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        v1 = p1;
        v2 = p2;
        v3 = p3;
        e1 = new Edge(p1, p2);
        e2 = new Edge(p2, p3);
        e3 = new Edge(p3, p1);
        circumcenter = CalculateCircumcenter();
    }

    //return center of circumcircle
    private Vector2 CalculateCircumcenter()
    {
        float ab = v1.sqrMagnitude;
        float cd = v2.sqrMagnitude;
        float ef = v3.sqrMagnitude;
        float circumX = (ab * (v3.y - v2.y) + cd * (v1.y - v3.y)
            + ef * (v2.y - v1.y)) / (v1.x * (v3.y - v2.y) + v2.x *
            (v1.y - v3.y) + v3.x * (v2.y - v1.y)) / 2;
        float circumY = (ab * (v3.x - v2.x) + cd * (v1.x - v3.x)
            + ef * (v2.x - v1.x)) / (v1.y * (v3.x - v2.x) + v2.y *
            (v1.x - v3.x) + v3.y * (v2.x - v1.x)) / 2;
        return new Vector2(circumX / 2, circumY / 2);
    }

    public bool ContainsVertex(Vector2 point)
    {
        return Vector2.Distance(v1, point) < 0.01f || Vector2.Distance(v2, point) < 0.01f
            || Vector2.Distance(v3, point) < 0.01f;
    }

    //Check if the triangle's circumcircle contains a vertex.
    public bool CircumcircleContainsVertex(Vector2 point)
    {
        circumcenter = CalculateCircumcenter();
        float radiusSquared = Mathf.Pow(v1.x - circumcenter.x, 2)
            + Mathf.Pow(v1.y - circumcenter.y, 2);

        float distSquared = Mathf.Pow(point.x - circumcenter.x, 2)
            + Mathf.Pow(point.y - circumcenter.y, 2);

        return distSquared <= radiusSquared;
    }
}

public class Edge
{
    public Vector2 p1, p2;

    public Edge(Vector2 point1, Vector2 point2)
    {
        p1 = point1;
        p2 = point2;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()){
            return false;
        }
        Edge other = (Edge)obj;
        return (p1 == other.p1 && p2 == other.p2) || (p1 == other.p2 && p2 == other.p1);
    }

    public override int GetHashCode()
    {
        return p1.GetHashCode() ^ p2.GetHashCode();
    }
}
