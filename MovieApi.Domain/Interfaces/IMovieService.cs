﻿using MovieApi.Domain.Entities;

namespace MovieApi.Domain.Interfaces;

public interface IMovieService 
{
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteByIdAsync(int id);
}