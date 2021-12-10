using HtmlAgilityPack;
using MambaSportBot.Models;
using System.Collections.Generic;
using System.Linq;

namespace MambaSportBot.ParserMethods
{
    public class Parser
    {
        public HtmlWeb Web { get; set; }
        public List<string> LigsList { get; set; }
        public List<Team> Teams { get; set; }
        public Parser(List<Team> teamList)
        {
            Web = new HtmlWeb();
            LigsList = new List<string>() { "https://www.sports.ru/epl/", "https://www.sports.ru/bundesliga/", "https://www.sports.ru/la-liga/", "https://www.sports.ru/seria-a/",
                "https://www.sports.ru/ligue-1/", "https://www.sports.ru/rfpl/", "https://www.sports.ru/fnl/", "https://www.sports.ru/championship/",
                "https://www.sports.ru/eredivisie/", "https://www.sports.ru/primeira-liga/" };
            Teams = teamList;
        }

        public List<Team> Enter()
        {
            for(int i = 0; i < LigsList.Count; ++i)
            {
                var ligaName = LigsList[i].Substring(22, LigsList[i].Length - 23);
                GetTeams(LigsList[i] + "table/", ligaName);
            }
            return Teams;
        }
        public void Enter(string liga)
        {
            var l = LigsList.Where(t => t == liga).ToList()[0];
                HtmlDocument document = Web.Load(l);
                var ligaName = document.DocumentNode.SelectSingleNode("//h1[@class='titleH1']").InnerText;
                GetTeams(l + "table/", ligaName);
        }

        public void GetTeams(string url, string ligaName)
        {
            HtmlDocument document = Web.Load(url);
            var teamsLinks = document.DocumentNode.SelectNodes("//tbody/tr//a[@class='name']").ToList();
            for(int i = 0; i < teamsLinks.Count; ++i)
            {
                var team = new Team() { Name = teamsLinks[i].InnerText.Trim().ToLower(), Liga = ligaName, Url = teamsLinks[i].GetAttributeValue("href", null) };
                Teams.Add(team);
            }
        }

        public Team GeatTeamsInfo(string url, Team team)
        {
            GeatTeamsCalendar(url + "/calendar/", team);
            GeatTeamsSquad(url + "/team/", team);
            return team;
        }

        public void GeatTeamsSquad(string url, Team team)
        {
            HtmlDocument document = Web.Load(url);
            var p = document.DocumentNode.SelectSingleNode("//table[@class='stat-table sortable-table']");
            List<Player> playerList = new List<Player>();
            for (int i = 1; i <= p.SelectNodes(".//tbody/tr").Count(); ++i)
            {
                Player player = new Player();
                player.Number = p.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[1]", i)).InnerText;
                player.Name = p.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[2]/a", i)).InnerText;
                player.Age = p.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[3]", i)).InnerText;
                player.Height = p.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[4]", i)).InnerText;
                player.Weight = p.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[5]", i)).InnerText;
                player.Position = p.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[6]", i)).InnerText;
                playerList.Add(player);
            }
            team.Players = playerList;
        }

        public void GeatTeamsCalendar(string url, Team team)
        {
            HtmlDocument document = Web.Load(url);
            var cal = document.DocumentNode.SelectSingleNode("//table[@class='stat-table']");
            List<Calendar> calendarList = new List<Calendar>();
            for (int i = 1; i <= cal.SelectNodes(".//tbody/tr").Count(); ++i)
            {
                Calendar calendar = new Calendar(); 
                calendar.Date = cal.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[1]", i)).InnerText.Trim();
                calendar.Tournament = cal.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[2]//a", i)).InnerText;
                calendar.Opponent = cal.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[3]//a", i)).InnerText;
                calendar.Field = cal.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[4]", i)).InnerText.ToLower();
                calendar.Score = cal.SelectSingleNode(string.Format(".//tbody/tr[{0}]/td[5]//a", i)).InnerText.Trim();
                calendarList.Add(calendar);
            }
            team.Calendar = calendarList;
        }
    }
}
