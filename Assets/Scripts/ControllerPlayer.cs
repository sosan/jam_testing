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

    private bool isCompletedMoveLeftStick = false;
    private float m_MovementInputValue; 
    private Vector2 _inputs = Vector2.zero;
    public float m_Speed = 12f; 
    

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        
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


    private void BotonOeste(InputAction.CallbackContext obj)
    {



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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        print("colision=" + collision.name);



    }

}
