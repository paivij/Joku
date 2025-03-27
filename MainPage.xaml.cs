
using MySqlConnector;
using System.Collections.ObjectModel;

namespace Joku
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Henkilo> Henkilot { get; set; } = new();
       

        public MainPage()
        {
            InitializeComponent();
            HenkilotListView.ItemsSource = Henkilot; // Sidotaan lista näkymään
        }

        private async void OnDatabaseClicked(object sender, EventArgs e)
        {
            DatabaseConnector dbc = new DatabaseConnector();
            try
            {
                var conn = dbc._getConnection();
                conn.Open();
                await DisplayAlert("Onnistui", "Tietokanta yhteys aukesi", "OK"); 
            }
            catch (MySqlException ex)
            {
                await DisplayAlert("Failure", ex.Message, "OK");
            }
        }

        private async void HaeHenkilot_Clicked(object sender, EventArgs e)
        {
            Henkilot.Clear(); // Tyhjennetään vanhat tiedot

            DatabaseConnector db = new DatabaseConnector();

            using (MySqlConnection conn = db._getConnection())
            {
                try
                {
                    await conn.OpenAsync();
                    string sql = "SELECT htun, enimi, snimi, kunta FROM henkilo";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Henkilot.Add(new Henkilo
                            {
                                Htun = reader.GetString(0),
                                Enimi = reader.GetString(1),
                                Snimi = reader.GetString(2),
                                Kunta = reader.GetString(3)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Virhe", $"Tietojen haku epäonnistui: {ex.Message}", "OK");
                }
            }
        }


    }

}
