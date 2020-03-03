using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InControlActions;
using UnityEngine.InputSystem;
using UniRx.Async;
using UniRx;
using System;
using Photon.Pun;

public class ControllerElegirPersonaje : MonoBehaviour
{

    public ControlActions inputActions;

    [Header("Manager")]
    [SerializeField] private GameController gameController = null;
    [SerializeField] private GameCharactersSettings gameCharactersSettings;
    //public VariablesOverScenes variablesOverScenes;
    
    [Header("Paneles Players")]
    [SerializeField] private GameObject[] panel_players = null;


    [Header("Characters Images")]
    public Image[] charactersImagesSmall;
    public Image[] charactersImagesBig;

    [Header("Barra HP")]
    public Image[] barraHP;

    [Header("Barra Power")]
    public Image[] barraPower;

    [Header("Barra Defensa")]
    public Image[] barraDefense;

    [Header("Nombre Personajes")]
    public TextMeshProUGUI[] nameCharactersPlayer;

    //[Header("texto empezar lucha")]
    //[SerializeField] private UILocalization textoempezarlucha = null;


    [SerializeField] private TextMeshProUGUI[] listoMensaje = null;

    public Image[] mandosImage;
    public TextMeshProUGUI[] entrada_txt = null;
    public Sprite[] prefabMandosImage;


    //private TwoAxisInputControl lstick = new TwoAxisInputControl();
    //private TwoAxisInputControl rstick = new TwoAxisInputControl();
    //private TwoAxisInputControl dpad = new TwoAxisInputControl();

   

    //---------
    public Animation[] focusButtonX;

    public GameObject[] readyImage = null;

    public Color[] prefabColorsPlayers;
    public GameObject prefabPlayer = null;
    public GameObject playerPrefabOnline = null;
    public Transform[] initialPositionInstantiation = null;

    public TextMeshProUGUI[] namePlayers = null;


   

    public TextMeshProUGUI timeTxt;
    public ushort timeNum = 60;

    public GameObject[] focusPlayers = null;
    public Image[] bigSelectionPlayers = null;
    public Image selectionAlAtaker = null;
    public Image[] playerImage = null;
    

    public Sprite pairSprite = null;
    public Sprite noPairSprite = null;

    public GameObject[] initialPlayerPosition = null;

    public MatrixCharacters[] matrixCharacters = null;

    public Image[] recuadros = null;
    

    private bool isCompletedMoveRightStick = false;
    private bool isCompletedMoveLeftStick = false;
    //private bool isOnline = false;

    [SerializeField] public TextMeshProUGUI mensajeBotonEntrar = null;
    


    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();

        }

    }

    //bool isInsertingCharacter = true;

    private void Awake()
    {

        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += ControlDpad;

        inputActions.Menu.Buttons.performed += BotonSur;
        //inputActions.Menu.WestButton.performed += BotonSur;
        
        inputActions.Menu.ExitButton.performed += ButtonExit;
        //inputActions.Menu.NorteButton.performed += ButtonExit;
        
        inputActions.Menu.LeftStick.performed += ControlLeftStick;
        inputActions.Menu.LeftStick.canceled += ResetLeftStick;

        inputActions.Menu.RightStick.performed += ControlRightStick;
        inputActions.Menu.RightStick.canceled += ResetRightStick;

        //inputActions.Menu.MovementMouse.performed += MovementMouse;
        for (ushort i = 0; i < panel_players.Length; i++)
        { 
            panel_players[i].SetActive(false);
            focusPlayers[i].SetActive(false);
            entrada_txt[i].text = Localization.Get("pulsaboton");
            entrada_txt[i].gameObject.SetActive(true);
            
            focusPlayers[i].SetActive(false);
            recuadros[i].color = Color.white;
            listoMensaje[i].text = Localization.Get("confirmar");
            listoMensaje[i].gameObject.SetActive(false);

        }

        //textoempezarlucha.key = "esperandojugadores";

        //fitting alpha image with collider. needs read/write enabled from texture
        for (ushort i = 0; i < charactersImagesSmall.Length; i++)
        {

            charactersImagesSmall[i].alphaHitTestMinimumThreshold = 1;

        }

        for (ushort i = 0; i < matrixCharacters.Length; i++)
        {

            matrixCharacters[i].health = gameCharactersSettings.health[i];
            matrixCharacters[i].healthMax = gameCharactersSettings.healthMax;
            matrixCharacters[i].power = gameCharactersSettings.powerDamage[i];
            matrixCharacters[i].powerMax = gameCharactersSettings.powerMax;
            matrixCharacters[i].defense = gameCharactersSettings.defense[i];
            matrixCharacters[i].defenseMax = gameCharactersSettings.defenseMax;
            matrixCharacters[i].energy = gameCharactersSettings.energy[i];
            matrixCharacters[i].energyMax = gameCharactersSettings.energyMax;

            matrixCharacters[i].fireCooldown = gameCharactersSettings.fireCooldown[i];
            matrixCharacters[i].speedMovement = gameCharactersSettings.speedMovement[i];
            matrixCharacters[i].durationShotSeconds = gameCharactersSettings.shootSpeed[i];
            matrixCharacters[i].bombCooldown = gameCharactersSettings.bombCooldown[i];
         

            matrixCharacters[i].imageCharacter = gameCharactersSettings.imageCharactersBig.GetSprite(i+"big");
            charactersImagesSmall[i].sprite = gameCharactersSettings.imageCharactersSmall.GetSprite(i.ToString());
            matrixCharacters[i].nameCharacter = gameCharactersSettings.nameCharacters[i];

        }


        for (ushort i = 0; i < focusPlayers.Length; i++)
        {

            focusPlayers[i].GetComponent<Image>().color = prefabColorsPlayers[i];
            bigSelectionPlayers[i].color = prefabColorsPlayers[i];

            barraHP[i].fillAmount = 0;
            barraDefense[i].fillAmount = 0;
            barraPower[i].fillAmount =  0;


            nameCharactersPlayer[i].text = "";
            mandosImage[i].gameObject.SetActive(false);


        }

        



    }

    

    public void InitActions(bool online)
    { 
        gameController.isOnline = online;
        if (online == true)
        {
            
            entrada_txt[0].text = Localization.Get("pulsaboton");
            

            //si es online ocultamos los menus
            for(ushort i = 1; i < entrada_txt.Length; i++)
            { 
                entrada_txt[i].text = Localization.Get("esperando");
            
            }
           
            //cambiar color al jugador....
        
        
        }
        else
        { 
        
            explicaciones[0].text = Localization.Get("explicaciona");
            explicaciones[1].text = Localization.Get("explicacionb");

        }

        
        gameController.canvasMenu[3].SetActive(true);


        inputActions.Enable();



    
    }

    private void ResetLeftStick(InputAction.CallbackContext obj)
    { 
        isCompletedMoveLeftStick = false;
        //print("reset l");
    }

    private void ResetRightStick(InputAction.CallbackContext obj)
    { 
        //print("reset r");
        isCompletedMoveRightStick = false;
    
    }


    private void ControlDpad(InputAction.CallbackContext obj)
    {
        if (gameController.dictPlayers.ContainsKey(obj.control.device.deviceId) == false)
        { 
            return;
        }

        Vector2 move = obj.ReadValue<Vector2>();

        int posicionPlayer = gameController.dictPlayers[obj.control.device.deviceId];
        
        ControlEleccionPersonajes(move, gameController.jugadores[posicionPlayer], isLeftStick: false, isRightStick: false);

    }

    private void ControlLeftStick(InputAction.CallbackContext obj)
    { 


        if (isCompletedMoveLeftStick == true) return;
        
        if (gameController.dictPlayers.ContainsKey(obj.control.device.deviceId) == false)
        { 
            return;
        }

        Vector2 move = obj.ReadValue<Vector2>();

        int posicionPlayer = gameController.dictPlayers[obj.control.device.deviceId];
        
        //print("X=" + move.x +  " y=" + move.y);

        ControlEleccionPersonajes(move, gameController.jugadores[posicionPlayer], isLeftStick: true, isRightStick: false);
        
    }

    private void ControlRightStick(InputAction.CallbackContext obj)
    { 
    
        if (isCompletedMoveRightStick == true) return;
        if (gameController.dictPlayers.ContainsKey(obj.control.device.deviceId) == false)
        { 
            return;
        }

        Vector2 move = obj.ReadValue<Vector2>();

        int posicionPlayer = gameController.dictPlayers[obj.control.device.deviceId];
        
        ControlEleccionPersonajes(move, gameController.jugadores[posicionPlayer], isLeftStick: false, isRightStick: true);
    
    }


    private void ControlEleccionPersonajes(Vector2 posicion, InfoPlayer player, bool isLeftStick, bool isRightStick)
    { 
    
        if (posicion.y >= 0.5f)
        { 
            
            if (isLeftStick == true)
            { 
                isCompletedMoveLeftStick = true;
            }

            if (isRightStick == true)
            { 
                isCompletedMoveRightStick = true;
            }

            
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().up is null) == false)
            {

                player.focusPlayer.GetComponent<MatrixCharacters>().up.down.taken = false;

            }
            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().up, player.posX, player.posicionPlayer );


            //print("arriba");
            return;
        }

        if (posicion.y <= -0.5f)
        { 

            if (isLeftStick == true)
            { 
                isCompletedMoveLeftStick = true;
            }

            if (isRightStick == true)
            { 
                isCompletedMoveRightStick = true;
            }


            //print("abajo");
            
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().down is null) == false)
            {
                player.focusPlayer.GetComponent<MatrixCharacters>().down.up.taken = false;
            }


            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().down, player.posX, player.posicionPlayer);
            return;
        
        }

        if (posicion.x >= 0.5f)
        { 

            if (isLeftStick == true)
            { 
                isCompletedMoveLeftStick = true;
            }

            if (isRightStick == true)
            { 
                isCompletedMoveRightStick = true;
            }



            if (player.posX >= 0 && player.posX < 5)
            {
                player.posX++;
            }


            //print("derecha");
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().right is null) == false)
            {

                player.focusPlayer.GetComponent<MatrixCharacters>().right.left.taken = false;

            }

            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().right, player.posX, player.posicionPlayer );
            return;

        }

        if (posicion.x <= -0.5f)
        { 


            if (isLeftStick == true)
            { 
                isCompletedMoveLeftStick = true;
            }

            if (isRightStick == true)
            { 
                isCompletedMoveRightStick = true;
            }
        
            if (player.posX > 0 && player.posX <= 5)
            {
                player.posX--;
            }

            //print("izquierda");
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().left is null) == false)
            {
                player.focusPlayer.GetComponent<MatrixCharacters>().left.right.taken = false;

            }

            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().left, player.posX, player.posicionPlayer);
            return;


        }





    }

    
    private async void BotonSur(InputAction.CallbackContext obj)
    {

        //print(gameController.dictPlayers.ContainsKey(obj.control.device.deviceId));
        if (gameController.isOnline == true)
        { 
            if (PhotonNetwork.IsMasterClient == true)
            {
                gameController.HacerVibrarMando(obj.control.device.deviceId);
                mensajeBotonEntrar.text = Localization.Get("conectando");

                await UniTask.Delay(TimeSpan.FromSeconds(2));

                if (inputActions != null)
                {
                    inputActions.Disable();

                }
                gameController.estaEmpezado = true;



                var view = PhotonNetwork.GetPhotonView(gameController.viewIdMasterclient);
                view.RPC("Empezar", RpcTarget.OthersBuffered, true);

                gameController.canvasMenu[5].SetActive(true);
                gameController.canvasMenu[6].SetActive(true);

                gameController.canvasMenu[4].SetActive(true);
                gameController.canvasMenu[3].SetActive(false);
                    

                gameController.InitGame();

            }

            return;
        
        }

        if (gameController.dictPlayers.ContainsKey(obj.control.device.deviceId) == false)
        { 
            //print("insertado: " + gameController.contadorJugadores + " device=" + obj.control.device.deviceId);
            //insertar player
            

            
            gameController.contadorJugadores++;
            if (gameController.contadorJugadores > gameController.JUGADORES_MAXIMO)
            { 
        
                gameController.contadorJugadores = gameController.JUGADORES_MAXIMO;
                recuadros[3].color = Color.black;
                return;

        
            }

            short posicionLibre = GetPosicionLibre();
                      

            if (posicionLibre == -1)
            { 
                return;
            
            }
            
            gameController.jugadores[posicionLibre].vacio = false;
            recuadros[posicionLibre].gameObject.SetActive(false);

            listoMensaje[posicionLibre].text = Localization.Get("confirmar");
            listoMensaje[posicionLibre].gameObject.SetActive(true);

            //gameController.jugadores[gameController.contadorJugadores].idDevice = obj.control.device.deviceId;
            gameController.HacerVibrarMando(obj.control.device.deviceId);
            entrada_txt[posicionLibre].text = Localization.Get("pulsaboton");
            entrada_txt[posicionLibre].gameObject.SetActive(false);
            panel_players[posicionLibre].SetActive(true);
            //focusPlayers[gameController.contadorJugadores - 1].transform.position = initialPlayerPosition[gameController.contadorJugadores - 1].transform.position;
            focusPlayers[posicionLibre].SetActive(true);
            mandosImage[posicionLibre].gameObject.SetActive(true);

            AddPlayer(posicionLibre, obj.control.device.deviceId, false);
            (ushort, ushort)posicion = PosicionPlayerMatrix(posicionLibre);
            
            MoveFocus(focusPlayers[posicionLibre], initialPlayerPosition[posicionLibre].GetComponent<MatrixCharacters>(), posicion.Item1, posicionLibre);
            
            

            
            


        }
        else
        { 
        
            //print("player listo: " + gameController.contadorJugadores + " device=" + obj.control.device.deviceId);
            
            int posicionPlayer = gameController.dictPlayers[obj.control.device.deviceId];
            
            if (gameController.jugadores[posicionPlayer].listo == true)
            { 

                //print("3 vez");
                //print("devices count=" + Gamepad.all.Count);
                //print("contadojugadores=" + gameController.contadorJugadores);
                // tercera vez
                //ComprobarPersonajesListo();
            
            }
            else
            { 
                //print("devices count=" + Gamepad.all.Count);
                //print("contadojugadores=" + gameController.contadorJugadores);

                //ejecutar que el player esta listo
                gameController.jugadores[posicionPlayer].listo = true;
                
                gameController.HacerVibrarMando(obj.control.device.deviceId);
                
                //ushort o = gameController.dictPlayers[obj.control.device.deviceId].posicionPlayer;

                listoMensaje[posicionPlayer].text = Localization.Get("listo");
                listoMensaje[posicionPlayer].gameObject.SetActive(true);
                readyImage[posicionPlayer].SetActive(
                    !readyImage[posicionPlayer].activeSelf
                );

                //print("devices count=" + Gamepad.all.Count);
                //print("contadojugadores=" + gameController.contadorJugadores);

                ComprobarPersonajesListo();
               
                






            }



        
        }


        
        

        
        
        
    }

    private void ButtonExit(InputAction.CallbackContext obj)
    {

        if (gameController.isOnline == true)
        { 
        

            return;
        
        }

        if (gameController.dictPlayers.ContainsKey(obj.control.device.deviceId) == true)
        { 

            int posicionPlayer = gameController.dictPlayers[obj.control.device.deviceId];
            
            if(gameController.jugadores[posicionPlayer].listo == true)
            { 
                return;
            }

            gameController.HacerVibrarMando(obj.control.device.deviceId);
            
            gameController.contadorJugadores--;
            if (gameController.contadorJugadores < 0)
            { 
                gameController.contadorJugadores = 0;
            }

           
            //ushort o = gameController.dictPlayers[obj.control.device.deviceId].posicionPlayer;
            gameController.jugadores[posicionPlayer].listo = false;
            gameController.jugadores[posicionPlayer].vacio = true;

            listoMensaje[posicionPlayer].text = Localization.Get("confirmar");
            listoMensaje[posicionPlayer].gameObject.SetActive(false);
            readyImage[posicionPlayer].SetActive(false);


            recuadros[posicionPlayer].gameObject.SetActive(true);
            panel_players[posicionPlayer].SetActive(false);
            entrada_txt[posicionPlayer].text = Localization.Get("pulsaboton");
            entrada_txt[posicionPlayer].gameObject.SetActive(true);
            focusPlayers[posicionPlayer].SetActive(false);
            focusPlayers[posicionPlayer].transform.position = initialPlayerPosition[posicionPlayer].transform.position;
            
            gameController.dictPlayers.Remove(obj.control.device.deviceId);
            


        }
        else
        { 
            //
            print("queremos volver atras???");
        
        }


    }


    

    private short GetPosicionLibre()
    { 
    
        //short posicionLibre = -1;


        for(short i = 0; i < gameController.JUGADORES_MAXIMO; i++)
        { 
            //print(gameController.jugadores[i]);    
            if (gameController.jugadores[i].vacio == true)
            { 
                //posicionLibre = i;
                
                return i;
                
            }
            
        }

        return -1;
    
    
    }

    private void Start()
    {
      

        for (ushort i = 0; i < readyImage.Length; i++)
        {

            readyImage[i].gameObject.SetActive(false);

        }


        for (ushort i = 0; i < charactersImagesSmall.Length; i++)
        {

            charactersImagesSmall[i].alphaHitTestMinimumThreshold = 1;

        }

    }


    public void IniciarCronoSeleccionPersonaje()
    {

        InvokeRepeating("DisplayCrono", 0, 1f);


    }


    private void DisplayCrono()
    {

        timeTxt.text = "TIME:" + timeNum;
        timeNum--;
        if (timeNum == 0)
        {

            CancelInvoke("DisplayCrono");
            alAtaque();

        }

    }


   
    private (ushort, ushort) PosicionPlayerMatrix(int contadorJugador)
    { 
    
        ushort x = 0;
        ushort y = 0;
        switch (contadorJugador)
        {
            case 0: x = 0; y = 2; break;
            case 1: x = 5; y = 2; break;
            case 2: x = 0; y = 0; break;
            case 3: x = 5; y = 0; break;
            default: Debug.LogError("demasiados"); break;

        }
    
        return (x, y);
    }


    // contadorujugadores es mala estrategia...
    public void AddPlayer(int posicionLibre, int deviceId, bool isBot )
    {
        ushort x = 0; 
        ushort y = 0;
        nombreColores nombrePlayerColor = nombreColores.None;
        switch (posicionLibre)
        {
            case 0: x = 0; y = 2; nombrePlayerColor = nombreColores.amarillo; break;
            case 1: x = 5; y = 2; nombrePlayerColor = nombreColores.azul; break;
            case 2: x = 0; y = 0; nombrePlayerColor = nombreColores.rojo; break;
            case 3: x = 5; y = 0; nombrePlayerColor = nombreColores.blanco; break;
            default: Debug.LogError("demasiados"); break;

        }

        //int posicionLibre = GetPosicionLibre();
        if (deviceId != -1)
        { 
            gameController.dictPlayers.Add(deviceId, posicionLibre);
        
        }
        

        //print("deviceid=" + deviceId);

        gameController.jugadores[posicionLibre] = new InfoPlayer(
                focusPlayers[posicionLibre],
                null,
                initialPlayerPosition[posicionLibre],
                prefabColorsPlayers[posicionLibre],
                x, y,
                (ushort)(posicionLibre),
                true,
                bigSelectionPlayers[posicionLibre],
                false,
                false,
                isBot,
                nombrePlayerColor,
                deviceId

            );
        //print("deviceid=" + deviceId + "" + gameController.jugadores[posicionLibre].deviceId);
        //gameController.dictPlayers.Add(deviceId, new InfoPlayer(
        //        focusPlayers[gameController.contadorJugadores],
        //        null,
        //        initialPlayerPosition[gameController.contadorJugadores],
        //        prefabColorsPlayers[gameController.contadorJugadores],
        //        x, y,
        //        (ushort)(gameController.contadorJugadores),
        //        true,
        //        bigSelectionPlayers[gameController.contadorJugadores],
        //        false,
        //        false,
        //        isBot

        //    ));



    }

   


    private void MoveFocus(GameObject focus, MatrixCharacters matrixPos, ushort posX, int posicionPlayer)
    {

        //print("nombre=" + focus.name + " matriz=" + matrixPos + " posX=" + posX + " playerID=" + posicionPlayer);
        
        if ((matrixPos is null) == false)
        {


            //if (playerId < 0 || playerId > gameController.JUGADORES_MAXIMO - 1) return;


            if (posX % 2 == 0)
            {

                focus.GetComponent<Image>().sprite = pairSprite;
            }
            else
            {

                focus.GetComponent<Image>().sprite = noPairSprite;
            }


            focus.transform.position = matrixPos.gameObject.transform.position;
            matrixPos.taken = true;

            var tFocus = focus.GetComponent<MatrixCharacters>();

            tFocus.up = matrixPos.up;
            tFocus.down = matrixPos.down;
            tFocus.right = matrixPos.right;
            tFocus.left = matrixPos.left;


            tFocus.health = matrixPos.health;
            tFocus.healthMax = matrixPos.healthMax;
            tFocus.energy = matrixPos.energy;
            tFocus.energyMax = matrixPos.energyMax;
            tFocus.power = matrixPos.power;
            tFocus.powerMax = matrixPos.powerMax;
            tFocus.defense = matrixPos.defense;
            tFocus.defenseMax = matrixPos.defenseMax;
            
            tFocus.bombCooldown = matrixPos.bombCooldown;
            tFocus.durationShotSeconds = matrixPos.durationShotSeconds;
            tFocus.fireCooldown = matrixPos.fireCooldown;
            tFocus.speedMovement = matrixPos.speedMovement;

            tFocus.nameCharacter = matrixPos.nameCharacter;

            //charactersImagesBig[playerId].sprite = matrixPos.imageCharacter;
            playerImage[posicionPlayer].sprite = matrixPos.imageCharacter;
            playerImage[posicionPlayer].preserveAspect = true;
            barraHP[posicionPlayer].fillAmount = matrixPos.health / matrixPos.healthMax;
            barraDefense[posicionPlayer].fillAmount = matrixPos.defense / matrixPos.defenseMax;
            barraPower[posicionPlayer].fillAmount = matrixPos.power / matrixPos.powerMax;
            nameCharactersPlayer[posicionPlayer].text = matrixPos.nameCharacter;






        }



    }


    private async void ComprobarPersonajesListo()
    { 
        if (Gamepad.all.Count == gameController.contadorJugadores)
        { 
            
            //comprobar que esten todos listos
            bool completado = true;
            for (ushort i = 0; i < Gamepad.all.Count; i++)
            { 
                if (gameController.jugadores[i].listo == false)
                { 
                    completado = false;
                    return;
                
                }
                
            
            
            }
            //List<int> keys = new List<int>(gameController.dictPlayers.Keys);

            //for (ushort i = 0; i < keys.Count; i++)
            //{
            //    //CORREGIR....testesar
            //    if (gameController.dictPlayers[keys[i]].listo == false)
            //    {
            //        completado = false;
            //        return;
            //    }

            //}

            //todos los gameController.jugadores al completo listos
            if (completado == true)
            {
#if UNITY_EDITOR
                print("al ataker");
#endif
                //RellenarBots();


                if (gameController.isOnline == true)
                {
                    mensajeBotonEntrar.text = Localization.Get("conectando");
                    //gameController.lobbyClientPun.ConnectToPun(gameController.jugadores[0].focusPlayer.GetComponent<MatrixCharacters>().nameCharacter);

                    // hay que esperar a que haya al menos 2 jugadores ???
                    await UniTask.WaitUntil(() => pistaLibre.Value == true);

                    if (inputActions != null)
                    {
                        inputActions.Disable();

                    }

                    gameController.canvasMenu[5].SetActive(true);
                    gameController.canvasMenu[6].SetActive(true);

                    gameController.canvasMenu[4].SetActive(true);
                    gameController.canvasMenu[3].SetActive(false);
                    

                    gameController.InitGame();

                
                }
                else
                { 

                    mensajeBotonEntrar.text = Localization.Get("generandomapa");
                    await UniTask.Delay(TimeSpan.FromSeconds(2));
                    alAtaque();
                
                }

                

                

            }


            
        }
        else
        { 
            
            

        }
    
    
    }


    private void RellenarBots()
    { 
        //List<int> keys = new List<int>(playersById.Keys);

        //for (ushort i = 0; i < keys.Count; i++)
        //{
        //    if (playersById[keys[i]].listo == false)
        //    {
        //        // bot
        //        AddPlayer(i, 100, true);
        //        int rnd = Random.Range(0, gameCharactersSettings.health.Length);

        //        float health = gameCharactersSettings.health[rnd];
        //        float power = gameCharactersSettings.powerDamage[rnd];
        //        float defense = gameCharactersSettings.defense[rnd];

        //        gameController.contadorJugadores++;


        //    }

        //}
    
    
    }


    public void alAtaque()
    {

        if (inputActions != null)
        {
            inputActions.Disable();

        }

        gameController.canvasMenu[5].SetActive(true);
        gameController.canvasMenu[6].SetActive(true);

        gameController.canvasMenu[4].SetActive(true);
        gameController.canvasMenu[3].SetActive(false);

        gameController.InstanciarJugadorLocal();
        gameController.InitGame();

    }

    //private void DisplayFX(ushort index)
    //{

    //    focusButtonX[0].Play();


    //}

    

    public void ClickPersonajes(int i)
    {

        if (i == 0) return;
        //teclado

        if (gameController.contadorJugadores >= gameController.JUGADORES_MAXIMO)
        {
            print("no se admiten mas jugadores");
            return;
        }



        //if (variablesOverScenes.gameController.dictPlayers.ContainsKey("teclado") == true)
        //{

        //    print("encontrado");
        //    //cambiar de personaje
        //    playerKeyboardLastTaken.taken = false;
        //}
        //else
        //{

        //    print("no encontrado");


        //    ushort x = 0; ushort y = 0;
        //    switch (menuManager.countPlayers)
        //    {
        //        case 0: x = 0; y = 2; break;
        //        case 1: x = 5; y = 2;  break;
        //        case 2: x = 0; y = 0;  break;
        //        case 3: x = 5; y = 0;  break;
        //        default: Debug.LogError("demasiados"); break;

        //    }



        //    //añadir personaje y cambiar de personaje
        //    variablesOverScenes.gameController.dictPlayers.Add("teclado",
        //   new InfoPlayer(

        //       //tInput,
        //       focusPlayers[menuManager.countPlayers],
        //       null,
        //       initialPlayerPosition[menuManager.countPlayers],
        //       //initialPlayerPosition[menuManager.countPlayers].GetComponent<MatrixCharacters>(),
        //       prefabColorsPlayers[menuManager.countPlayers],
        //       x, y,
        //       (ushort)(menuManager.countPlayers + 1),
        //       true,
        //       bigSelectionPlayers[menuManager.countPlayers],
        //       false
               

        //       )


        //   );




        //    menuManager.countPlayers++;

        //}



        //InfoPlayer thisPlayer = variablesOverScenes.gameController.dictPlayers["teclado"];



        //i--;
        //var tID = thisPlayer.playerId--;

        //thisPlayer.selected = true;
        //readyImage[tID].SetActive(true);

        //thisPlayer.focusPlayer.transform.position = matrixCharacters[i].gameObject.transform.position;

        //thisPlayer.namePlayer = gameCharactersSettings.nameCharacters[i];

        //if ((i)% 2 == 0)
        //{

        //    thisPlayer.focusPlayer.GetComponent<Image>().sprite = pairSprite;
        //}
        //else
        //{

        //    thisPlayer.focusPlayer.GetComponent<Image>().sprite = noPairSprite;
        //}

        //thisPlayer.focusPlayer.GetComponent<Image>().enabled = true;

        //var mPlayer = thisPlayer.focusPlayer.GetComponent<MatrixCharacters>();

        //mPlayer = matrixCharacters[i];

        //print("health teclado=" + mPlayer.health);

        //matrixCharacters[i].taken = true;
        //playerKeyboardLastTaken = matrixCharacters[i];


        //charactersImagesBig[tID].sprite = matrixCharacters[i].imageCharacter;
        //barraHP[tID].fillAmount = matrixCharacters[i].health / matrixCharacters[i].healthMax;
        //barraDefense[tID].fillAmount = matrixCharacters[i].defense / matrixCharacters[i].defenseMax;
        //barraPower[tID].fillAmount = matrixCharacters[i].power / matrixCharacters[i].powerMax;
        //nameCharactersPlayer[tID].text = matrixCharacters[i].nameCharacter;


        //mandosImage[tID].sprite = prefabMandosImage[1];

        //mandosImage[tID].gameObject.SetActive(true);







    }


    ReactiveProperty<bool> pistaLibre = new ReactiveProperty<bool>(false);

    public void SetPistaLibre()
    { 
    
        pistaLibre.Value = true;
    
    }

    [SerializeField] private TextMeshProUGUI[] explicaciones = null;

    public void AddPlayerFromOnline(int posicionLibre)
    { 
    
        print("posicionlibre" + posicionLibre);
        
        explicaciones[0].text = "";
        explicaciones[1].text = "";

        print("es mastercliente=" + Photon.Pun.PhotonNetwork.IsMasterClient);
        if (Photon.Pun.PhotonNetwork.IsMasterClient == true)
        { 
            explicaciones[0].text = Localization.Get("explicacionaonline");
            explicaciones[1].text = Localization.Get("explicacionbonline");
        
        }

        gameController.jugadores[posicionLibre].vacio = false;
        recuadros[posicionLibre].gameObject.SetActive(false);

        listoMensaje[posicionLibre].text = Localization.Get("listo");
        listoMensaje[posicionLibre].gameObject.SetActive(true);

        entrada_txt[posicionLibre].text = "";
        entrada_txt[posicionLibre].gameObject.SetActive(false);
        panel_players[posicionLibre].SetActive(true);
        focusPlayers[posicionLibre].SetActive(true);
        mandosImage[posicionLibre].gameObject.SetActive(true);

        AddPlayer(posicionLibre, -1, false);
        (ushort, ushort)posicion = PosicionPlayerMatrix(posicionLibre);
            
        MoveFocus(focusPlayers[posicionLibre], initialPlayerPosition[posicionLibre].GetComponent<MatrixCharacters>(), posicion.Item1, posicionLibre);
            
    
    
    
    }




}