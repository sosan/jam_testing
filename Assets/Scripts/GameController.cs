using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using InControlActions;


public class InfoPlayer
{
    public GameObject focusPlayer;
    public GameObject playerGameObject;
    public GameObject playerPos;
    public Color colorPlayer;
    public nombreColores nombreColorPlayer;
    public ushort posX;
    public ushort posY;

    public float lastPosX = 0;
    public float lastPosY = 0;
    public float currentPosX = 0;
    public float currentPosY = 0;

    public ushort posicionPlayer;
    public bool isFirstMove;
    public bool listo;
    public Image bigSelectionPlayer;
    public bool selected;
    public string namePlayer;

    public int expCurrent;
    public int expMax;
    public int levelCurrent;
    public int levelMax;

    public bool vacio = true;


    public int indexShotPrefab;

    public float fireCooldown = 0;
    public float speedMovement = 0;
    public float powerDamage = 0;
    public float durationShotSeconds = 0;
    public float bombCooldown = 0;
    public float defense = 0;
    public float defenseMax = 0;

    public short numerobloques = 0;
    public int deviceId = -1;

    public bool bot;



    public InfoPlayer() { }

    public InfoPlayer(GameObject focusPlayer, GameObject playerGameObject, GameObject playerPos, Color colorPlayer, ushort posX, ushort posY,
        ushort posicionPlayer, bool isFirstMove, Image bigSelectionPlayer, bool selected, bool listo, bool bot, nombreColores coloresplayer, int deviceId)
    {
        this.focusPlayer = focusPlayer;
        this.playerGameObject = playerGameObject;
        this.playerPos = playerPos;
        this.colorPlayer = colorPlayer;
        this.posX = posX;
        this.posY = posY;
        this.posicionPlayer = posicionPlayer;
        this.isFirstMove = isFirstMove;
        this.bigSelectionPlayer = bigSelectionPlayer;
        this.listo = listo;
        this.bot = bot;
        this.nombreColorPlayer = coloresplayer;
        this.deviceId = deviceId;
    }


}

public enum nombreColores
{ 
    amarillo,
    azul,
    rojo,
    blanco,
    None,
    fondo,
    hueco


}


public class GameController : MonoBehaviour
{

    [Header("canvas")]
    [SerializeField] public GameObject[] canvasMenu = null;

    [Header("Animation Events")]
    [SerializeField] public Animation animaciones = null;
    [SerializeField] public AnimationEvents eventos = null;



     // fundamentales
    [SerializeField] public readonly ushort JUGADORES_MAXIMO = 4;
    public InfoPlayer[] jugadores = new InfoPlayer[4];
    [HideInInspector] public ushort contadorJugadores = 0;
    [HideInInspector] public Dictionary<int, int> dictPlayers = new Dictionary<int, int>();


    [SerializeField] public Transform[] initialPlayerPositions = null;
    public ReactiveProperty<bool> faseConcluida = new ReactiveProperty<bool>(false);


    [HideInInspector] public ReactiveProperty<bool> isActiveFase = new ReactiveProperty<bool>(false);

    [SerializeField] private TextMeshProUGUI tiempoBatalla = null;

    private IDisposable crono = null;
    private IDisposable spawnerpowerups = null;

    
    [SerializeField] private ushort TIEMPOMAXBATALLA = 180;
    private short tiempoCurrentBatalla = 180;

    [SerializeField] public GameObject prefabBomba = null;
    [SerializeField] public GameObject prefabBullet = null;
    [SerializeField] public GameObject prefabPowerup = null;
    [SerializeField] public GameObject prefabDestroyer = null;

    [SerializeField] public Transform[] positionsPowerups = null;

    [SerializeField] public Image[] barrasPuntuaje = null;


    private bool isMutedDefault = false;
    private ushort mainVolumenDefault = 100;
    private ushort mainSoundDefault = 30;
    private ushort mainSfxDefault = 60;

    private bool isMutedInternal = false;
    private ushort mainVolumenInternal = 100;
    private ushort mainSoundInternal = 30;
    private ushort mainSfxInternal = 60;

    public bool isDebug = true;
    public float speed = 0.8f;
    
    private short minimoBloques = 10;
    [SerializeField] public short bloquesAmarillos = 0;
    [SerializeField] public short bloquesAzules = 0;
    [SerializeField] public short bloquesRojos = 0;
    [SerializeField] public short bloquesBlancos = 0;

    [SerializeField] public TextMeshProUGUI ready = null;

    [HideInInspector] public bool playerSePuedenMover = false;
    [HideInInspector] public string texto_fase2 = "go";
    private ushort NUM_BLOQUES = 145;
    
    //[SerializeField] public nombreColores nombreColoresPlayers;




    private void Awake()
    {


        // inicizalicion de jugadores
        for(ushort i = 0; i < jugadores.Length; i++)
        { 
        
            jugadores[i] = new InfoPlayer();
            barrasPuntuaje[i].fillAmount = 0.1f;
        
        }


        tiempoBatalla.text = Localization.Get("tiempo") + "00:00";

    }

    void Start()
    {
        GettingPlayerPrefsValues();

    }



    private void GettingPlayerPrefsValues()
    {

        if (PlayerPrefs.HasKey("isMutedInternal") == true)
        {

            int t = PlayerPrefs.GetInt("isMutedInternal");


            if (t == 0)
            {

                isMutedInternal = false;
            }
            else if (t == 1)
            {

                isMutedInternal = true;

            }



        }
        else
        {

            if (isMutedDefault == true)
            {

                PlayerPrefs.SetInt("isMutedInternal", 1);

            }
            else
            {

                PlayerPrefs.SetInt("isMutedInternal", 0);

            }


        }

        if (PlayerPrefs.HasKey("mainVolumenInternal") == true)
        {

            mainVolumenInternal = (ushort)PlayerPrefs.GetInt("mainVolumenInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainVolumenInternal", mainVolumenDefault);
        }


        if (PlayerPrefs.HasKey("mainSoundInternal") == true)
        {

            mainSoundInternal = (ushort)PlayerPrefs.GetInt("mainSoundInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainSoundInternal", mainSoundDefault);
        }



        if (PlayerPrefs.HasKey("mainSfxInternal") == true)
        {

            mainSfxInternal = (ushort)PlayerPrefs.GetInt("mainSfxInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainSfxInternal", mainSfxDefault);
        }



    }



    public void InitGame()
    {

      //  MusicController.MusicInstance.SettingsMixer(
      //    isMutedInternal,
      //    mainVolumenInternal,
      //    mainSoundInternal,
      //    mainSfxInternal
      //);
        animaciones.Play("ready");
        
        SpawnerPowerups(40);






    }


    private void ActualizarPuntuacion()
    {
#if UNITY_EDITOR
        //print("bloques amarillos=" + bloquesAmarillos);
#endif
        if (bloquesAmarillos > minimoBloques)
        { 
            barrasPuntuaje[0].fillAmount = (float)bloquesAmarillos / NUM_BLOQUES;
            //print(barrasPuntuaje[0].fillAmount);
        }
        
        if (bloquesAzules > minimoBloques)
        { 
            barrasPuntuaje[1].fillAmount = (float)bloquesAzules / NUM_BLOQUES;
        }

        if (bloquesRojos > minimoBloques)
        { 
            barrasPuntuaje[2].fillAmount = (float)bloquesRojos / NUM_BLOQUES;
        }

        if (bloquesBlancos > minimoBloques)
        { 
            barrasPuntuaje[3].fillAmount = (float)bloquesBlancos / NUM_BLOQUES;
        }

    }


    public void IniciarCrono()
    {

        playerSePuedenMover = true;
        faseConcluida.Value = false;
        long binlocal = new DateTime(year:2019, month:1, day:1, hour:9, minute:0, second:0).ToBinary();

        crono = Observable.Timer(
        TimeSpan.FromSeconds(0), //esperamos 1 segundos 
        TimeSpan.FromSeconds(1), Scheduler.MainThread).Do(x => { }).
        ObserveOnMainThread().Take(TIEMPOMAXBATALLA)
        .Subscribe
        (_ =>
        {
            tiempoBatalla.text = Localization.Get("tiempo") + DateTime.FromBinary(binlocal).AddSeconds(tiempoCurrentBatalla).ToString("mm:ss");
            //print(tiempoCurrentBatalla);
            tiempoCurrentBatalla--;

            ActualizarPuntuacion();

        }
        , ex => { Debug.Log(" cuentaatrasantes OnError:" + ex.Message); if (crono != null) crono.Dispose(); },
        () => //completado
        {

            crono.Dispose();

            faseConcluida.Value = true;

            tiempoBatalla.text = "";
            tiempoCurrentBatalla = 0;


        }).AddTo(this.gameObject);




    }


    public async void HacerVibrarMando(int deviceId)
    { 
        //vibrar el gamepad que ha pulsado el boton
        var todosgamepads = Gamepad.all;
        for (ushort i = 0; i < todosgamepads.Count; i++)
        { 
            if (todosgamepads[i].deviceId == deviceId)
            {
                todosgamepads[i].SetMotorSpeeds(0.5f, 0.5f);
                await UniTask.Delay(110);
                todosgamepads[i].SetMotorSpeeds(0f, 0f);


            }
        }
    
    
    }

    public void SpawnerPowerups(ushort timeInSeconds)
    {

        spawnerpowerups = Observable
        .Timer(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(timeInSeconds))
        .TakeUntil(faseConcluida.Where(concluido => concluido == true))
        .Subscribe(time =>
        {
            var rndPos = UnityEngine.Random.Range(0, positionsPowerups.Length);
            var powerup = GameObject.Instantiate(prefabPowerup, positionsPowerups[rndPos].position, Quaternion.identity, canvasMenu[4].transform);

            powerup.GetComponent<ControllerPowerup>().gameController = this;
            
            

        }
        , ex => { Debug.Log(" OnError:" + ex.Message); spawnerpowerups.Dispose(); },
        () => //completado
        {

            //Debug.Log("barraprogreso disposed");
            if (spawnerpowerups is null == false) spawnerpowerups.Dispose();

        }).AddTo(this.gameObject);




    }





  

}
