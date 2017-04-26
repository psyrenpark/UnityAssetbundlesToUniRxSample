using System.Collections;
using UnityEngine;
using UniRx;
using AssetBundles;
using System.Collections.Generic;
using Unity.Linq;

public class ResourcesManagerSample2 : Singleton<ResourcesManagerSample2> {

    public string  GuiObjectsParentName = "GuiObjectsParent";
    public GameObject GuiObjectsParent;

    //private ReactiveDictionary<string , CanvasObject> GuiObjects = new ReactiveDictionary<string , CanvasObject> ();
    private Dictionary<string, GUIObject> guiObjects = new Dictionary<string, GUIObject>();
    //public GameObject GuiObjectsParent;

    private void Awake () {
        //
        GuiObjectsParent = new GameObject ( GuiObjectsParentName );
        GuiObjectsParent.transform.SetParent ( this.transform );
    }






    public IEnumerator Initialize ( string assetBundleURL ) {
        // Don't destroy this gameObject as we depend on it to run the loading script.     

        InitializeSourceURL ( assetBundleURL );

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize ();
        if ( request != null )
            yield return StartCoroutine ( request );
    }


    private void InitializeSourceURL ( string assetBundle_URL ) {
        // If ODR is available and enabled, then use it and let Xcode handle download requests.
#if ENABLE_IOS_ON_DEMAND_RESOURCES
        if (UnityEngine.iOS.OnDemandResources.enabled)
        {
            AssetBundleManager.SetSourceAssetBundleURL("odr://");
            return;
        }
#endif
#if DEVELOPMENT_BUILD || UNITY_EDITOR

        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project.
        //      Another approach would be to make this configurable in the standalone player.)
        AssetBundleManager.SetDevelopmentAssetBundleServer ();

        return;
#else
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        //AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
        // Or customize the URL based on your deployment or configuration
        AssetBundleManager.SetSourceAssetBundleURL(assetBundle_URL);
        return;
#endif
    }

    public IEnumerator LoadAssetAsync<T> ( string assetBundlePath , string assetName , System.Action<T> Callback = null , bool isNowFree = true )
   where T : UnityEngine.Object {

        var temp = ObservableAssetBundle.LoadAssetBundle<T> ( assetBundlePath
        , assetName )
            .Subscribe ( obj => {
                Callback ( obj as T );


            } );

        yield return temp;
    }


    //TODO : [ Criticality : high ]
    //생성중 부르는경우가 있을수 있음
    //다중생성됨;
    //비동기 문제점 => GUIObject내부에서 에셋번들 호출하는 루프 체크 할까 고민중
    public void SelectGUIObject ( string assetBundlePath , string assetName , 
        System.Action<GUIObject> Callback = null , bool isNowFree = true ) {

        GUIObject guiObject;
        if ( guiObjects.ContainsKey ( assetName ) ) {
            guiObject = guiObjects[ assetName ];

            Callback ( ( guiObject ) );

            return;
        }

        var temp = ObservableAssetBundle.LoadAssetBundle<GameObject> ( assetBundlePath , assetName )
            .Subscribe ( obj => {

                if ( obj == null ) {
                    Debug.LogError ( " AssetBundle is null.  Check Asset Path and Name. \n" +
                            "assetBundlePath : " + assetBundlePath +  
                            " / " +
                            "assetName : "+ assetName
                        );
                    return;
                }

                var go = Instantiate ( obj , GuiObjectsParent.transform );
                go.name = assetName;

                guiObject = new GUIObject ( assetBundlePath , assetName , go );
                guiObjects.Add ( assetName , guiObject );

                Callback ( ( guiObject )  );

                if ( isNowFree ) {
                    AssetBundleManager.UnloadAssetBundle ( StaticMethod.GetAssetBundleName ( assetName ) );
                }

                } );
    }


    public void ResistPanelToPresenter ( string assetBundlePath,  string canvasName ,
         string penelName , string presnterName , bool flag = true ){

        SelectGUIObject ( assetBundlePath , canvasName , (x)=> {

           var panelObject = x.GetPenel ( penelName );
            panelObject.SetActive ( flag );
            var temp = System.Type.GetType ( presnterName );
            panelObject.AddComponent ( temp );

        } );

    }

    public void CanvasEnabled (string assetBundlePath , string cavasName , bool flag ) {
        SelectGUIObject (  assetBundlePath , cavasName , (x ) => x.GetCanvas.enabled = flag);
    }



}
