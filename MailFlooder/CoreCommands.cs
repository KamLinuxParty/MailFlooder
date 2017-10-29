using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MailFlooder
{
    class CoreCommands
    {
        // Создание команды send
        private static RoutedUICommand send;

        static CoreCommands()
        {
            // Инициализация команды
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl + S"));
            send = new RoutedUICommand("Отправить письма", "Send", typeof(CoreCommands), inputs);
           
        }

        public static RoutedUICommand Send
        {
            get { return send; }
        }
    }
}
