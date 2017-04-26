using System.Collections;
using UnityEngine;
using UniRx;
using AssetBundles;
using System.Collections.Generic;
using Unity.Linq;

public class ResourcesManagerSample : Singleton<ResourcesManagerSample> {

    public IEnumerator Initialize ( string assetBundleURL ) {
        // Don't destroy this gameObject as we depend on it to run the loading script.     

        InitializeSourceURL ( assetBundleURL );

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize ();
        if ( request != null )
            yield return StartCoroutine ( request );
    }


    private void InitializeSourceURL (string assetBundle_URL ) {
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

    public IEnumerator LoadAssetAsync<T> (string assetBundlePath, string assetName , System.Action<T> Callback = null , bool isNowFree = true )
   where T : UnityEngine.Object {

        var temp = ObservableAssetBundle.LoadAssetBundle<T> ( assetBundlePath
        , assetName )
            .Subscribe ( obj => {
                    Callback ( obj as T );
                if ( isNowFree ) {
                    AssetBundleManager.UnloadAssetBundle ( assetBundlePath);
                }

            } );

        yield return temp;
    }


}
