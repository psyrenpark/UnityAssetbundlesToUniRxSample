using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using System.Linq;
using System;
using AssetBundles;

//DI구조로 변경을 해볼까하다 Zenject 으로 통합적으로 변경할 예정
//https://github.com/modesttree/Zenject

[System.Serializable]
public class GUIObject : MonoBehaviour  {

    //TODO: 설정해야할듯 
    enum PanelType {
        BAR,
        CONTENT,
        TEMPORARY,
        CONTROL
    }

    [SerializeField]
    private GameObject      canvasObject;
    [SerializeField]
    private Canvas          canvas;
    [SerializeField]
    private GameObject[]    panels;
    //private IDictionary<Type,Presenter>    panelPresenters = new Dictionary<Type,Presenter>();

    private Queue<System.Action<GUIObject>> callbackQueue;


    public GameObject       GetGameObject   { get { return canvasObject; } }
    public Canvas           GetCanvas       { get { return canvas; } }
    public GameObject       GetPenel    (string name) {
        return panels.Where ( ( x ) => x.name == name ).SingleOrDefault ();
    }



    private string guiName;
    [SerializeField]
    public string GuiName { get { return guiName; } }

    private string assetBundlePath;
    [SerializeField]
    public string AssetBundlePath { get { return assetBundlePath; } }


    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="canvasPrefab">생성된 캔바스 게임오브젝트</param>
    public GUIObject ( string AssetBundlePath , string GuiName , GameObject go ) {

        this.guiName = GuiName;
        this.assetBundlePath = AssetBundlePath;
        this.canvasObject       = go;
        this.canvas             = this.canvasObject.GetComponent<Canvas> ();

        var penelCount = canvasObject.Children ().ToArrayNonAlloc ( x => x.name.EndsWith ( "Panel" ) , ref panels );

        //  for ( int i = 0 ; i < penelCount ; i++ ) {
        //     panels[ i ].SetActive ( false );
        //TODO: 캔버스 용도에 따라 행동 
        // }
    }

   public void Initialize ( string AssetBundlePath , string GuiName ) {

        this.guiName            = GuiName;
        this.assetBundlePath    = AssetBundlePath;
        this.callbackQueue      = new Queue<Action<GUIObject>> ();
        this.panels             = new GameObject[ 0 ];
    }

    public void Start () {

        ObservableAssetBundle.LoadAssetBundle<GameObject> ( AssetBundlePath , GuiName )
          .Timeout ( TimeSpan.FromSeconds ( 5 ) )
          .Subscribe ( obj => {

              if ( obj == null ) {

                  Debug.LogError ( " AssetBundle is null.  Check Asset Path and Name. \n" +
                          "assetBundlePath : " + AssetBundlePath +
                          " / " +
                          "assetName : " + GuiName
                      );

                    FailAssetBundle ( assetBundlePath , GuiName );

                    return;
              }

              var go = Instantiate ( obj , gameObject.transform );
              go.name = GuiName;

              initCanvasObject ( go );

              //if ( isNowFree ) {
              AssetBundleManager.UnloadAssetBundle ( StaticMethod.GetAssetBundleName ( GuiName ) );
            //}

            } , err => {

                FailAssetBundle ( assetBundlePath , GuiName );

            } );

        /*
        this.UpdateAsObservable ()
            .Where ( _ => canvasObject )
            .Where ( _ => callbackQueue.Count > 0 )
            .Select( x => callbackQueue.Dequeue() )
            .Subscribe ( callback => {
                callback ( this );
            } );
            */

        /*
        var temp = canvasObject.ObserveEveryValueChanged ( x => x );

        temp
        .TakeUntil(temp)
        .Subscribe ( _ => {

            while ( callbackQueue.Count > 0 ) {
                var callback = callbackQueue.Dequeue ();
                callback( this );
                Debug.Log ( "aaaa" );
            }

        } );

        */


        //스트림을 강제로 종료하는 방법은 없는지 고민
        //강제로 컴플리트 시키는 방법
        //->first
        this.UpdateAsObservable ()
            .Where ( _ => canvasObject )
            //.DistinctUntilChanged ()
            .First()
            .Subscribe ( _ => {
                while ( callbackQueue.Count > 0 ) {
                    var callback = callbackQueue.Dequeue ();
                    callback ( this );
                }
            },() => Debug.Log("complete") );


    }


    public void FailAssetBundle ( string assetBundlePath , string assetName ) {

        GameObject FindGameobject;
        if ( FindGameobject = GameObject.Find ( assetName ) ) {

            FindGameobject.transform.parent = gameObject.transform;

            initCanvasObject ( FindGameobject );

            return;
        }

        Debug.LogError ( "  Gameobject Find  null.  Check Find Path and Name. \n" +
            "Find Path : " + assetBundlePath +
            " / " +
            "assetName : " + assetName
        );

        /*
        GameObject ResourcesLoadObject;
        if ( ResourcesLoadObject = Resources.Load<GameObject> ( assetName ) ) {

            ResourcesLoadObject.transform.parent = GuiObjectsParent.transform;

            var guiObject = new GUIObject ( "ResourcesLoadObject" , assetName , ResourcesLoadObject );
            guiObjects.Add ( assetName , guiObject );
            Callback ( ( guiObject ) );

            return;
        }

        Debug.LogError ( "   Resources Load  null.  Check Load Path and Name. \n" +
            "Load Path : " + assetBundlePath +
            " / " +
            "assetName : " + assetName
        );
        */


    }

    private void initCanvasObject (GameObject go) {

        this.canvasObject = go;
        this.canvas = this.canvasObject.GetComponent<Canvas> ();

        var penelCount = canvasObject.Children ().ToArrayNonAlloc ( x => x.name.EndsWith ( "Panel" ) , ref panels );

        //  for ( int i = 0 ; i < penelCount ; i++ ) {
        //     panels[ i ].SetActive ( false );
        //TODO: 캔버스 용도에 따라 행동 
        // }

    }

    public void  reset () {

        var penelCount = panels.Length;
        for ( int i = 0 ; i < penelCount ; i++ ) {
            panels[ i ].SetActive ( false );
            //TODO: 캔버스 용도에 따라 행동 

        }

    }


    //유니RX로  핫으로 할지 큐로 할지 코루틴으로 처리할지 고민하것 
    public void CallBackProcess ( System.Action<GUIObject> Callback ) {

        if ( canvasObject == null ) {
            if( Callback != null)
            callbackQueue.Enqueue ( Callback );
            return;
        }

        Callback ( this );
    }

    /*
    public void Update () {

        if ( canvasObject != null ) {

            if ( callbackQueue.Count != 0 ) {
                var callback = callbackQueue.Dequeue ();

                if ( callback != null ) callback ( this );
            }
        }
    }
    */

        /*
    public void ADD<T> ( Presenter instance ) {
        panelPresenters.Add ( typeof ( T ) , instance );
    }
    public Presenter GetInstance<T> () {
        return ( Presenter ) panelPresenters[ typeof ( T ) ];
    }
    */


}
