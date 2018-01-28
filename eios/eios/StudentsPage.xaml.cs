using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentsPage : ContentPage
	{
		public StudentsPage ()
		{
			InitializeComponent ();
            var L_Students = new List<Student>();
            int index = 0;
            var s1 = new Student() { sIcon = "unchecked-checkbox", sText = "Вася пупкин", sFlag = false, iIndex = index };
            index++;
            var s2 = new Student() { sIcon = "unchecked-checkbox", sText = "Вася пупкин", sFlag = false, iIndex = index };
            L_Students.Add(s1);
            L_Students.Add(s2);
            studentListView.ItemTemplate = new DataTemplate(typeof(StudentsViewCell));
            studentListView.ItemsSource = L_Students;
            studentListView.ItemTapped += async (sender, e) =>
            {
                var LVElement = (Student)e.Item;
                if (LVElement.sFlag) 
                {
                    L_Students[LVElement.iIndex].sFlag = false;
                    L_Students[LVElement.iIndex].sIcon = "checked-checkbox";
                }
                else
                {
                    L_Students[LVElement.iIndex].sFlag = true;
                    L_Students[LVElement.iIndex].sIcon = "unchecked-checkbox";
                }
                studentListView.ItemTemplate = new DataTemplate(typeof(StudentsViewCell));
            };

        }
	}
    public class Student
    {
        public string sIcon { get; set; } 
        public string sText { get; set; } 
        public bool sFlag { get; set; } 
        public int iIndex { get; set; }
    }
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filename = (string)value;
            return ImageSource.FromFile(filename);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class StudentsViewCell : ViewCell 
    {
        public StudentsViewCell() 
        {
            var NameLable = new Label();
            NameLable.SetBinding(Label.TextProperty, "sText");
            NameLable.VerticalOptions = LayoutOptions.Center;
            NameLable.BindingContextChanged += (sender, e) =>
            {
            };
            var Icon = new Image();
            Icon.SetBinding(Image.SourceProperty, new Binding("sIcon", BindingMode.OneWay, new StringToImageConverter()));
            Icon.HeightRequest = 15;
            Icon.VerticalOptions = LayoutOptions.Center;
            var s = new StackLayout();
            s.Orientation = StackOrientation.Horizontal;     
            s.Children.Add(Icon);
            s.Children.Add(NameLable);
            this.View = s;
        }
    }

}