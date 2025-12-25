using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.DTOs
{
    internal class StudySession
    {
        public int StudySessionId { set; get; }
        public int StackId { set; get; }
        public string StackName { set; get; }
        public string Points { set; get; }
        public DateTime Date { set; get; }
    }
}
