using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace rpbd_lw2
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public Model.Parked parked;

        public Add()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var session = Logic.NHibHelper.OpenSession())
            {
                var list = session.CreateSQLQuery("select holder.id, holder.sector, holder.number from holder left join parked on holder.id = parked.holder_id where parked.car is null").AddEntity(typeof(Model.Holder)).List<Model.Holder>();
                PlaceBox.SelectedValuePath = "Id";
                foreach (var i in list)
                { 
                    PlaceBox.Items.Add(i);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parked = new Model.Parked
            {
                Car = carBox.Text,
                Color = colorBox.Text,
                Number = NumBox.Text,
                holder = new Model.Holder { Sector = PlaceBox.Text[0], Number = Int32.Parse(PlaceBox.Text.Substring(1)), Id=(int)PlaceBox.SelectedValue },
                Entry = DateTime.Now
            };
            bool result = true;
            using (var session = Logic.NHibHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(parked);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("На зарезервированное место\nСтавится неверная машина", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        result = false;
                    }
                }
            }
            DialogResult = result;
        }
    }
}
