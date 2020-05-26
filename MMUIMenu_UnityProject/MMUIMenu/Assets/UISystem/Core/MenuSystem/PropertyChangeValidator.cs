namespace MVVM
{
    public class PropertyChangeValidator
    {
        private int _prevPLDHash = -1;

        public bool IsPropertyDirty(object propertyValue)
        {
            int newHashCode = propertyValue.GetHashCode();

            if (newHashCode.Equals(_prevPLDHash))
                return false;

            _prevPLDHash = newHashCode;

            return true;
        }
    }
}