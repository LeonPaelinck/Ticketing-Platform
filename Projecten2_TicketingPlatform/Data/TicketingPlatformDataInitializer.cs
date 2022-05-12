using Microsoft.AspNetCore.Identity;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Data
{
    public class TicketingPlatformDataInitializer
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketingPlatformDataInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            //_dbcontext.Database.EnsureDeleted();

            if(false) //(_dbcontext.Database.EnsureCreated()) 
            {
                //await InitializeUsers();
                string userId = "210771fc - 21f2 - 47e4 - a902 - 986e2d199105";
                string loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
                ////Nog aan te vullen verdere data later in project.
                var tickets = new List<Ticket>
            {
                     new Ticket("2020-Error 2980", TicketStatus.Afgehandeld, DateTime.Today.AddMonths(-4), loremIpsum, "1", userId, "Colin C. Watkins"),
                     new Ticket("2020-Error 109271", TicketStatus.Afgehandeld, DateTime.Today.AddMonths(-2), loremIpsum, "1", userId, "Colin C. Watkins"),

                     new Ticket("2020-Authorisatie Probleem", TicketStatus.Geannuleerd, DateTime.Today.AddDays(-4), loremIpsum, "1", userId, "Colin C. Watkins"),
                     new Ticket("2020-Error 5038", TicketStatus.Aangemaakt, DateTime.Today.AddDays(-3), "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.", "1", userId, "Patrick D. Wimbush"),
                     new Ticket("2020-Foutcode 966", TicketStatus.Aangemaakt, DateTime.Today.AddDays(-2),"Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?", "1", userId, "Colin C. Watkins", null, "66f292e6-c7d2-4621-80fe-1d2a6548d540_6._Indexes_and_Performance.pdf"),
                     new Ticket("2020-Foutcode 409 bij validatie", TicketStatus.InBehandeling, DateTime.Today.AddDays(-1), "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio.", "1", userId, "Colin C. Watkins", null, "f5f56bc3-eae9-46b1-838f-5c3c9199e17b_RESTAPI.pdf"),
                     new Ticket("2020-Error 5200", TicketStatus.InBehandeling, DateTime.Today, "Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.", "1", userId, "Patrick D. Wimbush", null, "9b1726dc-3145-41c5-b420-7d752d81401c_linked_in_1533006838.png?v=f3mnTIryXzgbi-b2IA4jZg7pnYZukNZJ6fcuKTydhow")
            };

                _dbcontext.Tickets.AddRange(tickets);

                ContractType contract24_7Email = new ContractType("Email Contract 24/7", ContractEnContractTypeStatus.Actief, ManierVanAanmakenTicket.Email, TijdstipTicketAanmaken.Altijd, 3, 10, 100);
                ContractType contract24_7Email2 = new ContractType("Email Contract 24/7 2 jaar", ContractEnContractTypeStatus.Actief, ManierVanAanmakenTicket.Email, TijdstipTicketAanmaken.Altijd, 2, 10, 100);

                ContractType contract24_7 = new ContractType("Applicatie Contract 24/7", ContractEnContractTypeStatus.Actief, ManierVanAanmakenTicket.EmailEnTelefonischEnApplicatie, TijdstipTicketAanmaken.Altijd, 1, 10, 100);
                ContractType contractWerkuren = new ContractType("Applicatie Contract Werkuren", ContractEnContractTypeStatus.Actief, ManierVanAanmakenTicket.EmailEnTelefonischEnApplicatie, TijdstipTicketAanmaken.TijdensWerkdagen, 1, 10, 100);
                _dbcontext.ContractTypes.Add(contract24_7);
                _dbcontext.ContractTypes.Add(contractWerkuren);
                _dbcontext.ContractTypes.Add(contract24_7Email);
                _dbcontext.ContractTypes.Add(contract24_7Email2);

                foreach (Contract c in _dbcontext.Contracten)
                {
                    _dbcontext.Contracten.Remove(c);
                }

                var contractenVoorKlant = new List<Contract>
                {
                    new Contract(DateTime.Today.AddMonths(-13), contract24_7, 1, userId,ContractEnContractTypeStatus.Afgelopen),
                    new Contract( DateTime.Today.AddMonths(-8), contract24_7, 1, userId, ContractEnContractTypeStatus.InBehandeling),
                    new Contract(DateTime.Today.AddMonths(-26), contract24_7, 2, userId,ContractEnContractTypeStatus.Afgelopen),
                    new Contract(DateTime.Today.AddMonths(-28), contract24_7, 2, userId, ContractEnContractTypeStatus.Afgelopen),
                    new Contract(DateTime.Today.AddMonths(-23).AddDays(-11), contractWerkuren, 2, userId, ContractEnContractTypeStatus.Actief),
                    new Contract(DateTime.Today.AddYears(-4), contract24_7, 3, userId,ContractEnContractTypeStatus.Afgelopen),
                    new Contract(DateTime.Today.AddMonths(-4), contract24_7,  3, userId,ContractEnContractTypeStatus.NietActief),
                    new Contract(DateTime.Today.AddMonths(-4), contract24_7Email,  3, "210771fc - 5d15 - 4e7c - a902 - a3a2fd4e2815",ContractEnContractTypeStatus.Actief)

                };
                _dbcontext.Contracten.AddRange(contractenVoorKlant);

                var knowledgdeBases = new List<KnowledgeBase>
            {
                     new KnowledgeBase("Hoe kan ik me aanmelden?",loremIpsum, DateTime.Today),
                     new KnowledgeBase("Hoe kan ik mijn wachtwoord aanpassen?",loremIpsum, DateTime.Today),
                     new KnowledgeBase("Wat doe ik bij Error 5038",loremIpsum, DateTime.Today),
                     new KnowledgeBase("Hoe annuleer ik een ticket?","Oplossing 4", DateTime.Today),
                     new KnowledgeBase("Mijn nieuwe contract staat niet op actief","Oplossing 5", DateTime.Today),
                     new KnowledgeBase("Ik kan mijn bijlage niet uploaden","Oplossing 6", DateTime.Today),
                     new KnowledgeBase("Waar vind ik  mijn oude contracten terug?","Oplossing 7", DateTime.Today),
                     new KnowledgeBase("Ik kan de naam van mijn ticket niet meer aanpassen","Oplossing 8", DateTime.Today),
      
            };

                _dbcontext.KnowledgeBase.AddRange(knowledgdeBases);


                _dbcontext.SaveChanges();
            }
        }

        private async Task InitializeUsers()
        {
            string eMailAddress1 = "johan@actemium.be";
            IdentityUser user1 = new IdentityUser { UserName = eMailAddress1, Email = eMailAddress1, Id = "210771fc - 21f2 - 47e4 - a902 - 986e2d199105" };
            await _userManager.CreateAsync(user1, "P@ssword1");
            await _userManager.AddClaimAsync(user1, new Claim(ClaimTypes.Role, "klant"));

            string eMailAddress1a = "stijn@vyncke.be";
            IdentityUser user1a = new IdentityUser { UserName = eMailAddress1a, Email = eMailAddress1a, Id = "210771fc - 5d15 - 4e7c - a902 - a3a2fd4e2815" };
            await _userManager.CreateAsync(user1a, "P@ssword1");
            await _userManager.AddClaimAsync(user1a, new Claim(ClaimTypes.Role, "klant"));

            string eMailAddress2 = "technieker@hogent.be";
            IdentityUser user2 = new IdentityUser { UserName = eMailAddress2, Email = eMailAddress2, Id= "ff3bc350-5d15-47da-bb68-69ad26e059ae" };
            await _userManager.CreateAsync(user2, "P@ssword1");
            await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "technieker"));

            string eMailAddress3 = "sm@hogent.be";
            IdentityUser user3 = new IdentityUser { UserName = eMailAddress3, Email = eMailAddress3, Id= "f6d6bf42-2c83-4e7c-abbf-f207381286c2" };
            await _userManager.CreateAsync(user3, "P@ssword1");
            await _userManager.AddClaimAsync(user3, new Claim(ClaimTypes.Role, "supportmanager"));

            string eMailAddress4 = "admin@hogent.be";
            IdentityUser user4 = new IdentityUser { UserName = eMailAddress4, Email = eMailAddress4, Id= "9ea3de52 - 78c8 - 4cd5 - 877a - a3a2fd4e2815" };
            await _userManager.CreateAsync(user4, "P@ssword1");
            await _userManager.AddClaimAsync(user4, new Claim(ClaimTypes.Role, "administrator"));
        }
    }
}
