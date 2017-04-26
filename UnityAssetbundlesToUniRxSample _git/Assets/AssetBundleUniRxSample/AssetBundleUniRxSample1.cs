using AssetBundles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class AssetBundleUniRxSample1 : MonoBehaviour {

    private const string assetBundle_URL  = "https://test.com/AssetBundles/";
    private const string assetBundlePath  = "base_canvas";             
    private const string assetName        = "BaseCanvas";

    IEnumerator Start () {

        yield return StartCoroutine ( Initialize () );

        ObservableAssetBundle.LoadAssetBundle<GameObject> ( assetBundlePath , assetName )
            .Subscribe ( assetGameObject => {

                GameObject obj          = Instantiate ( assetGameObject ) as GameObject;
                obj.transform.position  = new Vector3 ( 0.0f , 0.0f , 0.0f );

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
