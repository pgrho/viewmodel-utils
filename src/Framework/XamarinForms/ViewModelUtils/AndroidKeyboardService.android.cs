﻿using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Xamarin.Forms;
namespace Shipwreck.ViewModelUtils
{
    public class AndroidKeyboardService : IKeyboardService
    {
        public void Hide()
        {
            var context = Forms.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}