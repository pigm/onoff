using System;
using Android.Support.V4.View;
using Android.Views;

namespace BancoSecurityOnOff.Droid.Resources.ViewPageHelper.Transformer
{
    public class FadeTrasnsformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        public void TransformPage(View page, float position)
        {
            page.TranslationX = page.Width * -position;
            if (position <= -1 || position >=1)
            {
                page.Alpha = 0;

            }
            else if (position == 0)
            {
                page.Alpha = 1;
            }
            else
            {
                page.Alpha = 1 - Math.Abs(position);
            }
        }
    }
}
