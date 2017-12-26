using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rpbd_lw2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Model.Parked> parkedCollection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(var session = Logic.NHibHelper.OpenSession())
            {
                parkedCollection = new ObservableCollection<Model.Parked>(session.CreateQuery("from Parked p join fetch p.holder").List<Model.Parked>());
                parkedGrid.ItemsSource = parkedCollection;
            }
            clearButton.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var session = Logic.NHibHelper.OpenSession())
            {
                holderGrid.ItemsSource = from x in (session.Query<Model.Holder>().ToList())
                                         select x;
                parkedCollection = new ObservableCollection<Model.Parked>(session.Query<Model.Parked>().ToList());
                parkedGrid.ItemsSource = parkedCollection;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Filter form = new Filter();
            if (form.ShowDialog()==true)
            {
                using (var session = Logic.NHibHelper.OpenSession()) {
                    parkedCollection = new ObservableCollection<Model.Parked>( session.CreateQuery(form.queryString).List<Model.Parked>());
                    parkedGrid.ItemsSource = parkedCollection;
                }
                clearButton.IsEnabled = true;
            }
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            Add form = new Add();
            if (form.ShowDialog() == true)
            {
                parkedCollection.Add(form.parked);
            }
        }

        private void RemoveCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту машину?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (parkedGrid.SelectedIndex != -1)
                {
                    Model.Parked removed = (Model.Parked)parkedGrid.Items.GetItemAt(parkedGrid.SelectedIndex);
                    using (var session = Logic.NHibHelper.OpenSession())
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            session.Delete(removed);
                            parkedCollection.RemoveAt(parkedGrid.SelectedIndex);
                            transaction.Commit();
                        }
                    }
                }
            }
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (parkedGrid.SelectedIndex == -1)
                return;
            
            Model.Parked edited = (Model.Parked)parkedGrid.Items.GetItemAt(parkedGrid.SelectedIndex);
            EditForm form = new EditForm(edited);
            if(form.ShowDialog()==true)
                parkedCollection[parkedGrid.SelectedIndex] = form.parked;
        }

        private void ReservedCarsButton_Click(object sender, RoutedEventArgs e)
        {
            Reserved form = new Reserved();
            form.Show();
        }

        private void boomParking_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ALLAHU AKBAR!!\nINSHALLAH TAKBIR", "Allah");

            using(var session = Logic.NHibHelper.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {
                    foreach(var car in parkedCollection)
                    {
                        session.Delete(car);
                    }
                    transact.Commit();
                    session.Close();
                    parkedCollection.Clear();
                }
            }
        }
    }
}
