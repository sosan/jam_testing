using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InControlActions;
using UnityEngine.InputSystem;



public class InfoJugador
{
    public Color color;
    public int idDevice;
    public int x;
    public bool listo = false;
    public GameObject objPlayer = null;

}



public class ControllerElegirPersonaje : MonoBehaviour
{

    public ControlActions inputActions;

    [Header("Manager")]
    //public MenuManager menuManager;
    public GameCharactersSettings gameCharactersSettings;
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

    [Header("texto empezar lucha")]
    [SerializeField] private UILocalization textoempezarlucha = null;


    public TextMeshProUGUI[] explicacion;

    public Image[] mandosImage;
    public Sprite[] prefabMandosImage;


    //private TwoAxisInputControl lstick = new TwoAxisInputControl();
    //private TwoAxisInputControl rstick = new TwoAxisInputControl();
    //private TwoAxisInputControl dpad = new TwoAxisInputControl();

    // fundamentales
    [SerializeField] private const ushort JUGADORES_MAXIMO = 4;
    //private InfoJugador[] jugadores = new InfoJugador[JUGADORES_MAXIMO];
    [HideInInspector] public ushort contadorJugadores = 0;
    [HideInInspector] public Dictionary<int, InfoJugador> playersById = new Dictionary<int, InfoJugador>();

    //---------
    public Animation[] focusButtonX;

    public GameObject[] readyImage = null;

    public Color[] prefabColorsPlayers;
    public GameObject prefabPlayer;

    public TextMeshProUGUI[] namePlayers = null;


   

    public TextMeshProUGUI timeTxt;
    public ushort timeNum = 60;

    public GameObject[] focusPlayers = null;
    public Image[] bigSelectionPlayers = null;
    public Image selectionAlAtaker = null;
    public Image[] playerInfoImage = null;

    public Sprite pairSprite = null;
    public Sprite noPairSprite = null;

    public GameObject[] initialPlayerPosition = null;

    public MatrixCharacters[] matrixCharacters = null;
    private MatrixCharacters playerKeyboardLastTaken = null;


    private void OnEnable()
    {

        //inputActions.Enable();


    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();

        }

    }



    private void Awake()
    {

        //lstick.StateThreshold = 0.1f; //0.5f
        //rstick.StateThreshold = 0.1f;
        //dpad.StateThreshold = 0.5f;
        inputActions = new ControlActions();
        inputActions.Menu.Dpad.performed += ControlDpad;

        inputActions.Menu.Buttons.performed += BotonSur;
        inputActions.Menu.ExitButton.performed += ButtonExit;

        //inputActions.Menu.LeftStick.performed += MenuStickHandle;
        //inputActions.Menu.LeftStick.canceled += MenuResetStickMove;

        //inputActions.Menu.RightStick.performed += MenuStickHandle;
        //inputActions.Menu.RightStick.canceled += MenuResetStickMove;

        //inputActions.Menu.MovementMouse.performed += MovementMouse;
        for (ushort i = 0; i < panel_players.Length; i++)
        { 
            panel_players[i].SetActive(false);
            focusPlayers[i].SetActive(false);
        
        }

        textoempezarlucha.key = "esperandojugadores";



    }

    public void InitActions()
    { 
        inputActions.Enable();
    
    }

    private void ControlDpad(InputAction.CallbackContext obj)
    {

        Vector2 move = obj.ReadValue<Vector2>();
        ControlEleccionPersonajes(move);

    }

    private void ControlEleccionPersonajes(Vector2 posicion)
    { 
    
    
    
    }

    
    private void BotonSur(InputAction.CallbackContext obj)
    {

        print(playersById.ContainsKey(obj.control.device.deviceId));

        if (playersById.ContainsKey(obj.control.device.deviceId) == false)
        { 
            print("insertado: " + contadorJugadores + " device=" + obj.control.device.deviceId);
            //insertar player
            contadorJugadores++;
            if (contadorJugadores > JUGADORES_MAXIMO)
            { 
        
                contadorJugadores = JUGADORES_MAXIMO;
        
            }

            //jugadores[contadorJugadores].idDevice = obj.control.device.deviceId;
            
            playersById.Add(obj.control.device.deviceId, new InfoJugador());

        }
        else
        { 
        
            print("player listo: " + contadorJugadores + " device=" + obj.control.device.deviceId);
            //ejecutar que el player esta listo
            playersById[obj.control.device.deviceId].listo = true;
            var numdevices = InputSystem.devices.Count;

            // si solo hay uno tendriamos que ir para ready  todos..


            //comprobar que esten todos listos
            bool completado = true;
            List<int> keys = new List<int>(playersById.Keys);

            for (ushort i = 0; i < playersById.Count; i++)
            {
                //CORREGIR....testesar
                if (readyImage[i].activeSelf == false && playersById[keys[i]].listo == false)
                {
                    completado = false;
                    return;
                }

            }

            //todos los jugadores al completo listos
            if (completado == true)
            {

                alAtaque();

            }
            else
            {

                return;
            }


            //readyImage[contadorJugadores].SetActive(
            //    !readyImage[contadorJugadores].activeSelf
            //);

            //thisPlayer.focusPlayer.GetComponent<Image>().enabled = false;

            //thisPlayer.selected = !thisPlayer.selected;

            //selectionAlAtaker.enabled = true;

            //if (thisPlayer.playerId - 1 == 0)
            //{

            //    explicacion[thisPlayer.playerId - 1].text = "PULSA B PARA DESMARCAR\nPULSA A PARA EMPEZAR";
            //}
            //else
            //{

            //    explicacion[thisPlayer.playerId - 1].text = "PULSA B PARA DESMARCAR";

            //}


        
        }


        
        

        
        
        
    }

    private void ButtonExit(InputAction.CallbackContext obj)
    {

        contadorJugadores--;
        if (contadorJugadores < 0)
        { 
            contadorJugadores = 0;
        }


        if (playersById.ContainsKey(obj.control.device.deviceId) == true)
        { 
            playersById.Remove(obj.control.device.deviceId);
        }


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


   




    public void AddPlayer(InputDevice inputDevice)
    {

        //if (variablesOverScenes.dictPlayers.ContainsKey(inputDevice.GUID.ToString()) == false)
        //{


        //    if (variablesOverScenes.dictPlayers.Count < VariablesOverScenes.MAX_PLAYERS)
        //    {

        //        ushort x = 0; ushort y = 0;
        //        switch (menuManager.countPlayers)
        //        {
        //            case 0: x = 0; y = 2; break;
        //            case 1: x = 5; y = 2; break;
        //            case 2: x = 0; y = 0; break;
        //            case 3: x = 5; y = 0; break;
        //            default: Debug.LogError("demasiados"); break;

        //        }

        //        variablesOverScenes.dictPlayers.Add(inputDevice.GUID.ToString(),
        //        new InfoPlayer(
        //            focusPlayers[menuManager.countPlayers],
        //            null,
        //            initialPlayerPosition[menuManager.countPlayers],
        //            //initialPlayerPosition[menuManager.countPlayers].GetComponent<MatrixCharacters>(),
        //            prefabColorsPlayers[menuManager.countPlayers],
        //            x, y,
        //            (ushort)(menuManager.countPlayers + 1),
        //            true,
        //            bigSelectionPlayers[menuManager.countPlayers],
        //            false
                    

        //        ));


        //        menuManager.countPlayers++;
        //        menuManager.inputDevices.Add(inputDevice);

        //    }

        //}

    }

    private void Update()
    {



        //if (menuManager.pantallaElegirMando.activeSelf == false) return;
        //if (InputManager.ActiveDevices.Count <= 0) return;



        //var activeInputDevice = InputManager.ActiveDevice;
       
        //if (variablesOverScenes.dictPlayers.ContainsKey(activeInputDevice.GUID.ToString()) == false)
        //{


        //    if (variablesOverScenes.dictPlayers.Count < VariablesOverScenes.MAX_PLAYERS)
        //    {


               


        //    }
        //    else
        //    {



        //    }





        //}


        //InfoPlayer thisPlayer = variablesOverScenes.dictPlayers[activeInputDevice.GUID.ToString()];

        //if (thisPlayer.playerId < 0 || thisPlayer.playerId > VariablesOverScenes.MAX_PLAYERS) return; //comprobaciones extra mas adelante

        


        //if (activeInputDevice.Action1.WasPressed && thisPlayer.isFirstMove == false)
        //{


        //    if (thisPlayer.playerId == 1 && thisPlayer.selected == true)
        //    {



        //        bool completado = true;
        //        List<string> keys = new List<string>(variablesOverScenes.dictPlayers.Keys);

        //        for (ushort i = 0; i < variablesOverScenes.dictPlayers.Count; i++)
        //        {


        //            //CORREGIR....testesar
        //            if (readyImage[i].activeSelf == false && variablesOverScenes.dictPlayers[keys[i]].isFirstMove == true)
        //            {
        //                completado = false;
        //                return;
        //            }

        //        }


        //        if (completado == true)
        //        {

        //            alAtaque();

        //        }
        //        else
        //        {

        //            return;
        //        }




        //    }
           
        //    readyImage[thisPlayer.playerId - 1].SetActive(
        //        !readyImage[thisPlayer.playerId - 1].activeSelf
        //    );

        //    thisPlayer.focusPlayer.GetComponent<Image>().enabled = false;

        //    thisPlayer.selected = !thisPlayer.selected;

        //    selectionAlAtaker.enabled = true;

        //    if (thisPlayer.playerId - 1 == 0)
        //    {

        //        explicacion[thisPlayer.playerId - 1].text = "PULSA B PARA DESMARCAR\nPULSA A PARA EMPEZAR";
        //    }
        //    else
        //    {

        //        explicacion[thisPlayer.playerId - 1].text = "PULSA B PARA DESMARCAR";

        //    }


        //}


        //if (activeInputDevice.Action2.WasPressed && thisPlayer.isFirstMove == false)
        //{


        //    readyImage[thisPlayer.playerId - 1].SetActive(false);

        //    thisPlayer.selected = false;
        //    thisPlayer.focusPlayer.GetComponent<Image>().enabled = true;
        //    selectionAlAtaker.enabled = false;
        //    explicacion[thisPlayer.playerId - 1].text = "PULSA A PARA MARCAR";

        //    print("selected=" + thisPlayer.selected);





        //}




        //if (thisPlayer.selected == true) return;

        //if (activeInputDevice.RightStick.WasPressed)
        //{


        //    rstick.Filter(activeInputDevice.RightStick, Time.deltaTime);
        //    ControlWithStickRaw(thisPlayer, rstick);


        //    return;


        //}

        //if (activeInputDevice.DPad.WasPressed)
        //{

        //    //print("dpad");

        //    dpad.Filter(activeInputDevice.Direction, Time.deltaTime);
        //    ControlWithDpadRaw(thisPlayer, dpad);

        //    return;

        //}
        


        //if(activeInputDevice.LeftStick.WasPressed)
        //{


        //    //print("leftstick");

        //    lstick.Filter(activeInputDevice.LeftStick, Time.deltaTime);
        //    ControlWithStickRaw(thisPlayer, lstick);
        //    return;


        //}


      


    }


    //private void ControlWithStickRaw(InfoPlayer thisPlayer, TwoAxisInputControl stick)
    //{

    //    //print("x=" + stick.Value.normalized.x + " Y=" + stick.Value.normalized.y);


    //    //if (stick.Value.normalized.y >= 0.5f)
    //    //{


    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {
    //    //        return;

    //    //    }

    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().up is null) == false)
    //    //    {

    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().up.down.taken = false;

    //    //    }

    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().up, thisPlayer.posX, thisPlayer.playerId - 1);
    //    //}

        
    //    //if (stick.Value.normalized.y <= -0.5f)
    //    //{

    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {
    //    //        return;

    //    //    }

    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().down is null) == false)
    //    //    {
    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().down.up.taken = false;
    //    //    }


    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().down, thisPlayer.posX, thisPlayer.playerId - 1);

    //    //}

    //    //if (stick.Value.normalized.x >= 0.5f)
    //    //{

    //    //    //print("derecha");

    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {

    //    //        if (thisPlayer.playerId < 1 || thisPlayer.playerId > VariablesOverScenes.MAX_PLAYERS) return;

    //    //        //nopair, p1 o p3
    //    //        if (thisPlayer.playerId % 2 != 0)
    //    //        {
    //    //            thisPlayer.isFirstMove = false;
    //    //            thisPlayer.focusPlayer.GetComponent<Image>().enabled = true;
    //    //            FirstUpdateUICharacters(thisPlayer.playerId - 1, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>());


    //    //        }
    //    //        return;


    //    //    }


    //    //    if (thisPlayer.posX >= 0 && thisPlayer.posX < 5)
    //    //    {
    //    //        thisPlayer.posX++;
    //    //    }



    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().right is null) == false)
    //    //    {

    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().right.left.taken = false;

    //    //    }

    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().right, thisPlayer.posX, thisPlayer.playerId -1);
    //    //}

    //    //if (stick.Value.normalized.x <= -0.5f)
    //    //{



    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {
    //    //        if (thisPlayer.playerId < 1 || thisPlayer.playerId > VariablesOverScenes.MAX_PLAYERS) return;

    //    //        //pair, p2 o p4
    //    //        if (thisPlayer.playerId % 2 == 0)
    //    //        {



    //    //            thisPlayer.isFirstMove = false;
    //    //            thisPlayer.focusPlayer.GetComponent<Image>().enabled = true;
    //    //            FirstUpdateUICharacters(thisPlayer.playerId - 1, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>());

    //    //        }
    //    //        return;


    //    //    }


    //    //    if (thisPlayer.posX > 0 && thisPlayer.posX <= 5)
    //    //    {
    //    //        thisPlayer.posX--;
    //    //    }


    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().left is null) == false)
    //    //    {
    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().left.right.taken = false;

    //    //    }

    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().left, thisPlayer.posX, thisPlayer.playerId - 1);


    //    //}


    //}

    //private void ControlWithDpadRaw(InfoPlayer thisPlayer, TwoAxisInputControl stick)
    //{

    //    //print("x=" + stick.X + " Y=" + stick.Value.normalized.y);


    //    //if (stick.Y == 1f)
    //    //{

    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {
    //    //        return;

    //    //    }

    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().up is null) == false)
    //    //    {

    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().up.down.taken = false;

    //    //    }

    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().up, thisPlayer.posX, thisPlayer.playerId -1 );
    //    //    return;
    //    //}


    //    //if (stick.Y == -1f)
    //    //{

    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {
    //    //        return;

    //    //    }


    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().down is null) == false)
    //    //    {
    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().down.up.taken = false;
    //    //    }


    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().down, thisPlayer.posX, thisPlayer.playerId - 1);
    //    //    return;
    //    //}

    //    //if (stick.X == 1f)
    //    //{

    //    //    //print("derecha");

    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {

    //    //        if (thisPlayer.playerId < 1 || thisPlayer.playerId > VariablesOverScenes.MAX_PLAYERS) return;
    //    //        //nopair, p1 o p3
    //    //        if (thisPlayer.playerId % 2 != 0)
    //    //        {
    //    //            thisPlayer.isFirstMove = false;
    //    //            thisPlayer.focusPlayer.GetComponent<Image>().enabled = true;

    //    //            FirstUpdateUICharacters(thisPlayer.playerId - 1, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>());


    //    //        }
    //    //        return;


    //    //    }



    //    //    if (thisPlayer.posX >= 0 && thisPlayer.posX < 5)
    //    //    {
    //    //        thisPlayer.posX++;
    //    //    }



    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().right is null) == false)
    //    //    {

    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().right.left.taken = false;

    //    //    }

    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().right, thisPlayer.posX, thisPlayer.playerId -1 );
    //    //    return;
    //    //}

    //    //if (stick.X == -1f)
    //    //{



    //    //    if (thisPlayer.isFirstMove == true)
    //    //    {

    //    //        if (thisPlayer.playerId < 1 || thisPlayer.playerId > VariablesOverScenes.MAX_PLAYERS) return;
    //    //        //pair, p2 o p4
    //    //        if (thisPlayer.playerId % 2 == 0)
    //    //        {
    //    //            thisPlayer.isFirstMove = false;
    //    //            thisPlayer.focusPlayer.GetComponent<Image>().enabled = true;

    //    //            FirstUpdateUICharacters(thisPlayer.playerId - 1, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>());


    //    //        }
    //    //        return;


    //    //    }


    //    //    if (thisPlayer.posX > 0 && thisPlayer.posX <= 5)
    //    //    {
    //    //        thisPlayer.posX--;
    //    //    }


    //    //    if ((thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().left is null) == false)
    //    //    {
    //    //        thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().left.right.taken = false;

    //    //    }

    //    //    MoveFocus(thisPlayer.focusPlayer, thisPlayer.focusPlayer.GetComponent<MatrixCharacters>().left, thisPlayer.posX, thisPlayer.playerId - 1);
    //    //    return;

    //    //}


    //}

    private void MoveFocus(GameObject focus, MatrixCharacters matrixPos, ushort posX, int playerId)
    {


        //if ((matrixPos is null) == false)
        //{


        //    if (playerId < 0 || playerId > VariablesOverScenes.MAX_PLAYERS - 1) return;


        //    if (posX % 2 == 0)
        //    {

        //        focus.GetComponent<Image>().sprite = pairSprite;
        //    }
        //    else
        //    {

        //        focus.GetComponent<Image>().sprite = noPairSprite;
        //    }


        //    focus.transform.position = matrixPos.gameObject.transform.position;
        //    matrixPos.taken = true;

        //    var tFocus = focus.GetComponent<MatrixCharacters>();

        //    tFocus.up = matrixPos.up;
        //    tFocus.down = matrixPos.down;
        //    tFocus.right = matrixPos.right;
        //    tFocus.left = matrixPos.left;


        //    tFocus.health = matrixPos.health;
        //    tFocus.healthMax = matrixPos.healthMax;
        //    tFocus.energy = matrixPos.energy;
        //    tFocus.energyMax = matrixPos.energyMax;
        //    tFocus.power = matrixPos.power;
        //    tFocus.powerMax = matrixPos.powerMax;
        //    tFocus.defense = matrixPos.defense;
        //    tFocus.defenseMax = matrixPos.defenseMax;
        //    tFocus.nameCharacter = matrixPos.nameCharacter;

        //    charactersImagesBig[playerId].sprite = matrixPos.imageCharacter;
        //    barraHP[playerId].fillAmount = matrixPos.health / matrixPos.healthMax;
        //    barraDefense[playerId].fillAmount = matrixPos.defense / matrixPos.defenseMax;
        //    barraPower[playerId].fillAmount = matrixPos.power / matrixPos.powerMax;
        //    nameCharactersPlayer[playerId].text = matrixPos.nameCharacter;






        //}

       
        
    }

    private void FirstUpdateUICharacters(int playerId, MatrixCharacters matrixPlayer)
    {


      
        {



            var tInitial = initialPlayerPosition[playerId].GetComponent<MatrixCharacters>();

            charactersImagesBig[playerId].sprite = tInitial.imageCharacter;

            barraHP[playerId].fillAmount = tInitial.health /
                tInitial.healthMax;

            barraDefense[playerId].fillAmount = tInitial.defense /
                tInitial.defenseMax;

            barraPower[playerId].fillAmount = tInitial.power /
                tInitial.powerMax;
            nameCharactersPlayer[playerId].text = tInitial.nameCharacter;



            matrixPlayer.health = tInitial.health;
            matrixPlayer.healthMax = tInitial.healthMax;
            matrixPlayer.energy = tInitial.energy;
            matrixPlayer.energyMax = tInitial.energyMax;
            matrixPlayer.power = tInitial.power;
            matrixPlayer.powerMax = tInitial.powerMax;
            matrixPlayer.defense = tInitial.defense;
            matrixPlayer.defenseMax = tInitial.defenseMax;
            matrixPlayer.nameCharacter = tInitial.nameCharacter;


        }



        mandosImage[playerId].sprite = prefabMandosImage[0];
        mandosImage[playerId].gameObject.SetActive(true);

        
        explicacion[playerId].text = "PULSA A PARA MARCAR";

        namePlayers[playerId].text = "P" + (playerId + 1);


    }

    public void alAtaque()
    {
        //int id = 0;


        //foreach (var playerInfo in variablesOverScenes.dictPlayers)
        //{
        //    if (playerInfo.Key is null) continue;


           
        //    var newPlayer = GameObject.Instantiate(prefabPlayer, prefabPlayer.transform.position, Quaternion.identity);
        //    playerInfo.Value.playerGameObject = newPlayer;


        //    var itemHealth = newPlayer.GetComponent<Health>();
        //    var matrixPlayer = playerInfo.Value.focusPlayer.GetComponent<MatrixCharacters>();

        //    itemHealth.HP = matrixPlayer.health;
        //    itemHealth.maxHP = matrixPlayer.health;
        //    itemHealth.energy = matrixPlayer.energy;
        //    itemHealth.energyMax = matrixPlayer.energy;
        //    itemHealth.power = matrixPlayer.power;
        //    itemHealth.powerMax = matrixPlayer.power;
        //    itemHealth.defence = matrixPlayer.defense;
        //    itemHealth.defenceMax = matrixPlayer.defense;




        //    newPlayer.GetComponent<PlayerGravity>().playerColor = playerInfo.Value.colorPlayer;
        //    newPlayer.GetComponent<SpriteRenderer>().color = playerInfo.Value.colorPlayer;
        //    print("name=" + prefabPlayer.GetComponentsInChildren<SpriteRenderer>(true)[2].name);
        //    newPlayer.GetComponentsInChildren<SpriteRenderer>(true)[2].color = playerInfo.Value.colorPlayer;
        //    newPlayer.GetComponent<PlayerGravity>().gamepadPosition = id;
        //    newPlayer.SetActive(false);
        //    id++;

        //}

        
        //menuManager.ClickPlay();

    }

    //private void DisplayFX(ushort index)
    //{

    //    focusButtonX[0].Play();


    //}

    

    //public void Personaje4()
    //{
    //    DisplayFX(3);
    //    InfoJugador infoJugador = new InfoJugador();
    //    infoJugador.x = 3;
    //    jugadores[0] = infoJugador;

    //}


    //public void Personaje5()
    //{
    //    DisplayFX(4);
    //    InfoJugador infoJugador = new InfoJugador();
    //    infoJugador.x = 4;
    //    jugadores[0] = infoJugador;
    //}

    //public void Personaje6()
    //{

    //    DisplayFX(5);
    //    InfoJugador infoJugador = new InfoJugador();
    //    infoJugador.x = 5;
    //    jugadores[0] = infoJugador;
    //}

    //public void Personaje1()
    //{
    //    print("personaje1");
    //    DisplayFX(0);
    //    InfoJugador infoJugador = new InfoJugador();
    //    infoJugador.x = 0;
    //    jugadores[0] = infoJugador;
    //}

    //public void Personaje2()
    //{
    //    DisplayFX(1);

    //    InfoJugador infoJugador = new InfoJugador();
    //    infoJugador.x = 1;
    //    jugadores[0] = infoJugador;
    //}

    //public void Personaje3()
    //{
    //    DisplayFX(2);
    //    InfoJugador infoJugador = new InfoJugador();
    //    infoJugador.x = 2;
    //    jugadores[0] = infoJugador;

    //}


    public void ClickPersonajes(int i)
    {

        if (i == 0) return;
        //teclado

        if (contadorJugadores >= JUGADORES_MAXIMO)
        {
            print("no se admiten mas jugadores");
            return;
        }



        //if (variablesOverScenes.dictPlayers.ContainsKey("teclado") == true)
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
        //    variablesOverScenes.dictPlayers.Add("teclado",
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



        //InfoPlayer thisPlayer = variablesOverScenes.dictPlayers["teclado"];



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





}