using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionModel : Model
{
    public void Setup( )
    {
        // Data 저장유무 파악하여 없는값이 있으면 해당값 초기화.
        Initializing_Data( );

        // PlayerPrefs에 저장된 데이터를 불러온다.
        Load_Data( );
    }

    private void Initializing_Data( )
    {

    }

    public void Load_Data( )
    {

    }
}