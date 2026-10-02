using System;
using UnityEngine;

//force the gameobject to always have a component
//if it doesn't, unity attatches one
[RequireComponent (typeof(SkinnedMeshRenderer))]
public class SoftBody : MonoBehaviour
{
    [Range(0f, 2f)]
    public float softness = 1f; //how far verticies can move. Higher = more floppy
    //how much motion slows down (like friction)
    [Range(0.01f, 1f)]
    public float damping = 0.1f;
    //how resistant it is to bending
    public float stiffness = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateSoftBodyPhysics();
    }

    void CreateSoftBodyPhysics()
    {
        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;

        //add unity cloth physics component to object at runtime (when u click play)
        Cloth cloth = gameObject.AddComponent<Cloth>();
        cloth.damping = damping;
        cloth.bendingStiffness = stiffness;

        //every vertex in the mesh gets a physics rule
        //we generate the rules with our function
        cloth.coefficients = GenerateClothCoefficients(smr.sharedMesh.vertices.Length);
    }

    //we are making an array so we have multiple coefficient for all the verticies
    //EX: mesh might have 500 verticies, so cloth needs 500 coefficients (one per vertex)
    //so that's why we are returning an array
    private ClothSkinningCoefficient[] GenerateClothCoefficients(int vertexCount)
    {
        //[] creates an array, one entry per vertex
        //make a list with vertexcount slots
        ClothSkinningCoefficient[] coefficients = new ClothSkinningCoefficient[vertexCount];

        //loop through every vertex, set rules for each vertex one by one
        for(int i = 0; i < vertexCount; i++)
        {
            //how far that vertex can move
            coefficients[i].maxDistance = softness;
            //collision buffer 0 = tight
            coefficients[i].collisionSphereDistance = 0f;
            //so basically, every vertex can move up to softness distance
        }

        return coefficients; //send it back to the cloth component
    }
}
