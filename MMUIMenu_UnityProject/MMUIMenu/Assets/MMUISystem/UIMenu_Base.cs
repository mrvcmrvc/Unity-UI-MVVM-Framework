public abstract class UIMenu<T> : UIMenu where T : UIMenu<T>
{
    public static T Instance { get; private set; }

    protected override void Awake()
    {
        Instance = (T)this;

        base.Awake();
    }

    protected override void OnDestroy()
    {
        Instance = null;

        base.OnDestroy();
    }
}