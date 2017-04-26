using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//이름좀 다시 지어야함
public static class StaticMethod  {

    
    public static string GetAssetBundleName ( string GuiName ) {
        
        
        GuiName = GuiName.ToLower ();
        //GUI-> _gui로 변경하는데 GuiName이 GUI가 포함되면 문제발생할수있음
        //GuiName = GuiName.Replace ( "GUI" , "_gui" );
        GuiName = GuiName.Substring (0, GuiName.Length - 3 ) + "_canvas";

        return GuiName;
    }

    public static string GetAssetName ( string AssetBundleName ) {

        AssetBundleName = FirstCharToUpper(AssetBundleName);
        AssetBundleName = AssetBundleName.Replace ( "_canvas" , "Canvas" );
        return AssetBundleName;
    }

    public static string FirstCharToUpper ( string input ) {
        if ( String.IsNullOrEmpty ( input ) )
            throw new ArgumentException ( "ARGH!" );
        return input.First ().ToString ().ToUpper () + input.Substring ( 1 );
    }

    //스태틱으로 뺄예정
    public static T ConvertJsonData<T> ( string data ) {

        //T UserInformation = LitJson.JsonMapper.ToObject<T> ( data.Trim () );

        //return UserInformation;
        return default(T);
    }


}
