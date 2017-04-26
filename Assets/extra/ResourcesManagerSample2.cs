using System.Collections;
using UnityEngine;
using UniRx;
using AssetBundles;
using System.Collections.Generic;
using Unity.Linq;

public class ResourcesManagerSample2 : Singleton<ResourcesManagerSample> {

    public string  GuiObjectsParentName = "GuiObjectsParent";

    //private ReactiveDictionary<string , CanvasObject> GuiObjects = new ReactiveDictionary<string , CanvasObject> ();
    private Dictionary<string, CanvasObject> guiObjects = new Dictionary<string, CanvasObject>();
    //public GameObject GuiObjectsParent;

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
        //AssetBundleManager.SetDevelopmentAssetBundleServer ();

        // Or customize the URL based on your deployment or configuration
        AssetBundleManager.SetSourceAssetBundleURL ( assetBundle_URL );
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
                if ( isNowFree ) {
                    AssetBundleManager.UnloadAssetBundle ( assetBundlePath );
                }

            } );

        yield return temp;
    }



    public IEnumerator InstantiateGUIObjectAsync<T> ( string assetName , System.Action<T> Callback = null , bool Init = true )
where T : UnityEngine.Object {

        var temp = ObservableAssetBundle.LoadAssetBundle<T> ( StaticMethod.GetAssetBundleName ( assetName )
        , assetName )
            .Subscribe ( obj => {

                //에셋 다운받고 바로 생성과 비활성 구분해야함
                if ( Init ) {
                    InitGameOBject<T> ( assetName , obj );

                    var temp2 = FindGUIObject<T> ( assetName );
                    //CanvasObject need  up parent class
                    Callback ( ( temp2.GetGameObject ) as T );
                    AssetBundleManager.UnloadAssetBundle ( StaticMethod.GetAssetBundleName ( assetName ) );

                }

            } );

        yield return temp;
    }

    private GameObject InitGameOBject<T> ( string cavasName , T obj ) where T : UnityEngine.Object {


        GameObject go = Instantiate ( obj as GameObject );
        go.name = cavasName;

        var GuiObjectsParent = this.gameObject.Child ( GuiObjectsParentName );

        if ( GuiObjectsParent == null ) {

            GuiObjectsParent = new GameObject ( GuiObjectsParentName );
            GuiObjectsParent.transform.SetParent ( this.transform );
        }

        go.transform.SetParent ( GuiObjectsParent.transform );

        return go;
    }


    public IEnumerator ResistPanelToPresenter<T> ( string assetBundleName , string assetName , string penelName , string presnterName , bool flag = true )
          where T : UnityEngine.Object {

        var panelObject = FindGUIObject<T> ( assetName ).GetPenel ( penelName );

        //Debug.Log ( panelObject );
        panelObject.SetActive ( flag );
        var temp = System.Type.GetType ( presnterName );
        panelObject.AddComponent ( temp );

        yield return null;
    }

    public T FindGUIPanelObjcet<T> ( string assetName , string penelName )
         where T : UnityEngine.Object {

        var temp2 = FindGUIObject<T> ( assetName ).GetPenel ( penelName );

        return temp2 as T;
    }

    public CanvasObject FindGUIObject<T> ( string cavasName ) where T : UnityEngine.Object {

        CanvasObject guiObject;
        if ( guiObjects.ContainsKey ( cavasName ) ) {
            guiObject = guiObjects[ cavasName ];
        } else {
            //guiObjects.Add(cavasName, guiObject = new GUIObject(cavasName));

            guiObject = new CanvasObject ( cavasName );
            guiObject.GuiName = cavasName;
            //guiObject = gameObject.AddComponent<CanvasObject> ();
            guiObjects.Add ( cavasName , guiObject );
        }

        return guiObject;
    }


    public void CanvasEnabled<T> ( string cavasName , bool flag ) where T : UnityEngine.Object {
        FindGUIObject<T> ( cavasName ).GetCanvas.enabled = flag;
    }



}
