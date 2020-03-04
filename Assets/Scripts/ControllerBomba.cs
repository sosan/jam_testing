using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx.Async;
using Photon.Pun;


public class ControllerBomba : MonoBehaviour, IPunInstantiateMagicCallback
{

    [SerializeField] public ControllerPlayer controllerPlayer = null;
    [SerializeField] public GameController gameController = null;
    [SerializeField] private PhotonView photonView = null;
    [SerializeField] private ushort timeExplode = 3;
    [SerializeField] private ParticleSystem explosion = null;
    [SerializeField] private ParticleSystem.MainModule mainmodule;
    [SerializeField] private string ignoreTag;
    [SerializeField] private LayerMask raycastLayerMask = -1;
    [SerializeField] public Color color = Color.white;
    [SerializeField] public Color colorInicial = Color.white;
    [SerializeField] public SpriteRenderer cruz = null;
    [SerializeField] private Rigidbody2D rigid = null;
    [SerializeField] private SpriteRenderer fondo = null;



    private bool lerpingColor = false;
    
    private float progresoLerp = 0;
    private float interpolateDuration = 0.2f;
    private bool isExplosion = false;

    public bool isOnline = false;
    public int viewidPlayer = -1;
    public bool isShooted = false;

    
    short bloquesAmarillos = 0;
    short bloquesAzules = 0;
    short bloquesRojos = 0; 
    short bloquesBlancos = 0; 

    private List<string> quitarAmarillos = new List<string>();
    private List<string> quitarAzules = new List<string>();
    private List<string> quitarRojos = new List<string>();
    private List<string> quitarBlancos = new List<string>();

    private void Awake()
    {
        
        if(gameController is null == true)
        { 
        
            gameController = GameObject.FindObjectOfType<GameController>();
        
        }

        mainmodule = explosion.main;
        mainmodule.startColor = new ParticleSystem.MinMaxGradient(color);

    }



    private async void Start()
    {
        
        isExplosion = false;
        isShooted = false;
        await UniTask.Delay(TimeSpan.FromSeconds(timeExplode - 1));
        lerpingColor = true;
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        if (isExplosion == false)
        {

            if (isOnline == true)
            {
                CalcularExplosionOnline();

            }
            else
            {
                CalcularExplosion();

            }
            
            
        }
        
        
    }

    public async void CalcularExplosionOnline()
    {
        if (isShooted == true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }

        try
        {
            
            if (controllerPlayer is null == true)
            { 

                print("viewidPlayer=" + viewidPlayer);
                controllerPlayer = PhotonNetwork.GetPhotonView(viewidPlayer).gameObject.GetComponent<ControllerPlayer>();
                    
                    
            }
            
            isExplosion = true;
            var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(2, 2), 0, layerMask: raycastLayerMask);
            if (hits is null == false)
            {
            
                
                //print("longitud hits=" + hits.Length);
                //ushort numerohitsefectivo = 0;

                for (ushort i = 0 ; i < hits.Length; i++)
                { 
                    //print("hits" + i + " nombre" + hits[i].name + " tag=" + hits[i].tag);
                    if (hits[i].CompareTag("fondo") == true ||
                        hits[i].CompareTag("amarillo") == true ||
                        hits[i].CompareTag("azul") == true ||
                        hits[i].CompareTag("rojo") == true ||
                        hits[i].CompareTag("blanco") == true 
                        )
                    { 
                        hits[i].transform.gameObject.GetComponent<SpriteRenderer>().color = color;
                    
                        //numerohitsefectivo++;
                        //restamos puntuacion al enemigo
                        switch (hits[i].transform.tag)
                        {
                            case "amarillo": quitarAmarillos.Add(hits[i].gameObject.name); break;
                            case "azul": quitarAzules.Add(hits[i].gameObject.name); break;
                            case "rojo": quitarRojos.Add(hits[i].gameObject.name);  break;
                            case "blanco": quitarBlancos.Add(hits[i].gameObject.name); break;
                            case "fondo": break;

                        }


                        hits[i].transform.tag = controllerPlayer.player.nombreColorPlayer.ToString();
                    
                    }

                    

            
                }

                photonView.RPC("DatosBomba", RpcTarget.AllBuffered, 
                    quitarAmarillos.ToArray(), 
                    quitarAzules.ToArray(), 
                    quitarRojos.ToArray(), 
                    quitarBlancos.ToArray()
                    , viewidPlayer
                    
                    );

            }



            
        }
        catch (MissingReferenceException)
        {


        }



    }

    public async void CalcularExplosion()
    { 


        if (isShooted == true)
        { 
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }

        try
        { 
            isExplosion = true;
            explosion.Play();
            var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(2, 2), 0, layerMask: raycastLayerMask);
            if (hits is null == false)
            {
            
                //play explision
                //explosion.Play();
                //await UniTask.Delay(TimeSpan.FromSeconds(explosion.main.duration));
                print("longitud hits=" + hits.Length);

                

                for (ushort i = 0 ; i < hits.Length; i++)
                { 
                    //print("hits" + i + " nombre" + hits[i].name);
                    if (hits[i].CompareTag("fondo") == true ||
                        hits[i].CompareTag("amarillo") == true ||
                        hits[i].CompareTag("azul") == true ||
                        hits[i].CompareTag("rojo") == true ||
                        hits[i].CompareTag("blanco") == true 
                    )
                    { 

                        hits[i].transform.gameObject.GetComponent<SpriteRenderer>().color = color;
                    

                        //restamos puntuacion al enemigo
                        switch (hits[i].transform.tag)
                        {
                            case "amarillo": gameController.bloquesAmarillos -= 1; break;
                            case "azul": gameController.bloquesAzules  -= 1; break;
                            case "rojo": gameController.bloquesRojos -= 1;break;
                            case "blanco":gameController.bloquesBlancos -= 1; break;
                            case "fondo": break;
                
                        }

                        if (gameController.bloquesAmarillos < 0)
                        { 
                        gameController.bloquesAmarillos = 0;
                
                        }

                        if (gameController.bloquesAzules < 0)
                        { 
                        gameController.bloquesAzules = 0;
                
                        }

                        if (gameController.bloquesRojos < 0)
                        { 
                        gameController.bloquesRojos = 0;
                
                        }

                        if (gameController.bloquesBlancos < 0)
                        { 
                        gameController.bloquesBlancos = 0;
                
                        }

                   

                        hits[i].transform.tag = controllerPlayer.player.nombreColorPlayer.ToString();

                    

                        //subimos puntuacion al player
                        switch(controllerPlayer.player.nombreColorPlayer)
                        {
                        case nombreColores.amarillo: gameController.bloquesAmarillos += 1; break;
                        case nombreColores.azul: gameController.bloquesAzules  += 1; break;
                        case nombreColores.rojo: gameController.bloquesRojos += 1;break;
                        case nombreColores.blanco:gameController.bloquesBlancos += 1; break;
                        case nombreColores.fondo: break;
                
                        }

                        if (gameController.bloquesAmarillos > 162)
                        { 
                        gameController.bloquesAmarillos = 162;
                
                        }

                        if (gameController.bloquesAzules > 162)
                        { 
                        gameController.bloquesAzules = 162;
                
                        }

                        if (gameController.bloquesRojos > 162)
                        { 
                        gameController.bloquesRojos = 162;
                
                        }

                        if (gameController.bloquesBlancos > 162)
                        { 
                        gameController.bloquesBlancos = 162;
                
                        }

                    }

            
                }
        
            }

            Transform[] childrens = this.gameObject.GetComponentsInChildren<Transform>();
            for (ushort i = 0; i < childrens.Length; i++)
            { 
                Destroy(childrens[i].gameObject);
        
            }


            Destroy(this.gameObject);
        
        }
        catch (MissingReferenceException)
        { 
            
        
        }

        
    
    
    
    }

    private void Update()
    {

        if (lerpingColor == true)
        {
            progresoLerp = Mathf.PingPong(Time.time, interpolateDuration) / interpolateDuration;
            cruz.color = Color.Lerp(colorInicial, color, progresoLerp);

        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (explosion == true) return;

        if (collision.CompareTag("Bullet") == true)
        { 
            isShooted = true;
            rigid.isKinematic = false;
            rigid.mass = 5;
            var contact = collision.ClosestPoint(this.transform.position).normalized;
            print(contact.ToString());
            
            short xTemp = 4;
            if (contact.x <= 0)
            { 
                xTemp = -4;
            
            }
            float rnd = UnityEngine.Random.Range(-1f, 1f);
            

            rigid.AddForce( new Vector2(xTemp, rnd) * 8f, ForceMode2D.Impulse);
            rigid.isKinematic = true;


        }

    }



    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
         object[] data = photonView.InstantiationData;

        Color t_color = new Color((float)data[0], (float)data[1], (float)data[2]);
        viewidPlayer = (int)data[3];

        fondo.color = t_color;
        cruz.color = t_color;
        colorInicial = t_color;
        color = t_color;

        //print("idplayerowner" + viewidPlayer);

        var player = PhotonNetwork.GetPhotonView(viewidPlayer).gameObject;
        controllerPlayer = player.GetComponent<ControllerPlayer>();




    }

    private void ProcesarBloques(object[] bloques, Color colorBloque, string tag)
    { 
    
        for (ushort i = 0; i < bloques.Length; i++)
        { 
            var index = (string)bloques[i];
            gameController.listadoBoxes[index].GetComponent<SpriteRenderer>().color = colorBloque;
            gameController.listadoBoxes[index].tag = tag;
            
        }
    }

    [PunRPC]
    private void DatosBomba(object[] datosAmarillos, object[] datosAzules, object[] datosRojos, object[] datosBlancos, int viewid, PhotonMessageInfo info )
    { 
        print("datos bomba dentro de la bomba");
        //bomba.GetComponent<ControllerBomba>().viewidPlayer = bomba.GetComponent<PhotonView>().ViewID;
                    
        ////local

        //bomba.GetComponent<SpriteRenderer>().color =  player.colorPlayer;
        //bomba.GetComponent<ControllerBomba>().color = player.colorPlayer;
        //bomba.GetComponent<ControllerBomba>().cruz.color = player.colorPlayer;
        //if (photonView.IsMine == true)
        { 
            print("dentro");
            gameController.bloquesAmarillos += (short)datosAmarillos.Length;
            gameController.bloquesAzules += (short)datosAzules.Length;
            gameController.bloquesRojos += (short)datosRojos.Length;
            gameController.bloquesBlancos += (short)datosBlancos.Length;


            ProcesarBloques(datosAmarillos, gameController.elegirPersonaje.prefabColorsPlayers[0], "amarillo");
            ProcesarBloques(datosAzules, gameController.elegirPersonaje.prefabColorsPlayers[1], "azul");
            ProcesarBloques(datosRojos, gameController.elegirPersonaje.prefabColorsPlayers[2], "rojo");
            ProcesarBloques(datosBlancos, gameController.elegirPersonaje.prefabColorsPlayers[3], "blanco");

        
        
        }




        if (photonView.IsMine == true)
        { 

            print("es mio y lo destruyo?");
            PhotonNetwork.Destroy(photonView);
        
        }
        
        

    
    }

}
