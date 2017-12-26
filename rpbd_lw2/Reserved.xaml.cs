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
using rpbd_lw2.Model;
using System.Collections.ObjectModel;

namespace rpbd_lw2
{
    /// <summary>
    /// Логика взаимодействия для Reserved.xaml
    /// </summary>
    public partial class Reserved : Window
    {
        ObservableCollection<Specials> specialsCollection;

        public Reserved()
        {
            InitializeComponent();
            using(var session = Logic.NHibHelper.OpenSession())
            {
                specialsCollection = new ObservableCollection<Specials>(session.Query<Model.Specials>().ToList());
                reservedGrid.ItemsSource=specialsCollection;
                placeBox.SelectedValuePath = "Id";
                foreach(var item in session.Query<Model.Holder>().ToList())
                {
                    placeBox.Items.Add(item);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Model.Specials specials = new Model.Specials
            {
                holder = new Model.Holder { Id = (int)placeBox.SelectedValue, Sector = placeBox.Text[0], Number = Int32.Parse(placeBox.Text.Substring(1)) },
                Number = numBox.Text,
            };
            using(var session = Logic.NHibHelper.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {
                    session.Save(specials);
                    transact.Commit();
                    specialsCollection.Add(specials);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (reservedGrid.SelectedIndex == -1)
                return;
            Model.Specials specials = (Model.Specials) reservedGrid.Items.GetItemAt(reservedGrid.SelectedIndex);
            using (var session = Logic.NHibHelper.OpenSession())
            {
                using (var transact = session.BeginTransaction())
                {
                    session.Delete(specials);
                    specialsCollection.Remove(specials);
                    transact.Commit();
                }
            }
        }
    }
}
