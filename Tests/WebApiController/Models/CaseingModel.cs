namespace WebApiController.Models
{
    public class CasingModel
    {
        public string alllower { get; set; }
        public string ALLUPPER { get; set; }
        public string PascalCase { get; set; }
        public string camelCase { get; set; }
        public string snake_case { get; set; }
        public string UPPER_SNAKE_CASE { get; set; }

        public CasingModel()
        {
            this.alllower = KY.Core.Random2.NextString();
            this.ALLUPPER = KY.Core.Random2.NextString();
            this.PascalCase = KY.Core.Random2.NextString();
            this.camelCase = KY.Core.Random2.NextString();
            this.snake_case = KY.Core.Random2.NextString();
            this.UPPER_SNAKE_CASE = KY.Core.Random2.NextString();
        }
    }
}
