using GymManagmentBLL.ViewModels.MembershipViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Interfaces
{
    public interface IMembership
    {
        public IEnumerable<MembershipViewModel>? GetAllMembership();


    }
}
