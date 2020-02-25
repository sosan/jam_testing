using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InControlActions;
using UnityEngine.InputSystem;
using UniRx.Async;


//public class InfoJugador
//{
//    public Color color;
//    public int idDevice;
//    public int x;
//    public bool listo = false;
//    public GameObject objPlayer = null;

//}

public class InfoPlayer
{
    public GameObject focusPlayer;
    public GameObject playerGameObject;
    public GameObject playerPos;
    public Color colorPlayer;
    public ushort posX;
    public ushort posY;

    public float lastPosX = 0;
    public float lastPosY = 0;
    public float currentPosX = 0;
    public float currentPosY = 0;

    public ushort posicionPlayer;
    public bool isFirstMove;
    public bool listo;
    public Image bigSelectionPlayer;
    public bool selected;
    public string namePlayer;

    public int expCurrent;
    public int expMax;
    public int levelCurrent;
    public int levelMax;

    public bool vacio = true;


    public int indexShotPrefab;

    public float fireCooldown = 0;
    public float speedMovement = 0;
    public float powerDamage = 0;
    public float durationShotSeconds = 0;
    public float bombCooldown = 0;
    public float defense = 0;
    public float defenseMax = 0;


    public bool bot;


    public InfoPlayer() { }

    public InfoPlayer(GameObject focusPlayer, GameObject playerGameObject, GameObject playerPos, Color colorPlayer, ushort posX, ushort posY,
        ushort posicionPlayer, bool isFirstMove, Image bigSelectionPlayer, bool selected, bool listo, bool bot)
    {
        this.focusPlayer = focusPlayer;
        this.playerGameObject = playerGameObject;
        this.playerPos = playerPos;
        this.colorPlayer = colorPlayer;
        this.posX = posX;
        this.posY = posY;
        this.posicionPlayer = posicionPlayer;
        this.isFirstMove = isFirstMove;
        this.bigSelectionPlayer = bigSelectionPlayer;
        this.listo = listo;
        this.bot = bot;
    }


}



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

    [Header("texto empezar lucha")]
    [SerializeField] private UILocalization textoempezarlucha = null;


    [SerializeField] private GameObject[] listoMensaje = null;

    public Image[] mandosImage;
    public GameObject[] entrada_txt = null;
    public Sprite[] prefabMandosImage;


    //private TwoAxisInputControl lstick = new TwoAxisInputControl();
    //private TwoAxisInputControl rstick = new TwoAxisInputControl();
    //private TwoAxisInputControl dpad = new TwoAxisInputControl();

    // fundamentales
    [SerializeField] private const ushort JUGADORES_MAXIMO = 4;
    private InfoPlayer[] jugadores = new InfoPlayer[JUGADORES_MAXIMO];
    [HideInInspector] public ushort contadorJugadores = 0;
    [HideInInspector] public Dictionary<int, InfoPlayer> dictPlayers = new Dictionary<int, InfoPlayer>();

    //---------
    public Animation[] focusButtonX;

    public GameObject[] readyImage = null;

    public Color[] prefabColorsPlayers;
    public GameObject prefabPlayer = null;
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
    private MatrixCharacters playerKeyboardLastTaken = null;

    public Image[] recuadros = null;


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
            entrada_txt[i].SetActive(true);
            focusPlayers[i].SetActive(false);
            recuadros[i].color = Color.white;
            listoMensaje[i].SetActive(false);
        }

        textoempezarlucha.key = "esperandojugadores";

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

            //if (i % 2 == 0)
            //{
            //    explicacion[i].text = "VES A DERECHA PARA AÑADIR JUGADOR";


            //}
            //else
            //{

            //    explicacion[i].text = "VES A IZQUIERDA PARA AÑADIR JUGADOR";
            //}

            //namePlayers[i].text = "NONE";



        }

        



    }

    public void InitActions()
    { 
        inputActions.Enable();
    
    }

    private void ControlDpad(InputAction.CallbackContext obj)
    {
        if (dictPlayers.ContainsKey(obj.control.device.deviceId) == false)
        { 
            return;
        }

        Vector2 move = obj.ReadValue<Vector2>();

        
        ControlEleccionPersonajes(move, dictPlayers[obj.control.device.deviceId]);

    }

    private void ControlEleccionPersonajes(Vector2 posicion, InfoPlayer player)
    { 
    
        if (posicion.normalized.y == 1f)
        { 
            

            if ((player.focusPlayer.GetComponent<MatrixCharacters>().up is null) == false)
            {

                player.focusPlayer.GetComponent<MatrixCharacters>().up.down.taken = false;

            }
            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().up, player.posX, player.posicionPlayer );


            print("arriba");
            return;
        }

        if (posicion.normalized.y == -1f)
        { 
            print("abajo");
            
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().down is null) == false)
            {
                player.focusPlayer.GetComponent<MatrixCharacters>().down.up.taken = false;
            }


            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().down, player.posX, player.posicionPlayer);
            return;
        
        }

        if (posicion.normalized.x == 1f)
        { 

            if (player.posX >= 0 && player.posX < 5)
            {
                player.posX++;
            }


            print("derecha");
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().right is null) == false)
            {

                player.focusPlayer.GetComponent<MatrixCharacters>().right.left.taken = false;

            }

            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().right, player.posX, player.posicionPlayer );
            return;

        }

        if (posicion.normalized.x == -1f)
        { 
        
            if (player.posX > 0 && player.posX <= 5)
            {
                player.posX--;
            }

            print("izquierda");
            if ((player.focusPlayer.GetComponent<MatrixCharacters>().left is null) == false)
            {
                player.focusPlayer.GetComponent<MatrixCharacters>().left.right.taken = false;

            }

            MoveFocus(player.focusPlayer, player.focusPlayer.GetComponent<MatrixCharacters>().left, player.posX, player.posicionPlayer);
            return;


        }





    }

    
    private void BotonSur(InputAction.CallbackContext obj)
    {

        print(dictPlayers.ContainsKey(obj.control.device.deviceId));

        if (dictPlayers.ContainsKey(obj.control.device.deviceId) == false)
        { 
            print("insertado: " + contadorJugadores + " device=" + obj.control.device.deviceId);
            //insertar player
            

            
            contadorJugadores++;
            if (contadorJugadores > JUGADORES_MAXIMO)
            { 
        
                contadorJugadores = JUGADORES_MAXIMO;
                recuadros[3].color = Color.black;
                return;

        
            }

            short posicionLibre = GetPosicionLibre();
                      

            if (posicionLibre == -1)
            { 
                return;
            
            }
            
            jugadores[posicionLibre].vacio = false;
            recuadros[posicionLibre].gameObject.SetActive(false);

            //jugadores[contadorJugadores].idDevice = obj.control.device.deviceId;
            HacerVibrarMando(obj.control.device.deviceId);

            entrada_txt[posicionLibre].SetActive(false);
            panel_players[posicionLibre].SetActive(true);
            //focusPlayers[contadorJugadores - 1].transform.position = initialPlayerPosition[contadorJugadores - 1].transform.position;
            focusPlayers[posicionLibre].SetActive(true);
            mandosImage[posicionLibre].gameObject.SetActive(true);

            AddPlayer(posicionLibre, obj.control.device.deviceId, false);
            (ushort, ushort)posicion = PosicionPlayerMatrix(posicionLibre);
            
            MoveFocus(focusPlayers[posicionLibre], initialPlayerPosition[posicionLibre].GetComponent<MatrixCharacters>(), posicion.Item1, posicionLibre);
            
            

            
            


        }
        else
        { 
        
            print("player listo: " + contadorJugadores + " device=" + obj.control.device.deviceId);
            

            
            if (dictPlayers[obj.control.device.deviceId].listo == true)
            { 

                print("3 vez");
                print("devices count=" + Gamepad.all.Count);
                print("contadojugadores=" + contadorJugadores);
                // tercera vez
                //ComprobarPersonajesListo();
            
            }
            else
            { 
                print("devices count=" + Gamepad.all.Count);
                print("contadojugadores=" + contadorJugadores);

                //ejecutar que el player esta listo
                dictPlayers[obj.control.device.deviceId].listo = true;
                
                HacerVibrarMando(obj.control.device.deviceId);
                
                ushort o = dictPlayers[obj.control.device.deviceId].posicionPlayer;
                listoMensaje[o].SetActive(true);
                readyImage[o].SetActive(
                    !readyImage[o].activeSelf
                );

                print("devices count=" + Gamepad.all.Count);
                print("contadojugadores=" + contadorJugadores);

                ComprobarPersonajesListo();
               
                






            }



        
        }


        
        

        
        
        
    }

    private void ButtonExit(InputAction.CallbackContext obj)
    {

        if (dictPlayers.ContainsKey(obj.control.device.deviceId) == true)
        { 
            
            HacerVibrarMando(obj.control.device.deviceId);
            
            contadorJugadores--;
            if (contadorJugadores < 0)
            { 
                contadorJugadores = 0;
            }

            ushort o = dictPlayers[obj.control.device.deviceId].posicionPlayer;

            listoMensaje[o].SetActive(false);
            readyImage[o].SetActive(false);


            recuadros[o].gameObject.SetActive(true);
            panel_players[o].SetActive(false);
            entrada_txt[o].SetActive(true);
            focusPlayers[o].SetActive(false);
            focusPlayers[o].transform.position = initialPlayerPosition[o].transform.position;
            
            dictPlayers.Remove(obj.control.device.deviceId);
            


        }


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

    private short GetPosicionLibre()
    { 
    
        //short posicionLibre = -1;

        for(short i = 0; i < JUGADORES_MAXIMO; i++)
        { 
            
            if (jugadores[i].vacio == true)
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
    public void AddPlayer(int contadorJugadores, int deviceId, bool isBot )
    {
        ushort x = 0; 
        ushort y = 0;
        
        switch (contadorJugadores)
        {
            case 0: x = 0; y = 2; break;
            case 1: x = 5; y = 2; break;
            case 2: x = 0; y = 0; break;
            case 3: x = 5; y = 0; break;
            default: Debug.LogError("demasiados"); break;

        }

        dictPlayers.Add(deviceId, new InfoPlayer(
                focusPlayers[contadorJugadores],
                null,
                initialPlayerPosition[contadorJugadores],
                prefabColorsPlayers[contadorJugadores],
                x, y,
                (ushort)(contadorJugadores),
                true,
                bigSelectionPlayers[contadorJugadores],
                false,
                false,
                isBot

            ));
            
       

    }

   


    private void MoveFocus(GameObject focus, MatrixCharacters matrixPos, ushort posX, int posicionPlayer)
    {

        print("nombre=" + focus.name + " matriz=" + matrixPos + " posX=" + posX + " playerID=" + posicionPlayer);
        
        if ((matrixPos is null) == false)
        {


            //if (playerId < 0 || playerId > JUGADORES_MAXIMO - 1) return;


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


    private void ComprobarPersonajesListo()
    { 
        if (Gamepad.all.Count == contadorJugadores)
        { 
            
            //comprobar que esten todos listos
            bool completado = true;
            List<int> keys = new List<int>(dictPlayers.Keys);

            for (ushort i = 0; i < keys.Count; i++)
            {
                //CORREGIR....testesar
                if (dictPlayers[keys[i]].listo == false)
                {
                    completado = false;
                    return;
                }

            }

            //todos los jugadores al completo listos
            if (completado == true)
            {
                print("al ataker");
                RellenarBots();
                alAtaque();

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

        //        contadorJugadores++;


        //    }

        //}
    
    
    }


    public void alAtaque()
    {


        for(ushort i = 0; i < jugadores.Length; i++)
        { 
            if (jugadores[i].listo == true)
            { 
            
                
            
            }
        
        }




        //int id = 0;
        List<int> keys = new List<int>(dictPlayers.Keys);


        for(ushort i = 0; i < keys.Count; i++)
        { 

            ----------

            GameObject playerGo = GameObject.Instantiate(prefabPlayer, gameController.initialPlayerPositions[i].position, Quaternion.identity);

            var controllerplayer = playerGo.GetComponent<ControllerPlayer>().player;
            var configplayer = dictPlayers[keys[i]].focusPlayer.GetComponent<MatrixCharacters>();


            controllerplayer.fireCooldown = configplayer.fireCooldown;
            controllerplayer.speedMovement = configplayer.speedMovement;
            controllerplayer.powerDamage = configplayer.power;
            controllerplayer.durationShotSeconds = configplayer.durationShotSeconds;
            controllerplayer.bombCooldown = configplayer.bombCooldown;
            controllerplayer.defense = configplayer.defense;
            controllerplayer.defenseMax = configplayer.defenseMax;

            playerGo.GetComponent<SpriteRenderer>().color = dictPlayers[keys[i]].colorPlayer;




            dictPlayers[keys[i]].playerGameObject = playerGo;
        
        }
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