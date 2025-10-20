using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform wrapper;
    [SerializeField] public bool rotatable = false;
    public float Rotation => wrapper.transform.eulerAngles.y;
    private BuildingShapeUnit[] shapeUnits;

    private void Awake()
    {
        shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float rotationStep)
    {
        wrapper.Rotate(new(0, rotationStep));
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}
