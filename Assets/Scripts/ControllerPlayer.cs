using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InControlActions;
using UniRx.Async;
using System;
using Photon.Pun;
using UniRx;

public class ControllerPlayer : MonoBehaviour
{

    public ControlActions inputActions;
    public PunPlayer punPlayer = null;
    [SerializeField] public InfoPlayer player = new InfoPlayer();
    [SerializeField] public GameController gameController = null;
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
    
    //private bool posibleDashDerecha = false;
    //private bool posibleDashIzquierda = false;
    //private bool posibleDashArriba = false;
    //private bool posibleDashAbajo = false;
    


    private bool resetDashDerecha = false;
    private bool resetDashIzquierda = false;
    private bool resetDashArriba = false;
    private bool resetDashAbajo = false;

    public float m_Speed = 12f;
    private float m_MovementInputValue;
    private float progresoLerp = 0;
    private float interpolateDuration = 0.2f;
    //private float startTime = 0;
    private float durationForLongPressSeconds = 0.6f;
    private float startTimePosibleDashDerecha = 0;
    private float startTimePosibleDashIzquierda = 0;
    private float startTimePosibleDashArriba = 0;
    private float startTimePosibleDashAbajo = 0;


    private float startTimeResetDashDerecha = 0;
    private float startTimeResetDashIzquierda = 0;
    private float startTimeResetDashArriba = 0;
    private float startTimeResetDashAbajo = 0;


    private ushort countvecesDerecha = 0;
    private ushort countvecesIzquierda = 0;
    private ushort countvecesArriba = 0;
    private ushort countvecesAbajo = 0;
    
    private ushort faseDashDerecha = 0;
    private ushort faseDashIzquierda = 0;
    private ushort faseDashArriba = 0;
    private ushort faseDashAbajo = 0;
    
    IDisposable cronoCooldown = null;
    [SerializeField] private Transform cooldownBar = null;
    
    public int posicion = 0;
    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        thistransform = this.gameObject.transform;

        SetCooldownBar(0);

        if (gameController is null == true)
        { 
            gameController = GameObject.FindObjectOfType<GameController>();
        
        } 
        
    }

    void Start()
    {

        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += ControlDpad;

        inputActions.Menu.Buttons.performed += BotonSur;
        inputActions.Menu.ExitButton.performed += BotonOeste;

        inputActions.Menu.LeftStick.performed += ControlLeftStick;
        inputActions.Menu.LeftStick.canceled += ResetLeftStick;


        inputActions.Enable();

        
    }

    private void ControlDpad(InputAction.CallbackContext obj)
    {
       
        
        
    }


    private async void BotonOeste(InputAction.CallbackContext obj)
    {

        //print("obj=" + obj.control.device.deviceId +" deviceid="  +  player.deviceId +
        //    "players=" + gameController.playersSePuedenMover + " se=" + player.playerSePuedeMover + " awating" + bombAwaiting
            
        //    );


        if (gameController.isOnline == false)
        { 
            if (obj.control.device.deviceId != player.deviceId) return;
            if(gameController.playersSePuedenMover == false) return;
            if(player.playerSePuedeMover == false) return;

        
        }

        if (bombAwaiting == true) return;

         //raycast para saber si estamos encima de que estamos..
        var hit = Physics2D.OverlapBox(thistransform.position, new Vector2(0, 0), 0, layerMask: raycastLayerMask);
        if (hit is null == false)
        { 
            if (hit.CompareTag("fondo"))
            { 
                return;
            }

        
        }
        
        ////10ms tag
        //var stop = System.Diagnostics.Stopwatch.StartNew();
        //stop.Start();


        //for(int i = 0; i < 1000000; ++i)
        //{ 
        
        //    hit.gameObject.name = "amarillo";
        
        //}

        //stop.Stop();
        //print("tiempo=" + stop.Elapsed.TotalMilliseconds);

        gameController.HacerVibrarMando(obj.control.device.deviceId);


        float x = (float)Math.Round(thistransform.position.x * 2, MidpointRounding.ToEven) / 2;
        float y = (float)Math.Round(thistransform.position.y * 2, MidpointRounding.ToEven) / 2;

        //print("x=" + x + " y=" + y);
        if ((Math.Abs(x) % 1) == 0)
        {
            x += 0.5f;

        }

        if ((Math.Abs(y) % 1) == 0)
        {
            y += 0.5f;

        }


        if (isDestroyer == true)
        { 
            
            isDestroyer = false;
            spritePlayer.color = player.colorPlayer;

            if (gameController.isOnline)
            { 

                
                object[] data = new object[3];

                data[0] = player.colorPlayer.r;
                data[1] = player.colorPlayer.g;
                data[2] = player.colorPlayer.b;


                var destroyer = PhotonNetwork.Instantiate("DestroyerOnline", new Vector3(x, y, 0), Quaternion.identity, 0, data);
                
                destroyer.transform.SetParent(gameController.canvasMenu[4].transform);

                //destroyer.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
                //destroyer.GetComponent<ControllerDestroyer>().color = player.colorPlayer;
                //destroyer.GetComponent<ControllerDestroyer>().cruz.color = player.colorPlayer;
                //destroyer.GetComponent<ControllerDestroyer>().controllerplayer = this;


            
            }
            else
            { 
                GameObject destroyer = GameObject.Instantiate(gameController.prefabDestroyer, 
                new Vector3(x, y, 0), 
                Quaternion.identity,
                gameController.canvasMenu[4].transform);

                destroyer.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
                destroyer.GetComponent<ControllerDestroyer>().color = player.colorPlayer;
                destroyer.GetComponent<ControllerDestroyer>().cruz.color = player.colorPlayer;
                //destroyer.GetComponent<ControllerDestroyer>().controllerplayer = this;
            
            
            }


           
            
        
        }
        else
        { 
        
            GameObject bomba = GameObject.Instantiate(gameController.prefabBomba, 
                new Vector3(x, y, 0), 
                Quaternion.identity,
                gameController.canvasMenu[4].transform);

            bomba.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
            bomba.GetComponent<ControllerBomba>().color = player.colorPlayer;
            bomba.GetComponent<ControllerBomba>().cruz.color = player.colorPlayer;
            bomba.GetComponent<ControllerBomba>().controllerplayer = this;
        
        }

       
        
        bombAwaiting = true;
        //print("player cooldown=" + player.bombCooldown);
        CooldownShow((int)player.bombCooldown);
        await UniTask.Delay(TimeSpan.FromSeconds(player.bombCooldown));
        bombAwaiting = false;


       



    }

    private void SetCooldownBar(int x)
    { 
        var localScale = cooldownBar.localScale;
        localScale.x = x;
        cooldownBar.localScale = localScale;
    
    }


    private void CooldownShow(int TIEMPOMAXBATALLA)
    { 
        

        if (cronoCooldown is null == false) cronoCooldown.Dispose();

        SetCooldownBar(1);

        int duracion = TIEMPOMAXBATALLA * 1000 / 250;
        float desplazamiento = 1f / duracion; 
       
        cronoCooldown = Observable.Timer(
        TimeSpan.FromSeconds(0),
        TimeSpan.FromMilliseconds(250), Scheduler.MainThread).Do(x => { }).
        ObserveOnMainThread().Take(duracion)
        .Subscribe
        (_ =>
        {
            
            var localscale = cooldownBar.localScale;
            localscale.x -= desplazamiento; 
            if (localscale.x <= 0)
            { 
                localscale.x = 0;
            
            }

            cooldownBar.localScale = localscale;

            if (localscale.x <= 0)
            { 
                cronoCooldown.Dispose();
            
            }


        }
        , ex => { Debug.Log(" cooldown OnError:" + ex.Message); if (cronoCooldown != null) cronoCooldown.Dispose(); },
        () => //completado
        {

            cronoCooldown.Dispose();


        }).AddTo(this.gameObject);
    
    }

  

    private async void BotonSur(InputAction.CallbackContext obj)
    {
        //print("obj="  + obj.control.device.deviceId + " deviceidplayer=" +  player.deviceId);

        if (gameController.isOnline == false)
        { 
            if (obj.control.device.deviceId != player.deviceId) return;
            if(gameController.playersSePuedenMover == false) return;
            if(player.playerSePuedeMover == false) return;

        
        }
        

        if (isFireCooldown == true) return;


        //si mira pa la derecha o pa la izquierda?
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        Vector2 direction = Vector2.zero;

        if (_inputs.x >= 0f)
        { 
            direction = new Vector2(0.5f, 0); 
        
        }
        else
        { 
            direction = new Vector2(-0.5f, 0);
        
        }



        GameObject bullet = GameObject.Instantiate(gameController.prefabBullet, 
            new Vector3(thistransform.position.x + direction.x, thistransform.position.y, thistransform.position.z),
            Quaternion.AngleAxis(0, Vector3.forward),
            gameController.canvasMenu[4].transform );

        
        bullet.GetComponent<Rigidbody2D>().velocity = direction * player.shotSpeed;

        isFireCooldown = true;
        await UniTask.Delay(TimeSpan.FromSeconds(player.fireCooldown));
        isFireCooldown = false;


    }

    private void ResetLeftStick(InputAction.CallbackContext obj)
    {

        //print("reset obj=" + obj.ReadValue<Vector2>());

        if (gameController.isOnline == false)
        { 
        
            if (obj.control.device.deviceId != player.deviceId) return;
            if(gameController.playersSePuedenMover == false) return;
            if(player.playerSePuedeMover == false) return;
        
        }


        
        _inputs = Vector2.zero;
        //print("reset 0");

        if (isDashing == true) return;



        if (faseDashDerecha > 2)
        {
            print("derecha=" + countvecesDerecha);
            countvecesDerecha++;
            
        }

        //if (faseDashIzquierda == 1)
        //{ 
        //    print("izquierda=" + countvecesIzquierda);
        //    countvecesIzquierda++;
        //    return;
        //}

        //if (faseDashArriba == 1)
        //{ 
        //    print("arriba=" + countvecesArriba);
        //    countvecesArriba++;
        //    return;
        //}

        //if (faseDashAbajo == 1)
        //{ 
        //    print("abajo=" + countvecesAbajo);
        //    countvecesAbajo++;
        //    return;
        //}

        if (countvecesDerecha >= 1)
        { 
            Debug.LogError("dash derecha");
            startTimeResetDashDerecha = 0;
            resetDashDerecha = true;
            isDashing = true;
            return;

        }

        if (countvecesIzquierda >= 1)
        { 
            Debug.LogError("dash izquierda");

            startTimeResetDashIzquierda = 0;
            resetDashIzquierda = true;
            isDashing = true;
            return;
        
        }

        if (countvecesArriba >= 1)
        { 

            Debug.LogError("dash arriba");
            startTimeResetDashArriba = 0;
            resetDashArriba = true;
            isDashing = true;
            return;
        }

        if (countvecesAbajo >= 1)
        { 
            Debug.LogError("dash abajo");
            startTimeResetDashAbajo = 0;
            resetDashAbajo = true;
            isDashing = true;
            return;
        }



    }
    
    

    private void ControlLeftStick(InputAction.CallbackContext obj)
    {

        //print("obj=" + obj.control.device.deviceId + " deviceidplayer=" + player.deviceId);
        //print("gameController.playersSePuedenMover" + gameController.playersSePuedenMover + " player.playerSePuedeMover=" + player.playerSePuedeMover);

        if (gameController.isOnline == false)
        { 

            if (obj.control.device.deviceId != player.deviceId) return;
            if(gameController.playersSePuedenMover == false) return;
            if(player.playerSePuedeMover == false) return;


        }

        _inputs = obj.ReadValue<Vector2>();
        
        //print(_inputs.ToString());
        if (_inputs.x > 0.99f)
        {
            faseDashDerecha = 1;
            startTimePosibleDashDerecha = 0;
            //posibleDashDerecha = true;
            return;
            
        }
        
        if (_inputs.x < -0.99f)
        { 
            faseDashIzquierda = 1;
            //posibleDashIzquierda = true;
        }

        if (_inputs.y > 0.99f)
        { 
            faseDashArriba  =1;
            //posibleDashArriba = true;
        }
        
        if (_inputs.y < -0.99f)
        { 
            faseDashAbajo  =1;
            //posibleDashAbajo = true;
        }


        



        //if (_inputs.x >= 0.5f)
        //{ 
        //    spritePlayer.flipX = false;
        
        //}
        
        //if (_inputs.x <= -0.5f)
        //{ 
        //    spritePlayer.flipX = true;
        
        //}

    }

    

    private void Update()
    {
        
        if (faseDashDerecha == 1)
        {
            startTimePosibleDashDerecha += Time.deltaTime;
            
            if (startTimePosibleDashDerecha >= 1f)
            {
                //print("reset");
                faseDashDerecha = 0;

            }
            else if (startTimePosibleDashDerecha >= 0.5f && startTimePosibleDashDerecha < 1f)
            { 
                //print("dentro");
                if (countvecesDerecha == 1)
                { 
                    faseDashDerecha = 2;
                    startTimePosibleDashDerecha = 0;
            
                
                }

                
            }

        }

        if (faseDashDerecha == 2)
        {
            startTimePosibleDashDerecha += Time.deltaTime;
            if (startTimePosibleDashDerecha >= 1f)
            {
                faseDashDerecha = 0;

            }
            else if (startTimePosibleDashDerecha >= 0.5f && startTimePosibleDashDerecha < 1f)
            { 

                if (countvecesDerecha == 2)
                { 
                    faseDashDerecha = 3;

                }
            }

        }

        //if (posibleDashIzquierda == true)
        //{
        //    startTimePosibleDashIzquierda += Time.deltaTime;
        //    if (startTimePosibleDashIzquierda >= durationForLongPressSeconds)
        //    {
        //        posibleDashIzquierda = false;

        //    }

        //}

        //if (posibleDashArriba == true)
        //{
        //    startTimePosibleDashArriba += Time.deltaTime;
        //    if (startTimePosibleDashArriba >= durationForLongPressSeconds)
        //    {
        //        posibleDashArriba = false;

        //    }

        //}

        
        //if (posibleDashAbajo == true)
        //{
        //    startTimePosibleDashAbajo += Time.deltaTime;
        //    if (startTimePosibleDashAbajo >= durationForLongPressSeconds)
        //    {
        //        posibleDashAbajo = false;

        //    }

        //}

        


        if (resetDashDerecha == true)
        { 
            
            startTimeResetDashDerecha += Time.deltaTime;
            if (startTimeResetDashDerecha >= 5f)
            {
                countvecesDerecha = 0;
                isDashing = false;

            }
        
        }

        if (resetDashIzquierda == true)
        { 
            
            startTimeResetDashIzquierda += Time.deltaTime;
            if (startTimeResetDashIzquierda >= 5f)
            {
                countvecesIzquierda = 0;
                isDashing = false;
            }
        
        }

        if (resetDashArriba == true)
        { 
            
            startTimeResetDashArriba += Time.deltaTime;
            if (startTimeResetDashArriba >= 5f)
            {
                countvecesArriba = 0;
                isDashing = false;
            }
        
        }

        if (resetDashAbajo == true)
        { 
            
            startTimeResetDashAbajo += Time.deltaTime;
            if (startTimeResetDashAbajo >= 5f)
            {
                countvecesAbajo = 0;
                isDashing = false;
            }
        
        }


    }





    private void FixedUpdate()
    {
        
        if(gameController.playersSePuedenMover == false) return;
        if(player.playerSePuedeMover == true)
        { 
            rigid.MovePosition(rigid.position + _inputs * m_Speed * Time.fixedDeltaTime);
        
        }

        

        if (isDestroyer == true)
        { 
        
            progresoLerp = Mathf.PingPong(Time.time, interpolateDuration) / interpolateDuration;
            spritePlayer.color = Color.Lerp(colorInicial, colorDestino, progresoLerp);
        
        }


        if (caidoHueco == true)
        { 
            progresoLerp = Mathf.PingPong(Time.time, interpolateDuration) / interpolateDuration;
            spritePlayer.color = Color.Lerp(colorInicial, colorDestino, progresoLerp);
        
        
        }
        
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(gameController.playersSePuedenMover == false) return;
        if(player.playerSePuedeMover == false) return;

        if (collision.CompareTag("hueco") == true)
        { 

            if (gameController.isOnline == true)
            {
                punPlayer.photonview.RPC("ColisionConHueco", RpcTarget.OthersBuffered, false, true);

            }

            ProcesarHueco(collision);
            return;
        
        }
        
        ultimoBloquePisado = collision.transform;

        if (collision.CompareTag("fondo") == true)
        {

        
            ProcesarColisionConFondo(collision);
            return;

        }

        if (collision.CompareTag("powerup") == true)
        {

            if (gameController.isOnline == true)
            {
                punPlayer.photonview.RPC("ColisionConPowerup", RpcTarget.OthersBuffered, true);

            }

            ProcesarPowerUp(collision);
            return;

        }
        
        
    }

    public async void ProcesarColisionConFondo(Collider2D collision)
    {

        if (gameController is null == true)
        { 
            gameController = GameObject.FindObjectOfType<GameController>();
        
        } 

        switch(player.nombreColorPlayer)
        {
            case nombreColores.amarillo: gameController.bloquesAmarillos++; break;
            case nombreColores.azul: gameController.bloquesAzules++; break;
            case nombreColores.rojo: gameController.bloquesRojos++; break;
            case nombreColores.blanco: gameController.bloquesBlancos++; break;
            
        }

        

        if (gameController.isOnline == true)
        {
            
            //print("photonview" + punPlayer.photonView);
            punPlayer.photonview.RPC("ColisionConFondo", RpcTarget.OthersBuffered, 
            collision.gameObject.name,
            (byte)(player.colorPlayer.r * 255),
            (byte)(player.colorPlayer.g * 255),
            (byte)(player.colorPlayer.b * 255),
            gameController.bloquesAmarillos,
            gameController.bloquesAzules,
            gameController.bloquesRojos,
            gameController.bloquesBlancos
            );

        
        }
        else
        { 
            await UniTask.Delay(TimeSpan.FromMilliseconds(130));
        
        }

        collision.gameObject.GetComponent<SpriteRenderer>().color = player.colorPlayer;
        collision.gameObject.tag = player.nombreColorPlayer.ToString();

        
    
    }
        
    public void ProcesarPowerUp(Collider2D collision)
    {

        if (isDestroyer == false)
        { 
            isDestroyer = true;

            //fx

        }
       

        
    
    }

    public async void ProcesarHueco(Collider2D collision)
    { 

        player.playerSePuedeMover = false;
        caidoHueco = true;
        thistransform.position = ultimoBloquePisado.position;
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        caidoHueco = false;
        player.playerSePuedeMover = true;
        spritePlayer.color = player.colorPlayer;
    
    
    }


}
