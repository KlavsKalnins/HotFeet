using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMeshGeneration : MonoBehaviour
{
    [SerializeField] private List<Vector2> vert2;
    [SerializeField] private MeshGeneration meshGen;
    [SerializeField] private Vector3 secondOffset;

    private void Start()
    {
        meshGen.MeshGen(vert2.ToArray(), transform.position);
        meshGen.MeshGen(vert2.ToArray(), transform.position + secondOffset);
    }

    void Update()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if (kb.spaceKey.wasPressedThisFrame)
        {
            meshGen.MeshGen(vert2.ToArray(), transform.position);
        }
    }
}
