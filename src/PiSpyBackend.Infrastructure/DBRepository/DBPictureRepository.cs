using System.Reflection.Metadata.Ecma335;
using FluentResults;
using PiSpyBackend.Domain.Models;
using PiSpyBackend.Infrastructure.Interfaces;

namespace PiSpyBackend.Infrastructure.DBRepository
{
  public class DBPictureRepository : IDbPictureRepository
  {

    private readonly AppDbContext _context;

    public DBPictureRepository(AppDbContext context)
    {
      _context = context;
    }

    public Result AddPictureAsync(PictureDTO picture)
    {
      _context.Pictures.Add(picture);
      _context.SaveChanges();
      return Result.Ok();
    }

    public Result DeletePictureAsync(int id)
    {
      var picture = _context.Pictures.Find(id);
      if (picture == null)
      {
        return Result.Fail("Picture not found");
      }

      _context.Pictures.Remove(picture);
      _context.SaveChanges();
      return Result.Ok();
    }

    public Result<IEnumerable<PictureDTO>> GetAllPicturesAsync()
    {
      var pictures = _context.Pictures.AsEnumerable();
      if (pictures == null || !pictures.Any())
      {
        var emptyPictureList = new List<PictureDTO>().AsEnumerable();
        return Result.Ok(emptyPictureList);
      }

      return Result.Ok(pictures);
    }

    public Result<PictureDTO> GetPictureByIdAsync(int id)
    {
      var picture = _context.Pictures.Find(id);
      if (picture == null)
      {
        return Result.Fail("Picture not found");
      }

      return Result.Ok(picture);
    }
  }
}