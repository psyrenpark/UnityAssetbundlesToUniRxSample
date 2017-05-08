using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.Linq;
using System.Linq;
using System;

//ButtonPanel Presnter
public class SamplePresnter2_1 : MonoBehaviour {

    public Button   ShowMeTHeMoneyButton;
    public Text     ShowMeTHeMoneyText;

    public Button   CreateUnitButton;
    public Text     CreateUnitText;

    public Slider   UnitCountSlider;
    public Text     UnitCountText;

    // Use this for initialization
    void Start () {

        ShowMeTHeMoneyButton = this.gameObject.Child ( "ShowMeTheMoneyButton" )
             .GetComponent<Button> ();

        ShowMeTHeMoneyText = ShowMeTHeMoneyButton.gameObject.Child ( "Text" )
            .GetComponent<Text> ();
        ShowMeTHeMoneyText.text = "ShowMeTHeMoney !";



        CreateUnitButton = this.gameObject.Child ( "CreateUnitButton" )
            .GetComponent<Button> ();

        CreateUnitText = CreateUnitButton.gameObject.Child ( "Text" )
            .GetComponent<Text> ();
        CreateUnitText.text = "CreateUnit !";



        UnitCountSlider = this.gameObject.Child ( "UnitCountSlider" )
            .GetComponent<Slider> ();
        UnitCountSlider.value = 0.5f;
    
        UnitCountText = UnitCountSlider.gameObject.Child ( "Text" )
            .GetComponent<Text> ();
        UnitCountText.text = "" + UnitCountSlider.value;



        ShowMeTHeMoneyButton.OnClickAsObservable ()
            .Select ( x => x )
            .ThrottleFirst( TimeSpan.FromMilliseconds ( 300f ) )
            .Subscribe ( _ => {
                ModelManager.GetInst.Money.Value += 10000;
                Debug.Log ( "Click!!" );
            } );

        CreateUnitButton.OnClickAsObservable ()
            .Select ( x => x )
            .ThrottleFirst ( TimeSpan.FromMilliseconds ( 300f ) )
            .Subscribe ( _ => {
                int UnitCount = (int)( Math.Round ( UnitCountSlider.value , 1 ) * 10 );

                ModelManager.GetInst.Money.Value -= ( 100 * UnitCount );
                Debug.Log ( "Create Unit!!" );
            } );

        UnitCountSlider.OnValueChangedAsObservable ()
            .ThrottleFirst ( TimeSpan.FromMilliseconds ( 300f ) )
            .SubscribeToText ( UnitCountText , x => (Math.Round ( x , 1 ) * 10) .ToString () );

    }
	
}
