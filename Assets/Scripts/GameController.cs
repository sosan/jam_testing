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


    [SerializeField] public Transform[] initialPlayerPositions = null;
    public ReactiveProperty<bool> faseConcluida = new ReactiveProperty<bool>(false);


    [HideInInspector] public ReactiveProperty<bool> isActiveFase = new ReactiveProperty<bool>(false);



    private IDisposable crono = null;

    [HideInInspector] public ushort tiempoCurrentBatalla = 0;
    public ushort TIEMPOMAXBATALLA = 36; ////36=> 3 minutos / 61 => 5 minutos //5 => 25 segods;









    

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


    private void Awake()
    {

        if (isDebug == true)
        {


            //money.Value = 1000;
            speed = 0.8f;


        }


        //textTime.text = "";
        //textMoney.text = "";

    }

    void Start()
    {
        GettingPlayerPrefsValues();

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



    public void InitGame()
    {

      //  MusicController.MusicInstance.SettingsMixer(
      //    isMutedInternal,
      //    mainVolumenInternal,
      //    mainSoundInternal,
      //    mainSfxInternal
      //);



       


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

            //textTime.text = Localization.Get("tiempo") + DateTime.FromBinary(binLocal).AddMinutes(tiempoCurrentBatalla).ToString("HH:mm");
                tiempoCurrentBatalla += 28;





        }
        , ex => { Debug.Log(" cuentaatrasantes OnError:" + ex.Message); if (crono != null) crono.Dispose(); },
        () => //completado
        {


            //v1.Stop();
            //print("termiando crono. tiempo=" + v1.Elapsed.TotalSeconds + " /" + textTime.text);

            crono.Dispose();
                
                //textTime.text = "";
                tiempoCurrentBatalla = 0;
            //print("parartodo. P=" + party + " S=" + saltarseResumen);





        }).AddTo(this.gameObject);




    }



  

}
