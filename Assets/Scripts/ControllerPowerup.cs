using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPowerup : MonoBehaviour
{

    [SerializeField] public GameController gameController = null;
    [SerializeField] private Rigidbody2D rigid = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        { 
            collision.gameObject.GetComponent<ControllerPlayer>().isDestroyer = true;
            Destroy(this.gameObject);
        
        }

        if (collision.CompareTag("bullet"))
        { 
        
            //empujar
            var contact = collision.ClosestPoint(this.transform.position);
            rigid.AddForceAtPosition(this.gameObject.transform.position, contact );
        
        }


    }


}
