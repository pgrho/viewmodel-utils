namespace Shipwreck.ViewModelUtils
{
    public partial interface ICommandViewModel
    {
        string Title { get; }

        string Description { get; }

        bool IsVisible { get; }
        bool IsEnabled { get; }

        string Icon { get; }

        BorderStyle Style { get; }

        int BadgeCount { get; }

        void Execute();

        void Invalidate();
    }
}
