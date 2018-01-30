using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuListCell : ViewCell
	{
        public static readonly BindableProperty TitleProperty = 
            BindableProperty.Create("Title", typeof(string), typeof(MenuListCell), "Title");
        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public MenuListCell ()
		{
			InitializeComponent ();
		}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                label.Text = Title;
            }
        }
    }
}