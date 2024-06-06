using System.Drawing; // Certifique-se de que este namespace está presente
using System.Windows.Forms;

namespace to_do_list.Services
{
    public class NotificationService
    {
        public void ShowNotification(string title, string message)
        {
            NotifyIcon notifyIcon = new NotifyIcon
            {
                BalloonTipTitle = title,
                BalloonTipText = message,
                Icon = SystemIcons.Information, // Isso deve funcionar após adicionar a referência ao System.Drawing
                Visible = true
            };

            notifyIcon.ShowBalloonTip(30000); // Mostra a notificação por 30 segundos
        }
    }
}
