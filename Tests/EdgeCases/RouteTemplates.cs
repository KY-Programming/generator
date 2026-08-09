using KY.Generator;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCases;

/// <summary>
/// The <c>[controller]</c> and <c>[action]</c> tokens can be placed in the class level <c>[Route]</c>
/// template instead of the <c>[HttpGet]</c>/<c>[HttpPost]</c> attribute of every single action. Both
/// tokens have to be replaced in the generated URL, with the same lower case rule that is applied to a
/// token inside an action route (see <see cref="SignalsController"/>)
/// </summary>
[ApiController]
[Route("api/v1/[controller]/[action]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ClassRouteTokensController : ControllerBase
{
    [HttpGet]
    public string Get() => string.Empty;

    [HttpGet]
    public string GetWithMixedCaseName() => string.Empty;

    [HttpGet]
    public string Search([FromQuery] string filter) => filter;

    [HttpPost]
    public void Update(string value)
    { }
}

/// <summary>
/// The <c>[action]</c> token does not have to be the last segment of the class level route. Everything
/// behind it has to be kept as it is
/// </summary>
[ApiController]
[Route("api/[controller]/[action]/details")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class TokenInTheMiddleController : ControllerBase
{
    [HttpGet]
    public string Get() => string.Empty;

    [HttpPost]
    public void Update(string value)
    { }
}

/// <summary>
/// The tokens can be used in any order. The <c>[action]</c> token in front of the <c>[controller]</c>
/// token has to be replaced the same way
/// </summary>
[ApiController]
[Route("api/[action]/[controller]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ReversedTokensController : ControllerBase
{
    [HttpGet]
    public string Get() => string.Empty;

    [HttpPost]
    public void Update(string value)
    { }
}

/// <summary>
/// An inline parameter can follow the tokens in the class level route. The token has to be replaced,
/// the parameter has to be appended to the URL as usual
/// </summary>
[ApiController]
[Route("api/[controller]/[action]/{id}")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ClassRouteParameterController : ControllerBase
{
    [HttpGet]
    public string GetById(int id) => id.ToString();

    [HttpPost]
    public void UpdateById(int id, string value)
    { }
}

/// <summary>
/// Class level and action level route are combined. A token can appear in both of them, an action route
/// that starts with a slash replaces the class level route completely
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ClassAndActionRouteController : ControllerBase
{
    [HttpGet("sub")]
    public string GetSub() => string.Empty;

    [HttpGet("[action]")]
    public string GetTokenTwice() => string.Empty;

    [HttpGet("{id}")]
    public string GetById(int id) => id.ToString();

    [HttpGet("/api/override/[controller]/[action]")]
    public string GetAbsolute() => string.Empty;

    [HttpPost("sub")]
    public void UpdateSub(string value)
    { }
}

/// <summary>
/// The classic case: the class level route carries the <c>[controller]</c> token, the <c>[action]</c>
/// token sits on the verb attribute of every action
/// </summary>
[ApiController]
[Route("api/[controller]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ActionRouteTokensController : ControllerBase
{
    [HttpGet("[action]")]
    public string Get() => string.Empty;

    [HttpGet("[action]/{id}")]
    public string GetById(int id) => id.ToString();

    [HttpPost("[action]")]
    public void Update(string value)
    { }
}

/// <summary>
/// A class level route does not have to contain a token at all. The hard coded name has to be used as it
/// is, even if it differs from the controller name, and an <c>[action]</c> token on a verb attribute has
/// to be replaced anyway
/// </summary>
[ApiController]
[Route("api/fixed/values")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class HardcodedClassRouteController : ControllerBase
{
    [HttpGet]
    public string Get() => string.Empty;

    [HttpGet("[action]")]
    public string GetWithToken() => string.Empty;

    [HttpPost("save")]
    public void Update(string value)
    { }
}

/// <summary>
/// Hard coded names on the verb attributes. They have to be appended to the class level route unchanged,
/// including their casing and their inline parameters
/// </summary>
[ApiController]
[Route("api/[controller]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class HardcodedActionRoutesController : ControllerBase
{
    [HttpGet("list")]
    public string GetAll() => string.Empty;

    [HttpGet("by-id/{id}")]
    public string GetById(int id) => id.ToString();

    [HttpGet("Mixed/Case/Segment")]
    public string GetMixedCase() => string.Empty;

    [HttpPost("save")]
    public void Update(string value)
    { }
}

/// <summary>
/// One controller can mix all variants: an action without a route, a tokenized route, a hard coded route
/// and a route that starts with a slash and therefore ignores the class level route
/// </summary>
[ApiController]
[Route("api/[controller]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class MixedRoutesController : ControllerBase
{
    [HttpGet]
    public string Get() => string.Empty;

    [HttpGet("[action]")]
    public string GetWithToken() => string.Empty;

    [HttpGet("hardcoded")]
    public string GetHardcoded() => string.Empty;

    [HttpGet("/api/mixed/absolute")]
    public string GetAbsolute() => string.Empty;

    [HttpPost]
    public void Update(string value)
    { }
}

/// <summary>
/// The action route can be written into a separate <see cref="RouteAttribute"/> instead of the verb
/// attribute. Both spellings have to lead to the same URL
/// </summary>
[ApiController]
[Route("api/[controller]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class SeparateRouteAttributeController : ControllerBase
{
    [HttpGet]
    [Route("[action]")]
    public string Get() => string.Empty;

    [HttpGet]
    [Route("hardcoded")]
    public string GetHardcoded() => string.Empty;

    [HttpPost]
    [Route("save")]
    public void Update(string value)
    { }
}

/// <summary>
/// An inline parameter does not have to be the last segment of an action route. Literal segments behind
/// it have to be appended after the parameter value, and a route can contain more than one parameter
/// </summary>
[ApiController]
[Route("api/[controller]")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ParameterPositionsController : ControllerBase
{
    [HttpGet("{id}/details")]
    public string GetDetails(int id) => id.ToString();

    [HttpGet("[action]/{id}/sub/{name}")]
    public string GetSub(int id, string name) => name;

    [HttpGet("{id}/between/{name}/end")]
    public string GetBetween(int id, string name) => name;
}

/// <summary>
/// The same for the class level route: an inline parameter in front of the <c>[action]</c> token has to
/// keep its position, the token and everything behind it has to follow the parameter value
/// </summary>
[ApiController]
[Route("api/[controller]/{id}/[action]/details")]
[GenerateAngularService("Output/RouteTemplates", "Output/RouteTemplates")]
public class ParameterBeforeTokenController : ControllerBase
{
    [HttpGet]
    public string Get(int id) => id.ToString();

    [HttpGet("{name}/tail")]
    public string GetTail(int id, string name) => name;
}
