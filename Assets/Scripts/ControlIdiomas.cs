using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlIdiomas : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI[] textos = null;
    [SerializeField] private Color selectColor = Color.white;
    [SerializeField] private Color notSelectColor = Color.white;
    [SerializeField] public ParticleSystem[] particulas = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private Image ene = null;
    [SerializeField] private Image[] cuadro = null;


    private void Awake()
    {


        canvas.enabled = false;
        if (PlayerPrefs.HasKey("idioma") == true)
        {


            string t = PlayerPrefs.GetString("idioma");

            switch (t)
            {

                case "espanol": Localization.language = t; break;
                case "english": Localization.language = t; break;
                default: Debug.LogError("NOT SET IDIOMA"); return;

            }

            SceneManager.LoadSceneAsync("Menu");



        }
        else
        {

            canvas.enabled = true;

        }
       



    }

    // Start is called before the first frame update
    void Start()
    {
       

        DontDestroyOnLoad(this.gameObject);   
    }


    public void EnterHoverSpanish()
    {
        textos[0].color = selectColor;
        ene.color = selectColor;
        cuadro[0].color = selectColor;
    }

    public void ExitHoverSpanish()
    {

        textos[0].color = notSelectColor;
        ene.color = notSelectColor;
        cuadro[0].color = notSelectColor;
    }


    public void EnterHoverEnglish()
    {
        textos[1].color = selectColor;
        cuadro[1].color = selectColor;
    }


    public void ExitHoverEnglish()
    {

        textos[1].color = notSelectColor;
        cuadro[1].color = notSelectColor;
    }


    public async void SetSpanishLanguage()
    {
        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );
        Localization.language = "espanol";

        PlayerPrefs.SetString("idioma", "espanol");
        particulas[0].Play();
        await UniTask.Delay(300);
        _ = SceneManager.LoadSceneAsync("Menu");

    }

    public async void SetEnglishLanguage()
    {

        MusicController.MusicInstance.PlayFXSound(
            MusicController.MusicInstance.sfx[1]
            );

        Localization.language = "english";
        PlayerPrefs.SetString("idioma", "english");
        particulas[1].Play();
        await UniTask.Delay(300);
        _ = SceneManager.LoadSceneAsync("Menu");

    }


}
