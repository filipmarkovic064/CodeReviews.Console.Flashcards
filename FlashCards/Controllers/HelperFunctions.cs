using FlashCards.DTOs;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Controllers
{
    internal class HelperFunctions
    {
        internal static SqlConnection OpenConnection()
        {
            var ConnectionString = ConfigurationManager.AppSettings["connectionString"];
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        internal static List<Stack> GetStacks()
        {
            var connection = OpenConnection();
            var ShowAllStacks = connection.CreateCommand();
            ShowAllStacks.CommandText = "SELECT * FROM dbo.StacksTable";
            var reader = ShowAllStacks.ExecuteReader();
            List<Stack> AllStacks = new();
            if (!reader.HasRows)
            {
                AnsiConsole.MarkupLine("[red] No Stacks Found! Returning to Main Menu.\n[/]");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            while (reader.Read())
            {
                Stack stack = new Stack();
                stack.StackId = reader.GetInt32(0);
                stack.StackName = reader.GetString(1);
                AllStacks.Add(stack);
            }
            connection.Close();
            return AllStacks;
        }

        internal static Stack ChooseStack()
        {
            var AllStacks = GetStacks();
            var stack = AnsiConsole.Prompt(
                new SelectionPrompt<Stack>().
                Title("Choose a stack:")
                .UseConverter(s => s.StackName)
                .AddChoices(AllStacks));
            return stack;
        }

        internal static List<Flashcard> GetFlashcards()
        {
            List<Flashcard> AllFlashcardsInStack = new();
            var stack = ChooseStack();
            var connection = OpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM dbo.FlashcardsTable WHERE StackId = {stack.StackId}";
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                AnsiConsole.MarkupLine("[red] No Flashcards Found! Returning to Main Menu.\n [/]");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            while (reader.Read())
            {
                Flashcard flashcard = new Flashcard();
                flashcard.FlashcardId = reader.GetInt32(0);
                flashcard.StackId = reader.GetInt32(1);
                flashcard.FrontInfo = reader.GetString(2);
                flashcard.BackInfo = reader.GetString(3);
                AllFlashcardsInStack.Add(flashcard);
            }
            connection.Close();
            return AllFlashcardsInStack;
        }

        internal static List<Flashcard> GetFlashcards(Stack stack)
        {
            List<Flashcard> AllFlashcardsInStack = new();
            var connection = OpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM dbo.FlashcardsTable WHERE StackId = {stack.StackId}";
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                AnsiConsole.MarkupLine("[red] No Flashcards Found! Returning to Main Menu.\n [/]");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            while (reader.Read())
            {
                Flashcard flashcard = new Flashcard();
                flashcard.FlashcardId = reader.GetInt32(0);
                flashcard.StackId = reader.GetInt32(1);
                flashcard.FrontInfo = reader.GetString(2);
                flashcard.BackInfo = reader.GetString(3);
                AllFlashcardsInStack.Add(flashcard);
            }
            connection.Close();
            return AllFlashcardsInStack;
        }

        internal static Flashcard ChooseFlashcard()
        {
            var allFlashcardsInStack = GetFlashcards();
            var flashcard = AnsiConsole.Prompt(
                            new SelectionPrompt<Flashcard>()
                            .Title("Choose a Flashcard")
                            .UseConverter(s => s.FrontInfo)
                            .AddChoices(allFlashcardsInStack));
            return flashcard;
        }

        internal static void CheckOutput(int checkRows)
        {
            if (checkRows == 0) AnsiConsole.MarkupLine("[red]Command failed, returning to Main Menu[/]");
            else AnsiConsole.MarkupLine("[bold green]Command successful, returning to Main Menu[/]");
            Console.ReadKey();
        }
    }
}
