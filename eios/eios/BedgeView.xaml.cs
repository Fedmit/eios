using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BedgeView : Grid
	{
        public string Text
        {
            set
            {
                label.Text = value;
            }
        }

        public int Size
        {
            set
            {
                WidthRequest = value;
                HeightRequest = value;
                circle.WidthRequest = value;
                circle.HeightRequest = value;
            }
        }

		public BedgeView ()
		{
			InitializeComponent ();
		}
	}
}