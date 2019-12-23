using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : Model
{
    public ModelRef<SoundModel> _soundModel = new ModelRef<SoundModel>( );
    public ModelRef<UnitModel> _unitModel = new ModelRef<UnitModel>( );
    public ModelRef<OptionModel> _optionModel = new ModelRef<OptionModel>( );

    public bool loadCompleteGlobalContent = false;

    public void Setup( )
    {
        // 사운드 모델 로드
        _soundModel.Model = new SoundModel( );
        _soundModel.Model.Setup( );

        // 유닛 모델 로드
        _unitModel.Model = new UnitModel( );
        _unitModel.Model.Setup( );

        // 옵션 모델 로드
        _optionModel.Model = new OptionModel( );
        _optionModel.Model.Setup( );
    }
}