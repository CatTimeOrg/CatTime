
namespace CatTime.Frontend.Helper
{
    public class UrlQueryBuilder
    {
        private string _startUrl;
        private List<KeyValuePair<string, object>> _parameters;

        public UrlQueryBuilder(string startUrl)
        {
            this._startUrl = startUrl;
            this._parameters = new List<KeyValuePair<string, object>>();
        }

        public static implicit operator string(UrlQueryBuilder urlQueryBuilder)
        {
            return urlQueryBuilder.ToQueryString();
        }

        public void AddParameter(string parameterName, object? parameterValue, bool condition = true)
        {
            if (parameterName == null || parameterValue == null || condition == false)
                return;

            this._parameters.Add(new KeyValuePair<string, object>(parameterName, parameterValue));
        }

        public string ToQueryString()
        {
            var url = this._startUrl;

            foreach (var parameter in this._parameters)
            {
                url = this.AddQueryParameterToUrl(url, parameter.Key, this.ParameterValueToString(parameter.Value));
            }

            return url;
        }

        private string ParameterValueToString(object value)
        {
            if(value is string stringValue)
                return stringValue;

            if (value is DateOnly dateOnly)
                return (dateOnly.ToString("yyyy-MM-dd"));

            if (value is TimeOnly timeOnly)
                return (timeOnly.ToString("HH:mm:ss"));

            if(value is DateTime dateTime)
                return (dateTime.ToString("yyyy-MM-ddTHH:mm:ssz"));

            if(value is int || value is long || value is double || value is float || value is decimal)
                return value.ToString() ?? string.Empty;

            if(value is bool boolValue)
                return boolValue.ToString().ToLower();

            return string.Empty;
        }

        private string AddQueryParameterToUrl(string url, string parameterName, string? parameterValue)
        {
            if (parameterValue == null)
                return url;

            var seperator = url.Contains("?") ? "&" : "?";

            return $"{url}{seperator}{parameterName}={parameterValue}";
        }
    }
}
