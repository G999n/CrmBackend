using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using Microsoft.VisualBasic;

namespace CustomerRelationshipManagement.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

        // DbSet properties for each model (table in the database)
        public DbSet<CommunicationHistory> CommunicationHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<MarketingCampaign> MarketingCampaigns { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<SalesPipeline> SalesPipelines { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
