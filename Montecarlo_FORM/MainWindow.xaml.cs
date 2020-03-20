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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Montecarlo_FORM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataMentahViewModel dataMentahViewModel = new DataMentahViewModel();
        MonteCarlo monteCarlo;
        public MainWindow()
        {
            InitializeComponent();

            // pull data to the gridview
            RefreshDataGrid();            
        }

        private void RefreshDataGrid()
        {
            // update datagridview            
            dgv_datahistoris.ItemsSource = dataMentahViewModel.GetAllDataMentah();
            dgv_datahistoris.Items.Refresh();

            // update number of last index
            txtbox_mingguke.Text = dataMentahViewModel.GetLastMinggu();

            // MONTECARLO
            monteCarlo = new MonteCarlo(dataMentahViewModel.DataTunggal);

            #region Generate Table of Montecarlo --> variable penting, frek, Prob., Kum.Prob
            dgv_montecarlo.ItemsSource = monteCarlo.GenerateMonteCarloTable();
            dgv_montecarlo.Items.Refresh();
            #endregion
        }

        #region INTERACTION BUTTON
        private void btn_inputdata_Click(object sender, RoutedEventArgs e)
        {
            string minggu = txtbox_mingguke.Text;
            string datafaktual = txtbox_frekuensi.Text;

            if (datafaktual == "") MessageBox.Show("tidak ada data faktual");
            else
            {
                if (MessageBox.Show("Apakah anda yakin?", "asharilabs", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (dataMentahViewModel.InputDataBaru(minggu, datafaktual))
                    {
                        MessageBox.Show("Data baru telah diinputkan");
                        RefreshDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("terjadi kesalahan koneksi");
                    }
                }
            }
        }

        private void btn_generateForecast_Click(object sender, RoutedEventArgs e)
        {
            monteCarlo.NOF = int.Parse(txtbox_countOfForecast.Text);

            dgv_hasilForecast.ItemsSource = monteCarlo.Penaksiran();
            dgv_hasilForecast.Items.Refresh();
        }
        #endregion
        #region MISC. FUNCTION
        void ResetForecaster()
        {
            txtbox_countOfForecast.Clear();
            dgv_hasilForecast.Items.Clear();
        }
        #endregion
    }
}
