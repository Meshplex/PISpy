using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiSpyBackend.Application.Services;
using PiSpyBackend.Domain.Models;
using PiSpyBackend.Infrastructure.Interfaces;

namespace PiSpyBackend.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PictureController : ControllerBase
  {
    private readonly PictureService _service;

    public PictureController(PictureService service)
    {
      _service = service;
    }

    [HttpGet("getallpictures")]
    [Authorize]
    public Result<IEnumerable<PictureDTO>> GetAllPictures()
    {
      var result = _service.GetAllPictures();
      if (result.IsFailed)
      {
        return Result.Fail(result.Errors);
      }
      return result;
    }

    [HttpPost("addpicture")]
    public Result AddPicture(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return Result.Fail("Invalid picture path");
      }

      return _service.AddPicture(path);
    }

    [HttpGet("getpicturebyid/{id}")]
    [Authorize]
    public Result<PictureDTO> GetPictureById(int id)
    {
      if (id <= 0 || id > int.MaxValue)
      {
        return Result.Fail("Invalid picture ID");
      }

      return _service.GetPictureById(id);
    }

    [HttpDelete("deletepicture/{id}")]
    public Result DeletePicture(int id)
    {
      if (id <= 0 || id > int.MaxValue)
      {
        return Result.Fail("Invalid picture ID");
      }

      return _service.DeletePicture(id);
    }
  }
}