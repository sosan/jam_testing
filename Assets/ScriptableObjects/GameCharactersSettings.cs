using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(menuName = "ScriptableObjects/GameCharactersSettings", fileName = "GameCharactersSettings")]
public sealed class GameCharactersSettings : ScriptableObject
{


    //[SerializeField] public string namePlayer;
    //[SerializeField] public uint expCurrent;
    //[SerializeField] public uint expMax;
    //[SerializeField] public uint levelCurrent;
    //[SerializeField] public uint levelMax;

    [Header("[Health Characters]")]
    [SerializeField] public float healthMax;
    [SerializeField] public float[] health;


    [Header("[Energy Characters]")]
    [SerializeField] public float energyMax;
    [SerializeField] public float[] energy;


    [Header("[Defense Characters]")]
    [SerializeField] public float defenseMax;
    [SerializeField] public float[] defense;


    [Header("[Power Characters]")]
    [SerializeField] public float powerMax;
    [SerializeField] public float[] power;

    [Header("Nombre Characters")]
    [SerializeField] public string[] nameCharacters;

    [Header("[Initial Lifes Characters]")]
    [SerializeField] public ushort[] initialLifes;


    




    [Header("[Initial Lifes Characters]")]
    [SerializeField] public SpriteAtlas imageCharactersBig;
    [SerializeField] public SpriteAtlas imageCharactersSmall;





}
