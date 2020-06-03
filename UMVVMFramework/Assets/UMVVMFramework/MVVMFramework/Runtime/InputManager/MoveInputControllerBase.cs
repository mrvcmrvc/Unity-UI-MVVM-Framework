using System;
using UnityEngine;

namespace MVVM
{
    public abstract class MoveInputControllerBase : IDisposable
    {
        public Action<Vector2, MoveInputControllerBase> OnInputStarted { get; set; }
        public Action<Vector2, MoveInputControllerBase> OnInputPerformed { get; set; }
        public Action<Vector2, MoveInputControllerBase> OnInputCancelled { get; set; }

        protected bool IsMoveInputStarted { get; private set; }
        public bool IsActive { get; private set; }
        public bool IgnoreZeroDir { get; private set; }

        public MoveInputControllerBase(bool isActive, bool ignoreZeroDir)
        {
            IsActive = isActive;
            IgnoreZeroDir = ignoreZeroDir;

            IsMoveInputStarted = false;
        }

        public virtual void CheckInput()
        {
            if (!CheckIfInputShouldProcessed())
                return;

            Vector2 inputDirection = CheckDirection();

            CheckForEvents(inputDirection);
        }

        protected bool CheckIfInputShouldProcessed()
        {
            if (!IsMoveInputStarted
                && !IsAnyInput()
                && !AdditionalInputEligibleCheck())
                return false;
            else
                return true;
        }

        protected void CheckForEvents(Vector2 inputDir)
        {
            bool isAnyInput = IsAnyInput();

            if (!IsMoveInputStarted
                && isAnyInput)
            {
                if (IgnoreZeroDir && inputDir.normalized.magnitude <= Mathf.Epsilon)
                    return;

                MoveStarted(inputDir);
            }
            else if (IsMoveInputStarted
                && isAnyInput)
                MovePerformed(inputDir);
            else if (IsMoveInputStarted
                && !isAnyInput)
                MoveCancelled(inputDir);
        }

        private void MoveStarted(Vector2 direction)
        {
            IsMoveInputStarted = true;

            OnInputStarted?.Invoke(direction, this);
        }

        private void MovePerformed(Vector2 direction)
        {
            OnInputPerformed?.Invoke(direction, this);
        }

        private void MoveCancelled(Vector2 direction)
        {
            IsMoveInputStarted = false;

            OnInputCancelled?.Invoke(direction, this);
        }

        protected virtual void DisposeCustomActions()
        {
        }

        protected abstract Vector2 CheckDirection();
        protected abstract bool IsAnyInput();
        protected abstract bool AdditionalInputEligibleCheck();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeCustomActions();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}