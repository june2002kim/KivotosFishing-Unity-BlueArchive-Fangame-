using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum fishrarity{
    R,
    SR,
    SSR
}

public enum fisharea{
    COMMON,
    LAKE,
    SEA,
    JUNGLE,
    ICE

}

[CreateAssetMenu(fileName = "Fish Data", menuName = "Scriptable Object/Fish Data", order = int.MaxValue)]

public class FishData : ScriptableObject
{
    [SerializeField] private string fishName;
    public string FishName {get {return fishName;}}

    [SerializeField] private fishrarity fishRarity;
    public fishrarity FishRarity {get {return fishRarity;}}

    [SerializeField] private fisharea fishArea;
    public fisharea FishArea {get {return fishArea;}}

    [SerializeField] private Sprite fishImage;
    public Sprite FishImage {get {return fishImage;}}

    [SerializeField] private Sprite fishProfile;
    public Sprite FishProfile {get {return fishProfile;}}

    [SerializeField] [TextArea(3, 5)]private string fishDescription;
    public string FishDescription {get {return fishDescription;}}
}
