using Microsoft.Data.SqlClient;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Controllers
{

    internal class FlashcardsController
    {
        internal void ViewFlashcards()
        {
            var FlashcardsList = HelperFunctions.GetFlashcards();
            /* 
             Requirement: When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
             Based on everything i've read online everyone says that i shouldnt ever change Ids, so i thought that this could be a good way to implement it without changing the Id
             */
            int i = 0; 
            foreach (var flashcard in FlashcardsList)
            {
                i++;
                AnsiConsole.Write(new Markup($"{i}. Flashcard: \nQuestion: {flashcard.FrontInfo}, Answer: {flashcard.BackInfo}\n\n", UserInterface.MenuStyle));
            }
            AnsiConsole.MarkupLine("\n[italic yellow] Press any key to return[/]");
            Console.ReadKey();
            UserInterface.ManageFlashcardsMenu();

        }
        internal void InsertFlashcard()
        {
            var stack = HelperFunctions.ChooseStack();
            var frontInfo = AnsiConsole.Prompt(new TextPrompt<string>("Please input FrontInfo (Question)"));
            var backInfo = AnsiConsole.Prompt(new TextPrompt<string>("Please input BackInfo (Answer)"));

            var connection = HelperFunctions.OpenConnection();
            var insertFlashcard = connection.CreateCommand();
            insertFlashcard.CommandText = $@"INSERT INTO dbo.FlashcardsTable(StackId, FrontInfo, BackInfo)
                                           VALUES('{stack.StackId}', @frontInfo, @backInfo)";
            insertFlashcard.Parameters.AddWithValue("@frontInfo", frontInfo);
            insertFlashcard.Parameters.AddWithValue("@backInfo", backInfo);

            var check = insertFlashcard.ExecuteNonQuery();
            connection.Close();
            HelperFunctions.CheckOutput(check);
            UserInterface.ManageFlashcardsMenu();
        }

        internal void ChangeFrontInfo()
        {
            var flashcard = HelperFunctions.ChooseFlashcard();
            var connection = HelperFunctions.OpenConnection();
            var frontInfo = AnsiConsole.Prompt(new TextPrompt<string>("Please input updated FrontInfo (Question)"));
            var updateFlashcard = connection.CreateCommand();
            updateFlashcard.CommandText = $@"UPDATE dbo.FlashcardsTable 
                                             SET FrontInfo = @frontInfo
                                             WHERE FlashcardId = {flashcard.FlashcardId}";
            updateFlashcard.Parameters.AddWithValue("@frontInfo", frontInfo);
            var check = updateFlashcard.ExecuteNonQuery();
            connection.Close();
            HelperFunctions.CheckOutput(check);
            UserInterface.ManageFlashcardsMenu();
        }
        
        internal void ChangeBackInfo()
        {
            var flashcard = HelperFunctions.ChooseFlashcard();
            var connection = HelperFunctions.OpenConnection();
            var backInfo = AnsiConsole.Prompt(new TextPrompt<string>("Please input updated BackInfo (Answer)"));
            var updateFlashcard = connection.CreateCommand();
            updateFlashcard.CommandText = $@"UPDATE dbo.FlashcardsTable 
                                             SET BackInfo = @backInfo
                                             WHERE FlashcardId = {flashcard.FlashcardId}";
            updateFlashcard.Parameters.AddWithValue("@backInfo", backInfo);
            var check = updateFlashcard.ExecuteNonQuery();
            connection.Close();
            HelperFunctions.CheckOutput(check);
            UserInterface.ManageFlashcardsMenu();
        }
        
        internal void DeleteFlashcard()
        {
            var flashcard = HelperFunctions.ChooseFlashcard();
            var connection = HelperFunctions.OpenConnection();

            var deleteFlashcard = connection.CreateCommand();
            deleteFlashcard.CommandText = $"DELETE FROM dbo.FlashcardsTable WHERE FlashcardId = {flashcard.FlashcardId}";
            var check = deleteFlashcard.ExecuteNonQuery();
            connection.Close();
            HelperFunctions.CheckOutput(check);
            UserInterface.ManageFlashcardsMenu();
        }

    }
}
