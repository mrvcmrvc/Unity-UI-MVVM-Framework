namespace MVVM
{
    public abstract class VMBase<T> : VMBase
        where T : VMBase<T>
    {
        protected sealed override void ActivateUI()
        {
            bool isUIActive = UIMenuManager.Instance.IsUIActive<T>();
            if (isUIActive)
                return;

            UIMenuManager.Instance.OpenUIMenu(this);
        }
    }
}
