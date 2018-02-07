using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TripListLambaAlexa
{
    public class Function
    {
        private static HttpClient httpClient;
        public const string INVOCATION_NAME = "Trip Plan";

        public Function()
        {
            httpClient = new HttpClient();
        }

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var intentRequest = input.Request as IntentRequest;
            if (intentRequest.Intent.Name.Equals("AddTrip"))
            {
                var dayRequested = intentRequest.Intent.Slots["Date"].Value;

                return MakeSkillResponse($"I've added {dayRequested} to your trip list", true);
            }
            else
            {
                return MakeSkillResponse($"I don't know how to handle this. Please say something like Alexa, ask {INVOCATION_NAME} to plan a trip", true);
            }
        }

        private SkillResponse MakeSkillResponse(
            string outputSpeech, 
            bool shouldEndSession, 
            string repromptText = "Just say, tell me a date to plan to learn more. To exit, say, exit.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            if (repromptText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
            }

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;
        }
    }
}
