namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;

    public class UserActivitySubscriptionInfo
    {
        public string EventName { get; set; }

        public DateTime Timestamp { get; set; }

        public IEnumerable<long> Users { get; set; }

        public static UserActivitySubscriptionInfo Deserialize(
            byte[] payload)
        {
            var json = ConvertToString(payload);

            StringReader textReader = null;
            JsonReader jsonReader = null;

            try
            {
                textReader = new StringReader(json);
                jsonReader = new JsonTextReader(textReader);

                return new JsonSerializer()
                    .Deserialize<UserActivitySubscriptionInfo>(
                        jsonReader);
            }
            finally 
            {
                if (jsonReader != null)
                {
                    jsonReader.Close();
                }
                else
                {
                    if (textReader != null)
                    {
                        textReader.Dispose();
                    }
                }
            }
        }

        public byte[] Serialize()
        {
            var json = new StringBuilder();

            StringWriter textWriter = null;
            JsonTextWriter jsonWriter = null;

            try
            {
                textWriter = new StringWriter(
                    json,
                    CultureInfo.InvariantCulture);

                jsonWriter = new JsonTextWriter(textWriter);

                new JsonSerializer().Serialize(jsonWriter, this);
            }
            finally
            {
                if (jsonWriter != null)
                {
                    jsonWriter.Close();
                }
                else
                {
                    if (textWriter != null)
                    {
                        textWriter.Dispose();
                    }
                }
            }

            var buffer = ConvertToBytes(json.ToString());

            return buffer;
        }

        private static string ConvertToString(byte[] data)
        {
            var buffer = new char[data.Length / sizeof(char)];

            Buffer.BlockCopy(data, 0, buffer, 0, data.Length);

            return new string(buffer);
        }

        private static byte[] ConvertToBytes(string data)
        {
            var buffer = new byte[data.Length * sizeof(char)];

            Buffer.BlockCopy(
                data.ToCharArray(),
                0,
                buffer,
                0,
                buffer.Length);

            return buffer;
        }
    }
}