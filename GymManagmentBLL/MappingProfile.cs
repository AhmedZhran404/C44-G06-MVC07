using AutoMapper;
using GymManagmentBLL.ViewModels;
using GymManagmentBLL.ViewModels;
using GymManagmentBLL.ViewModelsls;
using GymManagmentDAL.Data.Configurations;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            MapSession();
        }

        private void MapSession()
        {

            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName , opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName , opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots , opt => opt.Ignore());
            
            CreateMap<Trainer , TrainerViewModel>()
                .ForMember(dest => dest.DataOfBirth,
                opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")));
            
            CreateMap<Trainer , TrainerToUpdateViewModel>().ReverseMap();
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
            
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name , opt => opt.MapFrom(src => src.CategoryName));

        }

    }

}
