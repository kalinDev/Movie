using MovieApi.Application.Interfaces;

namespace MovieApi.Application.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications;

    public Notifier()
        => _notifications = new List<Notification>();

    public void Handle(Notification notification)
        => _notifications.Add(notification);

    public List<Notification> GetNotifications()
        => _notifications;

    public bool HasNotification()
        => _notifications.Any();
}