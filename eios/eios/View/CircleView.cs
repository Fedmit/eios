using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace eios
{
    public partial class CircleView : BoxView
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(CircleView), 0.0);

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public CircleView()
        {
        }
    }
}
