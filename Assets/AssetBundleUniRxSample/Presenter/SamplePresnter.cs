using AssetBundles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using Unity.Linq;
using System.Linq;

public class SamplePresnter : Presenter, IProgress<float> {

    public Button button;
    public Text  buttonText;
    public Image[] imgs = new Image[0];


    //private string panelName;

    void Start () {

        Debug.Log ( "SamplePresnter" );



       //panelName = this.gameObject.name;

        button      = this.gameObject.Child ( "Button" )
                    .GetComponent<Button> ();

        //buttonText = this.gameObject.Descendants ( x => x.name == "Text" )
         //           .SingleOrDefault()
         //           .GetComponent<Text> ();

        buttonText = this.gameObject.Descendants ()
                    .OfComponent<Text>()
                    .SingleOrDefault ();

        buttonText.text = "ImgDownload";

        var size  = this.gameObject.Children ()
                    .Where ( x => x.name.StartsWith ( "Image" ) )
                    .Select ( x => x.GetComponent<Image> () )
                    .ToArrayNonAlloc ( ref imgs );
        /*
        button.OnClickAsObservable ()
            .First ()
            .Select(_=> buttonText.text = "Loding")
            .SelectMany ( ObservableAssetBundle.LoadAssetBundle<Sprite> ( "sprites/weapons" , "weapon7" ) )
            .Delay(TimeSpan.FromMilliseconds(300f))
            .Subscribe ( sprite => 
                {
                    imgs[ 0 ].sprite = sprite;
                    buttonText.text = "end";
                }, err => Debug.LogError("errr") );
        */

        var temp = Observable.WhenAll (
            ObservableAssetBundle.LoadAssetBundle<Sprite> ( "sprites/weapons" , "weapon1" ) ,
            ObservableAssetBundle.LoadAssetBundle<Sprite> ( "sprites/weapons" , "weapon5" ) ,
            ObservableAssetBundle.LoadAssetBundle<Sprite> ( "sprites/weapons" , "weapon7" ) );

        button.OnClickAsObservable ()
            .First ()
            .Select ( _ => buttonText.text = "Loding" )
            .SelectMany( temp ).Timeout(TimeSpan.FromSeconds(5))
            .Delay ( TimeSpan.FromMilliseconds ( 300f ) )
            .Subscribe ( sprites =>
            {
                for ( int i = 0 ; i < sprites.Length ; i++ ) {
                    imgs[i].sprite = sprites[i];
                    buttonText.text = "end";
                }
            } , err => Debug.LogError ( "errr" ) );


    }

    public void Report ( float value ) {

    }
}
