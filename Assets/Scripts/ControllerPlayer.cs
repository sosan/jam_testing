using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InControlActions;
using UniRx.Async;
using System;

public class ControllerPlayer : MonoBehaviour
{

    public ControlActions inputActions;
    [SerializeField] public InfoPlayer player = new InfoPlayer();
    [SerializeField] public GameController gameController = null;
    [SerializeField] public Rigidbody2D rigid = null;
    [SerializeField] public Transform thistransform = null;
    [SerializeField] public LayerMask raycastLayerMask = -1;


    private bool isCompletedMoveLeftStick = false;
    private float m_MovementInputValue; 
    private Vector2 _inputs = Vector2.zero;
    public float m_Speed = 12f;

    private bool bombAwaiting = false;
    
    

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        thistransform = this.gameObject.transform;
        
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

        if(gameController.playerSePuedenMover == false) return;
        if (bombAwaiting == true) return;

        //raycast para saber si estamos encima de que estamos..
        var hit = Physics2D.OverlapBox(this.gameObject.transform.position, new Vector2(0, 0), 0, layerMask: raycastLayerMask);
        if (hit is null == false)
        { 
            if (hit.gameObject.tag == "fondo")
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


        float x = (float)Math.Round(this.transform.position.x * 2, MidpointRounding.ToEven) / 2;
        float y = (float)Math.Round(this.transform.position.y * 2, MidpointRounding.ToEven) / 2;

        //print("x=" + x + " y=" + y);
        if ((Math.Abs(x) % 1) == 0)
        {
            x += 0.5f;

        }

        if ((Math.Abs(y) % 1) == 0)
        {
            y += 0.5f;

        }

        GameObject bomba = GameObject.Instantiate(gameController.prefabBomba, 
            new Vector3(x, y, 0), 
            Quaternion.identity,
            gameController.canvasMenu[4].transform);

        bomba.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
        bomba.GetComponent<ControllerBomba>().color = player.colorPlayer;
        bomba.GetComponent<ControllerBomba>().cruz.color = player.colorPlayer;
        bomba.GetComponent<ControllerBomba>().controllerplayer = this;
        
        bombAwaiting = true;
        await UniTask.Delay(TimeSpan.FromSeconds(player.bombCooldown));
        bombAwaiting = false;

    }

    private void BotonSur(InputAction.CallbackContext obj)
    {

        if(gameController.playerSePuedenMover == false) return;

    }

    private void ResetLeftStick(InputAction.CallbackContext obj)
    {
        if(gameController.playerSePuedenMover == false) return;
       _inputs = Vector2.zero;
        
    }

    private void ControlLeftStick(InputAction.CallbackContext obj)
    {
        if(gameController.playerSePuedenMover == false) return;
        _inputs = obj.ReadValue<Vector2>();

    }

    



    private void FixedUpdate()
    {
        if(gameController.playerSePuedenMover == false) return;
        rigid.MovePosition(this.rigid.position + _inputs * m_Speed * Time.deltaTime);
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
        
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(gameController.playerSePuedenMover == false) return;
        ProcesarColision(collision);
        
        
    }

    public void ProcesarColision(Collider2D collision)
    { 
        if (collision.gameObject.tag == nombreColores.fondo.ToString())
        { 
            collision.gameObject.GetComponent<SpriteRenderer>().color = player.colorPlayer;
            switch(player.nombreColorPlayer)
            {
                case nombreColores.amarillo: gameController.bloquesAmarillos++; break;
                case nombreColores.azul: gameController.bloquesAzules++; break;
                case nombreColores.rojo: gameController.bloquesRojos++; break;
                case nombreColores.blanco: gameController.bloquesBlancos++; break;
            
            }
            //player.numerobloques++;
            collision.gameObject.tag = player.nombreColorPlayer.ToString();
        
        }

    
    
    }
        
    

}
