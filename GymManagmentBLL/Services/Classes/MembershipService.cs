using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.MembershipViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Classes
{
    public class MembershipService : IMembership
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembershipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<MembershipViewModel>? GetAllMembership()
        {
            var memberships = _unitOfWork.MembershipRepository.GetAllMemberShipWithMembersAndPlans() ?? [];

            if (memberships is null || !memberships.Any())
            {
                return [];
            }

            var membershipView = memberships.Select(m => new MembershipViewModel()
            {
                memberName = m.Member.Name,
                planName = m.Plan.Name,
                StartData = m.CreatedAt,
                EndData = m.EndDate,
            });

            return membershipView;

        }
    }
}
