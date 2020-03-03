using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx.Async;
using System;

public class ControllerBullet : MonoBehaviour
{


    [SerializeField] private bool isOnline = false;
    [SerializeField] private PhotonView photonView = null;
    // Start is called before the first frame update
    private async void Start()
    {

        if (isOnline == true)
        { 

            await UniTask.Delay(TimeSpan.FromSeconds(10));
            PhotonNetwork.Destroy(photonView);

        }
        else
        { 
            Destroy(this.gameObject, 10); 
        
        }
        
    }

    
}
