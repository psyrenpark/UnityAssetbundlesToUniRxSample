using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.Linq;
using System.Linq;
using System;

public class SamplePresnter2_2 : MonoBehaviour {

    public Image MoneyImg;
    public Text  MoneyText;

	// Use this for initialization
	void Start () {

        MoneyImg = this.gameObject.Child ( "MoneyImg" )
            .GetComponent<Image> ();
        MoneyImg.enabled = false;

        MoneyText = this.gameObject.Child ( "MoneyText" )
            .GetComponent<Text> ();
        MoneyText.text = "Money : " + ModelManager.GetInst.Money.Value;

        ModelManager.GetInst.Money
            .Subscribe ( x => {
                MoneyText.text = "Money : " + x;
            } );

        ModelManager.GetInst.Money
            .Select(_=> MoneyImg.enabled = true)
            //.DelaySubscription ( TimeSpan.FromMilliseconds ( 300f ) )
            .Delay ( TimeSpan.FromMilliseconds ( 300f ) )
            .Subscribe ( x => {
                MoneyImg.enabled = false;
            } );

    }
	

}
