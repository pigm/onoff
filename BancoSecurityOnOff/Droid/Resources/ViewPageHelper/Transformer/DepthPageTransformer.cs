using System;
using Android.Support.V4.View;
using Android.Views;

namespace BancoSecurityOnOff.Droid.Resources.ViewPageHelper.Transformer
{
    public class DepthPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        public void TransformPage(View page, float position)
        {
            int pageWidth = page.Width;
            if (position < -1)
            {
                page.Alpha = 0;
            }
            else if (position <=0)
            {
                page.Alpha = 1;
                page.TranslationX = 0;
                page.ScaleX = 1;
                page.ScaleY = 1;
            }
            else if (position <= 1)
            {
                page.Alpha = 1 - position;
                page.TranslationX = pageWidth * -position;
                float scaleFactor = 0.75f - (1 - 0.75f) * (1 - Math.Abs(position));
                page.ScaleX = scaleFactor;
                page.ScaleY = scaleFactor;
            }
            else
            {
                page.Alpha = 0;
            }
        }
    }
}
