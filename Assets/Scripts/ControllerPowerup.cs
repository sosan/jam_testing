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
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        { 
            collision.gameObject.GetComponent<ControllerPlayer>().isDestroyer = true;

            Transform[] childrens = this.gameObject.GetComponentsInChildren<Transform>();
            for (ushort i = 0; i < childrens.Length; i++)
            { 
                Destroy(childrens[i].gameObject);
        
            }


            Destroy(this.gameObject);
        
        }

        if (collision.CompareTag("Bullet"))
        { 
        
            //empujar
            var contact = collision.ClosestPoint(this.transform.position);
            rigid.AddForceAtPosition(this.gameObject.transform.position, contact );
        
        }


    }


}
