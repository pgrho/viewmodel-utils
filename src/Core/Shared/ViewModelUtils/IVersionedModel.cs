namespace Shipwreck.ViewModelUtils
{
    public interface IVersionedModel<TKey, TVersion>
    {
        TKey Key { get; }
        TVersion Version { get; }

        void Update(object other);
    }
}
