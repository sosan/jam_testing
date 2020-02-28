using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLimites : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rigid = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("bomba") == true)
        {

            //if (explosion == true) return;
            print("dentro limites");
            collision.attachedRigidbody.isKinematic = false;
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

            collision.gameObject.GetComponent<ControllerBomba>().CalcularExplosion(isShooted: true);
            
            


        }

    }
}
