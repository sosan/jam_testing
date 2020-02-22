using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMenuAnimations : MonoBehaviour
{


    [SerializeField] public Animation animations = null;
    [SerializeField] public AnimationClip[] animationClips = null;

    // Start is called before the first frame update
    void Start()
    {
        //animations.Play("Titulo");
    }



    public float DesactiveMenu()
    {

        animations.Play("MenuDesactivate");
        return animations.GetClip("MenuDesactivate").length;

    }


}
