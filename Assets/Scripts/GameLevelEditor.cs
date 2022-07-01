using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Editors/GameLevelEditor")]
[SerializeField]
public class GameLevelEditor : ScriptableObject
{
    [Header("WORD")]
    public List<WordData> wordData;
    [Header("LEVEL")]
    public List<LevelData> levelData;
    [Header("VEHICLE")]
    public List<Vehicle> vehicleData;
}

[System.Serializable]
public class WordData
{
    public string trackName;
    public int no;
    public int[] lettersToDisplay;
    public Vector3 cameraRot;
    public float cameraSize;
}

[System.Serializable]
public class LevelData
{
    public string levelCount;
    public int[] trackCount;
}

[System.Serializable]
public class Vehicle
{
    public string vehicleName;
    public GameObject vehicle;
    public int howLevel;
}
