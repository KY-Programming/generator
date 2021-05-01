using System;
using System.Collections.Generic;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [GenerateAngularService("ClientApp/src/app/date/services", "ClientApp/src/app/date/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class DateController : ControllerBase
    {
        [HttpGet("[action]")]
        public DateTime Get()
        {
            return DateTime.Now;
        }

        [HttpGet("[action]")]
        public DateTime[] GetArray()
        {
            return new[] { DateTime.Now, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-2) };
        }

        [HttpGet("[action]")]
        public List<DateTime> GetList()
        {
            return new List<DateTime>(this.GetArray());
        }

        [HttpGet("[action]")]
        public IEnumerable<DateTime> GetEnumerable()
        {
            return this.GetArray();
        }

        [HttpGet("[action]")]
        public DateModel GetComplex()
        {
            return new DateModel();
        }

        [HttpGet("[action]")]
        public DateModel[] GetComplexArray()
        {
            return new[] { new DateModel(), new DateModel() };
        }

        [HttpGet("[action]")]
        public List<DateModel> GetComplexList()
        {
            return new List<DateModel>(this.GetComplexArray());
        }

        [HttpGet("[action]")]
        public IEnumerable<DateModel> GetComplexEnumerable()
        {
            return this.GetComplexArray();
        }

        [HttpGet("[action]")]
        public DateArrayWrapper GetWrappedArray()
        {
            return new DateArrayWrapper();
        }

        [HttpGet("[action]")]
        public DateModelWrapper GetWrappedModel()
        {
            return new DateModelWrapper();
        }

        [HttpGet("[action]")]
        public DateModelWrapperWithDate GetWrappedModelWithDate()
        {
            return new DateModelWrapperWithDate();
        }

        [HttpGet("[action]")]
        public DateModelArrayWrapper GetWrappedModelArray()
        {
            return new DateModelArrayWrapper();
        }

        [HttpPost("[action]")]
        public void Post(DateTime date)
        { }
    }
}