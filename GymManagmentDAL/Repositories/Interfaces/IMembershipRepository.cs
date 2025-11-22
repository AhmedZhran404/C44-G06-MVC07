using GymManagmentDAL.Entities;


namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        IEnumerable<Membership>? GetAllMemberShipWithMembersAndPlans();
    }
}
