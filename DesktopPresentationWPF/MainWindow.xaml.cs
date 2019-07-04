using System;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Headers;
using DesktopPresentationWPF.Models;

namespace DesktopPresentationWPF {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:2021/Api/")
            };
        }

        private WpfMateriaModel GetData() {

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("Materias/1").Result;

            var prod = response.Content.ReadAsAsync<WpfMateriaModel>().Result;

            return prod;
        }

        // Client
        static HttpClient client = new HttpClient();

        private void GetConsulta_Click(object sender, RoutedEventArgs e) {
            // Consult from API and Modify Textbox
            WpfMateriaModel materia = GetData();
            consultaName.Text = materia.Id + " " + materia.Name + " Año: " + materia.Year + " Es electiva: " + materia.IsElectiva;
        }

        private void ShowConsulta_Click(object sender, RoutedEventArgs e) {
            // Show Obtained Name
            MessageBox.Show($"Name: { consultaName.Text }");
        }
    }
}
