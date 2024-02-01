using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Json
{
    public class JsonReader : IDisposable
    {

        private enum JsonReadingState
        {
            JsonNone,
            JsonReadingPropertyName,
            JsonPendingPropertyValue,
            JsonReadingPropertyValue
        }

        private string _json;
        private char _currChar => _json[_readingPosition];
        private int _readingPosition;
        private bool _isParsed;

        private JsonArray _jsonArray;

        public JsonReader(string json)
        {
            _json = json.Trim();
            _readingPosition = 0;
            _jsonArray = new();
            _isParsed = false;
        }

        public void Dispose()
        {
            _json = null;
            _jsonArray = null;
        }

        public bool Parse()
        {
            if (jsonHasInvalidChars() || _currChar != '[')
                return false;

            bool isReadingObject = false;
            JsonReadingState state = JsonReadingState.JsonNone;
            string name = "";
            StringBuilder builder = new();
            JsonObject currentObject = null;

            for (_readingPosition = 1; _readingPosition < _json.Length; _readingPosition++)
            {
                if (jsonHasInvalidChars())
                    continue;

                if (isReadingObject)
                {
                    if (_currChar == '}')
                    {
                        isReadingObject = false;
                        _jsonArray.Objects.Add(currentObject);
                        currentObject = null;
                    }
                    else if (state == JsonReadingState.JsonReadingPropertyName)
                    {
                        if (_currChar == '"')
                        {
                            name = builder.ToString();
                            state = JsonReadingState.JsonPendingPropertyValue;
                        }
                        else
                        {
                            builder.Append(_currChar);
                        }
                    }
                    else if (state == JsonReadingState.JsonPendingPropertyValue)
                    {
                        if (_currChar == '"')
                        {
                            builder = new();
                            state = JsonReadingState.JsonReadingPropertyValue;
                        }
                    }
                    else if (state == JsonReadingState.JsonReadingPropertyValue)
                    {
                        if (_currChar == '"')
                        {
                            string value = builder.ToString();
                            currentObject.Add(name, value);
                            state = JsonReadingState.JsonNone;
                        }
                        else
                        {
                            builder.Append(_currChar);
                        }
                    }
                    else if (_currChar == '"' && state == JsonReadingState.JsonNone)
                    {
                        state = JsonReadingState.JsonReadingPropertyName;
                        builder = new();
                    }
                }
                else if (_currChar == '{')
                {
                    isReadingObject = true;
                    currentObject = new();
                }
            }
            _readingPosition = 0;
            return true;
        }

        public JsonArray GetArray()
        {
            return _jsonArray;
        }

        public JsonObject this[int idx]
        {
            get
            {
                return _jsonArray.Objects[idx];
            }
        }

        private bool jsonHasInvalidChars()
        {
            return (_currChar == '\n' || _currChar == '\t' || _currChar == '\r');
        }

        
    }
}
