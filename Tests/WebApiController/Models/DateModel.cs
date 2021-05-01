using System;

namespace WebApiController.Models
{
    public class DateModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class DateModelWrapper
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateModel Model { get; set; } = new DateModel();
    }

    public class DateModelWrapperWithDate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.Now;
        public DateModel Model { get; set; } = new DateModel();
    }

    public class DateModelArrayWrapper
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateModel[] Models { get; set; } = { new DateModel(), new DateModel()};
    }

    public class DateArrayWrapper
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime[] Dates { get; set; } = { DateTime.Now, DateTime.Now.AddHours(-1) };
    }
}