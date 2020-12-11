using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateCubes : Singleton<EntitySpawner>
{
    public Material mat;
    public int howManyToSpawn;
    public Texture[] randomTex;
    private void Start()
    {
        for (int i = 0; i < howManyToSpawn; i++)
        {
           GameObject game= GameObject.CreatePrimitive(PrimitiveType.Quad);
            game.transform.position = new Vector3(UnityEngine.Random.Range(-5, 5f), UnityEngine.Random.Range(-5, 5f), UnityEngine.Random.Range(-5, 5f));
            Material temp = new Material(mat);
            temp.SetColor("_BaseColor", new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)));
            temp.SetTexture("_BaseMap", randomTex[UnityEngine.Random.Range(0,randomTex.Length)]);
            game.GetComponent<MeshRenderer>().material = temp;
            game.transform.localScale = new Vector3(8.4f, 5.6f, 1);
        }
    }
}