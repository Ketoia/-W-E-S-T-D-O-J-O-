using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test_gpuInstancing : MonoBehaviour
{
    [SerializeField] private GameObject myGameObject;
    [SerializeField] private Mesh instanceMesh;
    [SerializeField] private Material instanceMaterial;
    [SerializeField] private int subMeshIndex = 0;

    public ComputeBuffer computeBuffer;

    // Start is called before the first frame update
    void Start()
    {
        myGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        instanceMesh = myGameObject.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        //Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial, )
    }
}
