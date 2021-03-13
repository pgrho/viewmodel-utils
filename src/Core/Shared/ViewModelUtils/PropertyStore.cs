using System;

namespace Shipwreck.ViewModelUtils
{
    public static class PropertyStore
    {
        public static bool IsChanged<T>(this PropertyStore<T> store)
            => !((store.CurrentValue as IEquatable<T>)?.Equals(store.OriginalValue)
                ?? (store.OriginalValue as IEquatable<T>)?.Equals(store.CurrentValue)
                ?? store.CurrentValue?.Equals(store.OriginalValue)
                ?? store.OriginalValue?.Equals(store.CurrentValue)
                ?? true);

        public static bool IsChanged<T>(this PropertyStore<T?> store)
            where T : struct
            => !((store.CurrentValue as IEquatable<T>)?.Equals(store.OriginalValue)
                ?? (store.OriginalValue as IEquatable<T>)?.Equals(store.CurrentValue)
                ?? store.CurrentValue?.Equals(store.OriginalValue)
                ?? store.OriginalValue?.Equals(store.CurrentValue)
                ?? true);

        public static void Set<T>(this ref PropertyStore<T> store, T value)
            => store = new PropertyStore<T>(value);

        public static void SetCurrentValue<T>(this ref PropertyStore<T> store, T value)
            => store = new PropertyStore<T>(store.OriginalValue, value);

        public static void ClearOriginalValue<T>(this ref PropertyStore<T> store)
            => store = new PropertyStore<T>(store.CurrentValue);
    }
    public struct PropertyStore<T>
    {
        public PropertyStore(T value)
        {
            OriginalValue = value;
            CurrentValue = value;
        }

        public PropertyStore(T originalValue, T currentValue)
        {
            OriginalValue = originalValue;
            CurrentValue = currentValue;
        }

        public T OriginalValue { get; }
        public T CurrentValue { get; }
    }
}
