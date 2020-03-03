﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx.Async;
using Photon.Pun;


public class ControllerBomba : MonoBehaviourPun
{

    [SerializeField] public ControllerPlayer controllerPlayer = null;
    [SerializeField] public GameController gameController = null;
    [SerializeField] private ushort timeExplode = 3;
    [SerializeField] private ParticleSystem explosion = null;
    [SerializeField] private string ignoreTag;
    [SerializeField] private LayerMask raycastLayerMask = -1;
    [SerializeField] public Color color = Color.white;
    [SerializeField] public Color colorInicial = Color.white;
    [SerializeField] public SpriteRenderer cruz = null;
    [SerializeField] private Rigidbody2D rigid = null;

    private bool lerpingColor = false;
    
    private float progresoLerp = 0;
    private float interpolateDuration = 0.2f;
    private bool isExplosion = false;

    private bool isOnline = false;
    private int viewidPlayer = -1;
    public bool isShooted = false;

    private void Awake()
    {
        
        if(gameController is null == true)
        { 
        
            gameController = GameObject.FindObjectOfType<GameController>();
        
        }


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

            //if (isOnline == true)
            //{ 
            //    CalcularExplosionOnline();
            
            //}
            //else
            //{ 
                
            
            //}
            CalcularExplosion();
            
        }
        
        
    }

    //public async void CalcularExplosionOnline()
    //{ 
    //    if (isShooted == true)
    //    { 
    //        await UniTask.Delay(TimeSpan.FromSeconds(1));
    //    }

    //    try
    //    { 
    //        isExplosion = true;
    //        var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(2, 2), 0, layerMask: raycastLayerMask);

    //        PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
    //    }
    //    catch (MissingReferenceException)
    //    { 
            
        
    //    } 


    
    //}

    public async void CalcularExplosion()
    { 


        if (isShooted == true)
        { 
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }

        try
        { 
            isExplosion = true;
            var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(2, 2), 0, layerMask: raycastLayerMask);
            if (hits is null == false)
            {
            
                //play explision
                //explosion.Play();
                //await UniTask.Delay(TimeSpan.FromSeconds(explosion.main.duration));
                print("longitud hits=" + hits.Length);

                for (ushort i = 0 ; i < hits.Length; i++)
                { 
                    print("hits" + i + " nombre" + hits[i].name);
                    if (hits[i].CompareTag("fondo") == false) continue;

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


    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = photonView.InstantiationData;

        Color color = new Color((float)data[0], (float)data[1], (float)data[2]);
        int idplayerowner = (int)data[3];

        var player = PhotonNetwork.GetPhotonView(idplayerowner).gameObject;
        controllerPlayer = player.GetComponent<ControllerPlayer>();




    }


}
