using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class ILogic : MonoBehaviour
    {
        public void Setup( )
        {
            gameObject.name = GetType( ).Name;
            Message.Send<Global.TransformAttachMsg>(new Global.TransformAttachMsg(Constant.BehaviorType.Logic, this.transform));

            Initialize( );
        }

        public void ReleaseLogic( )
        {
            Release( );
        }

        protected virtual void Initialize( ) { }

        protected virtual void Release( ) { }
    }
}