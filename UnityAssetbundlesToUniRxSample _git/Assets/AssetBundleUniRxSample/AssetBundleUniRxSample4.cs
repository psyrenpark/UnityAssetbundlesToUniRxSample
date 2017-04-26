using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundles;
using UniRx;
using UnityEngine.Audio;
using System.Linq;

public class AssetBundleUniRxSample4 : MonoBehaviour {

    public AudioMixer Mixer;
    public AudioSource BgmAudioSource;

    private const long      requiredDiskSpaceMegabyte   = 100;
    private const long      requiredDiskSpaceByte       = requiredDiskSpaceMegabyte * 1024 * 1024;
    private const string    assetBundle_URL             = "https://test.com/AssetBundles/";
    private const string    assetBundlePath             = "bgm/test_bgm";
    private const string    assetName                   = "TestBGM";


    // Use this for initialization
    IEnumerator Start () {

        Mixer           = Resources.Load ( "SampleAudioMixer" ) as AudioMixer;
        BgmAudioSource  = this.gameObject.AddComponent<AudioSource> ();

        yield return StartCoroutine ( Initialize () );

        Caching.maximumAvailableDiskSpace = requiredDiskSpaceByte;
        ObservableAssetBundle.LoadAssetBundle<AudioClip> ( assetBundlePath , assetName )
            .Subscribe ( clip => {

                Debug.Log( "BGM Download complete!!");
                BgmAudioSource.clip = clip as AudioClip;
                BgmAudioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups ( "BGM" ).FirstOrDefault ();
                BgmAudioSource.Play ();

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

