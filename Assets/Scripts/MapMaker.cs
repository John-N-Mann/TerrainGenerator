using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapMaker : MonoBehaviour
{
  
    private int xSize = 200;
    private int zSize = 200;
    public int waveStrength = 10;
    public float timeStrength = 1f;
    public float peakStrength = 1f;
    public float octave1_f = 0.01f;
//    private float octave1_a = 10f;
    public float octave1_x = 0f;
    public float octave1_z = 0f;
    public float octave2_f = 0.02f;
//    private float octave2_a = 10f;
    public float octave2_x = 10f;
    public float octave2_z = 0f;
    public float octave3_f = 0.01f;
//    private float octave3_a = 10f;
    public float octave3_x = 10f;
    public float octave3_z = 0f;

    public Texture2D AlphaMap;
    Mesh mesh;
    Vector3[] vertices;
    Vector3[] normals;

    int[] triangles;

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float octave1 = Mathf.PerlinNoise(x * octave1_f + octave1_x, z * octave1_f + octave1_z);
                float octave2 = Mathf.PerlinNoise(x * octave2_f + octave2_x + 100, z * octave2_f + octave2_z);
                float octave3 = Mathf.PerlinNoise(x * octave3_f + octave3_x + 200, z * octave3_f + octave3_z);
                Color color = AlphaMap.GetPixel(x, z);


                //vertices[i] = new Vector3(x, 0, z); // flat plane
                // vertices[i] = new Vector3(x, color.r * 100, z); // texture height map
                // vertices[i] = new Vector3(x, octave2 * waveStrength, z); // basic noise
                // vertices[i] = new Vector3(x, Mathf.Abs(Mathf.Sin(octave1 * 10)) * waveStrength, z); // sin math (wrinkly balls)
                //vertices[i] = new Vector3(x, Mathf.Abs(Mathf.Tan(octave1)) * waveStrength, z); // tan math (chaos)
                //vertices[i] = new Vector3(x, (octave1 * 10) - (octave2 * 20) + (octave3 * 10), z); // noise1 + noise2 + noise3


                // vertices[i] = new Vector3(x, Mathf.Pow(Mathf.Abs(octave3 - octave2 - octave1), peakStrength) * waveStrength, z); // difference filter
                vertices[i] = new Vector3(x, Mathf.Pow(((1 - Mathf.Abs(octave1 - 0.5f))), peakStrength) * waveStrength, z); // mountain peak filter

                // Vector3 layer1 = new Vector3(x, (1 - Mathf.Abs(octave1 - 0.5f)) * waveStrength, z);
                // Vector3 layer2 = new Vector3(x, Mathf.Abs(octave3 - octave2 - octave1) * waveStrength, z);
                // vertices[i] = layer1 + layer2;


                // if (octave1 > 0.5f) // overlay function
                // {
                //     vertices[i] = new Vector3(x, ((octave1) * (1 - (1 - 2 * (octave1 - 0.5f)) * (1 - octave2))) * waveStrength, z);
                // }
                // else
                // {
                //     vertices[i] = new Vector3(x, ((octave1) * ((2 * octave1) * octave2)) * waveStrength, z);
                // }

                i++; //iterate up list

            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

    }

    void UpdateMesh()
    {

        mesh.Clear(); // start over
        mesh.vertices = vertices; // set vertices
        mesh.triangles = triangles; // set triangles

        normals = mesh.normals;
        
        // for (var i = 0; i < normals.Length; i++) // test function to add/subtract vertices based on normals Y angle
        // {
        //     if(Mathf.Abs(normals[i].y) <= 0.1f)
        //     {
        //         vertices[i].y *= (Random.value) * timeStrength;
        //     }else
        //     {
        //         vertices[i].y *= -(Random.value) * timeStrength;
        //     }
        // }
        // mesh.vertices = vertices;


        mesh.RecalculateNormals(); // recalculate lighting normals (so lighting works on new mesh)
    }

    void Start()
    {
        mesh = new Mesh(); // create the mesh
        GetComponent<MeshFilter>().mesh = mesh; // grab mesh filter
        CreateShape(); // create vertices and triangles
        UpdateMesh(); // draw information to mesh
    }

    void Update()
    {
        CreateShape();
        UpdateMesh();
        octave1_x += 0.01f * timeStrength;
        octave2_x += 0.01f * timeStrength;
        octave3_x += 0.01f * timeStrength;
    }
}
