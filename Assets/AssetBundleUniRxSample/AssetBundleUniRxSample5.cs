using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundles;
using UniRx;
using UnityEngine.Audio;
using System.Linq;
using UnityEngine.UI;


//TODO : [ Criticality : Middle ]
//시뮬레이터 모드에서 작동안됨; 
//audioSource는 되는데 sprite는 안됨; -> 특이점 샘플4 참고 
//내부 코드 수정해야함; 
//이 방법은 차후 고려;


//이 샘플 코드는 캔버스 안의 오브젝트에 add해야함
public class AssetBundleUniRxSample5 : MonoBehaviour {

    public Transform ParentTransform;

    private static int weaponCount =10;
    private static readonly int[] weapons       = Enumerable.Range(0, weaponCount).ToArray();
    private const string    assetBundle_URL     = "https://test.com/AssetBundles/";
    private const string    assetBundlePath     = "sprites/weapons";
    private const string    assetName           = "weapon";

    // Use this for initialization
    IEnumerator Start () {

        ParentTransform = this.gameObject.transform;

        yield return StartCoroutine ( Initialize () );

        int i = 0;
        foreach ( var weapon in weapons ) {
            var obj = new GameObject ( assetName + weapon);
            //TODO: [ Criticality : Low ]
            //조잡한거 같음;
            obj.transform.position = new Vector3( weaponCount * 50 / 2 * -1 + i++ * 50 , 0f , 0f );
            var script = obj.AddComponent<AssetBundleSpriteImage> ();
            script.AssetBundlePath = "sprites/weapons";
            script.AssetName = obj.name;
            obj.transform.SetParent ( ParentTransform , false );
        }
        

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
