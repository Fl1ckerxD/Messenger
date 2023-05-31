using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public ChatPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик событий запускаемый после загрузки страницы
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollToBottom();

            timer.Tick += new EventHandler(RefreshChat);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        /// <summary>
        /// Обновление чата
        /// </summary>
        private void RefreshChat(object sender, EventArgs e)
        {
            try
            {
                using (var context = new MessengerContext())
                {
                    int id = ViewModelManager.chatViewModel.SelectedChat.Id;
                    //Chat currentChat = context.Chats.Where(x => x.Id == id)
                    //    .Include(x => x.Users)
                    //    .Include(x => x.Messages).ThenInclude(x => x.Files)
                    //    .First();
                    Chat currentChat = context.Chats.Where(x => x.Id == id)
                        .Include(x => x.Messages).ThenInclude(x => x.Files)
                        .Include(x => x.Messages).ThenInclude(x => x.User)
                        .First();
                    //Chat currentChat = ViewModelManager.chatViewModel.SelectedChat; //context.Chats.Include(x => x.Messages).FirstOrDefault();
                    if (currentChat.Messages.Count != list_Messages.Items.Count)
                    {
                        ViewModelManager.chatViewModel.SelectedChat = currentChat;
                        ScrollToBottom();
                    }
                }
            }
            catch
            {
                throw new Exception("Ошибка подключения");
            }
        }
        /// <summary>
        /// Пролистать чат вниз
        /// </summary>
        private void ScrollToBottom()
        {
            if (list_Messages.Items.Count > 2)
                list_Messages.ScrollIntoView(list_Messages.Items[list_Messages.Items.Count - 1]);
        }
    }
}
