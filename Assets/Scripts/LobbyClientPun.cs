using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;
using System;
using UniRx;


public class LobbyClientPun : MonoBehaviourPunCallbacks
{

    [SerializeField] private ControllerElegirPersonaje elegirPersonaje = null;
    [SerializeField] private GameController gameController = null;
    [SerializeField] private TextMeshPro txtStatus;

    public short deviceId = -1;

    //private float tiempo = 30; //se/*g*/undos??
    private bool arrancartiempo = false;
    private short tiempoCurrent = 10;
    private short TIEMPO_MAXIMO = 30;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        TIEMPO_MAXIMO = tiempoCurrent;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {



    }


    public void ConnectToPun(string namePlayer)
    {

        PhotonNetwork.LocalPlayer.NickName = namePlayer;
        PhotonNetwork.ConnectUsingSettings();

    }


    public override void OnConnectedToMaster()
    {
#if UNITY_EDITOR
        print("connected to master");
#endif
        PhotonNetwork.JoinRandomRoom();


    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        contadorJugadores--;

    }

    public short contadorJugadores = 0;
    public override void OnJoinedRoom()
    {
#if UNIT_EDITOR
        print("joined room");
#endif
        ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable();
        initialProps = gameController.SetPlayerForOnline(ref initialProps);
        PhotonNetwork.NickName = PhotonNetwork.LocalPlayer.NickName ;

        PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
        PhotonNetwork.LocalPlayer.SetScore(0);


        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;
        //print("numero de jugadores=" + PhotonNetwork.CountOfPlayers);
        if (contadorJugadores >= 0 && contadorJugadores < 4)
        { 
            var playerOnline = PhotonNetwork.Instantiate(
                elegirPersonaje.playerPrefabOnline.name,
                gameController.initialPlayerPositions[contadorJugadores].position,
                Quaternion.identity, 0);



            playerOnline.transform.SetParent(gameController.canvasMenu[4].transform);
            contadorJugadores++;


            var configplayer = gameController.jugadores[0].focusPlayer.GetComponent<MatrixCharacters>();
            var punplayer = playerOnline.GetComponent<PunPlayer>();

            punplayer.gameController = gameController;
            punplayer.player.fireCooldown = configplayer.fireCooldown;
            punplayer.player.speedMovement = configplayer.speedMovement;
            punplayer.player.powerDamage = configplayer.power;
            punplayer.player.shotSpeed = configplayer.durationShotSeconds;
            punplayer.player.bombCooldown = configplayer.bombCooldown;
            punplayer.player.defense = configplayer.defense;
            punplayer.player.defenseMax = configplayer.defenseMax;
            punplayer.player.deviceId = gameController.jugadores[0].deviceId;

            playerOnline.GetComponent<PunPlayer>().colorInicial = Color.black;
            playerOnline.GetComponent<PunPlayer>().colorDestino = gameController.jugadores[0].colorPlayer;
            playerOnline.GetComponent<PunPlayer>().spritePlayer.color = gameController.jugadores[0].colorPlayer;

            playerOnline.name = PhotonNetwork.LocalPlayer.NickName;
            playerOnline.GetComponent<PhotonView>().RPC("InitialSetting", RpcTarget.AllBuffered,
                (byte)(gameController.jugadores[0].colorPlayer.r * 255),
                (byte)(gameController.jugadores[0].colorPlayer.g * 255),
                (byte)(gameController.jugadores[0].colorPlayer.b * 255),
                PhotonNetwork.LocalPlayer.NickName,

                punplayer.player.fireCooldown,
                punplayer.player.speedMovement,
                punplayer.player.powerDamage,
                punplayer.player.shotSpeed,
                punplayer.player.bombCooldown,
                punplayer.player.defense,
                punplayer.player.defenseMax,
                punplayer.player.deviceId 

                // todavia quedan mucho mas que enviar

                );

            //gameController.barrasPuntuaje[contadorJugadores].color = gameController.jugadores[0].colorPlayer;


        }

        IniciarCrono();

    }

    


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + UnityEngine.Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    
    public void Update()
    {
        txtStatus.text = "Connection Status: "+ PhotonNetwork.NetworkClientState + " ping=" + PhotonNetwork.GetPing();
        if (PhotonNetwork.CountOfPlayers == 4)
        {
            arrancartiempo = false;
            elegirPersonaje.SetPistaLibre();


        }

    }

    IDisposable crono = null;
    public void IniciarCrono()
    {

        //arrancartiempo = true;
        long binlocal = new DateTime(year:2019, month:1, day:1, hour:9, minute:0, second:0).ToBinary();

        crono = Observable.Timer(
        TimeSpan.FromSeconds(0), //esperamos 1 segundos 
        TimeSpan.FromSeconds(1), Scheduler.MainThread).Do(x => { }).
        ObserveOnMainThread().Take(TIEMPO_MAXIMO)
        .Subscribe
        (_ =>
        {
            gameController.elegirPersonaje.timeTxt.text = Localization.Get("tiempo") + DateTime.FromBinary(binlocal).AddSeconds(tiempoCurrent).ToString("mm:ss");
            tiempoCurrent--;
            gameController.elegirPersonaje.mensajeBotonEntrar.text = Localization.Get("esperando");

        }
        , ex => { Debug.Log(" cuentaatrasantes OnError:" + ex.Message); if (crono != null) crono.Dispose(); },
        () => //completado
        {

            crono.Dispose();
            arrancartiempo = false;

            elegirPersonaje.SetPistaLibre();


        }).AddTo(this.gameObject);




    }




}
