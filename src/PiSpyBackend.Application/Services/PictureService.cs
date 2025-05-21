using FluentResults;
using PiSpyBackend.Domain.Models;
using PiSpyBackend.Infrastructure;
using PiSpyBackend.Infrastructure.DBRepository;
using PiSpyBackend.Infrastructure.Interfaces;

namespace PiSpyBackend.Application.Services
{
  public class PictureService
  {
    private readonly IDbPictureRepository _dbPictureRepository;

    public PictureService(AppDbContext dbContext)
    {
      _dbPictureRepository = new DBPictureRepository(dbContext);
    }

    public Result AddPicture(string path)
    {
      var dto = new PictureDTO
      {
        Path = path,
        CreatedAt = DateTime.UtcNow
      };
      return _dbPictureRepository.AddPictureAsync(dto);
    }

    public Result DeletePicture(int id)
    {
      return _dbPictureRepository.DeletePictureAsync(id);
    }

    public Result<IEnumerable<PictureDTO>> GetAllPictures()
    {
      return _dbPictureRepository.GetAllPicturesAsync();
    }

    public Result<PictureDTO> GetPictureById(int id)
    {
      return _dbPictureRepository.GetPictureByIdAsync(id);
    }
  }
}