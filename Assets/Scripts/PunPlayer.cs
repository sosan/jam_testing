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



    // Start is called before the first frame update
    void Start()
    {

        //inputActions = new ControlActions();
        //inputActions.Menu.Dpad.performed += ControlDpad;

        //inputActions.Menu.Buttons.performed += BotonSur;
        //inputActions.Menu.ExitButton.performed += BotonOeste;

        //inputActions.Menu.LeftStick.performed += ControlLeftStick;
        //inputActions.Menu.LeftStick.canceled += ResetLeftStick;


        //inputActions.Enable();

        if (gameController is null == true)
        { 
            gameController = GameObject.FindObjectOfType<GameController>();
        
        }
        
        
        

    }

    private void ControlDpad(InputAction.CallbackContext obj)
    {
       
        
        
    }


    //private async void BotonOeste(InputAction.CallbackContext obj)
    //{
    //    if (photonView.IsMine == false) return;
    //    //if (obj.control.device.deviceId != player.deviceId) return;
    //    if(gameController.playersSePuedenMover == false) return;
    //    if(player.playerSePuedeMover == false) return;
    //    if (bombAwaiting == true) return;

    //     //raycast para saber si estamos encima de que estamos..
    //    var hit = Physics2D.OverlapBox(thistransform.position, new Vector2(0, 0), 0, layerMask: raycastLayerMask);
    //    if (hit is null == false)
    //    { 
    //        if (hit.CompareTag("fondo"))
    //        { 
    //            return;
    //        }

        
    //    }
       

    //    gameController.HacerVibrarMando(obj.control.device.deviceId);


    //    float x = (float)Math.Round(thistransform.position.x * 2, MidpointRounding.ToEven) / 2;
    //    float y = (float)Math.Round(thistransform.position.y * 2, MidpointRounding.ToEven) / 2;

    //    //print("x=" + x + " y=" + y);
    //    if ((Math.Abs(x) % 1) == 0)
    //    {
    //        x += 0.5f;

    //    }

    //    if ((Math.Abs(y) % 1) == 0)
    //    {
    //        y += 0.5f;

    //    }


    //    if (isDestroyer == true)
    //    { 
            
    //        isDestroyer = false;
    //        spritePlayer.color = player.colorPlayer;
    //        GameObject destroyer = GameObject.Instantiate(gameController.prefabDestroyer, 
    //            new Vector3(x, y, 0), 
    //            Quaternion.identity,
    //            gameController.canvasMenu[4].transform);

    //        destroyer.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
    //        destroyer.GetComponent<ControllerDestroyer>().color = player.colorPlayer;
    //        destroyer.GetComponent<ControllerDestroyer>().cruz.color = player.colorPlayer;
    //        //destroyer.GetComponent<ControllerDestroyer>().controllerplayer = this;
            
        
    //    }
    //    else
    //    { 
        
    //        GameObject bomba = GameObject.Instantiate(gameController.prefabBomba, 
    //            new Vector3(x, y, 0), 
    //            Quaternion.identity,
    //            gameController.canvasMenu[4].transform);

    //        bomba.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
    //        bomba.GetComponent<ControllerBomba>().color = player.colorPlayer;
    //        bomba.GetComponent<ControllerBomba>().cruz.color = player.colorPlayer;
    //        //bomba.GetComponent<ControllerBomba>().controllerplayer = this;
        
    //    }

       
        
    //    bombAwaiting = true;
    //    //print("player cooldown=" + player.bombCooldown);
    //    await UniTask.Delay(TimeSpan.FromSeconds(player.bombCooldown));
    //    bombAwaiting = false;


       



    //}

  

    //private async void BotonSur(InputAction.CallbackContext obj)
    //{
    //    //print("obj="  + obj.control.device.deviceId + " deviceidplayer=" +  player.deviceId);
    //    //if (obj.control.device.deviceId != player.deviceId) return;
    //    if (photonView.IsMine == false) return;
    //    if(gameController.playersSePuedenMover == false) return;
    //    if(player.playerSePuedeMover == false) return;


    //    if (isFireCooldown == true) return;


    //    //si mira pa la derecha o pa la izquierda?
    //    //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


    //    Vector2 direction = Vector2.zero;

    //    if (_inputs.x >= 0f)
    //    { 
    //        direction = new Vector2(0.5f, 0); 
        
    //    }
    //    else
    //    { 
    //        direction = new Vector2(-0.5f, 0);
        
    //    }



    //    GameObject bullet = GameObject.Instantiate(gameController.prefabBullet, 
    //        new Vector3(thistransform.position.x + direction.x, thistransform.position.y, thistransform.position.z),
    //        Quaternion.AngleAxis(0, Vector3.forward),
    //        gameController.canvasMenu[4].transform );

        
    //    bullet.GetComponent<Rigidbody2D>().velocity = direction * player.shotSpeed;

    //    isFireCooldown = true;
    //    await UniTask.Delay(TimeSpan.FromSeconds(player.fireCooldown));
    //    isFireCooldown = false;


    //}

    //private void ResetLeftStick(InputAction.CallbackContext obj)
    //{

    //    //print("reset obj=" + obj.ReadValue<Vector2>());
    //    if (photonView.IsMine == false) return;
    //    if (obj.control.device.deviceId != player.deviceId) return;
    //    if(gameController.playersSePuedenMover == false) return;
    //    _inputs = Vector2.zero;
    //    //print("reset 0");

    //    if (isDashing == true) return;



    //    //if (faseDashDerecha > 2)
    //    //{
    //    //    print("derecha=" + countvecesDerecha);
    //    //    countvecesDerecha++;
            
    //    //}

    //    //if (faseDashIzquierda == 1)
    //    //{ 
    //    //    print("izquierda=" + countvecesIzquierda);
    //    //    countvecesIzquierda++;
    //    //    return;
    //    //}

    //    //if (faseDashArriba == 1)
    //    //{ 
    //    //    print("arriba=" + countvecesArriba);
    //    //    countvecesArriba++;
    //    //    return;
    //    //}

    //    //if (faseDashAbajo == 1)
    //    //{ 
    //    //    print("abajo=" + countvecesAbajo);
    //    //    countvecesAbajo++;
    //    //    return;
    //    //}

       
    //}
    
    

    //private void ControlLeftStick(InputAction.CallbackContext obj)
    //{

    //    //print(
    //    //    "obj=" + obj.control.device.deviceId + 
    //    //    " deviceidplayer=" + player.deviceId +
    //    //    " gameController.playersSePuedenMover=" + gameController.playersSePuedenMover +
    //    //    " player.playerSePuedeMover=" + player.playerSePuedeMover + 
    //    //    "photonView.IsMine=" + photonView.IsMine
            
    //    //    );
    //    //if (obj.control.device.deviceId != player.deviceId) return;
    //    if (photonView.IsMine == false) return;
    //    if (gameController.playersSePuedenMover == false) return;
    //    if(player.playerSePuedeMover == false) return;

    //    _inputs = obj.ReadValue<Vector2>();
        
    //    //if (_inputs.x > 0.99f)
    //    //{
    //    //    faseDashDerecha = 1;
    //    //    startTimePosibleDashDerecha = 0;
    //    //    return;
            
    //    //}
        
    //    //if (_inputs.x < -0.99f)
    //    //{ 
    //    //    faseDashIzquierda = 1;
    //    //}

    //    //if (_inputs.y > 0.99f)
    //    //{ 
    //    //    faseDashArriba  =1;
    //    //}
        
    //    //if (_inputs.y < -0.99f)
    //    //{ 
    //    //    faseDashAbajo  =1;
    //    //}


        


    //}



    //void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{


    //}



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigid.position);
            stream.SendNext(rigid.rotation);
            stream.SendNext(rigid.velocity);
            stream.SendNext(rigid.angularVelocity);
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
        int deviceId ,

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

        GameObject playerGo = photonView.gameObject;

        playerGo.GetComponent<SpriteRenderer>().color = new Color32(r, g, b, 255);


        gameController.barrasPuntuaje[lobby.contadorJugadores - 1].color = new Color32(r, g, b, 255);


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


}
