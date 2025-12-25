using FlashCards.DTOs;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Controllers
{
    internal static class StudySessionController
    {
        internal static void StartStudySession()
        {
            Stack stack = HelperFunctions.ChooseStack();
            var FlashcardsList = HelperFunctions.GetFlashcards(stack);
            Random rand = new();
            int points = 0;
            int MaxPoints = FlashcardsList.Count;
            int FlashcardIndex;
            List<int> UsedFlashcards = new();

            for(int i = 0; i < MaxPoints; i++)
            {
                do
                {
                    FlashcardIndex = rand.Next(0, MaxPoints);
                    if (UsedFlashcards.Count == MaxPoints) break;
                } while (UsedFlashcards.Contains(FlashcardIndex));
                
                UsedFlashcards.Add(FlashcardIndex);
                Flashcard flashcard = FlashcardsList.ElementAt(FlashcardIndex);

                var answer = AnsiConsole.Prompt(
                             new TextPrompt<string>(flashcard.FrontInfo));
                if (answer.Trim().ToLower() == flashcard.BackInfo.Trim().ToLower())
                {
                    AnsiConsole.MarkupLine("[green] Correct! [/]");
                    points++;
                }
                else AnsiConsole.MarkupLine($"[red] Wrong!, the correct answer was {flashcard.BackInfo} [/]");
            }
            string totalPoints = $"{points.ToString()} / {MaxPoints.ToString()}";
            AnsiConsole.Write(new Markup($"Session complete! Total score: {totalPoints} points \n", UserInterface.MenuStyle));
            var connection = HelperFunctions.OpenConnection();
            var ReportSession = connection.CreateCommand();
            ReportSession.CommandText = @$"INSERT INTO dbo.StudySessionsTable(StackId,StackName, Points) 
                                                                   VALUES('{stack.StackId}','{stack.StackName}','{totalPoints}')";

            var check = ReportSession.ExecuteNonQuery();
            HelperFunctions.CheckOutput(check);
            UserInterface.MainMenu();
        }

        internal static void SeeStudyHistory()
        {
            List<StudySession> AllStudySessions = new();
            var connection = HelperFunctions.OpenConnection();
            var ReadHistory = connection.CreateCommand();
            ReadHistory.CommandText = "SELECT * FROM dbo.StudySessionsTable";
            var reader = ReadHistory.ExecuteReader();
            if (!reader.HasRows)
            {
                AnsiConsole.MarkupLine("[red] No Study Sessions Found! Returning to Main Menu.\n [/]");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            while (reader.Read())
            {
                StudySession session = new();
                session.StudySessionId = reader.GetInt32(0);
                session.StackId = reader.GetInt32(1);
                session.StackName = reader.GetString(2);
                session.Points = reader.GetString(3);
                session.Date = reader.GetDateTime(4);
                AllStudySessions.Add(session);
            }
            foreach(var sesh in AllStudySessions)
            {
                AnsiConsole.Write(new Markup($"\n\tStudySession #{sesh.StudySessionId} ({sesh.StackName.Trim()}) at {sesh.Date.ToString("dd.MM.yyyy")} -> Points: {sesh.Points} \n", UserInterface.MenuStyle));
            }
            connection.Close();
            AnsiConsole.Markup("[italic yellow]\n Press anything to go back to the Main Menu[/]");
            Console.ReadKey();
            UserInterface.MainMenu();
        }
    }
}