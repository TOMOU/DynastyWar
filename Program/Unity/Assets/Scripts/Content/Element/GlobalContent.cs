using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Content
{
    public class GlobalContent : IContent
    {
        protected override void OnLoad( )
        {
            _isGlobalContent = true;

            // UI 등록
            // _dialogList.Add(typeof(Dialog.GlobalOptionDialog));

            base.OnLoad( );

            Model.First<GameModel>( ).loadCompleteGlobalContent = true;
        }

        protected override void OnUnload( )
        {

        }

        protected override void OnEnter( )
        {

        }

        protected override void OnExit( )
        {

        }
    }
}