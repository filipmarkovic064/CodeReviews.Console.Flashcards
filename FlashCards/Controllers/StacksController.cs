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
    internal class StacksController
    {

        internal void ViewStacks()
        {
            var StacksList = HelperFunctions.GetStacks();
            int i = 1;
            foreach(var stack in StacksList)
            {
                AnsiConsole.Write(new Markup($"Stack #{i}: {stack.StackName.Trim()}, ID: {stack.StackId}\n", UserInterface.MenuStyle));
                i++;
            }
            AnsiConsole.MarkupLine("\n[italic yellow] Press any key to return[/]");
            Console.ReadKey();
            UserInterface.ManageStacksMenu();
        }
        internal void RenameStack()
        {

            var connection = HelperFunctions.OpenConnection();
            var stack = HelperFunctions.ChooseStack();
            var NewName = AnsiConsole.Prompt(
                          new TextPrompt<string>("Please provide a new name for the stack:"));

            var command = connection.CreateCommand();
            command.CommandText = @$"UPDATE dbo.StacksTable
                           SET StackName = @NewName
                           WHERE StackId = '{stack.StackId}';";
            command.Parameters.AddWithValue("@NewName", NewName);
            var check = command.ExecuteNonQuery();
            connection.Close();
            HelperFunctions.CheckOutput(check);
            UserInterface.ManageStacksMenu();
        }

        internal void DeleteStack()
        {
            var connection = HelperFunctions.OpenConnection();
            var stack = HelperFunctions.ChooseStack();

            var WipeFlashcards = connection.CreateCommand();
            WipeFlashcards.CommandText = $@"DELETE FROM dbo.FlashcardsTable WHERE StackId = '{stack.StackId}'";
            WipeFlashcards.ExecuteNonQuery();


            var WipeStudySessions = connection.CreateCommand();
            WipeStudySessions.CommandText = $@"DELETE FROM dbo.StudySessionsTable WHERE StackId = '{stack.StackId}'";
            WipeStudySessions.ExecuteNonQuery();

            var WipeStack = connection.CreateCommand();
            WipeStack.CommandText = $@"DELETE FROM dbo.StacksTable WHERE StackId = '{stack.StackId}'";
            var check = WipeStack.ExecuteNonQuery();
            
            connection.Close();
            HelperFunctions.CheckOutput(check);
            UserInterface.ManageStacksMenu();
        }

        internal void InsertStack()
        {
            var connection = HelperFunctions.OpenConnection();
            var Name = AnsiConsole.Prompt(new TextPrompt<string>("Please provide a stack name:"));
            try
            {
                var InsertNewStack = connection.CreateCommand();
                InsertNewStack.CommandText = @"INSERT INTO dbo.StacksTable(StackName)
                                                                VALUES (@Name)";
                InsertNewStack.Parameters.AddWithValue("@Name", Name);
                var check = InsertNewStack.ExecuteNonQuery();
                HelperFunctions.CheckOutput(check);
            }
            catch(SqlException)
            {
                AnsiConsole.MarkupLine("[red]A stack with that name already exists. Please choose a different name.[/]");
                Console.ReadKey();
                InsertStack();
            }
            connection.Close();
            UserInterface.ManageStacksMenu();
        }
    }
}
