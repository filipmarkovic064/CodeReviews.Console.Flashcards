using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
    internal class Enums
    {
        internal enum MenuChoices
        {
            Study,
            ManageStacks,
            ManageFlashcards,
            ViewStudySessions,
            Exit
        }
        internal enum ManageStacksMenu
        {
            ViewStacks,
            AddStack,
            RenameStack,
            DeleteStack,
            MainMenu
        }
        internal enum ManageFlashcardsMenu
        {   
            ViewFlashcards,
            AddFlashcard,
            ChangeFrontInfo,
            ChangeBackInfo,
            DeleteFlashcard,
            MainMenu
        }
    }
}
