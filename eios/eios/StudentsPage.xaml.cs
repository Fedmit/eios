using eios.ViewModel;
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
        public StudentsPage()
        {
            InitializeComponent();
            var studentsViewModel = new StudentsListViewModel();
            BindingContext = studentsViewModel;
            timeLable.Text = "Время";
            timeLable.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            occupationLable.Text = "Название предмета";
            occupationLable.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            defaultLable.Text = "Всего : X";
            defaultLable.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            onSiteLable.Text = "Присутствуют : Y";
            onSiteLable.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            marked.Text = "Отметить посещаемость";
            marked.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            back.Text = "Назад";
            back.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            studentListView.SeparatorVisibility = SeparatorVisibility.None;
            studentListView.RowHeight = 40;
            studentListView.ItemTemplate = new DataTemplate(() =>
            {
                ImageCell imageCell = new ImageCell();
                imageCell.SetBinding(ImageCell.TextProperty, "sText");
                imageCell.SetBinding(ImageCell.ImageSourceProperty, "sIcon");
                return imageCell;
            });
            studentListView.ItemTapped += (sender, e) =>
            {
                var LVElement = (Student)e.Item;
                if (LVElement.sFlag)
                {
                    L_Students[LVElement.iIndex].sFlag = false;
                    L_Students[LVElement.iIndex].sIcon = "Check.jpg";
                }
                else
                {
                    L_Students[LVElement.iIndex].sFlag = true;
                    L_Students[LVElement.iIndex].sIcon = "Uncheck.jpg";
                }
                studentListView.ItemTemplate = new DataTemplate(() =>
                {
                    ImageCell imageCell = new ImageCell();
                    imageCell.SetBinding(ImageCell.TextProperty, "FullName");
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "sIcon");
                    return imageCell;
                });
            };

        }
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
}