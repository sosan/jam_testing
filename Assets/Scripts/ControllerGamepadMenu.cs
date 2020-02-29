using System;
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
    [SerializeField] private ControllerElegirPersonaje elegirPersonaje = null;
    
    //[SerializeField] private Animation camAnim = null;
    //[SerializeField] private Animation spriteInicioAnim = null;
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
    //[SerializeField] private ParticleSystem particleCreditsSelected = null;
    [SerializeField] private ParticleSystem particleLogo = null;
    [SerializeField] private CanvasGroup canvasAlphaMenu = null;
    [SerializeField] private TextMeshProUGUI descripcion = null;
    //[SerializeField] private TextMeshProUGUI textMovement = null;
    //[SerializeField] private GameObject mar = null;

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


    //[Header("Credits")]
    //[SerializeField] private TextMeshProUGUI textoCredits = null;


    //private CancellationTokenSource[] tokenSource = null;

    private void OnEnable()
    {

       

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

        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        MusicController.MusicInstance.SettingsMixer(
           isMutedInternal,
           mainVolumenInternal,
           mainSoundInternal,
           mainSfxInternal
       );


        MusicController.MusicInstance.PlayInGameMusic( MusicController.MusicInstance.sounds[0]);
        DisableCanvas();

        canvasAlphaMenu.alpha = 0;

        gameController.canvasMenu[0].SetActive(true);
        //particleLogo.Play();
        await UniTask.Delay(3000);


        for (ushort i = 0; i < textos.Length; i++)
        {

            textos[i].color = notSelectedColor;
            

        }

        gameController.canvasMenu[0].SetActive(false);

        //textos[contMenuPosition].color = selectedColor;
        gameController.canvasMenu[1].SetActive(true);
        //mar.SetActive(true);

        controllerMenuAnimations.animations.Play("MenuActivate");
        await UniTask.Delay(TimeSpan.FromMilliseconds( 1000 ));

        controllerMenuAnimations.animations.Play("Titulo");

        //ShowPositionMenu(0);
        contMenuPosition = 0;

        ShowPositionMenuWithGamePad();
        inputActions.Enable();

    }



    private void Awake()
    {

        GettingPlayerPrefsValues();

       
        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += MenuDPADHandle;

        inputActions.Menu.Buttons.performed += MenuButtonPressedGamepad;
        inputActions.Menu.ExitButton.performed += ButtonExit;

        inputActions.Menu.LeftStick.performed += MenuStickHandle;
        inputActions.Menu.LeftStick.canceled += MenuResetStickMove;

        inputActions.Menu.RightStick.performed += MenuStickHandle;
        inputActions.Menu.RightStick.canceled += MenuResetStickMove;

        //inputActions.Menu.MovementMouse.performed += MovementMouse;

        descripcion.text = Localization.Get("movimientodescripcion_gamepad");



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
        if (isBegun == true) return;
        
        if (gameController.canvasMenu[1].activeSelf == true)
        {

            gameController.HacerVibrarMando(obj.control.device.deviceId);
            
            switch (contMenuPosition)
            {

                case 0: ShowInitGameGamepad(online: false); break;
                case 1: ShowInitGameGamepad(online:true);  break;
                case 2: ShowOptionsGamepad();  break;
                case 3: ShowExitGamepad(); break;
                default: return;
            }


         
            return;

        }

        //options
        if (gameController.canvasMenu[2].activeSelf == true)
        {
            gameController.HacerVibrarMando(obj.control.device.deviceId);
            switch (contOptionsPosition)
            {

                
                case 4: ResetDefaultValuesGamepad(); break;
                case 5: BotonExitOptionsGamepad(); break;
                default: return;


            }

         


            return;
        }


        
    }


    private void MovementMouse(InputAction.CallbackContext obj)
    {

       descripcion.text = Localization.Get("movimientodescripcion_mouse");

    }

    private void ButtonExit(InputAction.CallbackContext obj)
    {


        if (isBegun == true) return;

        //options
        if (gameController.canvasMenu[2].activeSelf == true)
        {

            BotonExitOptionsGamepad();
        }




    }

    private void MenuResetStickMove(InputAction.CallbackContext obj)
    {
        if (isBegun == true) return;
        isCompletedVertical = false;
        isCompletedHorizontal = false;
    }

  

    private void MenuDPADHandle(InputAction.CallbackContext obj)
    {
        if (isBegun == true) return;

        //Menu
        if (gameController.canvasMenu[1].activeSelf == true)
        {

            descripcion.text = Localization.Get("movimientodescripcion_gamepad");
            var move = obj.ReadValue<Vector2>();
            ControlMainMenu(move);
            return;

        }

        //Options
        if (gameController.canvasMenu[2].activeSelf == true)
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


   


    private void ShowPositionMenuWithGamePad()
    {

        //print(contMenuPosition);
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
        bola_seleccion.gameObject.SetActive(true);


        


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
        if (isBegun == true) return;


        //Menu
        if (gameController.canvasMenu[1].activeSelf == true)
        {
            
            descripcion.text = Localization.Get("movimientodescripcion_gamepad");
            var mov = obj.ReadValue<Vector2>();
            ControlMenuStick(mov);
            return;

        }

        //Options
        if (gameController.canvasMenu[2].activeSelf == true)
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
        //if (isBegun == true) return;

        //textos[0].color = selectedColor;
        
        //contMenuPosition = 0;
        //ShowPositionMenuWithGamePad();

    }


    public void ExitHoverEnterGame()
    {

        //if (isBegun == true) return;
        //textos[0].color = notSelectedColor;
        //bola_seleccion.gameObject.SetActive(false);

    }



    public void EnterHoverOptions()
    {

        //if (isBegun == true) return;
        //textos[1].color = selectedColor;
        //contMenuPosition = 1;
        //ShowPositionMenuWithGamePad();
    }


    public void ExitHoverOptions()
    {
        //if (isBegun == true) return;
        //textos[1].color = notSelectedColor;
        //bola_seleccion.gameObject.SetActive(false);

    }


    //public void EnterHoverCredits()
    //{
    //    if (isBegun == true) return;
    //    textos[2].color = selectedColor;

    //}


    //public void ExitHoverCredits()
    //{
    //    if (isBegun == true) return;
    //    textos[2].color = notSelectedColor;

    //}

    public void EnterHoverExit()
    {
        //if (isBegun == true) return;
        //textos[2].color = selectedColor;
        //contMenuPosition = 2;
        //ShowPositionMenuWithGamePad();
    }


    public void ExitHoverExit()
    {
        //if (isBegun == true) return;
        //textos[2].color = notSelectedColor;
        //bola_seleccion.gameObject.SetActive(false);

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


    private void ShowInitGameGamepad(bool online)
    {
        print("online0" + online);
        if (isBegun == true) return;
        isBegun = true;

        //MusicController.MusicInstance.PlayFXSound(
        //    MusicController.MusicInstance.sfx[1]
        //);

        descripcion.text = Localization.Get("movimientodescripcion_gamepad");


        //controllerMenuAnimations.DesactiveMenu();

        //await UniTask.Delay(5000);

        //DisableCanvas();
# if UNITY_EDITOR
        print("entramos por gamepad");
# endif




        ShowElegirPersonaje(online: online);


    }

 


    public async void ShowOptionsGamepad()
    {

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        //ShowFX(-5, -67);
        ShowFX(positionTransformparticles[2].anchoredPosition);
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

        gameController.canvasMenu[2].SetActive(true);


    }

    private async void ShowExitGamepad()
    {
        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );


        ShowPositionMenu(4);
        //ShowFX(-5, -144);
        ShowFX(positionTransformparticles[3].anchoredPosition);
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


        gameController.canvasMenu[2].SetActive(true);
    }



    public async void ShowExit()
    {

//        MusicController.MusicInstance.PlayFXSound(
//            MusicController.MusicInstance.sfx[1]
//            );

//        ShowPositionMenu(3);
//        //ShowFX(-34, -126);
//        ShowFX(positionTransformparticles[2].anchoredPosition);
//        await UniTask.Delay(400);


//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;

//#elif UNITY_STANDALONE
//        Application.Quit();
//#endif

    }

    private void DisableCanvas()
    {
        for (ushort i = 0; i < gameController.canvasMenu.Length; i++)
        {

            gameController.canvasMenu[i].SetActive(false);

        }


    }

    


    public void ClickedEnterGame()
    {

        //if (isBegun == true) return;

        //print("entramos por click");
        //isBegun = true;
        //descripcionMovimientoLocalization.key = "movimientodescripcion_mouse";


        //ShowPositionMenu(0);
        ////MusicController.MusicInstance.PlayFXSound(
        ////    MusicController.MusicInstance.sfx[1]
        ////    );
        
        ////ShowFX(positionTransformparticles[0].anchoredPosition);
        //ShowElegirPersonaje();


    }


    private async void ShowElegirPersonaje(bool online)
    {


        inputActions.Disable();

        if (online == true)
        { 
        
            ShowFX(positionTransformparticles[1].anchoredPosition);
        
        }
        else
        { 
            ShowFX(positionTransformparticles[0].anchoredPosition);
        
        }

        MusicController.MusicInstance.PlayFXSound(MusicController.MusicInstance.sfx[1]);

        float duration = controllerMenuAnimations.DesactiveMenu();

        await UniTask.Delay(TimeSpan.FromMilliseconds(duration * 1000));

        print("online=" + online);
        if (online == true)
        {
            elegirPersonaje.entrada_txt[0].text = Localization.Get("pulsaboton");

            //si es online ocultamos los menus
            for(ushort i = 1; i < elegirPersonaje.entrada_txt.Length; i++)
            { 
                elegirPersonaje.entrada_txt[i].text = Localization.Get("disabled");
            
            }

            
            
        
        }

        DisableCanvas();


        

        gameController.canvasMenu[3].SetActive(true);




        elegirPersonaje.InitActions(online: online);

        //gameController.InitGame();

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

        ShowFXOptions(0, -193);

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

        ShowFXOptions(0, -193);

        DeColorOptionsReset(4);
        contOptionsPosition = 0;
        await UniTask.Delay(500);
        textosOptions[0].color = selectedColor;
        txtOptions[0].color = selectedColor;



    }

    public async void BotonExitOptions()
    {

        //SavePlayerPrefsValues();

        //ShowFXOptions(0, -249);

        //DeColorOptionsReset(5);

        //MusicController.MusicInstance.PlayFXSound(
        //    MusicController.MusicInstance.sfx[1]
        //    );


        //await UniTask.Delay(500);
        //DisableCanvas();

        //contMenuPosition = 0;
        //contOptionsPosition = 0;
        ////ShowPositionMenu();
        //ShowPositionMenuWithGamePad();

        //descripcionMovimientoLocalization.key = "movimientodescripcion_mouse";
        //gameController.canvasMenu[1].SetActive(true);
        ////mar.SetActive(true);

    }

    public async void BotonExitOptionsGamepad()
    {

        MusicController.MusicInstance.PlayFXSound(
                   MusicController.MusicInstance.sfx[1]
               );
        SavePlayerPrefsValues();
        ShowFXOptions(0, -250);


        DeColorOptionsReset(5);


        await UniTask.Delay(400);
        DisableCanvas();

        contMenuPosition = 0;
        contOptionsPosition = 0;
        ShowPositionMenuWithGamePad();
        descripcion.text = Localization.Get("movimientodescripcion_gamepad");
        gameController.canvasMenu[1].SetActive(true);
        //mar.SetActive(true);

    }




    //public async void BotonExitCredits()
    //{

        
    //    //DeColorOptions(5);
    //    particleCreditsSelected.Play();

    //    MusicController.MusicInstance.PlayFXSound(
    //        MusicController.MusicInstance.sfx[1]
    //        );


    //    textoCredits.color = selectedColor;
    //    await UniTask.Delay(150);
    //    textoCredits.color = notSelectedColor;

    //    await UniTask.Delay(200);



    //    contMenuPosition = 0;
    //    contOptionsPosition = 0;
    //    DisableCanvas();


    //    gameController.canvasMenu[1].SetActive(true); 
    //    //mar.SetActive(true);
    //    ShowPositionMenuWithGamePad();



    //}


    //private async void BotonExitCreditsGamepad()
    //{


    //    //DeColorOptions(5); //.... no tendria que estar
    //    particleCreditsSelected.Play();

    //    MusicController.MusicInstance.PlayFXSound(
    //        MusicController.MusicInstance.sfx[1]
    //        );


    //    textoCredits.color = selectedColor;
    //    await UniTask.Delay(150);
    //    textoCredits.color = notSelectedColor;

    //    await UniTask.Delay(200);

    //    contMenuPosition = 0;
    //    contOptionsPosition = 0;
    //    DisableCanvas();


    //    ShowPositionMenuWithGamePad();
    //    gameController.canvasMenu[1].SetActive(true);
    //    ///*mar.*/SetActive(true);



    //}





    public void ResetPlayerPrefs()
    {

        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        Application.Quit();
#endif


    }


#region OPCIONES
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
    #endregion

}
