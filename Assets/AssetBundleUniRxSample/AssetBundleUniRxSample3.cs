using AssetBundles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Unity.Linq;

public class AssetBundleUniRxSample3 : MonoBehaviour {

    private const string assetBundle_URL  = "https://test.com/AssetBundles/";
    private const string assetBundlePath  = "test_box";
    private const string assetName        = "TestBox";

    IEnumerator Start () {

        yield return StartCoroutine ( Initialize () );

        //개별적으로 처리
        /*
        for (int i = 0 ; i < 3 ; i++ ) {

            var temp = i;

            ObservableAssetBundle.LoadAssetBundle<GameObject> ( assetBundlePath , assetName + i )
            .Subscribe ( assetGameObject => {

                GameObject obj = Instantiate ( assetGameObject ) as GameObject;
                obj.transform.position = new Vector3 ( 0.0f + temp , 0.0f , 0.0f );

                AssetBundleManager.UnloadAssetBundle ( assetBundlePath );
            } );
        }
        */

        //한번에 처리

        Observable.WhenAll (
            ObservableAssetBundle.LoadAssetBundle<GameObject> ( "test_box" , "TestBox0" ) ,
            ObservableAssetBundle.LoadAssetBundle<GameObject> ( "test_box" , "TestBox1" ) ,
            ObservableAssetBundle.LoadAssetBundle<GameObject> ( "test_box" , "TestBox2" ) )
         .Subscribe ( prefabs => {

                for ( int i = 0 ; i < prefabs.Length ; i++ ) {
                    GameObject obj = Instantiate ( prefabs[i] ) as GameObject;
                    obj.transform.position = new Vector3 ( 0.0f + i , 0.0f , 0.0f );
                }
                AssetBundleManager.UnloadAssetBundle ( assetBundlePath );
            } );

    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Initialize () {
        // Don't destroy this gameObject as we depend on it to run the loading script.
        //DontDestroyOnLoad ( gameObject );

        InitializeSourceURL ();

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize ();
        if ( request != null )
            yield return StartCoroutine ( request );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void InitializeSourceURL () {
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
}
