namespace MVVM
{
    public abstract class VMBase<T> : VMBase
        where T : VMBase<T>
    {
        protected sealed override void ActivateUI()
        {
            bool isUIActive = VMManager.Instance.IsVMActive<T>();
            if (isUIActive)
                return;

            VMManager.Instance.OpenVM(this);
        }

        protected sealed override void DeactivateUI()
        {
            bool isUIActive = VMManager.Instance.IsVMActive<T>();
            if (!isUIActive)
                return;

            VMManager.Instance.CloseVM(this);
        }
    }
}
