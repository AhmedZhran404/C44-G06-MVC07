using AutoMapper;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels;
using GymManagmentBLL.ViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagmentBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel inputModel)
        {
            try
            {
               
                var IsEmailExist = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == inputModel.Email).Any();
                var IsPhoneExist = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == inputModel.Phone).Any();

                if(IsEmailExist || IsPhoneExist)
                {
                    return false;
                }

                var trainer = new Trainer()
                {
                    Name = inputModel.Name,
                    Phone = inputModel.Phone,
                    Email = inputModel.Email,
                    DateOfBirth = inputModel.DateOfBirth,
                    Gender = inputModel.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = inputModel.BuildingNumber,
                        Street = inputModel.Street,
                        City = inputModel.City,
                    },
                    Specialities = inputModel.Specialization,

                };

                _unitOfWork.GetRepository<Trainer>().Add(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                Console.WriteLine("Trainer Cannot Added In Database");
                return false;
            }
            
        }

        public IEnumerable<TrainerViewModel>? GetAllTrainers()
        {
            
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();

            if(trainers is null || !trainers.Any())
            {
                return null;
            }

            return trainers.Select(t => new TrainerViewModel()
                    {
                        TrainerId = t.Id,
                        Name = t.Name,
                        Phone = t.Phone,
                        Specialities = t.Specialities.ToString(),
                        Email = t.Email,
                    });

        }

        public TrainerViewModel? GetTrainerById(int id)
        {
            
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);

            if(trainer is null)
                return null;

            
            var mappedTrainer = _mapper.Map<Trainer , TrainerViewModel>(trainer);

            mappedTrainer.Address = FormatAddress(trainer.Address);

            return mappedTrainer;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);

            if(trainer is null) return null;

           var MappedTrainerToUpdate = _mapper.Map<Trainer , TrainerToUpdateViewModel>(trainer);

            MappedTrainerToUpdate.BuildingNumber = trainer.Address.BuildingNumber;
            MappedTrainerToUpdate.City = trainer.Address.City;
            MappedTrainerToUpdate.Street = trainer.Address.Street;

            return MappedTrainerToUpdate;
        }

        public bool UpdateTrainer(int Id, TrainerToUpdateViewModel inputModel)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(Id);
            if(trainer is null) return false;


            if (IsEmailExist(inputModel.Email , Id) || IsPhoneExist(inputModel.Phone , Id))
            {
                return false;
            }


            _mapper.Map(inputModel, trainer);

            trainer.Address.BuildingNumber = inputModel.BuildingNumber;
            trainer.Address.City = inputModel.City;
            trainer.Address.Street = inputModel.Street;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);

            return _unitOfWork.SaveChanges() > 0;
        }


        public bool RemoveTrainer(int Id)
        {
            if(IsRemoveTrainerIsAvilable(Id))
                return false;

            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(Id);

            if(trainer is null) return false;

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);

            return _unitOfWork.SaveChanges() > 0;
        }



        #region Helper Method

        private bool IsEmailExist(string email , int Id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == email && x.Id != Id);

            if (trainer is null || !trainer.Any())
            {
                return false;
            }
            return true;
        }
        private bool IsPhoneExist(string phone , int Id) 
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == phone && x.Id != Id);

            if (trainer is null || !trainer.Any())
            {
                return false;
            }
            return true;
        }

        private bool IsRemoveTrainerIsAvilable(int Id)
        {
            var trainersSessions =  _unitOfWork.GetRepository<Session>().GetAll(x => x.TrainerId == Id);

            if (trainersSessions.Any() == false)
                return false;
               

            foreach (var session in trainersSessions)
            {
                if (session.StartDate >= DateTime.Now)
                {
                    return true;
                }
            }

            return false;
        }

        private string FormatAddress(Address address)
        {
            if (address is null)
            {
                return "N/A";
            }

            return $"{address.BuildingNumber} - {address.Street} - {address.City}";

        }
        #endregion

    }
}
