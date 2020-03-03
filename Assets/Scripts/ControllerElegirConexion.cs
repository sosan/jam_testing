using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControlActions;
using UnityEngine.InputSystem;
using System;

public class ControllerElegirConexion : MonoBehaviour
{

    public ControlActions inputActions;
    public LobbyClientPun lobby = null;
    [SerializeField] private Color selectColor = Color.white;
    [SerializeField] private Color notSelectColor = Color.white;
    [SerializeField] private TextMeshProUGUI[] textos = null;
    [SerializeField] public ParticleSystem[] particulas = null;
    [SerializeField] private Canvas canvas = null;

    [SerializeField] private RectTransform[] posiciones = null;
    [SerializeField] private RectTransform cuadro = null;

    private short posicionCuadro = 1;

    private void Awake()
    {


        canvas.gameObject.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {

       
        

    }


    public void Init()
    { 
        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += ControlDpad;
        inputActions.Menu.LeftStick.performed += ControlLeftStick;

        inputActions.Menu.Buttons.performed += BotonSur;

        
        inputActions.Menu.LeftStick.canceled += ResetLeftStick;


        inputActions.Enable();
        canvas.gameObject.SetActive(true);

    
    
    }

    private void ControlDpad(InputAction.CallbackContext obj)
    {
        ControlLeftStick(obj);

    }

    private void ResetLeftStick(InputAction.CallbackContext obj)
    {
        
    }

    private void BotonSur(InputAction.CallbackContext obj)
    {
        switch (posicionCuadro)
        {

            case 0: CrearPartida(obj.control.device.deviceId);  break;
            case 1: UnirsePartida(obj.control.device.deviceId); break;

        
        }
    }

    private void ControlLeftStick(InputAction.CallbackContext obj)
    {

        var input = obj.ReadValue<Vector2>();
        //print(input);

        

        if (input.x >= 0.5f)
        { 
            posicionCuadro++;
            HacerVibrarMando(obj.control.device.deviceId);
            if (posicionCuadro >= 1)
            { 
                posicionCuadro = 1;
                //isCompletado = true;
            
            }

        
        }
        else if (input.x <= -0.5f)
        { 
            posicionCuadro--;
            HacerVibrarMando(obj.control.device.deviceId);
            if (posicionCuadro <= 0)
            { 
                posicionCuadro = 0;
            
            }

        
        }

        ShowPositionMenuWithGamePad();

        



    }

    private void ShowPositionMenuWithGamePad()
    {


        //print("sss" + posicionCuadro);
        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectColor;

        }

        textos[posicionCuadro].color = selectColor;
        
        var tempPos = posiciones[posicionCuadro].anchoredPosition;
        cuadro.anchoredPosition = tempPos;

    }


    public async void CrearPartida(int deviceId)
    {

        
        HacerVibrarMando(deviceId);
        
        
        particulas[0].Play();
        await UniTask.Delay(300);
        inputActions.Disable();
        canvas.gameObject.SetActive(false);
        MusicController.MusicInstance.PlayInGameMusic( MusicController.MusicInstance.sounds[1]);
        lobby.CrearPartida();

    }

    public async void UnirsePartida(int deviceId)
    {


        HacerVibrarMando(deviceId);
        particulas[1].Play();
        await UniTask.Delay(300);
        inputActions.Disable();
        canvas.gameObject.SetActive(false);
        MusicController.MusicInstance.PlayInGameMusic( MusicController.MusicInstance.sounds[1]);
        lobby.ConectarsePartida();


    }

    public async void HacerVibrarMando(int deviceId)
    { 
        //vibrar el gamepad que ha pulsado el boton
        var todosgamepads = Gamepad.all;
        for (ushort i = 0; i < todosgamepads.Count; i++)
        { 
            if (todosgamepads[i].deviceId == deviceId)
            {
                todosgamepads[i].SetMotorSpeeds(0.5f, 0.5f);
                await UniTask.Delay(110);
                todosgamepads[i].SetMotorSpeeds(0f, 0f);


            }
        }
    
    
    }


}
