using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx.Async;
using System;

public class ControllerPowerup : MonoBehaviour
{

    [SerializeField] public GameController gameController = null;
    [SerializeField] private Rigidbody2D rigid = null;
    [SerializeField] private bool isOnline = false;
    [SerializeField] public bool isCalled = false;
    


    // Start is called before the first frame update
    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            
            if (isOnline == true)
            {
                if (this.GetComponent<PhotonView>().IsMine == false) return;
                //this.GetComponent<PhotonView>().RPC("QuitarPowerup", RpcTarget.MasterClient, true);
                
            }
            else
            { 
                collision.gameObject.GetComponent<ControllerPlayer>().isDestroyer = true;

                Transform[] childrens = this.gameObject.GetComponentsInChildren<Transform>();
                for (ushort i = 0; i < childrens.Length; i++)
                { 
                    Destroy(childrens[i].gameObject);
        
                }

        
                Destroy(this.gameObject);
        
            
            }


            
        }


        //los powerups les afectan las balas?
        //if (collision.CompareTag("Bullet"))
        //{ 
        
        //    //empujar
        //    var contact = collision.ClosestPoint(this.transform.position);
        //    rigid.AddForceAtPosition(this.gameObject.transform.position, contact );
        
        //}


    }


}
