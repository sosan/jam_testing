using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixCharacters : MonoBehaviour
{

    public bool taken = false;

    public MatrixCharacters up = null;
    public MatrixCharacters down = null;
    public MatrixCharacters left = null;
    public MatrixCharacters right = null;

    [Header("Auto Settings")]

    [Header("Health Character")]
    [SerializeField] public float health = 0;
    [SerializeField] public float healthMax = 0;

    [Header("[Energy Character]")]
    [SerializeField] public float energyMax = 0;
    [SerializeField] public float energy = 0;
   

    [Header("[Defense Characters]")]
    [SerializeField] public float defenseMax = 0;
    [SerializeField] public float defense = 0;


    [Header("[Power Characters]")]
    [SerializeField] public float powerMax = 0;
    [SerializeField] public float power = 0;

    [Header("Nombre Characters")]
    [SerializeField] public string nameCharacter;



    [Header("[Initial Lifes Character]")]
    [SerializeField] public ushort initialLifes = 3;

    [Header("Sprite")]
    [SerializeField] public Sprite imageCharacter = null;

    private void Start()
    {

       

    }




}
