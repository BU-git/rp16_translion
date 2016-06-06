using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class AdminEditEmployerMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Your account was updated by Poortwachtersloket. Check your account details:\n\nGebruikersnaam: {0}\nBedrijfsnaam: {1}\nVoornaam: {2}\nTussenvoegsel: {3}\nAchternaam: {4}\nTelefoon: {5}\nPostalcode: {6}\nAdres: {7}\nPlaats: {8}\n\n You can change them after sign in.";
        private readonly string _subject = "Account was updated";

        public AdminEditEmployerMessageBuilder(string username, string company, string firstname, string nick, string lastname, string phone, string postcode, string adress, string city)
        {
            Body = String.Format(_body, username, company, firstname, nick, lastname, phone, postcode, adress, city);
            Subject = _subject;
        }
    }
}
