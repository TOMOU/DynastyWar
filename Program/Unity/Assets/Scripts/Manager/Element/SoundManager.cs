using System.Collections;
using System.Collections.Generic;
using Constant;
using Global;
using UnityEngine;

namespace Manager
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private class ClipCache
        {
            public AudioClip clip { get; private set; }
            public float volume { get; private set; }
            public bool isLoop { get; private set; }
            public ClipCache(AudioClip clip, float volume, bool isLoop)
            {
                this.clip = clip;
                this.volume = volume;
                this.isLoop = isLoop;
            }
        }

        // 원본 사운드 리소스 리스트
        private Dictionary<int, ClipCache> _clipDic = new Dictionary<int, ClipCache>( );
        // 사운드 컴포넌트 리스트
        private List<AudioSource> _sourceList = new List<AudioSource>( );
        // 사운드 테이블 
        private List<SoundModel.Sound> _soundTable;

        protected override void Init( )
        {
            // TODO : 사운드 테이블 로드
            var sm = Model.First<SoundModel>( );

            // TODO : 사운드 리소스 로드(현재는 테이블이 없으므로 임시값)
            if (sm != null)
            {
                _soundTable = sm.soundTable;
                AudioClip clip = null;
                ClipCache cache = null;

                for (int i = 0; i < _soundTable.Count; i++)
                {
                    clip = Resources.Load<AudioClip>(string.Format("Sound/{0}/{1}", _soundTable[i].type, _soundTable[i].name));
                    cache = new ClipCache(clip, _soundTable[i].volumm, _soundTable[i].isLoop);
                    _clipDic.Add(_soundTable[i].index, cache);
                }
            }
            else
            {
                Debug.LogErrorFormat("사운드 테이블 로드 실패");
            }

            Message.AddListener<PlaySoundMsg>(OnPlaySound);
            Message.AddListener<StopSoundMsg>(OnStopSound);
            Message.AddListener<StopAllSoundMsg>(OnStopAllSound);

            Message.Send<Global.TransformAttachMsg>(new Global.TransformAttachMsg(Constant.BehaviorType.Manager, this.transform));
        }

        protected override void Release( )
        {
            base.Release( );

            Message.RemoveListener<PlaySoundMsg>(OnPlaySound);
            Message.RemoveListener<StopSoundMsg>(OnStopSound);
            Message.RemoveListener<StopAllSoundMsg>(OnStopAllSound);
        }

        private void OnPlaySound(PlaySoundMsg msg)
        {
            var source = _sourceList.Find(e => e.isPlaying == false);

            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>( );
                source.playOnAwake = false;
                source.Stop( );
                _sourceList.Add(source);
            }

            ClipCache cache;
            if (_clipDic.TryGetValue((int) msg.soundName, out cache))
            {
                source.clip = cache.clip;
                source.volume = cache.volume;
                source.loop = cache.isLoop;
                source.Play( );
            }
            else
            {
                Debug.LogErrorFormat("{0} 사운드가 없습니다.", msg.soundName);
            }
        }

        private void OnStopSound(StopSoundMsg msg)
        {
            ClipCache cache;
            if (_clipDic.TryGetValue((int) msg.soundName, out cache))
            {
                var source = _sourceList.Find(e => e.clip == cache.clip);
                if (source != null && source.isPlaying)
                    source.Stop( );
            }
            else
            {
                Debug.LogErrorFormat("{0} 사운드가 없습니다.", msg.soundName);
            }
        }

        private void OnStopAllSound(StopAllSoundMsg msg)
        {
            foreach (AudioSource p in _sourceList)
            {
                if (p != null && p.isPlaying)
                {
                    p.Stop( );
                }
            }
        }
    }
}