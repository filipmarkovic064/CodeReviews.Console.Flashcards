using FlashCards.Controllers;
using Microsoft.Identity.Client;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
    internal static class UserInterface
    {
        public static Style MenuStyle = new Style(Color.Aqua, decoration: Decoration.Bold);
        internal static void MainMenu()
        {
            Console.Clear();
            AnsiConsole.Write(new Align(
                new Markup("Welcome to your flashcards!", UserInterface.MenuStyle), 
                HorizontalAlignment.Center, VerticalAlignment.Top));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.MenuChoices>()
                .Title("[bold aqua]What would you like to do?[/]")
                .AddChoices(Enum.GetValues<Enums.MenuChoices>()));

            switch (choice)
            {
                case Enums.MenuChoices.Study:
                    StudySessionController.StartStudySession();
                    break;
                case Enums.MenuChoices.ManageStacks:
                    ManageStacksMenu();
                    break;
                case Enums.MenuChoices.ManageFlashcards:
                    ManageFlashcardsMenu();
                    break;
                case Enums.MenuChoices.ViewStudySessions:
                    StudySessionController.SeeStudyHistory();
                    break;
                case Enums.MenuChoices.Exit:
                    AnsiConsole.Write(new Markup("Exiting the program!\n", MenuStyle));
                    Environment.Exit(0);
                    break;

            }

        }
        internal static void ManageStacksMenu()
        {
            Console.Clear();
            StacksController stacksController = new();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.ManageStacksMenu>()
                .Title("What would you like to do with your stacks?")
                .AddChoices(Enum.GetValues<Enums.ManageStacksMenu>()));

            switch (choice)
            {
                case Enums.ManageStacksMenu.ViewStacks:
                    stacksController.ViewStacks();
                    break;
                case Enums.ManageStacksMenu.AddStack:
                    stacksController.InsertStack();
                    break;
                case Enums.ManageStacksMenu.RenameStack:
                    stacksController.RenameStack();
                    break;
                case Enums.ManageStacksMenu.DeleteStack:
                    stacksController.DeleteStack();
                    break;
                case Enums.ManageStacksMenu.MainMenu:
                    MainMenu();
                    break;
            }
        }
        internal static void ManageFlashcardsMenu()
        {
            Console.Clear();
            FlashcardsController flashcardsController = new();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.ManageFlashcardsMenu>()
                .Title("Choose which stack the flashcards are in:")
                .AddChoices(Enum.GetValues<Enums.ManageFlashcardsMenu>()));

            switch (choice)
            {
                case Enums.ManageFlashcardsMenu.ViewFlashcards:
                    flashcardsController.ViewFlashcards();
                    break;
                case Enums.ManageFlashcardsMenu.AddFlashcard:
                    flashcardsController.InsertFlashcard();
                    break;
                case Enums.ManageFlashcardsMenu.ChangeFrontInfo:
                    flashcardsController.ChangeFrontInfo();
                    break;
                case Enums.ManageFlashcardsMenu.ChangeBackInfo:
                    flashcardsController.ChangeBackInfo();
                    break;
                case Enums.ManageFlashcardsMenu.DeleteFlashcard:
                    flashcardsController.DeleteFlashcard();
                    break;
                case Enums.ManageFlashcardsMenu.MainMenu:
                    MainMenu();
                    break;
            }
        }
    }
}
