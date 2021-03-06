﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx.Async;
using Photon.Pun;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class ControllerDestroyer :  MonoBehaviourPun, IPunInstantiateMagicCallback //, IPunObservable
{
    [SerializeField] public ControllerPlayer controllerplayer = null;
    [SerializeField] private ushort timeExplode = 3;
    [SerializeField] private ParticleSystem explosion = null;
    [SerializeField] private string ignoreTag;
    [SerializeField] private LayerMask raycastLayerMask = -1;
    [SerializeField] public Color color = Color.white;
    [SerializeField] public Color colorInicial = Color.white;
    [SerializeField] public SpriteRenderer cruz = null;


    private bool lerpingColor = false;

    private float progresoLerp = 0;
    private float interpolateDuration = 0.2f;
    private GameController gameController = null;


    private void Awake()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
    }




    // Start is called before the first frame update
    private async void Start()
    {



        
        await UniTask.Delay(TimeSpan.FromSeconds(timeExplode - 1));
        lerpingColor = true;
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        
        var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(2, 2), 0, layerMask: raycastLayerMask);
        if (hits is null == false)
        {
            
            //play explision
            //explosion.Play();
            //await UniTask.Delay(TimeSpan.FromSeconds(explosion.main.duration));
            //List<Transform> listadoObjetosBorrar = new List<Transform>();

            for (ushort i = 0 ; i < hits.Length; i++)
            { 

                //print("hit name=" + hits[i].name + " tag=" + hits[i].tag );
                if (hits[i].CompareTag("amarillo") == true ||  hits[i].CompareTag("azul") == true ||
                    hits[i].CompareTag("rojo") == true || hits[i].CompareTag("blanco") == true
                    )
                { 
                    hits[i].transform.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    hits[i].transform.tag = nombreColores.hueco.ToString();

                
                }

                if (hits[i].CompareTag("Player") == true)
                {
                    
                    //teletransportar al jugador al inicio?
                    hits[i].transform.position = gameController.initialPlayerPositions[hits[i].GetComponent<ControllerPlayer>().posicion].position;



                }
                
                //tienen hijos
                if (hits[i].CompareTag("powerup") == true || hits[i].CompareTag("bomba") == true || hits[i].CompareTag("Bullet") == true)
                {
                    Destroy(hits[i].gameObject);
                    //hits[i].gameObject.SetActive(false);                   
                    //DestruirObjetoConHijos(hits[i].transform);
                }


                //if ()
                //{ 
                //    print("destruir bullet");
                //    //listadoObjetosBorrar.Add(hits[i].transform);

                //    //hits[i].gameObject.SetActive(false);
                //    Destroy(hits[i].gameObject);

                //}


            }

            //controllerplayer.gameController.DestruirObjeto(listadoObjetosBorrar);
        
        }



        //DestruirObjetoConHijos(this.gameObject.transform);

        //Transform[] childrens = this.gameObject.GetComponentsInChildren<Transform>();
        //for (ushort i = 0; i < childrens.Length; i++)
        //{ 
        //    Destroy(childrens[i].gameObject);

        //}

        Destroy(this.gameObject);

    }


    //private void DestruirObjetoConHijos(Transform objeto)
    //{
    //    Transform[] childrens = objeto.GetComponentsInChildren<Transform>();
    //    for (ushort i = 0; i < childrens.Length; i++)
    //    {

    //        childrens[i].gameObject.SetActive(false);
    //        Destroy(childrens[i].gameObject);

    //    }

    //    //await UniTask.Delay(TimeSpan.FromSeconds(1));
    //    if (objeto is null == false)
    //    { 
    //        Destroy(objeto.gameObject);
    //    }



    //}



    private void Update()
    {

        if (lerpingColor == true)
        {
            progresoLerp = Mathf.PingPong(Time.time, interpolateDuration) / interpolateDuration;
            cruz.color = Color.Lerp(colorInicial, color, progresoLerp);

        }


    }


   
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = photonView.InstantiationData;

        Color color = new Color((float)data[0], (float)data[1], (float)data[2]);

        this.GetComponent<SpriteRenderer>().color = color;
        this.GetComponent<ControllerDestroyer>().color = color;
        this.GetComponent<ControllerDestroyer>().cruz.color = color;
        //this.GetComponent<ControllerDestroyer>().controllerplayer = this;


        if (gameController is null == true)
        { 
            gameController = GameObject.FindObjectOfType<GameController>();
        }

        this.gameObject.transform.SetParent(gameController.canvasMenu[4].transform);
    }
}
