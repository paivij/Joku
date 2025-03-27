using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Joku
{
    class DatabaceQuery
    {
        public async Task<List<Henkilo>> HaeKaikkiHenkilot()
        {
            List<Henkilo> henkilot = new List<Henkilo>();

            DatabaseConnector db = new DatabaseConnector();

            try
            {
                using (MySqlConnection conn = db._getConnection())  // Kutsutaan yhteysmetodia
                {
                    await conn.OpenAsync();  // Asynkroninen yhteyden avaus
                    string sql = "SELECT htun, enimi, snimi, kunta FROM henkilo";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()) // Asynkroninen lukeminen
                        {
                            while (await reader.ReadAsync())
                            {
                                string htun = reader.GetString(0);
                                string enimi = reader.GetString(1);
                                string snimi = reader.GetString(2);
                                string kunta = reader.GetString(3);

                                // Lisää tulokset listalle
                                henkilot.Add(new Henkilo { Htun = htun, Enimi = enimi, Snimi = snimi, Kunta = kunta });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Voit käyttää Debug.WriteLine tai näyttää virheilmoituksen käyttöliittymässä
                Console.WriteLine($"Virhe haettaessa tietoa: {ex.Message}");
            }

            return henkilot;  // Palautetaan haetut henkilöt
        }
    }
}

