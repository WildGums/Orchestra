namespace Orchestra.Automation.Views
{
    using System.Windows;
    using System.Windows.Automation.Peers;
    using Orc.Automation;

    public class MessageBoxWindowAutomationPeer : WindowAutomationPeer
    {
        public MessageBoxWindowAutomationPeer(Window owner) 
            : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            //NOTE: Support Message box automation control for Win32
            return Win32AutomationDialogsClassNames.MessageBoxClassName;
        }
    }
}
