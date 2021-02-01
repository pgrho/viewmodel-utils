using System;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace Shipwreck.ViewModelUtils
{
    public abstract class VersionedModel<TKey, TVersion> : ObservableModel, IVersionedModel<TKey, TVersion>
    {
        protected abstract TKey GetKey();

        TKey IVersionedModel<TKey, TVersion>.Key => GetKey();

        protected abstract TVersion GetVersion();

        TVersion IVersionedModel<TKey, TVersion>.Version => GetVersion();

        #region Update

        private WeakReference<object> _PreviousUpdate;
        private TVersion _PreviousUpdateVersion;

        protected abstract TVersion GetVersion(object parameter);

        protected virtual bool Equals(TVersion left, TVersion right)
            => object.Equals(left, right);

        [TargetedPatchingOptOut("")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool Update(object other)
        {
            if (other == this)
            {
                return false;
            }

            if (_PreviousUpdate != null
                && _PreviousUpdate.TryGetTarget(out var pu)
                && !ShouldUpdate(pu, _PreviousUpdateVersion, other))
            {
                return false;
            }

            _PreviousUpdate = new WeakReference<object>(other);
            _PreviousUpdateVersion = GetVersion(other);

            UpdateCore(other);

            return true;
        }

        protected bool ShouldUpdate(object previousUpdateParameter, TVersion previousUpdateVersion, object newParameter)
            => previousUpdateParameter != newParameter || !Equals(GetVersion(previousUpdateParameter), previousUpdateVersion);

        void IVersionedModel<TKey, TVersion>.Update(object other) => Update(other);

        protected abstract void UpdateCore(object other);

        #endregion Update
    }
}
