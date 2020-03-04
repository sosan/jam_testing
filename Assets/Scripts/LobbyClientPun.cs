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
using InControlActions;
using UnityEngine.InputSystem;
using UniRx.Async;

public class LobbyClientPun : MonoBehaviourPunCallbacks
{

    [SerializeField] private ControllerElegirConexion controllerElegirConexion = null;
    [SerializeField] private ControllerElegirPersonaje elegirPersonaje = null;
    [SerializeField] private GameController gameController = null;
    [SerializeField] private TextMeshPro txtStatus;
    [SerializeField] private ControllerExplicacion controllerExplicacion = null;


    public short deviceId = -1;

    //private float tiempo = 30; //se/*g*/undos??
    private bool arrancartiempo = false;
    private short tiempoCurrent = 10;
    private short TIEMPO_MAXIMO = 30;

    //public int contadorJugadores = 0;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        TIEMPO_MAXIMO = tiempoCurrent;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    


    public void ConnectToPun()
    {
        PhotonNetwork.LocalPlayer.NickName = "personaje" + UnityEngine.Random.Range(1, 100000000);
        //PhotonNetwork.LocalPlayer.NickName = namePlayer;
        PhotonNetwork.ConnectUsingSettings();

    }


   

    public ReactiveProperty<bool> siguienteFase = new ReactiveProperty<bool>(false);


    public override void OnConnectedToMaster()
    {
#if UNITY_EDITOR
        print("connected to master");
#endif

        controllerExplicacion.Init();

        //await UniTask.WaitUntil( () => siguienteFase.Value == true);

        //controllerElegirConexion.Init();


    }

    public void CrearPartida()
    { 
    
        string roomName = "Room " + UnityEngine.Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    
    
    }


    public void ConectarsePartida()
    { 

        //print("estado conexion=" + PhotonNetwork.NetworkingClient.State);

        if (PhotonNetwork.IsConnected && PhotonNetwork.NetworkingClient.State != ClientState.Joining  
            && PhotonNetwork.NetworkingClient.State != ClientState.ConnectingToGameserver)
        { 
        
            bool entrado = PhotonNetwork.JoinRandomRoom();
            //print("entrado" + entrado);
    
        
        }

        
    
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);
        //print("player entrado" + newPlayer.ActorNumber);

        Debug.Log( "OnPlayerEnteredRoom() " + newPlayer.NickName);

    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        
        

    }

    
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


        if (PhotonNetwork.IsMasterClient == true)
        { 
        
            
        
        }

      


        int contadorJugadores = PhotonNetwork.CurrentRoom.PlayerCount;
        contadorJugadores--;

        //print("numero de jugadores=" + PhotonNetwork.CurrentRoom.PlayerCount);
        //print("numero ju" + contadorJugadores);

        if (contadorJugadores <= 0)
        { 
            contadorJugadores = 0;
        
        }

        if (contadorJugadores >= 0 && contadorJugadores < 4)
        { 
            GameObject playerOnline = PhotonNetwork.Instantiate(
                elegirPersonaje.playerPrefabOnline.name,
                gameController.initialPlayerPositions[contadorJugadores].position,
                Quaternion.identity, 0);

            //contadorJugadores++;

            playerOnline.transform.SetParent(gameController.canvasMenu[4].transform);
            playerOnline.gameObject.name = PhotonNetwork.LocalPlayer.NickName;

            //var propiedad = PhotonNetwork.CurrentRoom.CustomProperties;
            
            for (int i = 0; i <  PhotonNetwork.PlayerList.Length; i++)
            { 
        
                gameController.elegirPersonaje.AddPlayerFromOnline(i);


                //if (PhotonNetwork.PlayerList[i].NickName != PhotonNetwork.LocalPlayer.NickName)
                //{ 
                //    string nombrePlayer = (string)PhotonNetwork.PlayerList[i].CustomProperties["namePlayer"];
            
                //}
        
            }
        

            //elegirPersonaje.AddPlayer(contadorJugadores, -1, false);

            //gameController.elegirPersonaje.AddPlayerFromOnline(contadorJugadores);

            if (PhotonNetwork.IsMasterClient == true)
            { 
                var view = playerOnline.GetPhotonView();
                gameController.viewIdMasterclient = view.ViewID;
            
            }

            playerOnline.GetComponent<PhotonView>().RPC("UpdateUI", RpcTarget.OthersBuffered);
           


            var configplayer = gameController.jugadores[contadorJugadores].focusPlayer.GetComponent<MatrixCharacters>();
            //playerOnline.GetComponent<ControllerPlayer>().player.deviceId =  gameController.jugadores[0].deviceId;


            var punplayer = playerOnline.GetComponent<ControllerPlayer>();

            punplayer.gameController = gameController;
            punplayer.player.fireCooldown = configplayer.fireCooldown;
            punplayer.player.speedMovement = configplayer.speedMovement;
            punplayer.player.powerDamage = configplayer.power;
            punplayer.player.shotSpeed = configplayer.durationShotSeconds;
            punplayer.player.bombCooldown = configplayer.bombCooldown;
            punplayer.player.defense = configplayer.defense;
            punplayer.player.defenseMax = configplayer.defenseMax;
            //punplayer.player.deviceId = gameController.jugadores[0].deviceId;

            punplayer.posicion = contadorJugadores;

            switch (contadorJugadores)
            {
                case 0: playerOnline.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.amarillo;  break;
                case 1: playerOnline.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.azul; break;
                case 2: playerOnline.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.rojo; break;
                case 3: playerOnline.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.blanco; break;

            
            
            }

            playerOnline.GetComponent<ControllerPlayer>().colorInicial = Color.black;
            playerOnline.GetComponent<ControllerPlayer>().colorDestino = gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores];
            playerOnline.GetComponent<ControllerPlayer>().spritePlayer.color = gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores];
            playerOnline.GetComponent<ControllerPlayer>().player.colorPlayer = gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores];

            gameController.barrasPuntuaje[contadorJugadores].color = gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores];


            playerOnline.GetComponent<PhotonView>().RPC("InitialSetting", RpcTarget.OthersBuffered,
                (byte)(gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores].r * 255),
                (byte)(gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores].g * 255),
                (byte)(gameController.elegirPersonaje.prefabColorsPlayers[contadorJugadores].b * 255),
                PhotonNetwork.LocalPlayer.NickName,

                punplayer.player.fireCooldown,
                punplayer.player.speedMovement,
                punplayer.player.powerDamage,
                punplayer.player.shotSpeed,
                punplayer.player.bombCooldown,
                punplayer.player.defense,
                punplayer.player.defenseMax,
            contadorJugadores,
            playerOnline.GetComponent<ControllerPlayer>().player.nombreColorPlayer.ToString()

                );


        }

        elegirPersonaje.InitActions(online: true);

        //IniciarCrono();

    }

    private void InstanciarPlayerOnline()
    { 
    
    
    
    }

    


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + UnityEngine.Random.Range(1000, 10000);
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options, null);
        //Debug.LogError("no posible conexion");

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

    public void InstanciarPowerups(Vector3 posicion)
    { 
        PhotonNetwork.InstantiateSceneObject("PowerupOnline", posicion, Quaternion.identity, 0);
        
    
    
    }




}
