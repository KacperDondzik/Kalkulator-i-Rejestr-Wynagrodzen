using KalkulatorWynagrodzenApp;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalkulator_i_Rejestr_Wynagrodzen
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ModelWpisu> Rejestr { get; set; } = new ObservableCollection<ModelWpisu>();

        public MainWindow()
        {
            InitializeComponent();
            dgRejestr.ItemsSource = Rejestr;

            Rejestr.Add(new ModelWpisu { Data = "2023-10-27", KwotaBrutto = "5000,00", Umowa = "O pracę", Netto = "3500,00", Ulga = "Tak" });
            Rejestr.Add(new ModelWpisu { Data = "2023-10-20", KwotaBrutto = "4500,00", Umowa = "Zlecenie", Netto = "3150,00", Ulga = "Nie" });
            Rejestr.Add(new ModelWpisu { Data = "2023-10-13", KwotaBrutto = "5500,00", Umowa = "O pracę", Netto = "3850,00", Ulga = "Tak" });
        }

        private void BtnOblicz_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtBrutto.Text.Replace('.', ','), out double brutto))
            {
                if (!KalkulatorWynagrodzen.SprawdzPoprawnoscBrutto(brutto))
                {
                    MessageBox.Show("Kwota brutto musi być większa od 0!", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TypUmowy typ = rbUmowaOPrace.IsChecked == true ? TypUmowy.UmowaOPrace : TypUmowy.UmowaZlecenie;
                bool ulga = chkUlga.IsChecked ?? false;

                try
                {
                    var kalkulator = new KalkulatorWynagrodzen(brutto, typ, ulga);
                    double zus = kalkulator.ObliczZusSpoleczne();
                    double zdrowotna = kalkulator.ObliczSkladkeZdrowotna();
                    double pit = kalkulator.ObliczPit();
                    double netto = kalkulator.ObliczNetto();

                    txtWynikNetto.Text = $"Wynagrodzenie netto: {netto:N2} PLN";
                    txtZus.Text = $"👥 Składki ZUS (społeczne): {zus:N2} PLN";
                    txtZdrowotna.Text = $"🛡️ Składka zdrowotna: {zdrowotna:N2} PLN";
                    txtPit.Text = $"📄 Zaliczka na podatek PIT: {pit:N2} PLN";

                    string nazwaUmowy = typ == TypUmowy.UmowaOPrace ? "O pracę" : "Zlecenie";
                    Rejestr.Insert(0, new ModelWpisu
                    {
                        Data = DateTime.Now.ToString("yyyy-MM-dd"),
                        KwotaBrutto = brutto.ToString("N2"),
                        Umowa = nazwaUmowy,
                        Netto = netto.ToString("N2"),
                        Ulga = ulga ? "Tak" : "Nie"
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną kwotę wynagrodzenia brutto!", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class ModelWpisu
    {
        public string Data { get; set; }
        public string KwotaBrutto { get; set; }
        public string Umowa { get; set; }
        public string Netto { get; set; }
        public string Ulga { get; set; }
    }
}
