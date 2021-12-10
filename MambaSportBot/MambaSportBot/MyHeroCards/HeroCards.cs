using MambaSportBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MambaSportBot.MyHeroCards
{
    public class HeroCards
    {
        public string TeamName { get; set; }
        public List<Team> Teams { get; set; }
        ParserMethods.Parser Parser { get; set; }
        StringBuilder OutPutTeams { get; set; }
        StringBuilder OutPutTeamCalendar { get; set; }
        StringBuilder OutPutTeamPlayers { get; set; }
        private const string LeaguesTop = "топ 5 лиг";
        private const string LeaguesOther = "другие";
        private const string Bundes = "bundesliga";
        private const string BPL = "epl";
        private const string SeriaA = "seria-a";
        private const string LaLiga = "la-liga";
        private const string LigaOne = "ligue-1";
        private const string RPL = "rfpl";
        private const string FNL = "fnl";
        private const string Championship = "championship";
        private const string Eredivisie = "eredivisie";
        private const string LigaNos = "primeira-liga";
        private const string Table = "таблица";
        private const string PFC = "выбор команды";
        private const string Calendar = "календарь";
        private const string Team = "состав";
        private const string Striker = "нападающий";
        private const string Midfielder = "полузащитник";
        private const string Defender = "защитник";
        private const string Goalkeeper = "вратарь";

        public string DialogId { get; set; }

        public HeroCards(string dialogId, WaterfallStepContext dc)
        {
            DialogId = dialogId;
            Teams = (List<Team>)dc.Options;
            Parser = new ParserMethods.Parser(Teams);
            OutPutTeams = new StringBuilder();
            OutPutTeamCalendar = new StringBuilder();
            OutPutTeamPlayers = new StringBuilder();
        }
        private static Attachment GetDefaultCard()
        {
            var heroCard = new HeroCard
            {
                Title = "*Группы лиг*",
                Subtitle = "_Выберите к какой группе лиг, относится ваша команда_",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "Топ 5 лиг", value:LeaguesTop ),
                    new CardAction(ActionTypes.ImBack, "Другие", value:LeaguesOther ),
                }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetHeroCardTop()
        {
            var heroCard = new HeroCard
            {
                Title = "*Лиги*",
                Subtitle = "_Выберите к какой лиге, относится ваша команда_",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "Бундеслига", value:Bundes ),
                    new CardAction(ActionTypes.ImBack, "АПЛ", value:BPL ),
                    new CardAction(ActionTypes.ImBack, "Серия А", value:SeriaA ),
                    new CardAction(ActionTypes.ImBack, "Ла лига", value:LaLiga ),
                    new CardAction(ActionTypes.ImBack, "Лига 1", value:LigaOne )
                }
            };

            return heroCard.ToAttachment();
        }
        private static Attachment GetHeroCardOther()
        {
            var heroCard = new HeroCard
            {
                Title = "*Лиги*",
                Subtitle = "_Выберите к какой лиге, относится ваша команда_",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "РПЛ", value:RPL ),
                    new CardAction(ActionTypes.ImBack, "ФНЛ", value:FNL ),
                    new CardAction(ActionTypes.ImBack, "Чемпионшип", value:Championship ),
                    new CardAction(ActionTypes.ImBack, "Эредивизи", value:Eredivisie ),
                    new CardAction(ActionTypes.ImBack, "Примейра Лига", value:LigaNos )
                }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetHeroCardTable(string league)
        {
            var heroCard = new HeroCard
            {
                Subtitle = "*Выберите команду или таблицу чемпионата*",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "Выбор команды", value:PFC ),
                    new CardAction(ActionTypes.OpenUrl, "Таблица", value:@"https://www.sports.ru/"+ league + "/table/" )
                }
            };

            return heroCard.ToAttachment();
        }
        private static Attachment GetHeroCardLeague(Team club)
        {
            var heroCard = new HeroCard
            {
                Subtitle = "*Выберите что вы хотите узнать о данной команде*",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "Календарь", value:Calendar ),
                    new CardAction(ActionTypes.ImBack, "Состав", value:Team ),
                    new CardAction(ActionTypes.OpenUrl, "Статистика", value:club.Url + "/stat/" )
                }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetHeroCardTeam()
        {
            var heroCard = new HeroCard
            {
                Subtitle = "*Выберите позицию игроков*",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "Нападающие", value:Striker ),
                    new CardAction(ActionTypes.ImBack, "Полузащитники", value:Midfielder ),
                    new CardAction(ActionTypes.ImBack, "Защитники", value:Defender ),
                    new CardAction(ActionTypes.ImBack, "Вратари", value:Goalkeeper )
                }
            };

            return heroCard.ToAttachment();
        }

        private Activity CreateResponse(Activity activity, Attachment attachment)
        {
            var response = activity.CreateReply();
            response.Attachments = new List<Attachment>() { attachment };
            return response;
        }

        public async Task<DialogTurnResult> HeroCardSwitch(WaterfallStepContext dc, string com, Activity act, CancellationToken cancellationToken)
        {
            var promptMessage = MessageFactory.Text("", "", InputHints.ExpectingInput);
            switch (com)
            {
                case LeaguesTop:
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTop()),
                        Choices = new[] { new Choice { Value = Bundes }, new Choice { Value = BPL }, new Choice { Value = SeriaA },
                                              new Choice { Value = LaLiga }, new Choice { Value = LigaOne } },

                    }, cancellationToken);
                case LeaguesOther:
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardOther()),
                        Choices = new[] { new Choice { Value = RPL }, new Choice { Value = FNL }, new Choice { Value = Championship },
                                              new Choice { Value = Eredivisie }, new Choice { Value = LigaNos } },
                    }, cancellationToken);

                case Team:
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTeam()),
                        Choices = new[] { new Choice { Value = Striker }, new Choice { Value = Midfielder },
                            new Choice { Value = Defender }, new Choice { Value = Goalkeeper } },
                    }, cancellationToken);

                case Calendar:
                    GetOutPutTeamCalendar(TeamName);
                    var str = OutPutTeamCalendar.ToString();
                    int lastIndex = 0;
                    for (int i = 0; i < str.Length / 1500 + 1; ++i)
                    {
                        if (1500 + lastIndex < str.Length)
                        {
                            await dc.Context.SendActivityAsync(str.Substring(lastIndex, str.IndexOf("\r\n\r\n", 1500 + lastIndex) - lastIndex));
                            lastIndex = str.IndexOf("\r\n\r\n", 1500 + lastIndex) + 1;
                        }
                        else
                            await dc.Context.SendActivityAsync(str.Substring(lastIndex));
                    }
                    return await HeroCardSwitch(dc, TeamName, act, cancellationToken);

                case Striker:
                    GetOutPutTeamPlayers(TeamName, Striker);
                    await dc.Context.SendActivityAsync(OutPutTeamPlayers.ToString());
                    return await HeroCardSwitch(dc, TeamName, act, cancellationToken);

                case Midfielder:
                    GetOutPutTeamPlayers(TeamName, Midfielder);
                    await dc.Context.SendActivityAsync(OutPutTeamPlayers.ToString());
                    return await HeroCardSwitch(dc, TeamName, act, cancellationToken);

                case Defender:
                    GetOutPutTeamPlayers(TeamName, Defender);
                    await dc.Context.SendActivityAsync(OutPutTeamPlayers.ToString());
                    return await HeroCardSwitch(dc, TeamName, act, cancellationToken);

                case Goalkeeper:
                    GetOutPutTeamPlayers(TeamName, Goalkeeper);
                    await dc.Context.SendActivityAsync(OutPutTeamPlayers.ToString());
                    return await HeroCardSwitch(dc, TeamName, act, cancellationToken);

                case PFC:
                    try
                    {
                        await dc.Context.SendActivityAsync("*Введите название комнады из предложенного списка*");
                        await dc.Context.SendActivityAsync(OutPutTeams.ToString());
                    }
                    catch (Exception ex)
                    {
                        await dc.Context.SendActivityAsync(ex.Message);
                    }

                    return await dc.PromptAsync(DialogId, new PromptOptions { Prompt = promptMessage }, cancellationToken);

                case Bundes:
                    GetOutPutTeams(Bundes);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(Bundes)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case BPL:
                    GetOutPutTeams(BPL);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(BPL)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case SeriaA:
                    GetOutPutTeams(SeriaA);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(SeriaA)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case LaLiga:
                    GetOutPutTeams(LaLiga);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(LaLiga)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case LigaOne:
                    GetOutPutTeams(LigaOne);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(LigaOne)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case RPL:
                    GetOutPutTeams(RPL);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(RPL)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case FNL:
                    GetOutPutTeams(FNL);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(FNL)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case Championship:
                    GetOutPutTeams(Championship);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(Championship)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case Eredivisie:
                    GetOutPutTeams(Eredivisie);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(Eredivisie)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);
                case LigaNos:
                    GetOutPutTeams(LigaNos);
                    return await dc.PromptAsync(DialogId, new PromptOptions
                    {
                        Prompt = CreateResponse(act, GetHeroCardTable(LigaNos)),
                        Choices = new[] { new Choice { Value = PFC }, new Choice { Value = Table } },
                    }, cancellationToken);

                default:
                    var club = new Team();
                    if (Teams == null)
                        return await dc.PromptAsync(DialogId, new PromptOptions
                        {
                            Prompt = CreateResponse(act, GetDefaultCard()),
                            Choices = new[] { new Choice { Value = LeaguesTop }, new Choice { Value = LeaguesOther } },
                        }, cancellationToken);
                    if (Teams.Where(t => t.Name == com).ToList().Count == 0)
                    {
                        //await dc.Context.SendActivityAsync(Teams[0].Name);
                        //await dc.Context.SendActivityAsync(com);
                        return await dc.PromptAsync(DialogId, new PromptOptions
                        {
                            Prompt = CreateResponse(act, GetDefaultCard()),
                            Choices = new[] { new Choice { Value = LeaguesTop }, new Choice { Value = LeaguesOther } },
                        }, cancellationToken);
                    }
                    else
                    {
                        club = Teams.Where(t => t.Name == com).ToList()[0];
                        TeamName = com;
                        return await dc.PromptAsync(DialogId, new PromptOptions
                        {
                            Prompt = CreateResponse(act, GetHeroCardLeague(club)),
                            Choices = new[] { new Choice { Value = Calendar }, new Choice { Value = Team }, new Choice { Value = "Статистика" } },
                        }, cancellationToken);
                    }

            }
        }
        public void GetOutPutTeams(string liga)
        {
            OutPutTeams.Clear();
            if (Teams.Count == 0)
                Teams = Parser.Enter();
            var teamOutPutList = Teams.Where(t => t.Liga == liga).ToList();
            for (int i = 0; i < teamOutPutList.Count(); ++i)
            {
                OutPutTeams.Append(teamOutPutList[i].Name + "\r\n\r\n");
            }
        }

        public void GetOutPutTeamCalendar(string teamName)
        {
            OutPutTeamCalendar.Clear();
            var team = Teams.Where(t => t.Name == teamName).ToList()[0];
            team = Parser.GeatTeamsInfo(team.Url, team);
            var calendarList = team.Calendar.ToList();
            for (int i = 0; i < calendarList.Count; ++i)
                OutPutTeamCalendar.Append("Дата: " + "**" + calendarList[i].Date + "**" + " турнир: " + "**" + calendarList[i].Tournament +
                    "**" + " соперник: " + "**" + calendarList[i].Opponent + "**" + " играют: " + "**" + calendarList[i].Field +
                    "**" + " счет: " + "**" + calendarList[i].Score + "**" + "\r\n\r\n");
        }
        public void GetOutPutTeamPlayers(string teamName, string position)
        {
            OutPutTeamPlayers.Clear();
            var team = Teams.Where(t => t.Name == teamName).ToList()[0];
            team = Parser.GeatTeamsInfo(team.Url, team);
            var playerList = team.Players.Where(p => p.Position == position).ToList();
            for (int i = 0; i < playerList.Count(); ++i)
                OutPutTeamPlayers.Append("Номер: " + "**" + playerList[i].Number + "**" + " ФИО: " + "**" + playerList[i].Name + "**" +
                    " возраст: " + "**" + playerList[i].Age + "**" + " рост: " + "**" + playerList[i].Height + "**" +
                    " вес: " + "**" + playerList[i].Weight + "**" + " амплуа: " + "**" + playerList[i].Position + "**" + "\r\n\r\n");
        }
    }
}
