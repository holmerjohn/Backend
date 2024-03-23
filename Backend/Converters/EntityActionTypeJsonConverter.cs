using Backend.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Converters
{
    public class EntityActionTypeJsonConverter : JsonConverter<EntityActionType>
    {
        public override EntityActionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Enum.TryParse<EntityActionType>(reader.GetString(), true, out EntityActionType actionType))
            {
                return actionType;
            }
            else
            {
                return EntityActionType.Unsupported;
            }
        }

        public override void Write(Utf8JsonWriter writer, EntityActionType value, JsonSerializerOptions options)
        {
            string text = null;
            switch (value)
            {
                case EntityActionType.CreateLoan:
                    text = "createLoan";
                    break;
                case EntityActionType.CreateBorrower:
                    text = "createBorrower";
                    break;
                case EntityActionType.SetLoanField:
                    text = "setLoanField";
                    break;
                case EntityActionType.SetBorrowerField:
                    text = "setBorrowerField";
                    break;
                default:
                    text = "unsupported";
                    break;
            }
            writer.WriteStringValue(text);
        }
    }
}
