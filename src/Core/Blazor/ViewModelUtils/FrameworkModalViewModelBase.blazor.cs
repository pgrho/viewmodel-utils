﻿using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils
{
    public partial class FrameworkModalViewModelBase
    {
        ModalPresenterBase IHasModalPresenter.ModalPresenter => (Page as IHasModalPresenter)?.ModalPresenter;
        ModalPresenterBase IHasPopoverPresenter.PopoverPresenter => (Page as IHasPopoverPresenter)?.PopoverPresenter;
    }
}