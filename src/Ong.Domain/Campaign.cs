using Ong.Domain.Enums;

namespace Ong.Domain
{
    public class Campaign
    {
        public const int MaxTitleLength = 100;
        public const int MaxDescriptionLength = 300;

        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public decimal FinancialGoal { get; private set; }
        public ECampaignStatus Status { get; private set; }

        private Campaign() { }

        private Campaign(
            Guid id,
            string title,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            decimal financialGoal,
            ECampaignStatus status)
        {
            Id = id;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            FinancialGoal = financialGoal;
            Status = status;
        }

        public static Campaign Restore(
            Guid id,
            string title,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            decimal financialGoal,
            ECampaignStatus status)
        {
            return new Campaign(
                id,
                title,
                description,
                startDate,
                endDate,
                financialGoal,
                status);
        }

        public static Campaign Create(
            string title,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            decimal financialGoal,
            ECampaignStatus status = ECampaignStatus.Active)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Título é obrigatório.", nameof(title));

            if (title.Length > MaxTitleLength)
                throw new ArgumentException("Título não pode exceder 100 caracteres.", nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição é obrigatória.", nameof(description));

            if (description.Length > MaxDescriptionLength)
                throw new ArgumentException("Descrição não pode exceder 300 caracteres.", nameof(description));

            if (financialGoal <= 0)
                throw new ArgumentException("A meta financeira deve ser maior que zero.", nameof(financialGoal));

            if (endDate < startDate)
                throw new ArgumentException("A data final não pode ser menor que a data inicial.");

            if (endDate < DateTimeOffset.UtcNow)
                throw new ArgumentException("A data final não pode estar no passado.", nameof(endDate));

            return new Campaign(
                Guid.NewGuid(),
                title,
                description,
                startDate,
                endDate,
                financialGoal,
                status);
        }

        public void UpdateStatus(ECampaignStatus newStatus)
        {
            Status = newStatus;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Descrição é obrigatória.", nameof(newDescription));

            if (newDescription.Length > MaxDescriptionLength)
                throw new ArgumentException("Descrição não pode exceder 300 caracteres.", nameof(newDescription));

            Description = newDescription;
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Título é obrigatório.", nameof(newTitle));

            if (newTitle.Length > MaxTitleLength)
                throw new ArgumentException("Título não pode exceder 100 caracteres.", nameof(newTitle));

            Title = newTitle;
        }

        public void UpdateDates(DateTimeOffset newStartDate, DateTimeOffset newEndDate)
        {
            if (newEndDate < newStartDate)
                throw new ArgumentException("A data final não pode ser menor que a data inicial.");

            StartDate = newStartDate;
            EndDate = newEndDate;
        }

        public void UpdateFinancialGoal(decimal newFinancialGoal)
        {
            if (newFinancialGoal <= 0)
                throw new ArgumentException("A meta financeira deve ser maior que zero.", nameof(newFinancialGoal));

            FinancialGoal = newFinancialGoal;
        }

        public bool IsActive() =>
            Status == ECampaignStatus.Active &&
            DateTimeOffset.UtcNow >= StartDate &&
            DateTimeOffset.UtcNow <= EndDate;

        public bool IsCompleted() =>
            Status == ECampaignStatus.Completed || DateTimeOffset.UtcNow > EndDate;

        public bool InProgress() =>
            Status == ECampaignStatus.Active && DateTimeOffset.UtcNow < EndDate;

        public void MarkAsCompleted()
        {
            Status = ECampaignStatus.Completed;
            EndDate = DateTimeOffset.UtcNow;
        }
    }
}
