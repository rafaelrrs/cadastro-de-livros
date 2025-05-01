using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;
using AppLivroCadastro.Domain.Entities;
using AutoMapper;

namespace AppLivroCadastro.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAutorDTO, Autor>().ReverseMap();
            CreateMap<CreateAssuntoDTO, Assunto>().ReverseMap();
            CreateMap<Autor, AutorDTO>().ReverseMap();
            CreateMap<Livro, LivroDTO>().ReverseMap();
            CreateMap<FormaCompra, FormaCompraDTO>().ReverseMap();
            CreateMap<CreateFormaCompraDTO, FormaCompra>().ReverseMap();
            CreateMap<Assunto, AssuntoDTO>().ReverseMap();
            CreateMap<CreateAssuntoDTO, Assunto>();

            CreateMap<CreateLivroDTO, Livro>()
                .ForMember(dest => dest.Codl, opt => opt.Ignore())
                .ForMember(dest => dest.LivroAutores, opt => opt.Ignore())
                .ForMember(dest => dest.LivroAssuntos, opt => opt.Ignore())
                .ForMember(dest => dest.PrecosLivroFormaCompra, opt => opt.Ignore()); 
        }
    }
}