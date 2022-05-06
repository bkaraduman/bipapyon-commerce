using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BiPapyon.Common.Models
{
    public class ApiResponseDTO
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponseDTO()
        {
            StatusCode = 200;
            Message = "";
            Error = "";
            Success = true;
        }

        public override string ToString()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

        }
    }
}
