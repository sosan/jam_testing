using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx.Async;


public class ControllerDestroyer : MonoBehaviour
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



    // Start is called before the first frame update
    private async void Start()
    {
        
        await UniTask.Delay(TimeSpan.FromSeconds(timeExplode - 1));
        lerpingColor = true;
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        
        var hits = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(1, 1), 0, layerMask: raycastLayerMask);
        if (hits is null == false)
        {
            
            //play explision
            //explosion.Play();
            //await UniTask.Delay(TimeSpan.FromSeconds(explosion.main.duration));

            for (ushort i = 0 ; i < hits.Length; i++)
            { 
            
                hits[i].transform.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                hits[i].transform.tag = nombreColores.hueco.ToString();

                

            
            }
        
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


}
