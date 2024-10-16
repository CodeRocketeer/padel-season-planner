namespace Padel.Domain.Models
{
    public class Player
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string? Name { get; set; }
        public required string? Sex { get; set; }

        public required Guid SeasonId { get; set; }


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
}