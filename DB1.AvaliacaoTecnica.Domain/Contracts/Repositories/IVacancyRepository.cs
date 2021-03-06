﻿using DB1.AvaliacaoTecnica.Domain.Models;
using System;
using System.Collections.Generic;

namespace DB1.AvaliacaoTecnica.Domain.Contracts.Repositories
{
    public interface IVacancyRepository
    {
        List<Vacancy> Get();
        List<Vacancy> Get(string description);
        Vacancy GetDetails(int id);
        Vacancy GetHeader(int id);
        void Create(Vacancy vacancy);
        void Update(Vacancy vacancy);
        void Delete(Vacancy vacancy);
    }
}
