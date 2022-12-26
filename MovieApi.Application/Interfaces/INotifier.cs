using MovieApi.Application.Notifications;

namespace MovieApi.Application.Interfaces;

public interface INotifier
{
    void Handle(Notification notification);
    List<Notification> GetNotifications();
    bool HasNotification();
}