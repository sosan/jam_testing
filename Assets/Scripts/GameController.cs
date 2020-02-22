using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using TMPro;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{


    //[SerializeField] private Animator animatorTorre1 = null;
    //[SerializeField] private Animator animatorTorre2 = null;
    //[SerializeField] private Animator animatorTorre3 = null;
    [SerializeField] private GameObject game = null;
    [SerializeField] private GameObject[] objetosGame = null;
    [SerializeField] private GameObject fiesta = null;

    [SerializeField] private Animation animLunes = null;
    [SerializeField] private Animation animDemasDias = null;

    [HideInInspector] public ushort diaSemana = 1; //1 = lunes, 2 martes, 3 miercoles, 4 jueves, 5 viernes, 6 sabado, 7 domingo
    //[SerializeField] private BasuraSpawner basuraSpawner = null;
    [SerializeField] private TextMeshProUGUI textMoney = null;
    [SerializeField] private TextMeshProUGUI textTime = null;
    //[SerializeField] private Resumen resumen = null;

    //[SerializeField] private BarcoMovement barcoMovement = null;
    //[SerializeField] private NPCSpawnerController npcSpawner = null;

    public ReactiveProperty<bool> faseConcluida = new ReactiveProperty<bool>(false);


    //[HideInInspector] public ReactiveProperty<ushort> money = new ReactiveProperty<ushort>(100);
    [HideInInspector] public ReactiveProperty<bool> isActiveFase = new ReactiveProperty<bool>(false);



    private IDisposable crono = null;
    //public ushort numeroCubosBasura = 0;

    [HideInInspector] public ushort tiempoCurrentBatalla = 0;
    public ushort TIEMPOMAXBATALLA = 36; ////36=> 3 minutos / 61 => 5 minutos //5 => 25 segods;









    //
    //[SerializeField] private GameObject diaSemanaCanvas = null;
    //[SerializeField] private GameObject lunesCanvas = null;
    //[SerializeField] private TextMeshProUGUI textodiasemana = null;
    //[SerializeField] private TextMeshProUGUI textodialunes = null;
    //[SerializeField] private TextMeshProUGUI textoExplicacionGeneral = null;
    //[SerializeField] private GameObject tutor = null;
    //[SerializeField] private Animation tutorAnim = null;
    //[SerializeField] private GameObject canvasGeneral = null;
    //public ushort basuraRecogida = 0;
    //public ushort silbatosIncidencias = 0;
    //public ushort silbatosUsados = 0;



    //public ushort numeroTorres = 1;

    //public bool construidoBar1 = true;
    //public bool construidoBar2 = false;

    //[SerializeField] private BarController bar1 = null;
    //[SerializeField] private BarController bar2 = null;
    //[SerializeField] private Image p_ima1 = null;
    //[SerializeField] private Image p_ima2 = null;
    //[SerializeField] private Image p_ima3 = null;
    //[SerializeField] private Image p_ima4 = null;
    //[SerializeField] private Image p_ima5 = null;

    //[SerializeField] private GameObject f_ima1 = null;
    //[SerializeField] private GameObject f_ima2 = null;
    //[SerializeField] private GameObject f_ima3 = null;
    
    



    //[SerializeField] private TextMeshProUGUI txt_prohibido1 = null;
    //[SerializeField] private TextMeshProUGUI txt_prohibido2 = null;
    //[SerializeField] private TextMeshProUGUI txt_prohibido3 = null;
    //[SerializeField] private TextMeshProUGUI txt_prohibido4 = null;

    //[SerializeField] private Sprite imagenSalpicar = null;
    //[SerializeField] private Sprite imagenAgua = null;
    //[SerializeField] private Sprite imagenFumar = null;
    //[SerializeField] private Sprite imagenBarco = null;
    //[SerializeField] private Sprite imagenArma = null;
    //[SerializeField] private SilbatoDragController silbatoDragController = null;
    //[SerializeField] private BasuraDragController basuraDragController = null;
    //[SerializeField] public GameObject explicacionBasura = null;


    private bool isMutedDefault = false;
    private ushort mainVolumenDefault = 100;
    private ushort mainSoundDefault = 30;
    private ushort mainSfxDefault = 60;

    private bool isMutedInternal = false;
    private ushort mainVolumenInternal = 100;
    private ushort mainSoundInternal = 30;
    private ushort mainSfxInternal = 60;

    public bool isDebug = true;
    public float speed = 0.8f;

    //[Header("FIESTA")]
    //[HideInInspector] public bool party = false;


    //[SerializeField] private Animator animatordj = null;
    //[SerializeField] private Animator[] dancers = null;
    //[SerializeField] private Animator[] altavoces = null;
    //[SerializeField] private GameObject oscuridad = null;



    

    private void Awake()
    {

        if (isDebug == true)
        {


            //money.Value = 1000;
            speed = 0.8f;


        }


        var t = GameObject.FindGameObjectsWithTag("toallas");
        for (int i = 0; i < t.Length; i++)
        {
            //print("destroy");
            t[i].GetComponent<SpriteRenderer>().color = Color.white;
            t[i].gameObject.SetActive(false);



        }

        //canvasGeneral.SetActive(false);
        //lunesCanvas.SetActive(false);
        textTime.text = "";
        textMoney.text = "";
        //diaSemanaCanvas.SetActive(false);
        //textoExplicacionGeneral.text = "";
        //tutor.SetActive(false);
        fiesta.SetActive(false);

        //for (ushort i = 0; i < 5; i++)
        //{

        //    basuraSpawner.SpawnBasura();
        //}


      

    }

    // Start is called before the first frame update
    void Start()
    {
        diaSemana = 0;
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        GettingPlayerPrefsValues();

    }


    public async void InitExplication()
    {


        //p_ima1.gameObject.SetActive(false);
        //txt_prohibido1.text = "";


        //p_ima2.gameObject.SetActive(false);
        //txt_prohibido2.text = "";

        //p_ima3.gameObject.SetActive(false);
        //txt_prohibido3.text = "";

        //p_ima4.gameObject.SetActive(false);
        //txt_prohibido4.text = "";

        //f_ima1.SetActive(false);
        //f_ima2.SetActive(false);
        //f_ima3.SetActive(false);

        //if (diaSemana == 1)
        //{


            
        //    textodiasemana.text = Localization.Get("dia" + diaSemana);
        //    textodialunes.text = Localization.Get("dia" + diaSemana);
        //    //lunesCanvas.SetActive(true);
        //    diaSemanaCanvas.SetActive(true);

        //    p_ima4.gameObject.SetActive(true);
        //    txt_prohibido4.text = Localization.Get("nobarcos");

        //    animLunes.Play("CanvasLunes");

        //    //await UniTask.Delay(2000);
        //    //lunesCanvas.SetActive(false);

        //    //InitGame2();




        //}
        //else
        //{


        //    if (diaSemana == 1)
        //    {
        //        p_ima4.gameObject.SetActive(true);
        //        txt_prohibido4.text = Localization.Get("nobarcos");

        //    }
        //    else if (diaSemana == 2)
        //    {

        //        if (party == false)
        //        {

        //            p_ima3.sprite = imagenSalpicar;
        //            txt_prohibido3.text = Localization.Get("nosalpicar2");
        //            f_ima3.SetActive(true);
        //            p_ima3.gameObject.SetActive(true);

        //            p_ima4.gameObject.SetActive(true);
        //            txt_prohibido4.text = Localization.Get("nobarcos");


        //        }
        //        else
        //        {

        //            p_ima3.sprite = imagenAgua;
        //            txt_prohibido3.text = Localization.Get("noagua");
        //            f_ima3.SetActive(true);
        //            p_ima3.gameObject.SetActive(true);

        //            p_ima4.gameObject.SetActive(true);
        //            txt_prohibido4.text = Localization.Get("nobarcos");



        //        }


        //    }
        //    else if (diaSemana == 3)
        //    {


        //        p_ima1.sprite = imagenSalpicar;
        //        txt_prohibido1.text = Localization.Get("nosalpicar");
        //        f_ima1.SetActive(true);
        //        p_ima1.gameObject.SetActive(true);



        //        p_ima2.sprite = imagenFumar;
        //        txt_prohibido2.text = Localization.Get("nofumar");
        //        f_ima2.SetActive(true);
        //        p_ima2.gameObject.SetActive(true);

        //        p_ima3.sprite = imagenArma;
        //        txt_prohibido3.text = Localization.Get("noarmas");
        //        f_ima3.SetActive(true);
        //        p_ima3.gameObject.SetActive(true);

        //        p_ima4.gameObject.SetActive(true);
        //        txt_prohibido4.text = Localization.Get("nobarcos");




        //    }
        //    else if (diaSemana == 4)
        //    {




        //        f_ima1.SetActive(true);
        //        p_ima1.sprite = imagenArma;
        //        txt_prohibido1.text = Localization.Get("noarmas");
        //        p_ima1.gameObject.SetActive(true);

        //        p_ima4.enabled = true;
        //        p_ima4.gameObject.SetActive(true);
        //        txt_prohibido4.text = Localization.Get("nobarcos");




        //    }
        //    else if (diaSemana == 5)
        //    {


        //        p_ima1.sprite = imagenSalpicar;
        //        txt_prohibido1.text = Localization.Get("nosalpicar");
        //        f_ima1.SetActive(true);
        //        p_ima1.gameObject.SetActive(true);

        //        p_ima2.sprite = imagenFumar;
        //        txt_prohibido2.text = Localization.Get("nofumar");
        //        f_ima2.SetActive(true);
        //        p_ima2.gameObject.SetActive(true);

        //        p_ima4.gameObject.SetActive(true);
        //        p_ima4.gameObject.SetActive(true);
        //        txt_prohibido4.text = Localization.Get("nobarcos");

        //    }
        //    else 
        //    {


        //        p_ima1.sprite = imagenSalpicar;
        //        txt_prohibido1.text = Localization.Get("nosalpicar");
        //        f_ima1.SetActive(true);
        //        p_ima1.gameObject.SetActive(true);

        //        p_ima2.sprite = imagenFumar;
        //        txt_prohibido2.text = Localization.Get("nofumar");
        //        f_ima2.SetActive(true);
        //        p_ima2.gameObject.SetActive(true);

        //        p_ima3.sprite = imagenArma;
        //        txt_prohibido3.text = Localization.Get("noarmas");
        //        f_ima3.SetActive(true);
        //        p_ima3.gameObject.SetActive(true);
                
        //        p_ima4.gameObject.SetActive(true);
        //        txt_prohibido4.text = Localization.Get("nobarcos");

        //    }





        //    await UniTask.Delay(1000);
        //    animDemasDias.Play("CanvasLunes");

        //    if (party == true)
        //    {

        //        textodiasemana.text = Localization.Get("noche" + diaSemana);

        //    }
        //    else
        //    {

        //        textodiasemana.text = Localization.Get("dia" + diaSemana);
        //    }



        //    diaSemanaCanvas.SetActive(true);
        //}




    }

    private void GettingPlayerPrefsValues()
    {

        if (PlayerPrefs.HasKey("isMutedInternal") == true)
        {

            int t = PlayerPrefs.GetInt("isMutedInternal");


            if (t == 0)
            {

                isMutedInternal = false;
            }
            else if (t == 1)
            {

                isMutedInternal = true;

            }



        }
        else
        {

            if (isMutedDefault == true)
            {

                PlayerPrefs.SetInt("isMutedInternal", 1);

            }
            else
            {

                PlayerPrefs.SetInt("isMutedInternal", 0);

            }


        }

        if (PlayerPrefs.HasKey("mainVolumenInternal") == true)
        {

            mainVolumenInternal = (ushort)PlayerPrefs.GetInt("mainVolumenInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainVolumenInternal", mainVolumenDefault);
        }


        if (PlayerPrefs.HasKey("mainSoundInternal") == true)
        {

            mainSoundInternal = (ushort)PlayerPrefs.GetInt("mainSoundInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainSoundInternal", mainSoundDefault);
        }



        if (PlayerPrefs.HasKey("mainSfxInternal") == true)
        {

            mainSfxInternal = (ushort)PlayerPrefs.GetInt("mainSfxInternal");

        }
        else
        {

            PlayerPrefs.SetInt("mainSfxInternal", mainSfxDefault);
        }



    }


    //public void SetBar2(BarController bar)
    //{

    //    bar2 = bar;

    //}

    public void InitGame()
    {

        MusicController.MusicInstance.SettingsMixer(
          isMutedInternal,
          mainVolumenInternal,
          mainSoundInternal,
          mainSfxInternal
      );



        //if (party == true)
        //{
        //    print("party=" + party);
        //    textTime.text = Localization.Get("partytime");
        //    tiempoCurrentBatalla = 0;
        //    MusicController.MusicInstance.PlayInGameMusic(MusicController.MusicInstance.sounds[2]);

        //    //bar1.construido = true;
        //    //bar1.InitSystemBar();

        //    //var t = GameObject.Find("bar2_progreso");
        //    //t.SetActive(false);
            
        //    //if (construidoBar2 == true)
        //    //{

        //    //    bar2.construido = true;
        //    //    bar2.InitSystemBar();
        //    //}

        //    //canvasGeneral.SetActive(true);

        //    //npcSpawner.SetCaritaPrincipio();

        //    //InitExplication();


        //}
        //else
        //{



        //    MusicController.MusicInstance.PlayInGameMusic(
        //        MusicController.MusicInstance.sounds[1], true

        //        );
        //    diaSemana++;

        //    if (construidoBar2 == true)
        //    {

        //        bar2.construido = true;
        //        bar2.InitSystemBar();
        //    }

        //}

        //bar1.construido = true;
        //bar1.InitSystemBar();




        //canvasGeneral.SetActive(true);

        //npcSpawner.SetCaritaPrincipio();



        //InitExplication();




       


    }


    private void IniciarCrono()
    {


        DateTime date1 = new DateTime(2019, 1, 1,9, 0, 0);

        //DateTime date1 = new DateTime(2019, 1, 1,22, 0, 0);
        // 00636819300000000000;

#if UNITY_EDITOR
        print("inicar crono=") ;

#endif
       
        TIEMPOMAXBATALLA = 24; //2 minutos


        long binLocal = date1.ToBinary();

        //int minutos = 0;
        //int horas = 9;


        crono = Observable.Timer(
        TimeSpan.FromSeconds(0), //esperamos 1 segundos 
        TimeSpan.FromSeconds(5), Scheduler.MainThread).Do(x => { }).
        ObserveOnMainThread().Take(TIEMPOMAXBATALLA)
        .Subscribe
        (_ =>
        {

            textTime.text = Localization.Get("tiempo") + DateTime.FromBinary(binLocal).AddMinutes(tiempoCurrentBatalla).ToString("HH:mm");
                tiempoCurrentBatalla += 28;

            //if (diaSemana == 1)
            //{
            //}
            //else
            //{

            //    tiempoCurrentBatalla += 18;

            //}






        }
        , ex => { Debug.Log(" cuentaatrasantes OnError:" + ex.Message); if (crono != null) crono.Dispose(); },
        () => //completado
        {


            //v1.Stop();
            //print("termiando crono. tiempo=" + v1.Elapsed.TotalSeconds + " /" + textTime.text);

            crono.Dispose();
                
                textTime.text = "";
                tiempoCurrentBatalla = 0;
            //print("parartodo. P=" + party + " S=" + saltarseResumen);





        }).AddTo(this.gameObject);




    }



  

}
