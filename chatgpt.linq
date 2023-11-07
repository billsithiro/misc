<Query Kind="Program">
  <NuGetReference>Rock.Core.Newtonsoft</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

static string apiKey = "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";

void Main()
{
	//GetAssistant().Dump();
    //CreateThread().Dump();
	//CreateMessage("thread_Cz00jw3XHMVubKYNOWEuBn7F", "hi, who are you?").Dump();
	//CreateRun("thread_Cz00jw3XHMVubKYNOWEuBn7F").Dump();
	//GetMessages("thread_Cz00jw3XHMVubKYNOWEuBn7F").Dump();
	//CreateMessage("thread_Cz00jw3XHMVubKYNOWEuBn7F", "how to cancel a flight?").Dump();
	//GetMessages("thread_Cz00jw3XHMVubKYNOWEuBn7F").Dump();
	//CreateRun("thread_Cz00jw3XHMVubKYNOWEuBn7F").Dump();
	CreateMessage("thread_Cz00jw3XHMVubKYNOWEuBn7F", "where are user groups defined?");
	string runId = CreateRun("thread_Cz00jw3XHMVubKYNOWEuBn7F")["id"].ToString();
	while(true)
	{
		var result = GetRun("thread_Cz00jw3XHMVubKYNOWEuBn7F", runId);
		if (result["status"].ToString() == "completed")
		{
			var messages = GetMessages("thread_Cz00jw3XHMVubKYNOWEuBn7F");
			var message = messages["data"].First();
			var content = message["content"].First();
			var text = content["text"];
			text["value"].Dump();
			break;
		}
		Thread.Sleep(2000);
	}
}

JObject GetAssistant() 
{
	var uri = "https://api.openai.com/v1/assistants/asst_x03DAXTY1GQSdPIpuuZgiRoz"; 

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
       
		var response = client.GetAsync(uri).Result;        
        
		if (!response.IsSuccessStatusCode)        
            throw new Exception("Error: " + response.StatusCode);        
		
        var responseContent = response.Content.ReadAsStringAsync().Result;		
		return JsonConvert.DeserializeObject<JObject>(responseContent);
    }
}

JObject CreateThread() 
{
	var uri = "https://api.openai.com/v1/threads"; 

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
	
        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        var response = client.PostAsync(uri, content).Result;

        if (!response.IsSuccessStatusCode)        
            throw new Exception("Error: " + response.StatusCode);        
		
        var responseContent = response.Content.ReadAsStringAsync().Result;
		return JsonConvert.DeserializeObject<JObject>(responseContent);
    }
}

JObject CreateMessage(string threadId, string message) 
{
	if (string.IsNullOrEmpty(threadId))
		throw new Exception("The thread ID must not be null or empty.");

	if (string.IsNullOrEmpty(message))
		throw new Exception("The message must not be null or empty.");
	
	var uri = "https://api.openai.com/v1/threads/" + threadId + "/messages"; 

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        
		var json = new { 
			role= "user",
    		content= message, 
		};
	
        var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
        var response = client.PostAsync(uri, content).Result;

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error: " + response.StatusCode);
		
        var responseContent = response.Content.ReadAsStringAsync().Result;
		return JsonConvert.DeserializeObject<JObject>(responseContent);
    }
}

JObject CreateRun(string threadId) 
{
	if (string.IsNullOrEmpty(threadId))
		throw new Exception("The thread ID must not be null or empty.");

	var uri = "https://api.openai.com/v1/threads/" + threadId + "/runs"; 

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        
		var json = new { 
			assistant_id= "asst_x03DAXTY1GQSdPIpuuZgiRoz",
		};
	
        var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
        var response = client.PostAsync(uri, content).Result;

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error: " + response.StatusCode);
		
        var responseContent = response.Content.ReadAsStringAsync().Result;
		return JsonConvert.DeserializeObject<JObject>(responseContent);
    }
}

JObject GetRun(string threadId, string runId) 
{
	if (string.IsNullOrEmpty(threadId))
		throw new Exception("The thread ID must not be null or empty.");

	var uri = "https://api.openai.com/v1/threads/" + threadId + "/runs/" + runId; 

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        
		var response = client.GetAsync(uri).Result;

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error: " + response.StatusCode);
		
        var responseContent = response.Content.ReadAsStringAsync().Result;
		return JsonConvert.DeserializeObject<JObject>(responseContent);
    }
}

JObject GetMessages(string threadId) 
{
	if (string.IsNullOrEmpty(threadId))
		throw new Exception("The thread ID must not be null or empty.");

	var uri = "https://api.openai.com/v1/threads/" + threadId + "/messages"; 

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
       
		var response = client.GetAsync(uri).Result;        
        
		if (!response.IsSuccessStatusCode)        
            throw new Exception("Error: " + response.StatusCode);        
		
        var responseContent = response.Content.ReadAsStringAsync().Result;		
		return JsonConvert.DeserializeObject<JObject>(responseContent);
    }
}