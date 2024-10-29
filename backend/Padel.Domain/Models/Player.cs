using Padel.Application.Models;

namespace Padel.Domain.Models;

public class Player
{
    public int Id { get; set; }
    public Guid UserId { get; private set; }
    public Gender Gender { get; private set; }
    public string Name { get; private set; }

    public Player(Gender gender, string name, Guid userId)
    {
        ValidateName(name);
        ValidateUserId(userId);

        Gender = gender;
        Name = name;
        UserId = userId;
    }

    private static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be an empty GUID.", nameof(userId));
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        }
    }
}