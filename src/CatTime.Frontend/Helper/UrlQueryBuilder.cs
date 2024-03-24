
using Microsoft.AspNetCore.Components;
using System.Text;

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
            if(this._parameters.Count == 0)
                return this._startUrl;

            var stringBuilder = new StringBuilder(this._startUrl);            
            var seperator = this._startUrl.Contains("?") ? "&" : "?";

            foreach (var parameter in this._parameters)
            {
                stringBuilder.Append($"{seperator}{parameter.Key}={this.ParameterValueToString(parameter.Value)}");

                if(seperator == "?")
                    seperator = "&";
            }

            return stringBuilder.ToString();
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
    }
}
