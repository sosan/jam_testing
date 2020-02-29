using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

public class LobbyClientPun : MonoBehaviourPunCallbacks
{

    [SerializeField] private ControllerElegirPersonaje elegirPersonaje = null;
    [SerializeField] private GameController gameController = null;
    [SerializeField] private TextMeshPro txtStatus;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

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

        print("connected to master");

        PhotonNetwork.JoinRandomRoom();


    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }


    public override void OnJoinedRoom()
    {

        print("joined room");

        ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable();
        string nombre = "";
        (initialProps, nombre) = gameController.SetPlayerForOnline(ref initialProps);

        PhotonNetwork.NickName = nombre;

        PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
        PhotonNetwork.LocalPlayer.SetScore(0);


        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;
        print("numero de jugadores=" + PhotonNetwork.CountOfPlayers);
        if (PhotonNetwork.CountOfPlayers > 0)
        { 
            var playerOnline = PhotonNetwork.Instantiate(
                elegirPersonaje.playerPrefabOnline.name,
                gameController.initialPlayerPositions[PhotonNetwork.CountOfPlayers - 1].position,
                Quaternion.identity, 0);

            playerOnline.name = nombre;
            playerOnline.GetComponent<PhotonView>().RPC("InitialSetting", RpcTarget.AllBuffered,
                (byte)(gameController.jugadores[0].colorPlayer.r * 255),
                (byte)(gameController.jugadores[0].colorPlayer.g * 255),
                (byte)(gameController.jugadores[0].colorPlayer.b * 255),
                nombre,

                gameController.jugadores[0].powerDamage,
                gameController.jugadores[0].speedMovement,
                gameController.jugadores[0].shotSpeed                

                // todavia quedan mucho mas que enviar

                );


        }
        


        elegirPersonaje.SetPistaLibre();

        //PhotonNetwork.LoadLevel("Arena0_mobile");

    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    



    public void Update()
    {
        txtStatus.text = "Connection Status: "+ PhotonNetwork.NetworkClientState + " ping=" + PhotonNetwork.GetPing();

        
    }




}
