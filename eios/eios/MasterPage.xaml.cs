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
	public partial class MasterPage : ContentPage
	{
        public MenuListView MenuTop { get { return menuTop; } }
        public MenuListView MenuBottom { get { return menuBottom; } }

        public MasterPage ()
		{
			InitializeComponent ();
		}
	}
}