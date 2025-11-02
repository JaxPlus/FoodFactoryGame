using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building")]
[System.Serializable]
public class BuildingData : ScriptableObject
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public string Description { get; private set; }
    // Trzeba będzie zrobić z tym porządek
    [field:SerializeField] public List<GameObject> Cost { get; private set; }
    [field:SerializeField] public BuildingModel Model { get; private set; }
}
