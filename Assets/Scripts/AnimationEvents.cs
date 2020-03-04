using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimationEvents : MonoBehaviour
{

    [SerializeField] private GameController gameController = null;
    [SerializeField] private TextMeshProUGUI textoready = null;

    
    public void CambiarFrase()
    { 
        textoready.text = Localization.Get(gameController.texto_fase2);
    
    
    }

    public void IniciarCrono()
    { 
        gameController.IniciarCrono();
    
    
    }


    public void IniciarExplicacionLayoutMando()
    { 
    
        
    
    }

}
