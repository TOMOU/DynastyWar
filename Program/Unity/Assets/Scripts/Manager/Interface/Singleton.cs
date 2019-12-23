using UnityEngine;

namespace Manager
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance = null;
        public static T Singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(T)) as T;

                    // 아직 생성된 오브젝트가 없으면 새로 생성
                    if (_instance == null)
                    {
                        var obj = new GameObject( );
                        _instance = obj.AddComponent<T>( );

                        if (_instance == null)
                        {
                            Debug.LogErrorFormat("Singleton 생성 오류. 클래스명에 오류가 있는지 확인해 주세요.\n클래스명 = {0}", typeof(T).ToString( ));
                        }
                    }
                    else
                    {
                        _instance.Init( );
                    }
                }

                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }

        public void Setup( )
        {
            gameObject.name = GetType( ).Name;

            Init( );
            Message.Send<Global.TransformAttachMsg>(new Global.TransformAttachMsg(Constant.BehaviorType.Manager, this.transform));
        }

        protected virtual void Init( ) { }

        protected virtual void Release( ) { }

        private void OnDestroy( )
        {
            if (_instance == this)
            {
                _instance.Release( );
                _instance = null;
            }
        }
    }
}