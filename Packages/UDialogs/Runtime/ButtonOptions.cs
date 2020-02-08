using System;

namespace UDialogs
{
    public class ButtonOptions
    {
        public Action<UDialogMessage> action;
        public string displayText;

        public ButtonOptions(string displayText , Action<UDialogMessage> action)
        {
            this.displayText = displayText;
            this.action = action;
        }

        public ButtonOptions(string displayText)
        {
            this.displayText = displayText;
            action = null;
        }
    }
}