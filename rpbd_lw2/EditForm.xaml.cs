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
    /// Логика взаимодействия для EditForm.xaml
    /// </summary>
    public partial class EditForm : Window
    {
        public Model.Parked parked;

        public EditForm(Model.Parked parked)
        {
            InitializeComponent();
            this.parked = parked;
            carBox.Text = parked.Car;
            entryPicker.SelectedDate = parked.Entry;
            colorBox.Text = parked.Color;
            numBox.Text = parked.Number;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var session = Logic.NHibHelper.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {
                    session.Update(parked);
                    transact.Commit();
                }
            }
            DialogResult = true;
            Close();
        }
    }
}
