using Newtonsoft.Json;
using NLog;
using Oci.Common.Auth;
using Oci.GenerativeaiagentruntimeService;
using Oci.GenerativeaiagentruntimeService.Models;
using Oci.GenerativeaiagentruntimeService.Requests;

internal class Program
{
    private const string AgentEndpointId = "ocid1.genaiagentendpoint.oc1.eu-frankfurt-1.amaaaaaa4kbj7vyaxejbi2f6ynqow2gbjcjzwqavnpkcqs5qhuce3l6qg54q";

    private static async Task Main(string[] args)
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        logger.Info("Starting GenAI example");
        GenerativeAiAgentRuntimeClient? client = null;

        //log in via session authentication: oci session authenticate --profile-name DEFAULT

        try
        {
            var provider = new SessionTokenAuthenticationDetailsProvider(Environment.GetEnvironmentVariable("PROFILE_WITH_SESSION_TOKEN") ?? "DEFAULT");
            var clientConfiguration = new Oci.Common.ClientConfiguration
            {
                ClientCertificateOption = ClientCertificateOption.Manual
            };
            using (client = new GenerativeAiAgentRuntimeClient(provider, clientConfiguration))
            {


                while (true)
                {
                    Console.WriteLine("Enter your message (or type 'exit' to quit):");
                    var message = Console.ReadLine();

                    if (string.Equals("exit", message, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    await ChatWithAgent(client, logger, message);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Info($"Exception occurred with GenerativeAiAgentClient: {ex}");
            throw;
        }
        finally
        {
            client?.Dispose();
        }

        static async Task ChatWithAgent(GenerativeAiAgentRuntimeClient client, Logger logger, string message)
        {
            var sessionResponse = await client.CreateSession(new CreateSessionRequest()
            {
                AgentEndpointId = AgentEndpointId,
                CreateSessionDetails = new CreateSessionDetails()
            });


            var request = new ChatRequest
            {
                AgentEndpointId = AgentEndpointId,
                ChatDetails = new ChatDetails
                {
                    UserMessage = message,
                    ShouldStream = true,
                    SessionId = sessionResponse.Session.Id,
                }
            };

            logger.Info($"Request: {JsonConvert.SerializeObject(request)}");
            Console.WriteLine("Fetching response from model...");
            try
            {
                var response = await client.Chat(request, completionOption: HttpCompletionOption.ResponseHeadersRead);

                using (var stream = await response.httpResponseMessage.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();
                        if (line.StartsWith("data:"))
                        {
                            string jsonContent = line.Substring("data: ".Length);
                            Response chatResponse = JsonConvert.DeserializeObject<Response>(jsonContent);
                            foreach (var trace in chatResponse.Traces)
                            {
                                if (!string.IsNullOrWhiteSpace(trace?.Output))
                                    Console.WriteLine(trace.Output);
                                if (!string.IsNullOrWhiteSpace(trace?.Generation))
                                    Console.WriteLine(trace.Generation);
                            }
                        }
                    }
                    Console.WriteLine("");
                }

                logger.Info($"Response from chat: {JsonConvert.SerializeObject(response)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
