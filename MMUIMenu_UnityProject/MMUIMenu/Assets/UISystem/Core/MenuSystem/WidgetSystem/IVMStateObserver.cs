namespace MVVM
{
    public interface IVMStateObserver
    {
        void OnVMPreActivation();
        void OnVMPostActivation();
        void OnVMPreDeactivation();
        void OnVMPostDeactivation();

        VMBase GetViewModel();
    }

    public static class VMStateObserverExntensions
    {
        public static void RegisterToVMStateEvents(this IVMStateObserver targetInterface)
        {
            VMBase.OnVMStateChanged += targetInterface.OnVMStateChanged;
        }

        public static void UnregisterFromVMStateEvents(this IVMStateObserver targetInterface)
        {
            VMBase.OnVMStateChanged -= targetInterface.OnVMStateChanged;
        }

        public static void OnVMStateChanged(this IVMStateObserver targetInterface, VMBase targetVM, EVMState curState)
        {
            if (!targetInterface.GetViewModel().Equals(targetVM))
                return;

            switch (curState)
            {
                case EVMState.PreActivation:
                    targetInterface.OnVMPreActivation();
                    break;
                case EVMState.PostActivation:
                    targetInterface.OnVMPostActivation();
                    break;
                case EVMState.PreDeactivation:
                    targetInterface.OnVMPreDeactivation();
                    break;
                case EVMState.PostDeactivation:
                    targetInterface.OnVMPostDeactivation();
                    break;
            }
        }
    }
}

