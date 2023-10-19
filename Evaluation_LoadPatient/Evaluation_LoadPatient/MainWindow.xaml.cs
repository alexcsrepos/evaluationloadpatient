using Evaluation_LoadPatient.Managers;
using Evaluation_LoadPatient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Evaluation_LoadPatient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PatientsManager _patientsManager;
        public MainWindow()
        {
            InitializeComponent();
            _patientsManager = new PatientsManager();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var result = await _patientsManager.MigratePatientsAsync(textBox_FilePath.Text, textBox_Server.Text, textBox_DB.Text);
            if (result.IsSuccess)
            {
                txtResult.Text = "Task finished succesfully \n" + string.Join("\n", result.Warnings);
            }
            else
            {
                txtResult.Text = "Error: \n" + string.Join("\n", result.Errors);
            }
        }
    }
}
