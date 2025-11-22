using GymManagmentDAL.Data.GymDBContext;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Classes
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext context) : base(context)
        {
            this._context = context;
        }
        public IEnumerable<Membership>? GetAllMemberShipWithMembersAndPlans()
        {
            return _context.Memberships.Include(m => m.Member)
                                       .Include(p => p.Plan)
                                       .ToList();
        }
    }
}
