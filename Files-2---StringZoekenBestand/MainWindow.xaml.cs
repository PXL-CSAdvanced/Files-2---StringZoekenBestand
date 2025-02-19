using Microsoft.Win32;
using System.IO;
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

namespace Files_2___StringZoekenBestand;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = "Alle bestanden (*.*)|*.*|Tekstbestanden (*.txt)|*.txt",
            FilterIndex = 2,
            FileName = "Email.txt",
            InitialDirectory = Environment.CurrentDirectory //onder bin\Debug
        };

        if (ofd.ShowDialog() == true)
        {
            fileTextBox.Text = ofd.FileName; //volledig pad en bestandsnaam
        }
    }
    private void BtnZoeken_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Volledige pad van bestand
            string pad = fileTextBox.Text;
            // Bestandsnaam uit pad halen zonder de extensie.
            string bestandsnaam = System.IO.Path.GetFileName(fileTextBox.Text);
            // De string die we willen zoeken in het tekstbestand
            string gezochteString = searchStringTextBox.Text;

            StringBuilder sb = new StringBuilder();
            int aantalMatches = 0;
            int huidigeRegel = 1;

            using (StreamReader sr = new StreamReader(pad))
            {
                // Lees bestand regel per regel
                while (!sr.EndOfStream)
                {
                    string lijn = sr.ReadLine();
                    // IndexOf() geeft -­1 als het niet gevonden wordt en anders de index (0,1,2,...)
                    bool isGevonden = lijn.IndexOf(gezochteString) > -1;
                    if (isGevonden)
                    {
                        sb.AppendLine($"{bestandsnaam}: regel : {huidigeRegel} {lijn}");
                        aantalMatches++;
                    }
                    huidigeRegel++;
                }
            }
            sb.AppendLine($"\r\n{gezochteString} gevonden in {aantalMatches} regels (totaal {huidigeRegel - 1} regels).");
            resultTextBox.Text = sb.ToString();
        }
        catch (ArgumentException)
        {
            MessageBox.Show("Geen pad/bestand geselecteerd!",
                "Foutmelding: FileStream is leeg",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (FileNotFoundException)
        {
            MessageBox.Show("Bestand niet gevonden",
                "Foutmelding:",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show("Enkel directory gevonden.\r\nSelecteer een bestand aub!",
                "Foutmelding:",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Systeemfout\r\n{ex.Message}\r\n{ex.ToString()}", "Foutmelding");
        }
    }
}