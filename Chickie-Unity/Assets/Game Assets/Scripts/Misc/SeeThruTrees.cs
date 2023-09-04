using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SeeThruTrees : MonoBehaviour
{
    public Material seeThruBark;
    public Material ogBark;
    public Camera mainCamera;

    public List<MeshRenderer> currentTrees = new List<MeshRenderer>();

    [SerializeField] Transform pet;
    RaycastHit[] hitArr;

    public static SeeThruTrees Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (pet != null)
        {
            hitArr = Physics.RaycastAll(mainCamera.transform.position, pet.position - mainCamera.transform.position, Mathf.Infinity);
            foreach (var hit in hitArr)
            {
                if (hit.transform.tag == "Tree" && !currentTrees.Exists(x => x.transform.name == hit.transform.name))
                {
                    currentTrees.Add(hit.transform.GetComponent<MeshRenderer>());
                }

            }

            foreach (var tree in currentTrees.ToList())
            {
                if (!hitArr.ToList().Exists(x => x.transform.name == tree.transform.name))
                {
                    currentTrees.Remove(tree);
                }
            }
        }

        //if ()
        //{
        //    Debug.DrawRay(mainCamera.transform.position, pet.transform.position - mainCamera.transform.position, Color.red);
        //    if (hit.transform.gameObject.tag == "Tree")
        //    {
        //        currentTree = hit.transform.GetComponent<MeshRenderer>();
        //        Material[] materialArr = currentTree.materials;
        //        materialArr[1] = seeThruBark;
        //        currentTree.materials = materialArr;
        //    }

        //    else
        //    {
        //        if (currentTree != null)
        //        {
        //            Material[] materialArr = currentTree.materials;
        //            materialArr[1] = ogBark;
        //            currentTree.materials = materialArr;
        //            currentTree = null;
        //        }
        //    }
        //}

        //else
        //{
        //    currentTree = null;
        //}
    }

}

public static class Extensions
{
    public static T RemoveAndGet<T>(this IList<T> list, int index)
    {
        lock(list)
        {
            T value = list[index];
            list.RemoveAt(index);
            return value;
        }
    }
}
