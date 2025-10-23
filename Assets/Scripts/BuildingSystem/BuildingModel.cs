using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform wrapper;
    [SerializeField] public bool isRotatable = false;
    public float Rotation => wrapper.transform.eulerAngles.z;
    private BuildingShapeUnit[] shapeUnits;

    private void Awake()
    {
        shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float rotationStep)
    {
        if (!isRotatable) return;

        wrapper.Rotate(new Vector3(0, 0, rotationStep));
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}
