namespace Shipwreck.ViewModelUtils;

public interface IModelCreator<TDest>
{
    TDest Create(object host);
}
