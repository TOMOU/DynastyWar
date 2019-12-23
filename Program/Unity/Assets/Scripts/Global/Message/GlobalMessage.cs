namespace Global
{
    public class ChangeSceneMsg : Message
    {
        public Constant.GameScene scene { get; private set; }
        public ChangeSceneMsg(Constant.GameScene scene)
        {
            this.scene = scene;
        }
    }

    public class TransformAttachMsg : Message
    {
        public Constant.BehaviorType type { get; private set; }
        public UnityEngine.Transform transform { get; private set; }
        public TransformAttachMsg(Constant.BehaviorType type, UnityEngine.Transform transform)
        {
            this.type = type;
            this.transform = transform;
        }
    }

    public class PlaySoundMsg : Message
    {
        public Constant.SoundName soundName { get; private set; }
        public PlaySoundMsg(Constant.SoundName soundName)
        {
            this.soundName = soundName;
        }
    }

    public class StopSoundMsg : Message
    {
        public Constant.SoundName soundName { get; private set; }
        public StopSoundMsg(Constant.SoundName soundName)
        {
            this.soundName = soundName;
        }
    }

    public class StopAllSoundMsg : Message
    {
        public StopAllSoundMsg( ) { }
    }
}