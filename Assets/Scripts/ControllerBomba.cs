using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx.Async;


public class ControllerBomba : MonoBehaviour
{
    [SerializeField] public ControllerPlayer controllerplayer = null;
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



    // Start is called before the first frame update
    private async void Start()
    {
        
        isExplosion = false;

        await UniTask.Delay(TimeSpan.FromSeconds(timeExplode - 1));
        lerpingColor = true;
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        isExplosion = true;
        var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(1, 1), 0, layerMask: raycastLayerMask);
        if (hits is null == false)
        {
            
            //play explision
            //explosion.Play();
            //await UniTask.Delay(TimeSpan.FromSeconds(explosion.main.duration));
        

            for (ushort i = 0 ; i < hits.Length; i++)
            { 
                
                if (hits[i].CompareTag("fondo") == false) continue;

                hits[i].transform.gameObject.GetComponent<SpriteRenderer>().color = color;

                //restamos puntuacion al enemigo
                switch (hits[i].transform.tag)
                {
                    case "amarillo": controllerplayer.gameController.bloquesAmarillos -= 1; break;
                    case "azul": controllerplayer.gameController.bloquesAzules  -= 1; break;
                    case "rojo": controllerplayer.gameController.bloquesRojos -= 1;break;
                    case "blanco":controllerplayer.gameController.bloquesBlancos -= 1; break;
                    case "fondo": break;
                
                }

                if (controllerplayer.gameController.bloquesAmarillos < 0)
                { 
                    controllerplayer.gameController.bloquesAmarillos = 0;
                
                }

                if (controllerplayer.gameController.bloquesAzules < 0)
                { 
                    controllerplayer.gameController.bloquesAzules = 0;
                
                }

                if (controllerplayer.gameController.bloquesRojos < 0)
                { 
                    controllerplayer.gameController.bloquesRojos = 0;
                
                }

                if (controllerplayer.gameController.bloquesBlancos < 0)
                { 
                    controllerplayer.gameController.bloquesBlancos = 0;
                
                }


                hits[i].transform.tag = controllerplayer.player.nombreColorPlayer.ToString();

                //subimos puntuacion al player
                switch(controllerplayer.player.nombreColorPlayer)
                {
                    case nombreColores.amarillo: controllerplayer.gameController.bloquesAmarillos += 1; break;
                    case nombreColores.azul: controllerplayer.gameController.bloquesAzules  += 1; break;
                    case nombreColores.rojo: controllerplayer.gameController.bloquesRojos += 1;break;
                    case nombreColores.blanco:controllerplayer.gameController.bloquesBlancos += 1; break;
                    case nombreColores.fondo: break;
                
                }

                if (controllerplayer.gameController.bloquesAmarillos > 162)
                { 
                    controllerplayer.gameController.bloquesAmarillos = 162;
                
                }

                if (controllerplayer.gameController.bloquesAzules > 162)
                { 
                    controllerplayer.gameController.bloquesAzules = 162;
                
                }

                if (controllerplayer.gameController.bloquesRojos > 162)
                { 
                    controllerplayer.gameController.bloquesRojos = 162;
                
                }

                if (controllerplayer.gameController.bloquesBlancos > 162)
                { 
                    controllerplayer.gameController.bloquesBlancos = 162;
                
                }

            
            }
        
        }

        Transform[] childrens = this.gameObject.GetComponentsInChildren<Transform>();
        for (ushort i = 0; i < childrens.Length; i++)
        { 
            Destroy(childrens[i].gameObject);
        
        }


        Destroy(this.gameObject);
        
        //Destroy(this.gameObject, timeExplode);
        
        
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

            rigid.isKinematic = false;
            rigid.mass = 5;
            var contact = collision.ClosestPoint(this.transform.position).normalized;
            print(contact.ToString());
            rigid.AddForce( new Vector2(-contact.normalized.x, 0) * 5f, ForceMode2D.Impulse);
            rigid.isKinematic = true;


        }

    }


}
