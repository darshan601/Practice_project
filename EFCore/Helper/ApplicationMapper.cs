using AutoMapper;
using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Entity.ResponseModel;

namespace EFCore.Helper;

public class ApplicationMapper:Profile
{
    public ApplicationMapper()
    {
        CreateMap<Genre, GenreResponse>().ReverseMap();

        CreateMap<CreateGenreRequest, Genre>();
        
        CreateMap<UpdateGenreRequest, Genre>().ReverseMap();


        CreateMap<Movie, MovieResponse>()
            .ForMember(dest=>dest.Genres, opt=> opt.MapFrom(src=>src.Genres))
            .ReverseMap();

        CreateMap<CreateMovieRequest, Movie>()
            .ForMember(dest=>dest.Genres, opt=> opt.Ignore())
            .ReverseMap();

    }
}