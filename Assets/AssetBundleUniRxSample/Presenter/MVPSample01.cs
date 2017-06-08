using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MVPSample01 : MonoBehaviour {

   //public 

   // public ReactiveProperty<string> IntRxProp = new ReactiveProperty<string>();

    
  
    // Use this for initialization
    IEnumerator Start () {

        var rm = ResourcesManagerSample.GetInst;


        yield return rm.Initialize ( "https://psyrenpark.com/AssetBundles" );

        /*
        ResourcesManager.GetInst.LoadAsset ( "intro_gui" , "IntroGUI" , 
            ( x ) => Debug.Log ( x.name ),
            ( x ) => Debug.Log ( x.name ) );
            */


        //GameObject go = new GameObject ();
        //var temp = Observable.FromCoroutine<int> ( observer => test ( observer , go ) );

        //yield return temp.Subscribe ( x => Debug.Log ( "zzzzzz" + x ) );





        //yield return rm.GetInst.InstantiateGameObjectAsync3<GameObject> ( "intro_gui" , "IntroGUI" ,
       //     ( x ) => Debug.Log ( "exit : " + x.name ) );

        yield return new WaitForSeconds ( 1f );

        //yield return rm.ResistPanelToPresenter<GameObject> ( "IntroGUI" , "MainPanel" , "SamplePresnter" );


    }
    
    public IEnumerator test ( IObserver<int> observer , GameObject go) {



        observer.OnNext ( 2 );
        observer.OnCompleted ();


        yield return new WaitForSeconds ( 1f );

    }

}
