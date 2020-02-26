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


    //en segundos
    [SerializeField] public float bombCooldown = 5; 
    [SerializeField] public float fireCooldown = 0;
    [SerializeField] public float durationShotSeconds = 0;
    [SerializeField] public float speedMovement = 0;
    [SerializeField] public float defense = 0;
    [SerializeField] public float defenseMax = 0;


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

        if (bombAwaiting == true) return;


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
        
        bombAwaiting = true;
        await UniTask.Delay(TimeSpan.FromSeconds(bombCooldown));
        bombAwaiting = false;

    }

    private void BotonSur(InputAction.CallbackContext obj)
    {



    }

    private void ResetLeftStick(InputAction.CallbackContext obj)
    {
       //isCompletedMoveLeftStick = false;
       _inputs = Vector2.zero;
        
    }

    private void ControlLeftStick(InputAction.CallbackContext obj)
    {

        _inputs = obj.ReadValue<Vector2>();
        //CalculoMovimientoPlayer(_inputs);

    }


    //private void CalculoMovimientoPlayer(Vector2 move)
    //{ 

    //    if (move.x <= -0.5f)
    //    { 
            
    //        print("izq");
    //        isCompletedMoveLeftStick = true;
    //        //_inputs = Vector2.zero;

    //        return;
        
    //    }
    
    //    if (move.x >= 0.5f)
    //    { 
    //        print("der");
    //        isCompletedMoveLeftStick = true;
    //        //_inputs = Vector2.zero;
    //        return;
        
    //    }

    //    if (move.y <= -0.5f)
    //    { 
        
    //        print("aba");
    //        isCompletedMoveLeftStick = true;
    //        return;
    //    }
    
    //    if (move.y >= 0.5f)
    //    { 
    //        print("arr");
    //        isCompletedMoveLeftStick = true;
    //        return;
        
    //    }


    
    //}

    private void FixedUpdate()
    {
        rigid.MovePosition(this.rigid.position + _inputs * m_Speed * Time.deltaTime);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    private void OnTriggerStay2D(Collider2D collision)
    {
        

        collision.gameObject.GetComponent<SpriteRenderer>().color = player.colorPlayer;


    }

}
