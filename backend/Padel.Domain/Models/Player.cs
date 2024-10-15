public class Player
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Sex { get; private set; }

    public Player(Guid id, Guid userId, string name, string sex)
    {
        ValidatePlayer(name, sex);

        Id = id;
        UserId = userId;
        Name = name;
        Sex = sex;
    }

    // Update player details
    public void UpdatePlayer(string name, string sex)
    {
        ValidatePlayer(name, sex);

        Name = name;
        Sex = sex;
    }

    private void ValidatePlayer(string name, string sex)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be empty.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(sex))
        {
            throw new ArgumentException("Sex must not be empty.", nameof(sex));
        }

        if (sex != "M" && sex != "F")
        {
            throw new ArgumentException("Sex must be either 'M' or 'F'.", nameof(sex));
        }
    }
}