﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UniRx.Async;
using System.Threading;
using InControlActions;

public class ControllerGamepadMenu : MonoBehaviour
{

    public ControlActions inputActions;
    //[SerializeField] private Canvas[] canvasGeneral;
    //[SerializeField] private const ushort MAX_ = null;
    [SerializeField] private GameController gameController = null;
    [SerializeField] private GameObject[] canvasMenu = null;
    [SerializeField] private Animation camAnim = null;
    [SerializeField] private Animation spriteInicioAnim = null;
    [SerializeField] private ControllerMenuAnimations controllerMenuAnimations = null;
    [SerializeField] private TextMeshProUGUI[] textos = null;
    [SerializeField] private Color notSelectedColor = Color.white;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private ParticleSystem particleMenuSelected = null;
    [SerializeField] private RectTransform particleTransform = null;
    [SerializeField] private RectTransform[] positionTransformparticles = null;
    [SerializeField] private RectTransform bola_seleccion = null;

    [SerializeField] private ParticleSystem particleOptionsSelected = null;
    [SerializeField] private RectTransform particleTransformOptions = null;
    [SerializeField] private ParticleSystem particleCreditsSelected = null;
    [SerializeField] private ParticleSystem particleLogo = null;
    [SerializeField] private CanvasGroup canvasAlphaMenu = null;
    [SerializeField] private UILocalization descripcionMovimientoLocalization = null;
    //[SerializeField] private TextMeshProUGUI textMovement = null;
    [SerializeField] private GameObject mar = null;

    private bool isCompletedVertical = false;
    private bool isCompletedHorizontal = false;
    private short contMenuPosition = 0;


    //private string gamePadMovement = "<color=#bc008e>GAMEPAD</color> TO MOVE. <color=#bc008e>A</COLOR> TO FIRE. <color=#bc008e>B</COLOR> TO JUMP.";
    //private string keyboardMovement = "<color=#bc008e>AWSD</color> TO MOVE. <color=#bc008e>CONTROL</COLOR> TO FIRE. <color=#bc008e>SPACE</COLOR> TO JUMP.";


    private bool isMutedDefault = false;
    private ushort mainVolumenDefault = 100;
    private ushort mainSoundDefault = 40;
    private ushort mainSfxDefault = 60;

    private bool isMutedInternal = false;
    private ushort mainVolumenInternal = 100;
    private ushort mainSoundInternal = 40;
    private ushort mainSfxInternal = 60;

    private short contOptionsPosition = 0;

    private bool isBegun = false;
    
    
    //private List<string> nombreClips = new List<string>();

    [Header("Options Menu")]
    [SerializeField] private TextMeshProUGUI[] textosOptions = null;
    [SerializeField] private TextMeshProUGUI[] txtOptions = null;


    [Header("Credits")]
    [SerializeField] private TextMeshProUGUI textoCredits = null;


    //private CancellationTokenSource[] tokenSource = null;

    private void OnEnable()
    {

        inputActions.Enable();

    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();

        }

    }

    private async void Start()
    {



        MusicController.MusicInstance.SettingsMixer(
           isMutedInternal,
           mainVolumenInternal,
           mainSoundInternal,
           mainSfxInternal
       );


        MusicController.MusicInstance.PlayInGameMusic( MusicController.MusicInstance.sounds[0]);
        DisableCanvas();

        canvasAlphaMenu.alpha = 0;

        canvasMenu[0].SetActive(true);
        //particleLogo.Play();
        await UniTask.Delay(3000);


        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectedColor;
            

        }

        canvasMenu[0].SetActive(false);

        //textos[contMenuPosition].color = selectedColor;
        canvasMenu[1].SetActive(true);
        //mar.SetActive(true);

        controllerMenuAnimations.animations.Play("MenuActivate");
        await UniTask.Delay(TimeSpan.FromMilliseconds( 1000 ));

        controllerMenuAnimations.animations.Play("Titulo");
    }



    private void Awake()
    {

        //mar.SetActive(false);
        //Getting values
        GettingPlayerPrefsValues();

       
        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += MenuDPADHandle;

        inputActions.Menu.Buttons.performed += MenuButtonPressedGamepad;
        inputActions.Menu.ExitButton.performed += ButtonExit;

        inputActions.Menu.LeftStick.performed += MenuStickHandle;
        inputActions.Menu.LeftStick.canceled += MenuResetStickMove;

        inputActions.Menu.RightStick.performed += MenuStickHandle;
        inputActions.Menu.RightStick.canceled += MenuResetStickMove;

        inputActions.Menu.MovementMouse.performed += MovementMouse;





    }

    private void GettingPlayerPrefsValues()
    {

        if (PlayerPrefs.HasKey("isMutedInternal") == true)
        {

            int t = PlayerPrefs.GetInt("isMutedInternal");


            if (t == 0)
            {

                isMutedInternal = false;
            }
            else if (t == 1)
            {

                isMutedInternal = true;

            }



        }
        else
        {

            if (isMutedDefault == true)
            {

                PlayerPrefs.SetInt("isMutedInternal", 1);

            }
            else
            {

                PlayerPrefs.SetInt("isMutedInternal", 0);

            }


        }

        if (PlayerPrefs.HasKey("mainVolumenInternal") == true)
        {

            mainVolumenInternal = (ushort)PlayerPrefs.GetInt("mainVolumenInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainVolumenInternal", mainVolumenDefault);
        }


        if (PlayerPrefs.HasKey("mainSoundInternal") == true)
        {

            mainSoundInternal = (ushort)PlayerPrefs.GetInt("mainSoundInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainSoundInternal", mainSoundDefault);
        }



        if (PlayerPrefs.HasKey("mainSfxInternal") == true)
        {

            mainSfxInternal = (ushort)PlayerPrefs.GetInt("mainSfxInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainSfxInternal", mainSfxDefault);
        }



    }


    private void SavePlayerPrefsValues()
    {

        if (isMutedInternal == true)
        {

            PlayerPrefs.SetInt("isMutedInternal", 1);

        }
        else
        {

            PlayerPrefs.SetInt("isMutedInternal", 0);

        }

        PlayerPrefs.SetInt("mainVolumenInternal", mainVolumenInternal);
        PlayerPrefs.SetInt("mainSfxInternal", mainSfxInternal);
        PlayerPrefs.SetInt("mainVolumenInternal", mainVolumenInternal);

    }

    private void MenuButtonPressedGamepad(InputAction.CallbackContext obj)
    {

          

        if (canvasMenu[1].activeSelf == true)
        {



            switch (contMenuPosition)
            {

                case 0: ShowInitGameGamepad(); break;
                case 1: ShowOptionsGamepad();  break;
                case 2: ShowExitGamepad(); break;
                default: return;
            }


         
            return;

        }

        //options
        if (canvasMenu[2].activeSelf == true)
        {

            switch (contOptionsPosition)
            {

                
                case 4: ResetDefaultValuesGamepad(); break;
                case 5: BotonExitOptionsGamepad(); break;
                default: return;


            }

         


            return;
        }


        //credits
        if (canvasMenu[3].activeSelf == true)
        {

            BotonExitCreditsGamepad();
            return;
        }




    }


    private void MovementMouse(InputAction.CallbackContext obj)
    {

       
        descripcionMovimientoLocalization.key = "movimientodescripcion_mouse";
        

    }

    private void ButtonExit(InputAction.CallbackContext obj)
    {

        //options
        if (canvasMenu[2].activeSelf == true)
        {

            BotonExitOptionsGamepad();
        }


        //credits
        if (canvasMenu[3].activeSelf == true)
        {

            BotonExitCreditsGamepad();


        }


    }

    private void MenuResetStickMove(InputAction.CallbackContext obj)
    {

        isCompletedVertical = false;
        isCompletedHorizontal = false;
    }

  

    private void MenuDPADHandle(InputAction.CallbackContext obj)
    {


        //Menu
        if (canvasMenu[1].activeSelf == true)
        {
            descripcionMovimientoLocalization.key = "movimientodescripcion_gamepad";
            var move = obj.ReadValue<Vector2>();
            ControlMainMenu(move);
            return;

        }

        //Options
        if (canvasMenu[2].activeSelf == true)
        {
            var move = obj.ReadValue<Vector2>();
            ControlOptions(move);
            return;
        }






    }


    private void ControlMainMenu(Vector2 move)
    {

        if (move.y <= -0.5f)
        {

            contMenuPosition++;
            if (contMenuPosition > textos.Length - 1)
            {

                contMenuPosition = 0;
                isCompletedVertical = true;

            }


        }
        else if (move.y >= 0.5f)
        {

            contMenuPosition--;

            if (contMenuPosition < 0)
            {
                
                contMenuPosition = (short)(textos.Length - 1);
                isCompletedVertical = true;
            }


        }

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[0]
            );


        ShowPositionMenuWithGamePad();

    }

    

    private void ControlOptions(Vector2 move)
    {


        if (move.y <= -0.5f)
        {

            contOptionsPosition++;
            if (contOptionsPosition > 5)
            {

                contOptionsPosition = 0;
            }


        }
        else if (move.y >= 0.5f)
        {

            contOptionsPosition--;

            if (contOptionsPosition < 0)
            {

                contOptionsPosition = 5;
            }


        }


        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[0]
            );

        switch (contOptionsPosition)
        {


            case 0: ControlMute(move); break;
            case 1: ControlMainVolumen(move); break;
            case 2: ControlSoundVolumen(move); break;
            case 3: ControlSfxVolumen(move); break;
            //case 4: break;


        }


        ShowPositionOptions();




    }


    private void ControlMute(Vector2 move)
    {


        if (move.x <= -0.5f)
        {
            isMutedInternal = !isMutedInternal;
            isCompletedHorizontal = true;
            //
            MusicController.MusicInstance.PlayFXSound(
                MusicController.MusicInstance.sfx[1]
            );

            if (isMutedInternal == true)
            {
                textosOptions[0].text = "ON";
            }
            else
            {
                textosOptions[0].text = "OFF";
            }

                MusicController.MusicInstance.Mute();


          


        }
        else if (move.x > 0.5f)
        {

            isMutedInternal = !isMutedInternal;
            isCompletedHorizontal = true;

            MusicController.MusicInstance.PlayFXSound(
                MusicController.MusicInstance.sfx[1]
            );

            MusicController.MusicInstance.Mute();
            if (isMutedInternal == true)
            {
                textosOptions[0].text = "ON";
            }
            else
            {
                textosOptions[0].text = "OFF";
            }


        }


    }


    private void ControlMainVolumen(Vector2 move)
    {



        if (move.x <= -0.5f)
        {

            isCompletedHorizontal = true;
            MainVolumenMenos(true);

        }
        else if (move.x > 0.5f)
        {
            isCompletedHorizontal = true;
            MainVolumenMas(true);
        }




    }

    private void ControlSoundVolumen(Vector2 move)
    {




        if (move.x <= -0.5f)
        {

            isCompletedHorizontal = true;
            SoundVolumenMenos(true);

        }
        else if (move.x > 0.5f)
        {
            isCompletedHorizontal = true;
            SoundVolumenMas(true);
        }




    }



    private void ControlSfxVolumen(Vector2 move)
    {




        if (move.x <= -0.5f)
        {

            isCompletedHorizontal = true;
            SfxVolumenMenos(true);

        }
        else if (move.x > 0.5f)
        {
            isCompletedHorizontal = true;
            SfxVolumenMas(true);
        }




    }


    private void ShowPositionMenuWithGamePad()
    {


        var nombreclip = controllerMenuAnimations.animationClips[contMenuPosition].name;
        controllerMenuAnimations.animations.Play(nombreclip);


        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectedColor;

        }

        textos[contMenuPosition].color = selectedColor;
        
        var tempPos = positionTransformparticles[contMenuPosition].anchoredPosition;
        tempPos.x = bola_seleccion.anchoredPosition.x;
        bola_seleccion.anchoredPosition = tempPos;


        


    }




    private void ShowPositionOptions()
    {


        for (ushort i = 0; i < textosOptions.Length; i++)
        {

            textosOptions[i].color = notSelectedColor;
            txtOptions[i].color = notSelectedColor;

        }

        textosOptions[contOptionsPosition].color = selectedColor;
        txtOptions[contOptionsPosition].color = selectedColor;
    }



    private void DeColorOptions(ushort index)
    {


        for (ushort i = 0; i < textosOptions.Length; i++)
        {

            textosOptions[i].color = notSelectedColor;
            txtOptions[i].color = notSelectedColor;

        }


        textosOptions[index].color = selectedColor;
        txtOptions[index].color = selectedColor;

        //await UniTask.Delay(150);
        //textosOptions[index].color = notSelectedColor;
        //txtOptions[index].color = notSelectedColor;

        //await UniTask.Delay(200);

        //textosOptions[contOptionsPosition].color = selectedColor;
        //txtOptions[contOptionsPosition].color = selectedColor;

    }

    private async void DeColorOptionsReset(ushort index)
    {


        for (ushort i = 0; i < textosOptions.Length; i++)
        {

            textosOptions[i].color = notSelectedColor;
            txtOptions[i].color = notSelectedColor;

        }


        textosOptions[index].color = selectedColor;
        txtOptions[index].color = selectedColor;

        await UniTask.Delay(150);
        textosOptions[index].color = notSelectedColor;
        txtOptions[index].color = notSelectedColor;

       

    }


    private void MenuStickHandle(InputAction.CallbackContext obj)
    {

        if (isCompletedVertical == true) return;


        //Menu
        if (canvasMenu[1].activeSelf == true)
        {
            descripcionMovimientoLocalization.key = "movimientodescripcion_gamepad";
            var mov = obj.ReadValue<Vector2>();
            ControlMenuStick(mov);
            return;

        }

        //Options
        if (canvasMenu[2].activeSelf == true)
        {

            var mov = obj.ReadValue<Vector2>();
            ControlOptionStick(mov);
            return;
        }




       

    }


    private void ControlMenuStick(Vector2 move)
    {


        

        if (move.y <= -0.5f)
        {
            isCompletedVertical = true;
            MusicController.MusicInstance.PlayFXSound(MusicController.MusicInstance.sfx[0]);

            contMenuPosition++;
            if (contMenuPosition > textos.Length - 1)
            {

                contMenuPosition = 0;

            }


        }
        else if (move.y >= 0.5f)
        {
            isCompletedVertical = true;
            MusicController.MusicInstance.PlayFXSound(MusicController.MusicInstance.sfx[0]);
            contMenuPosition--;

            if (contMenuPosition < 0)
            {

                contMenuPosition = (short)(textos.Length - 1);

            }


        }

        ShowPositionMenuWithGamePad();


    }


    private void ControlOptionStick(Vector2 move)
    {

        



        if (move.y <= -0.5f)
        {

            contOptionsPosition++;
            isCompletedVertical = true;
            MusicController.MusicInstance.PlayFXSound(MusicController.MusicInstance.sfx[0]);
            if (contOptionsPosition > 5)
            {

                contOptionsPosition = 0;
            }


        }
        else if (move.y >= 0.5f)
        {

            contOptionsPosition--;
            isCompletedVertical = true;
            MusicController.MusicInstance.PlayFXSound(MusicController.MusicInstance.sfx[0]);

            if (contOptionsPosition < 0)
            {

                contOptionsPosition = 5;
            }


        }


        if (isCompletedHorizontal == false
            && (move.x <= -0.5f || move.x > 0.5f))
        {
            MusicController.MusicInstance.PlayFXSound(MusicController.MusicInstance.sfx[1]);

            switch (contOptionsPosition)
            {


                case 0: ControlMute(move); break;
                case 1: ControlMainVolumen(move); break;
                case 2:  ControlSoundVolumen(move); break;
                case 3: ControlSfxVolumen(move); break;
                default: return;


            }


        }


        ShowPositionOptions();




    }


    
    public void EnterHoverEnterGame()
    {
        if (isBegun == true) return;

        textos[0].color = selectedColor;

    }


    public void ExitHoverEnterGame()
    {

        if (isBegun == true) return;
        textos[0].color = notSelectedColor;

    }



    public void EnterHoverOptions()
    {

        if (isBegun == true) return;
        textos[1].color = selectedColor;

    }


    public void ExitHoverOptions()
    {
        if (isBegun == true) return;
        textos[1].color = notSelectedColor;

    }


    public void EnterHoverCredits()
    {
        if (isBegun == true) return;
        textos[2].color = selectedColor;

    }


    public void ExitHoverCredits()
    {
        if (isBegun == true) return;
        textos[2].color = notSelectedColor;

    }

    public void EnterHoverExit()
    {
        if (isBegun == true) return;
        textos[2].color = selectedColor;

    }


    public void ExitHoverExit()
    {
        if (isBegun == true) return;
        textos[2].color = notSelectedColor;

    }


    private async void ShowPositionMenu()
    {
        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectedColor;

        }

        textos[contMenuPosition].color = selectedColor;

        await UniTask.Delay(150);
        textos[contMenuPosition].color = notSelectedColor;


    }

    private async void ShowPositionMenu(ushort index)
    {
        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectedColor;

        }

        textos[index].color = selectedColor;

        await UniTask.Delay(150);
        textos[index].color = notSelectedColor;


    }

    //private void ShowFX(float x, float y)
    //{

    //    //delay para que se escuche el fx y pequeña transicion
    //    particleTransform.anchoredPosition = new Vector2(x, y);
    //    particleMenuSelected.Play();


    //}

    private void ShowFX(Vector2 position)
    {
        particleTransform.anchoredPosition = position;
        particleMenuSelected.Play();

    }

    private void ShowFXOptions(float x, float y)
    {

        particleTransformOptions.anchoredPosition = new Vector2(x, y);
        particleOptionsSelected.Play();



    }


    private async void ShowInitGameGamepad()
    {



        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
        );

        descripcionMovimientoLocalization.key = "movimientodescripcion_gamepad";
        //ShowFX(-39, -32);
        ShowFX(positionTransformparticles[0].anchoredPosition);
        controllerMenuAnimations.DesactiveMenu();

        await UniTask.Delay(5000);

        DisableCanvas();
        print("Show initgame");


    }

 


    public async void ShowOptionsGamepad()
    {

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        //ShowFX(-5, -67);
        ShowFX(positionTransformparticles[1].anchoredPosition);
        ShowPositionMenu(1);
        await UniTask.Delay(500);

        DisableCanvas();

        if (isMutedInternal == true)
        {
            textosOptions[0].text = "ON";
        }
        else
        {

            textosOptions[0].text = "OFF";


        }

        textosOptions[1].text = mainVolumenInternal.ToString();
        textosOptions[2].text = mainSoundInternal.ToString();
        textosOptions[3].text = mainSfxInternal.ToString();


        contOptionsPosition = 0;
        ShowPositionOptions();

        canvasMenu[2].SetActive(true);


    }


 
    //private async void ShowCreditsGamepad()
    //{
    //    MusicController.MusicInstance.PlayFXSound(
    //        MusicController.MusicInstance.sfx[1]
    //        );


    //    ShowPositionMenu(2);
    //    ShowFX(-5, -109);
    //    await UniTask.Delay(290);


    //    DisableCanvas();
    //    textoCredits.color = selectedColor;
    //    canvasMenu[3].SetActive(true);

    //}


  




    private async void ShowExitGamepad()
    {
        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        ShowPositionMenu(3);
        //ShowFX(-5, -144);
        ShowFX(positionTransformparticles[2].anchoredPosition);
        await UniTask.Delay(400);


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        Application.Quit();
#endif

    }


    //private void ShowInitGame()
    //{


    //    MusicController.MusicInstance.PlayFXSound(
    //        MusicController.MusicInstance.sfx[1]
    //        );



    //    textMovement.text = gamePadMovement;
    //    InitGame();



    //}

    public async void ShowOptions()
    {

        if (isBegun == true) return;

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        ShowFX(positionTransformparticles[1].anchoredPosition);
        ShowPositionMenu(1);
        await UniTask.Delay(500);

        DisableCanvas();

        if (isMutedInternal == true)
        {
            textosOptions[0].text = "ON";
        }
        else
        {

            textosOptions[0].text = "OFF";


        }

        textosOptions[1].text = mainVolumenInternal.ToString();
        textosOptions[2].text = mainSoundInternal.ToString();
        textosOptions[3].text = mainSfxInternal.ToString();

        contOptionsPosition = 0;
        ShowPositionOptions();


        canvasMenu[2].SetActive(true);
    }



//    public async void ShowCredits()
//    {
//        if (isBegun == true) return;
//#if UNITY_EDITOR
//        print("credits");
//#endif
//        MusicController.MusicInstance.PlayFXSound(
//            MusicController.MusicInstance.sfx[1]
//            );

//        ShowPositionMenu(2);
//        ShowFX(-5, -84);
//        await UniTask.Delay(290);


//        DisableCanvas();

//        canvasMenu[3].SetActive(true);




//    }



    public async void ShowExit()
    {

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        ShowPositionMenu(3);
        //ShowFX(-34, -126);
        ShowFX(positionTransformparticles[2].anchoredPosition);
        await UniTask.Delay(400);


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        Application.Quit();
#endif

    }

    private void DisableCanvas()
    {
        //mar.SetActive(false);

        for (ushort i = 0; i < canvasMenu.Length; i++)
        {

            canvasMenu[i].SetActive(false);

        }


    }

    public void ClickedEnterGame()
    {

        if (isBegun == true) return;

        //print("clicked");
        isBegun = true;
        ShowPositionMenu(0);
        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        descripcionMovimientoLocalization.key = "movimientodescripcion_mouse";
        ShowFX(positionTransformparticles[0].anchoredPosition);
        InitGame();







    }


    private async void InitGame()
    {

        

        float duration = controllerMenuAnimations.DesactiveMenu();

        await UniTask.Delay(TimeSpan.FromMilliseconds(duration * 1000));

        DisableCanvas();



        //print("Show initgame");

        canvasMenu[3].SetActive(true);

        spriteInicioAnim.Play("spriteprincipio");

        camAnim.Play("cameraanim");


        var t = camAnim.GetClip("cameraanim").length * 1000;
        await UniTask.Delay(TimeSpan.FromMilliseconds(t));

        gameController.InitGame();

    }





    //click events
    public void MuteOff()
    {

        isMutedInternal = !isMutedInternal;

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
        );

            MusicController.MusicInstance.Mute();
        if (isMutedInternal == true)
        {
            textosOptions[0].text = "ON";
        }
        else
        {
            textosOptions[0].text = "OFF";
        }



        DeColorOptions(0);

        contOptionsPosition = 0;






    }

    public void MuteOn()
    {

        isMutedInternal = !isMutedInternal;

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

            MusicController.MusicInstance.Mute();
        if (isMutedInternal == true)
        {
            textosOptions[0].text = "ON";
        }
        else
        {
            textosOptions[0].text = "OFF";
        }


        


        DeColorOptions(0);
        contOptionsPosition = 0;

    }


   

    public void MainVolumenMenos(bool gamepad)
    {

        if (mainVolumenInternal > 0)
        {

            mainVolumenInternal -= 10;

            if (mainVolumenInternal <= 0)
            {

                mainVolumenInternal = 0;
            }

        }

        MusicController.MusicInstance.SetVolumenMaster(mainVolumenInternal);

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        textosOptions[1].text = mainVolumenInternal.ToString();
        contOptionsPosition = 1;
        if (gamepad == false)
        {

            DeColorOptions(1);

        }



    }

    public void MainVolumenMas(bool gamepad)
    {


        if (mainVolumenInternal < 100)
        {

            mainVolumenInternal += 10;

            if (mainVolumenInternal > 100)
            {

                mainVolumenInternal = 0;
            }

        }

        textosOptions[1].text = mainVolumenInternal.ToString();

        MusicController.MusicInstance.SetVolumenMaster(mainVolumenInternal);
        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        contOptionsPosition = 1;
        if (gamepad == false)
        {
            DeColorOptions(1);


        }
    }



    public void SoundVolumenMenos(bool gamepad)
    {

        if (mainSoundInternal > 0)
        {

            mainSoundInternal -= 10;

            if (mainSoundInternal <= 0)
            {

                mainSoundInternal = 0;
            }

        }

        MusicController.MusicInstance.SetVolumenSounds(mainSoundInternal);

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        textosOptions[2].text = mainSoundInternal.ToString();
        contOptionsPosition = 2;
        if (gamepad == false)
        {

            DeColorOptions(2);

        }


    }


    public void SoundVolumenMas(bool gamepad)
    {


        if (mainSoundInternal < 100)
        {

            mainSoundInternal += 10;

            if (mainSoundInternal > 100)
            {

                mainSoundInternal = 0;
            }

        }

        MusicController.MusicInstance.SetVolumenSounds(mainSoundInternal);
        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        textosOptions[2].text = mainSoundInternal.ToString();
        contOptionsPosition = 2;
        if (gamepad == false)
        {

            DeColorOptions(2);

        }




    }
    public void SfxVolumenMenos(bool gamepad)
    {


        if (mainSfxInternal > 0)
        {

            mainSfxInternal -= 10;

            if (mainSfxInternal <= 0)
            {

                mainSfxInternal = 0;
            }

        }

        MusicController.MusicInstance.SetVolumenSfx(mainSfxInternal);

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        textosOptions[3].text = mainSfxInternal.ToString();
        contOptionsPosition = 3;
        if (gamepad == false)
        {

            DeColorOptions(3);

        }



    }



    public void SfxVolumenMas(bool gamepad)
    {

        if (mainSfxInternal < 100)
        {

            mainSfxInternal += 10;

            if (mainSfxInternal > 100)
            {

                mainSfxInternal = 0;
            }

        }

        MusicController.MusicInstance.SetVolumenSfx(mainSfxInternal);

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        textosOptions[3].text = mainSfxInternal.ToString();
        contOptionsPosition = 3;
        if (gamepad == false)
        {
            DeColorOptions(3);


        }

        

    }


    private async void ResetDefaultValuesGamepad()
    {


        isMutedInternal = isMutedDefault;
        mainVolumenInternal = mainVolumenDefault;
        mainSoundInternal = mainSoundDefault;
        mainSfxInternal = mainSfxDefault;

        if (isMutedInternal == true)
        {
            textosOptions[0].text = "ON";
        }
        else
        {

            textosOptions[0].text = "OFF";
        }



        MusicController.MusicInstance.PlayFXSound(
                   MusicController.MusicInstance.sfx[1]
               );

        MusicController.MusicInstance.MuteOff();

        MusicController.MusicInstance.SetVolumenMaster(mainVolumenInternal);
        MusicController.MusicInstance.SetVolumenSounds(mainSoundInternal);
        MusicController.MusicInstance.SetVolumenSfx(mainSfxInternal);

        textosOptions[1].text = mainVolumenInternal.ToString();
        textosOptions[2].text = mainSoundInternal.ToString();
        textosOptions[3].text = mainSfxInternal.ToString();

        ShowFXOptions(0, -130);

        DeColorOptionsReset(4);
        contOptionsPosition = 0;
        await UniTask.Delay(200);
        textosOptions[0].color = selectedColor;
        txtOptions[0].color = selectedColor;





    }

    public async void ResetDefaultValues()
    {


        isMutedInternal = isMutedDefault;
        mainVolumenInternal = mainVolumenDefault;
        mainSoundInternal = mainSoundDefault;
        mainSfxInternal = mainSfxDefault;

        if (isMutedInternal == true)
        {
            textosOptions[0].text = "ON";
        }
        else
        {

            textosOptions[0].text = "OFF";
        }

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        
        MusicController.MusicInstance.MuteOff();

        MusicController.MusicInstance.SetVolumenMaster(mainVolumenInternal);
        MusicController.MusicInstance.SetVolumenSounds(mainSoundInternal);
        MusicController.MusicInstance.SetVolumenSfx(mainSfxInternal);

        textosOptions[1].text = mainVolumenInternal.ToString();
        textosOptions[2].text = mainSoundInternal.ToString();
        textosOptions[3].text = mainSfxInternal.ToString();

        ShowFXOptions(0, -130);

        DeColorOptionsReset(4);
        contOptionsPosition = 0;
        await UniTask.Delay(200);
        textosOptions[0].color = selectedColor;
        txtOptions[0].color = selectedColor;



    }

    public async void BotonExitOptions()
    {

        SavePlayerPrefsValues();

        ShowFXOptions(0, -184);

        DeColorOptionsReset(5);

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        await UniTask.Delay(400);
        DisableCanvas();

        contMenuPosition = 0;
        contOptionsPosition = 0;
        //ShowPositionMenu();
        ShowPositionMenuWithGamePad();

        descripcionMovimientoLocalization.key = "movimientodescripcion_mouse";
        canvasMenu[1].SetActive(true);
        //mar.SetActive(true);

    }

    public async void BotonExitOptionsGamepad()
    {

        MusicController.MusicInstance.PlayFXSound(
                   MusicController.MusicInstance.sfx[1]
               );
        SavePlayerPrefsValues();
        ShowFXOptions(0, -184);


        DeColorOptionsReset(5);


        await UniTask.Delay(400);
        DisableCanvas();

        contMenuPosition = 0;
        contOptionsPosition = 0;
        ShowPositionMenuWithGamePad();
        descripcionMovimientoLocalization.key = "movimientodescripcion_gamepad";
        canvasMenu[1].SetActive(true);
        //mar.SetActive(true);

    }




    public async void BotonExitCredits()
    {

        
        //DeColorOptions(5);
        particleCreditsSelected.Play();

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        textoCredits.color = selectedColor;
        await UniTask.Delay(150);
        textoCredits.color = notSelectedColor;

        await UniTask.Delay(200);



        contMenuPosition = 0;
        contOptionsPosition = 0;
        DisableCanvas();


        canvasMenu[1].SetActive(true); 
        //mar.SetActive(true);
        ShowPositionMenuWithGamePad();



    }


    private async void BotonExitCreditsGamepad()
    {


        //DeColorOptions(5); //.... no tendria que estar
        particleCreditsSelected.Play();

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        textoCredits.color = selectedColor;
        await UniTask.Delay(150);
        textoCredits.color = notSelectedColor;

        await UniTask.Delay(200);

        contMenuPosition = 0;
        contOptionsPosition = 0;
        DisableCanvas();


        ShowPositionMenuWithGamePad();
        canvasMenu[1].SetActive(true);
        ///*mar.*/SetActive(true);



    }


    public void OpenDiscord()
    {
        if (isBegun == true) return;

        Application.OpenURL("https://discordapp.com/invite/cnURfjc");
        


    }

    public void OpenItchio()
    {
        if (isBegun == true) return;
        Application.OpenURL("https://joselu.itch.io/juegoheynaujam");

    }


    public void ResetPlayerPrefs()
    {

        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        Application.Quit();
#endif


    }


}