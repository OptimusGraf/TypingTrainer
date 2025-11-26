using System;
using System.Collections.Generic;
using System.Text;

namespace TypingBlazor.Account
{
    public  class UserDto
    {
        public string UserName { get; set; }

        public int TotalCharCount { get; set; }
        public int CorrectCharCount { get; set; }

        public double Accuracy { get; set; }

        public TimeSpan TimeTrained { get; set; }

        public int LastcharacterPerMinute { get; set; }
        public double LastAccuracy { get; set; }

    }
}
