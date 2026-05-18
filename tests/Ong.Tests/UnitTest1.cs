using Ong.Domain;
using Ong.Domain.Enums;
using Xunit;

namespace Ong.Tests;

public class CampaignTests
{
    [Fact]
    public void Campaign_DeveSerCriadoComSucesso()
    {
        var startDate = DateTimeOffset.UtcNow.AddDays(1);
        var endDate = DateTimeOffset.UtcNow.AddDays(30);
        var campaign = Campaign.Create("Campanha Teste", "Descrição da Campanha", startDate, endDate, 5000m);
        
        Assert.Equal("Campanha Teste", campaign.Title);
        Assert.Equal(5000m, campaign.FinancialGoal);
        Assert.Equal(ECampaignStatus.Active, campaign.Status);
    }

    [Fact]
    public void Campaign_DeveFalharComDataFinalInvalida()
    {
        var startDate = DateTimeOffset.UtcNow.AddDays(10);
        var endDate = DateTimeOffset.UtcNow.AddDays(5); // Antes do startDate
        
        Assert.Throws<ArgumentException>(() => Campaign.Create("Teste", "Desc", startDate, endDate, 1000m));
    }
    
    [Fact]
    public void Campaign_DeveFalharComMetaFinanceiraInvalida()
    {
        var startDate = DateTimeOffset.UtcNow.AddDays(1);
        var endDate = DateTimeOffset.UtcNow.AddDays(5);
        
        Assert.Throws<ArgumentException>(() => Campaign.Create("Teste", "Desc", startDate, endDate, 0));
    }
}

public class DonationTests
{
    [Fact]
    public void Donation_DeveSerCriadoComSucesso()
    {
        var campaignId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var donation = new Donation(campaignId, userId, 250.50m, DateTimeOffset.UtcNow);
        
        Assert.Equal(250.50m, donation.Amount);
        Assert.Equal(campaignId, donation.CampaignId);
        Assert.Equal(userId, donation.UserId);
    }
}
