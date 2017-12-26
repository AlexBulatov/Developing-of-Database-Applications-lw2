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
using System.Collections;

namespace rpbd_lw2
{
    /// <summary>
    /// Логика взаимодействия для Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        public string queryString = "";

        public Filter()
        {
            InitializeComponent();
            foreach (var i in GetColors()) {
                colorCombo.Items.Add(i);
            }
        }

        private IEnumerable<string> GetColors()
        {
            var allList = Logic.NHibHelper.OpenSession().Query<Model.Parked>().ToList();
            var colorsList = allList.GroupBy(x => x.Color).Select(g => g.First());
            var result = from x in colorsList
                         select x.Color;
            return result;
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            queryString += $"from Parked p join fetch p.holder where p.Color='{colorCombo.SelectedValue}' OR p.Number='{RegNum.Text}'";
        }
    }
}
