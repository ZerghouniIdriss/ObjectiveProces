using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Services
{
    public interface IRecognitionService
    {
        void Add(Recognition recognition);

        void AddRecognitionToUsers(Recognition recognition, ApplicationUser[] users);

        void AddRecognitionToTeams(Recognition recognition, Team[] users);

        void AddRecognitionToGoals(Recognition recognition, Goal[] users);

        Recognition GetById(long id);

        IList<Recognition> GetAll(); 
    }

    public class RecognitionService : CRUDServiceBase<RecognitionRepository, Recognition>, IRecognitionService
    {
        IRecognitionRepository _recognitionRepository;

        public RecognitionService(IRecognitionRepository recognitionRepository)
        {
            _recognitionRepository = recognitionRepository;
        }

        public void AddRecognitionToUsers(Recognition recognition, ApplicationUser[] users)
        {
            _recognitionRepository.AddRecognitionToUsers(recognition, users);
        }

        public void AddRecognitionToTeams(Recognition recognition, Team[] users)
        {
            _recognitionRepository.AddRecognitionToTeams(recognition, users);
        }

        public void AddRecognitionToGoals(Recognition recognition, Goal[] users)
        {
            _recognitionRepository.AddRecognitionToGoals(recognition, users);
        }

        public Recognition GetById(long id)
        {
           return _recognitionRepository.GetSingle(x => x.Id == id, p => p.Giver, p => p.Goal, p => p.Receivers, p => p.TeamReceivers);
        }

        public IList<Recognition> GetAll()
        {
            return _recognitionRepository.GetAll(x => x.Giver, x => x.Goal, x => x.Receivers, x => x.TeamReceivers);
        } 
    }
}
