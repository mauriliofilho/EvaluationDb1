﻿using DB1.AvaliacaoTecnica.Domain.Contracts.Repositories;
using DB1.AvaliacaoTecnica.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB1.AvaliacaoTecnica.Domain.Services;
using DB1.AvaliacaoTecnica.Domain.Models;
using DB1.AvaliacaoTecnica.Domain.Commands.TechnologyCommands;
using DB1.AvaliacaoTecnica.Domain.Contracts;

namespace DB1.AvaliacaoTecnica.ApplicationService
{
    public class VacancyApplicationService : ApplicationService, IVacancyApplicationService
    {
        private IVacancyRepository _vacancyRepository;
        
        public VacancyApplicationService(IVacancyRepository vacancyRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._vacancyRepository = vacancyRepository;
        }

        public Vacancy Create(CreateVacancyCommand command)
        {
            List<IVacancyTechnology> vacancyTechnologies = new List<IVacancyTechnology>();
            foreach(var vTechItem in command.VacancyTechnologies)
            {
                VacancyTechnology vacancyTechnology = new VacancyTechnology(vTechItem.TechnologyId);
                vacancyTechnologies.Add(vacancyTechnology);
            }

            List<ICandidate> candidates = new List<ICandidate>();
            foreach(var candItem in command.Candidates)
            {
                Candidate candidate = new Candidate();
                List<CandidateTechnology> candidateTechnlogies = candItem.CandidateTechnologies;
                foreach(var techItem in candidateTechnlogies)
                {
                    candidate.AddCandidateTechnology(techItem);
                }
                candidates.Add(candidate);
            }

            var vacancy = new Vacancy(command.Description, vacancyTechnologies, candidates);
            if(vacancy.CanAdd())
            {
                _vacancyRepository.Create(vacancy);

                if(Commit())
                    return vacancy;
            }

            return null;
        }

        public Vacancy Delete(int id)
        {
            Vacancy vacancy = _vacancyRepository.GetHeader(id);
            _vacancyRepository.Delete(vacancy);

            if(Commit())
                return vacancy;

            return null;
        }

        public List<Vacancy> Get()
        {
            return _vacancyRepository.Get();
        }

        public Vacancy Get(int id)
        {
            return _vacancyRepository.GetHeader(id);
        }

        public List<Vacancy> Get(string description)
        {
            return _vacancyRepository.Get(description);
        }

        public Vacancy GetDetails(int id)
        {
            return _vacancyRepository.GetDetails(id);
        }

        public Vacancy Update(EditVacancyCommand command)
        {
            Vacancy vacancy = _vacancyRepository.GetHeader(command.Id);
            if(vacancy.CanAdd())
            {
                _vacancyRepository.Update(vacancy);
                if(Commit())
                    return vacancy;
            }

            return null;
        }
        
        public void Cancel(int id)
        {
            Vacancy vacancy = _vacancyRepository.GetHeader(id);
            vacancy.Cancel();
            _vacancyRepository.Update(vacancy);

            Commit();
        }

        public void Suspend(int id)
        {
            Vacancy vacancy = _vacancyRepository.GetHeader(id);
            vacancy.Suspend();
            _vacancyRepository.Update(vacancy);

            Commit();
        }
    }
}