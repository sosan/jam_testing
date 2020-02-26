using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using TMPro;
using System;
using UnityEngine.UI;


public class InfoPlayer
{
    public GameObject focusPlayer;
    public GameObject playerGameObject;
    public GameObject playerPos;
    public Color colorPlayer;
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


    public bool bot;


    public InfoPlayer() { }

    public InfoPlayer(GameObject focusPlayer, GameObject playerGameObject, GameObject playerPos, Color colorPlayer, ushort posX, ushort posY,
        ushort posicionPlayer, bool isFirstMove, Image bigSelectionPlayer, bool selected, bool listo, bool bot)
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
    }


}



public class GameController : MonoBehaviour
{

    [Header("canvas")]
    [SerializeField] public GameObject[] canvasMenu = null;


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

    [HideInInspector] public ushort tiempoCurrentBatalla = 0;
    [SerializeField] private ushort TIEMPOMAXBATALLA = 180;


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


    private void Awake()
    {


        // inicizalicion de jugadores
        for(ushort i = 0; i < jugadores.Length; i++)
        { 
        
            jugadores[i] = new InfoPlayer();
        
        
        }



        if (isDebug == true)
        {


            //money.Value = 1000;
            speed = 0.8f;


        }


        tiempoBatalla.text = "";
        //textMoney.text = "";

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
        
       






    }


    private void IniciarCrono()
    {


        long binlocal = new DateTime(2019, 1, 1,9, 0, 0).ToBinary();

#if UNITY_EDITOR
        print("inicar crono=") ;

#endif
       


        crono = Observable.Timer(
        TimeSpan.FromSeconds(0), //esperamos 1 segundos 
        TimeSpan.FromSeconds(1), Scheduler.MainThread).Do(x => { }).
        ObserveOnMainThread().Take(TIEMPOMAXBATALLA)
        .Subscribe
        (_ =>
        {

            tiempoBatalla.text = Localization.Get("tiempo") + DateTime.FromBinary(binlocal).AddSeconds(tiempoCurrentBatalla).ToString("HH:mm");
            tiempoCurrentBatalla++;

        }
        , ex => { Debug.Log(" cuentaatrasantes OnError:" + ex.Message); if (crono != null) crono.Dispose(); },
        () => //completado
        {

            crono.Dispose();

            tiempoBatalla.text = "";
            tiempoCurrentBatalla = 0;


        }).AddTo(this.gameObject);




    }



  

}
