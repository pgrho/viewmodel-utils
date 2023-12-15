namespace Shipwreck.ViewModelUtils;

public static class BulkUpdateableHelper
{
    public static CommandBuilderBase GetEnterEditModeCommandBuilder<T>(this IBulkUpdateable<T> page)
        where T : class, IBulkUpdateableItem
         => new CommandBuilder(_ =>
         {
             foreach (var e in page.Items)
             {
                 e.CancelEdit();
             }
             page.IsInEditMode = true;
         })
         {
             Title = SR.Edit,
             Description = string.Format(SR.EditDescriptionOfArg0, page.TypeDisplayName),
             Style = BorderStyle.OutlineSecondary,
             Icon = "fas fa-edit",
             IsVisibleGetter = _ => !page.IsInEditMode,
             Handler = page,
         };

    public static CommandBuilderBase GetCommitEditCommandBuilder<T>(this IBulkUpdateable<T> page)
        where T : class, IBulkUpdateableItem
        => new AsyncCommandBuilder(async cmd =>
        {
            var dirty = page.Items.Where(e => e.ShouldSave()).ToList();
            if (dirty.Count > 0
            && !await page.ConfirmAsync(string.Format(SR.UpdatingCountArg0Confirmation, dirty.Count)))
            {
                return;
            }

            try
            {
                page.IsUpdating = true;

                foreach (var item in dirty)
                {
                    if (!await item.SaveAsync(true))
                    {
                        return;
                    }
                }

                page.Items.RemoveAll(e => e.IsNew);
                page.IsInEditMode = false;
            }
            finally
            {
                page.IsUpdating = false;
            }

            page.Refresh();
        })
        {
            Title = SR.Register,
            Description = SR.RegisterDescription,
            Style = BorderStyle.Primary,
            IconGetter = cmd => cmd.IsExecuting ? "fas fa-pulse fa-spinner" : "fas fa-save",
            IsVisibleGetter = _ => page.IsInEditMode,
        };

    public static CommandBuilderBase GetExitEditCommandBuilder<T>(this IBulkUpdateable<T> page)
        where T : class, IBulkUpdateableItem
        => new AsyncCommandBuilder(async () =>
        {
            var dirty = page.Items.Count(e => e.ShouldSave());
            if (dirty > 0
             && !await page.ConfirmAsync(
                 string.Format(SR.DiscardingCountArg0Confirmation, dirty)))
            {
                return;
            }
            page.Items.RemoveAll(e => e.IsNew);
            page.IsInEditMode = false;
        })
        {
            Title = SR.DiscardChanges,
            Description = SR.DiscardChangesDescription,
            Style = BorderStyle.OutlineDanger,
            Icon = "fas fa-times",
            IsVisibleGetter = _ => page.IsInEditMode
        };
}
