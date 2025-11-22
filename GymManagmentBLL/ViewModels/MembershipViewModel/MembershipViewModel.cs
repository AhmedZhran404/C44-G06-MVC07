using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MembershipViewModel
{
    public class MembershipViewModel
    {
        
        public string memberName { get; set; } = null!;

        public string planName { get; set; } = null!;

        public DateTime StartData { get; set; }

        public DateTime EndData { get; set; }


    }
}
