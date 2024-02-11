using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Character Data", order = int.MaxValue)]

public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName;
    public string CharacterName {get {return CharacterName;}}

    [SerializeField] private string characterSchool;
    public string CharacterSchool {get {return characterSchool;}}

    [SerializeField] private Sprite[] profileImage;
    public Sprite[] ProfileImage {get {return profileImage;}}

    [SerializeField] private Sprite[] standingImage;
    public Sprite[] StandingImage {get {return standingImage;}}
}
