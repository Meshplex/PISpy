using FluentResults;
using PiSpyBackend.Domain.Models;

namespace PiSpyBackend.Infrastructure.Interfaces
{
  public interface IDbPictureRepository
  {
    Result<PictureDTO> GetPictureByIdAsync(int id);
    Result<IEnumerable<PictureDTO>> GetAllPicturesAsync();
    Result AddPictureAsync(PictureDTO picture);
    Result DeletePictureAsync(int id);
  }
}