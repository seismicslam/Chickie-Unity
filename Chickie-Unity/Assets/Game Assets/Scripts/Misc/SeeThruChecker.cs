using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SeeThruChecker : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField] Transform pet;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {

        if(SeeThruTrees.Instance.currentTrees.Exists(x=>x.transform.name == transform.name) && pet.position.z > transform.position.z)
        {
            Material[] materialArr = meshRenderer.materials;
            materialArr[1] = SeeThruTrees.Instance.seeThruBark;
            meshRenderer.materials = materialArr;
        } else
        {
            Material[] materialArr = meshRenderer.materials;
            materialArr[1] = SeeThruTrees.Instance.ogBark;
            meshRenderer.materials = materialArr;
        }
    }

}
