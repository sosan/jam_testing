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

public class ControlIdiomas : MonoBehaviour
{

    public ControlActions inputActions;
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
        if (PlayerPrefs.HasKey("idioma") == true)
        {


            string t = PlayerPrefs.GetString("idioma");

            switch (t)
            {

                case "espanol": Localization.language = t; break;
                case "english": Localization.language = t; break;
                default: Debug.LogError("NOT SET IDIOMA"); return;

            }

            SceneManager.LoadSceneAsync("Main");



        }
        else
        {

            canvas.gameObject.SetActive(true);

        }
       



    }

    // Start is called before the first frame update
    void Start()
    {

        //DontDestroyOnLoad(this.gameObject);

        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += ControlDpad;
        inputActions.Menu.LeftStick.performed += ControlLeftStick;

        inputActions.Menu.Buttons.performed += BotonSur;

        
        inputActions.Menu.LeftStick.canceled += ResetLeftStick;


        inputActions.Enable();
        

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

            case 0: SetSpanishLanguage(obj.control.device.deviceId);  break;
            case 1: SetEnglishLanguage(obj.control.device.deviceId); break;

        
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

        //print(contMenuPosition);
        //var nombreclip = controllerMenuAnimations.animationClips[contMenuPosition].name;
        //controllerMenuAnimations.animations.Play(nombreclip);

        //print("sss" + posicionCuadro);
        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectColor;

        }

        textos[posicionCuadro].color = selectColor;
        
        var tempPos = posiciones[posicionCuadro].anchoredPosition;
        cuadro.anchoredPosition = tempPos;

    }


    public async void SetSpanishLanguage(int deviceId)
    {

        
        HacerVibrarMando(deviceId);
       
        Localization.language = "espanol";

        PlayerPrefs.SetString("idioma", "espanol");
        particulas[0].Play();
        await UniTask.Delay(300);
        inputActions.Disable();
        canvas.gameObject.SetActive(false);
        _ = SceneManager.LoadSceneAsync("Main");

    }

    public async void SetEnglishLanguage(int deviceId)
    {


        HacerVibrarMando(deviceId);
        Localization.language = "english";
        PlayerPrefs.SetString("idioma", "english");
        particulas[1].Play();
        await UniTask.Delay(300);
        inputActions.Disable();
        canvas.gameObject.SetActive(false);
        _ = SceneManager.LoadSceneAsync("Main");

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
