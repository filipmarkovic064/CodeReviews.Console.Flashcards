using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.DTOs
{
    internal class Flashcard
    {
        public int FlashcardId { set; get; }
        public int StackId { set; get; }
        public string FrontInfo { set; get; }
        public string BackInfo { set; get; }
    }
}
