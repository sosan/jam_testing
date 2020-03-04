using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControlActions;
using UnityEngine.InputSystem;
using System;
using UniRx.Async;
using UnityEngine.UI;

public class ControllerExplicacion : MonoBehaviour
{

    public ControlActions inputActions;
    [SerializeField] private ControllerGamepadMenu controllerGamepadMenu = null;
    [SerializeField] private GameController gameController = null;
    [SerializeField] private GameObject[] fases = null;

    [SerializeField] private Animation animacion = null;
    [SerializeField] private AnimationClip[] listadoclips = null;
    [SerializeField] private Image imagenfondo = null;
    [SerializeField] private Sprite imagenSpa = null;
    [SerializeField] private Sprite imagenEn = null;


    private ushort contadorFase = 0;

    private void Awake()
    {

        inputActions = new ControlActions();
        inputActions.Menu.Buttons.performed += BotonSurExplicacion;

        inputActions.Menu.ExitButton.performed += BotonRepetirExplicacion;
       
        string t = PlayerPrefs.GetString("idioma");

        switch (t)
        {

            case "espanol": imagenfondo.sprite = imagenSpa; break;
            case "english": imagenfondo.sprite = imagenEn; break;
            default: print("not set"); imagenfondo.sprite = imagenSpa; return;

        }
        


    }

    private void BotonRepetirExplicacion(InputAction.CallbackContext obj)
    {

        gameController.HacerVibrarMando(obj.control.device.deviceId);
        contadorFase = 0;
        Init();

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    { 

        for(ushort i= 0; i< fases.Length; i++)
        { 
            fases[i].SetActive(false);
        
        }

        inputActions.Enable();

        gameController.canvasMenu[7].SetActive(true);

        ProcesarSiguienteBoton();   
    
    }

    private void ProcesarSiguienteBoton()
    { 
    
        //fases[contadorFase].SetActive(false);
        //print("contadofase" + contadorFase);

        

        if (contadorFase == 5)
        { 
            
            inputActions.Disable();
            gameController.canvasMenu[7].SetActive(false);
            controllerGamepadMenu.ShowPantallaPersonajes();


        }
        else
        { 
            animacion.Play(listadoclips[contadorFase].name);    
            //fases[contadorFase].SetActive(true);
        }

        contadorFase++;
    
    }

    private void BotonSurExplicacion(InputAction.CallbackContext obj)
    {
        gameController.HacerVibrarMando(obj.control.device.deviceId);
        ProcesarSiguienteBoton();
    }

   

}
