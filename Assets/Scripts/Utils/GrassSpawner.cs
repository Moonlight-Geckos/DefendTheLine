using System;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [Serializable]
    class Area
    {
        public Vector3 leftBottomCorner;

        public Vector3 rightTopCorner;
    }

    [SerializeField]
    private Material grassMaterial;

    [SerializeField]
    private Mesh grassMesh;

    [SerializeField]
    private Area[] areasOfGrass;


    private List<Vector3> _positions;
    private ComputeBuffer _positionsBuffer;
    private ComputeBuffer _argsBuffer;
    private static readonly int PositionsBufferProperty = Shader.PropertyToID("_positionsBuffer");

    private void Awake()
    {
        SetupBuffers();

    }
    private void SetupBuffers()
    {

        _positions = new List<Vector3>();
        foreach (var area in areasOfGrass)
        {
            var rightTopCorner = area.rightTopCorner;
            var leftBottomCorner = area.leftBottomCorner;
            for (float i = leftBottomCorner.x; i <= rightTopCorner.x; i += 0.55f)
            {
                for (float j = leftBottomCorner.z; j <= rightTopCorner.z; j += 0.55f)
                {
                    _positions.Add(new Vector3(i, 0, j) + new Vector3(UnityEngine.Random.Range(0.5f, 0.7f), transform.position.y, UnityEngine.Random.Range(0.45f, 0.56f)));
                }
            }
        }

        _positionsBuffer = new ComputeBuffer(_positions.Count, sizeof(float) * 3);
        _positionsBuffer.SetData(_positions);

        uint[] args = {
            grassMesh.GetIndexCount(0),
            (uint) _positions.Count,
            grassMesh.GetIndexStart(0),
            grassMesh.GetBaseVertex(0),
            0
        };
        _argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        _argsBuffer.SetData(args);

        grassMaterial.SetBuffer(PositionsBufferProperty, _positionsBuffer);
    }
    private void Update()
    {
        Graphics.DrawMeshInstancedIndirect(grassMesh, 0, grassMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), _argsBuffer, castShadows: 0);
    }
    private void OnDestroy()
    {
        _argsBuffer.Release();
        _positionsBuffer.Release();
    }
}
