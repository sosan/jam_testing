using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using InControlActions;
using UnityEngine.InputSystem;
using System;
using UniRx.Async;

public class PunPlayer : MonoBehaviourPun, IPunObservable
{
    public ControlActions inputActions;
    [SerializeField] public LobbyClientPun lobby = null;
    [SerializeField] public GameController gameController = null;
    [SerializeField] public PhotonView photonview = null;

    [SerializeField] private GameObject[] shotPrefabs = null;
    

    [SerializeField] public InfoPlayer player = new InfoPlayer();
    [SerializeField] public Rigidbody2D rigid = null;
    [SerializeField] public Transform thistransform = null;
    [SerializeField] public LayerMask raycastLayerMask = -1;
    [SerializeField] public Color colorInicial = Color.white;
    [SerializeField] public Color colorDestino = Color.white;
    [SerializeField] public SpriteRenderer spritePlayer = null;

    private Transform ultimoBloquePisado = null;
    private Vector2 _inputs = Vector2.zero;

    [HideInInspector] public bool isDestroyer = false;
    [HideInInspector] public bool caidoHueco = false;
    private bool isFireCooldown = false;
    private bool bombAwaiting = false;
    private bool isCompletedMoveLeftStick = false;
    
    private bool isDashing = false;

    public float m_Speed = 5f;

    private Vector2 latestPos = Vector2.zero;
    private Vector2 latestVel = Vector2.zero;
    private float latestRot = 0;
    private Quaternion latestRotQ = Quaternion.identity;
    private float latestAngVel = 0;

    private float distanceOldPosition = 0;
    private float angleOldPos = 0;

    
    private int posicionBarras = -1;
    private Color colorBarra = Color.white;


    private void Awake()
    {
        if (gameController is null == true)
        { 
            gameController = GameObject.FindObjectOfType<GameController>();
        
        }

        if (lobby is null == true)
        { 
            lobby = GameObject.FindObjectOfType<LobbyClientPun>();
        
        }


        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void ControlDpad(InputAction.CallbackContext obj)
    {
       
        
        
    }





    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigid.position);
            stream.SendNext(rigid.rotation);
            stream.SendNext(rigid.velocity);
            stream.SendNext(rigid.angularVelocity);
            
            if (PhotonNetwork.IsMasterClient == true)
            { 
                stream.SendNext(gameController.tiempoCurrentBatalla);
            
            }
            


        }
        else
        {
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

            latestPos = (Vector2)stream.ReceiveNext();
            latestRot = (float)stream.ReceiveNext();

            //print("proxy rot=" + latestRot);

            rigid.velocity = (Vector2)stream.ReceiveNext();
            latestPos += rigid.velocity * lag;
            distanceOldPosition = Vector2.Distance(rigid.position, latestPos);

            rigid.angularVelocity = (float)stream.ReceiveNext();
            latestRot += rigid.angularVelocity * lag;
            angleOldPos = Mathf.Abs(rigid.rotation - latestRot);

            if (PhotonNetwork.IsMasterClient == false)
            {

                gameController.tiempoCurrentBatalla = (short)stream.ReceiveNext();

            }


        }
    }


    //sincronizacion principal
    private void FixedUpdate()
    {
        if(gameController.playersSePuedenMover == false) return;


        if (photonView.IsMine == true)
        {

            //if(player.playerSePuedeMover == true)
            //{ 

            //    rigid.MovePosition(rigid.position + _inputs * m_Speed * Time.fixedDeltaTime);
            //}

        }
        else
        { 
        
            rigid.position = Vector2.MoveTowards(rigid.position, latestPos, distanceOldPosition * (1f / PhotonNetwork.SerializationRate));
        
        }

    }



    [PunRPC]
    private void InitialSetting(byte r, byte g, byte b, string namePlayer,
        float fireCooldown,
        float speedMovement,
        float powerDamage,
        float shotSpeed,
        float bombCooldown,
        float defense,
        float defenseMax,
        int posicion,
        string nombreColorPlayer,

        PhotonMessageInfo info)
    {

        //print("r=" + r  + " g=" + g + " b=" + b);

        if (gameController is null == true)
        { 
            gameController = GameObject.FindObjectOfType<GameController>();
        
        }

        if (lobby is null == true)
        { 
        
            lobby = GameObject.FindObjectOfType<LobbyClientPun>();
        
        }

        PhotonView photonView = PhotonView.Get(info.photonView);

        
        if (gameController.playersPhotonViewIdDict.ContainsKey(photonView.ViewID) == false)
        {

            gameController.playersPhotonViewIdDict.Add(photonView.ViewID, photonView.gameObject);

        }
        else
        {

            gameController.playersPhotonViewIdDict[photonView.ViewID] = photonView.gameObject;
        }

        GameObject playerProxy = photonView.gameObject;

        playerProxy.transform.SetParent(gameController.canvasMenu[4].transform);
        playerProxy.gameObject.name = namePlayer;

        playerProxy.GetComponent<SpriteRenderer>().color = new Color32(r, g, b, 255);
        playerProxy.GetComponent<ControllerPlayer>().player.colorPlayer = new Color32(r, g, b, 255);

        playerProxy.GetComponent<ControllerPlayer>().colorInicial = Color.black;
        playerProxy.GetComponent<ControllerPlayer>().colorDestino = new Color32(r, g, b, 255);
        playerProxy.GetComponent<ControllerPlayer>().posicion = posicion;

        print("nombrecolorplayer=" + nombreColorPlayer);

        switch(nombreColorPlayer)
        {
            case "amarillo": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.amarillo; break;
            case "azul": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.azul; break;
            case "rojo": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.rojo; break;
            case "blanco": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.blanco; break;

            case "0": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.amarillo; break;
            case "1": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.azul; break;
            case "2": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.rojo; break;
            case "3": playerProxy.GetComponent<ControllerPlayer>().player.nombreColorPlayer = nombreColores.blanco; break;

        }

        print("posicion=" + posicion);
        posicionBarras = posicion;
        colorBarra = new Color32(r, g, b, 255);

        gameController.barrasPuntuaje[posicion].color = new Color32(r, g, b, 255);


    }
    
    [PunRPC]
    private void Empezar(bool empezar)
    { 
    
        gameController.canvasMenu[5].SetActive(true);
        gameController.canvasMenu[6].SetActive(true);

        gameController.canvasMenu[4].SetActive(true);
        gameController.canvasMenu[3].SetActive(false);
                    

        gameController.InitGame();
    
    }


    public void SendFireRpc(/*GameObject shotPrefab,*/ Vector3 playerPos, Vector2 touchPos, Color playerColor, GameObject crossHairPrefab, Vector2 centerFireZone,
    Vector2 fireZoneSize, float bulletSpeed, float destroyCrossSec, float destroyBulletSec
    )
    {





        //zona click localizada
        {

            //Vector2 localTouchPos = this.transform.InverseTransformPoint(touchPos);
            //localTouchPos.x = touchPos.x - fireZoneSize.x;

            //Vector2 dir = (localTouchPos - centerFireZone).normalized;
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        }


        //toda la pantalla
        //Vector2 touchWorld = Camera.main.ScreenToWorldPoint(touchPos);

        //print("touchworld=" + touchWorld);

        //Vector2 dir = (touchWorld - (new Vector2(playerPos.x, playerPos./y))).normalized;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        //photonview.RPC("InstantiateFire", RpcTarget.All,
        //    gameController..infoPlayer.indexShotPrefab, //shotPrefab.name,
        //    playerPos,
        //    angle,
        //    (byte)(playerColor.r * 255),
        //    (byte)(playerColor.g * 255),
        //    (byte)(playerColor.b * 255),
        //    dir,
        //    bulletSpeed,
        //    destroyBulletSec

        //    );

        

    }


    [PunRPC]
    private void InstantiateFire(
        int indexShotPrefab,//string shotprefabName,
        Vector3 playerPos, 
        float angle,
        byte r,
        byte g,
        byte b,
        Vector2 dir,
        float bulletSpeed,
        float destroyBulletSec


        )
    {
        print("instanciaon bullet");

        //if (indexShotPrefab < 0 || indexShotPrefab > ushort.MaxValue) return;


        //GameObject bullet = GameObject.Instantiate(shotPrefabs[indexShotPrefab], playerPos, Quaternion.AngleAxis(angle, Vector3.forward));


        //bullet.GetComponent<SpriteRenderer>().color = new Color32(r, g, b, 255); // playerColor;
        //bullet.GetComponent<Weapon>().player = this.gameObject; //<<< quizas fallo
        ////jl. ¿es mejor usar velocity sobre addforce para posicion en red?.
        ////bullet.GetComponent<Rigidbody2D>().AddForce(dir * 1000f);
        //bullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        //Destroy(bullet, destroyBulletSec);



        //playerGravity.armSprite.flipX = false;
        //playerGravity.pivotArm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);




    }

    [PunRPC]
    private void ColisionConFondo(string nameBox, byte r, byte g, byte b, short numeroAmarillo, short numeroAzules,
        short numeroRojos, short numeroBlancos, PhotonMessageInfo info)
    { 
        //if (gameController.playersPhotonViewIdDict.ContainsKey(info.photonView.ViewID) == true)
        //{

        //}
        if (gameController is null == true)
        { 
        
            gameController = GameObject.FindObjectOfType<GameController>();
        
        }


        Color32 color = new Color32(r, g, b, 255);
        //print("namebox" + nameBox + color.ToString());


        if (gameController.listadoBoxes.ContainsKey(nameBox) == true)
        { 
        
            gameController.listadoBoxes[nameBox].GetComponent<SpriteRenderer>().color = new Color32(r, g, b, 255);

            gameController.bloquesAmarillos = numeroAmarillo;
            gameController.bloquesAzules = numeroAzules;
            gameController.bloquesRojos = numeroRojos;
            gameController.bloquesBlancos = numeroBlancos;

        
        }


    }


    //public void SendRopeToMaster(Vector2 worldpos)
    //{

    //    photonview.RPC("MasterCheckPowerups", RpcTarget.MasterClient, worldpos);


    //}

    //[PunRPC]
    //private void MasterCheckPowerups(Vector2 worldPos, PhotonMessageInfo info)
    //{

    //    if (PhotonNetwork.IsMasterClient == false) return;


    //    //print("Checking IN Masterclient");

    //    (bool, string) isHitted = spawnerPowerups.CheckOverlap(worldPos);
    //    //print("ishitted=" + isHitted);
    //    if (isHitted.Item1 == true)
    //    {

    //        photonview.RPC("PowerupsMasterToAll", RpcTarget.All, info.photonView.ViewID, worldPos, isHitted.Item2);

    //    }



    //}


    //[PunRPC]
    //private void PowerupsMasterToAll(int viewid, Vector2 worldPos, string powerupName)
    //{

    //    if (ManagerArenaPun.playersPhotonViewIdDict.ContainsKey(viewid) == true)
    //    {


    //        GameObject player = ManagerArenaPun.playersPhotonViewIdDict[viewid];



    //        ropeSystemMobile.PlayRopePun(worldPos, player);
    //        //player.GetComponent<RopeSystemMobile>().PlayRopePun(worldPos, player);
    //        if (spawnerPowerups.dictPowerups.ContainsKey(powerupName) == true)
    //        {

    //            spawnerPowerups.dictPowerups[powerupName].GetComponent<Powerup>().DisablePowerup();


    //        }
    //        /*player.GetComponentInChildren<ControlPlayerFx>()*/
    //        controlPlayerFx.GettingPowerup();


    //    }
    


    //}

    [PunRPC]
    private void UpdateUI()
    { 
        for (int i = 0; i <  PhotonNetwork.PlayerList.Length; i++)
        { 
        
            gameController.elegirPersonaje.AddPlayerFromOnline(i);

        }
    
    }


    [PunRPC]
    private void ColisionConHueco(bool playerSePuedeMover, bool caidohueco, PhotonMessageInfo info)
    { 
    
        var player = PhotonNetwork.GetPhotonView( info.photonView.ViewID).gameObject;
        player.GetComponent<ControllerPlayer>().ProcesarHueco(null);

    
    }


    [PunRPC]
    private void ColisionConPowerup(int idplayer, int idpowerup, PhotonMessageInfo info)
    { 
        
        print("colisionconpowerup=" + idplayer +  " idpowerup=" + idpowerup );

        var player = PhotonNetwork.GetPhotonView( idplayer).gameObject;
        player.GetComponent<ControllerPlayer>().ProcesarPowerUp();

        if (PhotonNetwork.IsMasterClient == true)
        { 
            var powerup = PhotonNetwork.GetPhotonView( idpowerup);
            PhotonNetwork.Destroy(powerup);
            
        
        }
    
    }

    [PunRPC]
    private void DatosBomba(List<string> datosAmarillos, List<string> datosAzules, List<string> datosRojos, List<string> datosBlancos)
    { 
    
        print("datosamarillos" + datosAmarillos.Count);
        print("datosamarillos" + datosAzules.Count);
    
    }


    //[PunRPC]
    //private void QuitarPowerup(int target, PhotonMessageInfo info)
    //{

    //    print("hola");
       
    //    var powerup = PhotonNetwork.GetPhotonView( target);
    //    PhotonNetwork.DestroyAll(powerup);
    
    
    //}


}
