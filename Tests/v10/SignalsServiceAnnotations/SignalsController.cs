using KY.Generator;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable UnusedMember.Global

namespace SignalsService;

[ApiController]
[Route("api/v1/[controller]")]
[GenerateAngularService("Output", "Output")]
public class SignalsController : ControllerBase
{
    [HttpGet("[action]")]
    public SignalModel Get()
    {
        return new SignalModel();
    }

    [HttpGet("[action]")]
    public List<SignalModel> GetAll()
    {
        return [];
    }

    [HttpPost("[action]")]
    public string Update(SignalModel model)
    {
        return model.Text;
    }

    [HttpPost("[action]")]
    public void UpdateAll(List<SignalModel> models)
    { }

    [HttpGet("[action]")]
    public PlainModel GetPlain()
    {
        return new PlainModel();
    }

    [HttpPost("[action]")]
    public void UpdatePlain(PlainModel model)
    { }
}
